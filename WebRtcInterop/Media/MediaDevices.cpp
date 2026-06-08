#include "pch.h"

// Make WebRtcNet.Api types accessible as friends
#pragma as_friend("WebRtcNet.Api")

#include <api/audio_options.h>
#include "MediaDevices.h"
#include "CameraVideoSource.h"
#include "Logging/InteropHResult.h"
#include "MediaStream.h"
#include "Marshaling/MarshalMedia.h"
#include "Marshaling/MarshalMediaDevices.h"
#include "RtcPeerConnectionFactory.h"

#include <api/peer_connection_interface.h>
#include <modules/video_capture/video_capture_factory.h>
#include <mmdeviceapi.h>
#include <Functiondiscoverykeys_devpkey.h>

#pragma comment(lib, "ole32.lib")
#pragma comment(lib, "mmdevapi.lib")

using namespace System::Collections::Generic;
using namespace System::Threading::Tasks;
using namespace System::Timers;

namespace WebRtcInterop::Media
{
	using namespace WebRtcNet::Media;

	static List<MediaDeviceInfo^>^ EnumerateAudioDevices();
	static List<MediaDeviceInfo^>^ EnumerateVideoDevices();

	namespace
	{
		String^ GetInteropMediaDevicesCategory()
		{
			return "Interop.MediaDevices";
		}

		String^ GetDeviceMapKey(MediaDeviceInfo^ device)
		{
			return String::Format("{0}|{1}", (int)device->Kind, device->DeviceId);
		}

		String^ GetAudioDeviceLabel(IMMDevice* device, String^ fallbackLabel)
		{
			auto label = fallbackLabel;
			IPropertyStore* propertyStore = nullptr;
			auto hr = device->OpenPropertyStore(STGM_READ, &propertyStore);
			if (InteropHResult::LogIfFailed(hr, "IMMDevice::OpenPropertyStore", GetInteropMediaDevicesCategory()) ||
				propertyStore == nullptr)
				return label;

			PROPVARIANT friendlyName;
			PropVariantInit(&friendlyName);
			hr = propertyStore->GetValue(PKEY_Device_FriendlyName, &friendlyName);
			InteropHResult::LogIfFailed(hr, "IPropertyStore::GetValue(PKEY_Device_FriendlyName)", GetInteropMediaDevicesCategory());
			if (SUCCEEDED(hr) &&
				friendlyName.vt == VT_LPWSTR)
				label = marshal_as<String^>(friendlyName.pwszVal);

			PropVariantClear(&friendlyName);
			propertyStore->Release();
			return label;
		}

		void EnumerateAudioEndpoints(
			IMMDeviceEnumerator* enumerator,
			const EDataFlow flow,
			String^ fallbackLabel,
			List<MediaDeviceInfo^>^ devices)
		{
			IMMDeviceCollection* collection = nullptr;
			auto hr = enumerator->EnumAudioEndpoints(flow, DEVICE_STATE_ACTIVE, &collection);
			if (InteropHResult::LogIfFailed(
				hr,
				String::Format("IMMDeviceEnumerator::EnumAudioEndpoints(flow={0})", static_cast<int>(flow)),
				GetInteropMediaDevicesCategory()) || collection == nullptr)
				return;

			UINT count = 0;
			hr = collection->GetCount(&count);
			if (InteropHResult::LogIfFailed(hr, "IMMDeviceCollection::GetCount", GetInteropMediaDevicesCategory()))
			{
				collection->Release();
				return;
			}

			const auto kind = marshal_as<MediaDeviceKind>(flow);
			const bool isInput = kind == MediaDeviceKind::AudioInput;

			for (UINT i = 0; i < count; ++i)
			{
				IMMDevice* device = nullptr;
				hr = collection->Item(i, &device);
				if (InteropHResult::LogIfFailed(
					hr,
					String::Format("IMMDeviceCollection::Item(index={0})", i),
					GetInteropMediaDevicesCategory()) || device == nullptr)
					continue;

				LPWSTR deviceId = nullptr;
				hr = device->GetId(&deviceId);
				if (InteropHResult::LogIfFailed(
					hr,
					String::Format("IMMDevice::GetId(index={0})", i),
					GetInteropMediaDevicesCategory()))
				{
					device->Release();
					continue;
				}

				auto id = marshal_as<String^>(deviceId);
				auto label = GetAudioDeviceLabel(device, fallbackLabel);
				CoTaskMemFree(deviceId);

				devices->Add(isInput
					? InputDeviceInfo::Create(id, kind, label, String::Empty)
					: MediaDeviceInfo::Create(id, kind, label, String::Empty));

				device->Release();
			}

			collection->Release();
		}

		void ValidateGetUserMediaConstraints(MediaStreamConstraints^ constraints)
		{
			if (constraints == nullptr)
				throw gcnew ArgumentNullException("constraints");

			if (!constraints->Audio && !constraints->Video)
				throw gcnew MediaStreamException("At least one of audio or video must be requested.");
		}

		void ValidateRequestedAudioDevices(MediaStreamConstraints^ constraints)
		{
			if (!constraints->Audio)
				return;

			auto audioDevices = EnumerateAudioDevices();
			if (audioDevices == nullptr || audioDevices->Count == 0)
				throw gcnew MediaStreamException("No audio input devices are currently available.");
		}

		List<MediaDeviceInfo^>^ ValidateAndGetRequestedVideoDevices(MediaStreamConstraints^ constraints)
		{
			if (!constraints->Video)
				return nullptr;

			auto videoDevices = EnumerateVideoDevices();
			if (videoDevices == nullptr || videoDevices->Count == 0)
				throw gcnew MediaStreamException("No video input devices are currently available.");

			return videoDevices;
		}

		std::string CreateGuidLabel(const std::string& prefix)
		{
			return prefix + "_" + marshal_as<std::string>(Guid::NewGuid().ToString());
		}

		webrtc::scoped_refptr<webrtc::MediaStreamInterface> CreateNativeStream(
			webrtc::PeerConnectionFactoryInterface* factory)
		{
			const auto streamId = marshal_as<std::string>(Guid::NewGuid().ToString());
			auto nativeStream = factory->CreateLocalMediaStream(streamId);
			if (!nativeStream)
				throw gcnew InvalidOperationException("Failed to create media stream.");

			return nativeStream;
		}

		void AddAudioTrack(
			webrtc::PeerConnectionFactoryInterface* factory,
			const webrtc::scoped_refptr<webrtc::MediaStreamInterface>& nativeStream)
		{
			const webrtc::AudioOptions audioOptions;
			const auto nativeAudioSource = factory->CreateAudioSource(audioOptions);
			if (!nativeAudioSource)
				throw gcnew MediaStreamException("Failed to create an audio source for the requested track.");

			const auto nativeAudioTrack = factory->CreateAudioTrack(
				CreateGuidLabel("audio"),
				nativeAudioSource.get());
			if (!nativeAudioTrack)
				throw gcnew MediaStreamException("Failed to create the requested audio track.");

			if (!nativeStream->AddTrack(nativeAudioTrack))
				throw gcnew MediaStreamException("Failed to add the audio track to the media stream.");
		}

		webrtc::scoped_refptr<webrtc::VideoTrackSourceInterface> CreateVideoSource(
			List<MediaDeviceInfo^>^ videoDevices)
		{
			for each (auto videoDevice in videoDevices)
			{
				if (videoDevice == nullptr ||
					String::IsNullOrEmpty(videoDevice->DeviceId) ||
					videoDevice->Kind != MediaDeviceKind::VideoInput)
					continue;

				auto videoDeviceId = marshal_as<std::string>(videoDevice->DeviceId);
				if (auto candidateSource = CameraVideoSource::Create(videoDeviceId))
					return candidateSource;
			}

			throw gcnew MediaStreamException("Failed to create a camera-backed video source for the requested track.");
		}

		void AddVideoTrack(
			webrtc::PeerConnectionFactoryInterface* factory,
			const webrtc::scoped_refptr<webrtc::MediaStreamInterface>& nativeStream,
			List<MediaDeviceInfo^>^ videoDevices)
		{
			const auto videoSource = CreateVideoSource(videoDevices);
			const auto nativeVideoTrack = factory->CreateVideoTrack(
				videoSource,
				CreateGuidLabel("video"));
			if (!nativeVideoTrack)
				throw gcnew MediaStreamException("Failed to create the requested video track.");

			if (!nativeStream->AddTrack(nativeVideoTrack))
				throw gcnew MediaStreamException("Failed to add the video track to the media stream.");
		}
	}

	MediaDevices::MediaDevices()
		: known_devices_(gcnew Dictionary<String^, MediaDeviceInfo^>()),
		  device_poll_timer_(gcnew Timer(2000.0)),
		  device_poll_gate_(gcnew Object()),
		  on_device_change_(nullptr)
	{
		RefreshKnownDevices(false);
		device_poll_timer_->AutoReset = true;
		device_poll_timer_->Elapsed += gcnew ElapsedEventHandler(this, &MediaDevices::OnDevicePoll);
		device_poll_timer_->Start();
	}

	MediaDevices::~MediaDevices()
	{
		this->!MediaDevices();
	}

	MediaDevices::!MediaDevices()
	{
		StopDevicePolling();
	}

	void MediaDevices::StopDevicePolling()
	{
		if (device_poll_timer_ == nullptr)
			return;

		device_poll_timer_->Stop();
		device_poll_timer_->Close();
		device_poll_timer_ = nullptr;
	}

	void MediaDevices::OnDevicePoll(Object^ sender, ElapsedEventArgs^ args)
	{
		RefreshKnownDevices(true);
	}

	void MediaDevices::RefreshKnownDevices(const bool raiseEvent)
	{
		Threading::Monitor::Enter(device_poll_gate_);
		try
		{
			auto enumerateTask = EnumerateDevices();
			if (enumerateTask == nullptr)
				return;

			auto devices = enumerateTask->GetAwaiter().GetResult();
			auto current = gcnew Dictionary<String^, MediaDeviceInfo^>();
			auto inserted = gcnew List<MediaDeviceInfo^>();
			auto all = gcnew List<MediaDeviceInfo^>();

			for each (auto device in devices)
			{
				auto key = GetDeviceMapKey(device);
				current[key] = device;
				all->Add(device);

				if (!known_devices_->ContainsKey(key))
					inserted->Add(device);
			}

			bool changed = current->Count != known_devices_->Count;
			if (!changed)
			{
				for each (auto key in current->Keys)
				{
					if (!known_devices_->ContainsKey(key))
					{
						changed = true;
						break;
					}
				}
			}

			known_devices_->Clear();
			for each (auto kvp in current)
				known_devices_->Add(kvp.Key, kvp.Value);

			if (raiseEvent && changed && on_device_change_ != nullptr)
				on_device_change_(this, gcnew DeviceChangeEventArgs(all, inserted));
		}
		catch (Exception^)
		{
			// Device polling must not crash the process. Failures are retried on next poll.
		}
		finally
		{
			Threading::Monitor::Exit(device_poll_gate_);
		}
	}

	// Helper to enumerate Windows audio devices
	static List<MediaDeviceInfo^>^ EnumerateAudioDevices()
	{
		auto devices = gcnew List<MediaDeviceInfo^>();

		try
		{
			// Initialize COM
			HRESULT hr = CoInitializeEx(nullptr, COINIT_MULTITHREADED);
			if (FAILED(hr) && hr != S_FALSE)
			{
				InteropHResult::LogIfFailed(hr, "CoInitializeEx", GetInteropMediaDevicesCategory());
				return devices; // COM already initialized or failed
			}

			IMMDeviceEnumerator* enumerator = nullptr;
			hr = CoCreateInstance(__uuidof(MMDeviceEnumerator), nullptr, CLSCTX_INPROC_SERVER,
				__uuidof(IMMDeviceEnumerator), reinterpret_cast<void**>(&enumerator));
			InteropHResult::LogIfFailed(hr, "CoCreateInstance(MMDeviceEnumerator)", GetInteropMediaDevicesCategory());

			if (SUCCEEDED(hr) && enumerator != nullptr)
			{
				EnumerateAudioEndpoints(
						enumerator,
						eCapture,
						gcnew String(L"Audio Input Device"),
						devices);
					EnumerateAudioEndpoints(
						enumerator,
						eRender,
						gcnew String(L"Audio Output Device"),
						devices);
				enumerator->Release();
			}

			CoUninitialize();
		}
		catch (...)
		{
			// Silently ignore errors in device enumeration
		}

		return devices;
	}

	// Helper to enumerate Windows video devices using DirectShow
	static List<MediaDeviceInfo^>^ EnumerateVideoDevices()
	{
		auto devices = gcnew List<MediaDeviceInfo^>();

		try
		{
			const std::unique_ptr<webrtc::VideoCaptureModule::DeviceInfo> deviceInfo(
				webrtc::VideoCaptureFactory::CreateDeviceInfo());
			if (!deviceInfo)
				return devices;

			const uint32_t count = deviceInfo->NumberOfDevices();
			for (uint32_t i = 0; i < count; ++i)
			{
				char deviceName[512] = {};
				char deviceUniqueId[512] = {};
				char productUniqueId[512] = {};

				const int32_t result = deviceInfo->GetDeviceName(
					i,
					deviceName,
					sizeof(deviceName),
					deviceUniqueId,
					sizeof(deviceUniqueId),
					productUniqueId,
					sizeof(productUniqueId));

				if (result != 0)
					continue;

				auto id = marshal_as<String^>(std::string(deviceUniqueId));
				auto label = marshal_as<String^>(std::string(deviceName));
				String^ groupId =
					productUniqueId[0] != '\0'
						? marshal_as<String^>(std::string(productUniqueId))
						: String::Empty;

				devices->Add(InputDeviceInfo::Create(
					id,
					MediaDeviceKind::VideoInput,
					label,
					groupId));
			}
		}
		catch (...)
		{
			// Silently ignore errors
		}

		return devices;
	}

	Task<IEnumerable<MediaDeviceInfo^>^>^ MediaDevices::EnumerateDevices()
	{
		try
		{
			auto allDevices = gcnew List<MediaDeviceInfo^>();

			// Enumerate audio devices
			if (auto audioDevices = EnumerateAudioDevices(); audioDevices != nullptr)
				allDevices->AddRange(audioDevices);

			// Enumerate video devices
			if (auto videoDevices = EnumerateVideoDevices(); videoDevices != nullptr)
				allDevices->AddRange(videoDevices);

			return Task::FromResult<IEnumerable<MediaDeviceInfo^>^>(allDevices);
		}
		catch (Exception^ ex)
		{
			return Task::FromException<IEnumerable<MediaDeviceInfo^>^>(ex);
		}
	}

	MediaTrackSupportedConstraints^ MediaDevices::GetSupportedConstraints()
	{
		return gcnew MediaTrackSupportedConstraints();
	}

	Task<WebRtcNet::Media::MediaStream^>^ MediaDevices::GetUserMedia(MediaStreamConstraints^ constraints)
	{
		try
		{
			ValidateGetUserMediaConstraints(constraints);
			ValidateRequestedAudioDevices(constraints);
			auto videoDevices = ValidateAndGetRequestedVideoDevices(constraints);

			// Get the native peer connection factory
			const auto factory = RtcPeerConnectionFactory::Instance->GetNativePeerConnectionFactoryInterface(true);
			if (factory == nullptr)
				throw gcnew InvalidOperationException("PeerConnectionFactory not initialized.");

			const auto nativeStream = CreateNativeStream(factory);

			// Create audio track if requested
			if (constraints->Audio)
				AddAudioTrack(factory, nativeStream);

			// Create video track if requested
			if (constraints->Video)
				AddVideoTrack(factory, nativeStream, videoDevices);

			// Create managed MediaStream wrapper
			auto managedStream = gcnew MediaStream(nativeStream);
			return Task::FromResult<WebRtcNet::Media::MediaStream^>(managedStream);
		}
		catch (Exception^ ex)
		{
			return Task::FromException<WebRtcNet::Media::MediaStream^>(ex);
		}
	}
}
