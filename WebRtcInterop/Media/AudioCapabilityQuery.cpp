#include "pch.h"

#include "AudioCapabilityQuery.h"
#include "Marshaling/MarshalMediaTrackCapabilities.h"
#include "../Logging/InteropHResult.h"

#include <audioclient.h>
#include <mmdeviceapi.h>

namespace WebRtcInterop::Media
{
	using namespace WebRtcNet;
	using namespace WebRtcNet::Logging;

	namespace
	{
		MediaTrackCapabilities^ QueryWithDevice(String^ endpointId, IMMDevice* device)
		{
			IAudioClient* audioClient = nullptr;
			WAVEFORMATEX* format = nullptr;

			try
			{
				auto hr = device->Activate(
					__uuidof(IAudioClient),
					CLSCTX_ALL,
					nullptr,
					reinterpret_cast<void**>(&audioClient));
				if (InteropHResult::LogIfFailed(hr, "IMMDevice::Activate(IAudioClient)", "Interop.Media.Audio.Capabilities")
					|| audioClient == nullptr)
					return MediaTrackCapabilities::CreateIdentity(endpointId);

				hr = audioClient->GetMixFormat(&format);
				if (InteropHResult::LogIfFailed(hr, "IAudioClient::GetMixFormat", "Interop.Media.Audio.Capabilities")
					|| format == nullptr)
					return MediaTrackCapabilities::CreateIdentity(endpointId);

				WebRtcLogWriterBridge::WriteInteropLog(
					0,
					static_cast<int>(WebRtcLogEventId::AudioCapabilityQueryCompleted),
					"Interop.Media.Audio.Capabilities",
					Threading::Thread::CurrentThread->ManagedThreadId,
					String::Format("Audio mix format for device {0}: {1}Hz, {2}-bit, {3}ch",
						endpointId, format->nSamplesPerSec, format->wBitsPerSample, format->nChannels));

				ValueRange<unsigned int>^ sampleRate = nullptr;
				ValueRange<unsigned int>^ sampleSize = nullptr;
				ValueRange<unsigned int>^ channelCount = nullptr;

				if (format->nSamplesPerSec > 0)
				{
					sampleRate = MarshalToValueRange<unsigned int>(
						System::Nullable<unsigned int>(
							static_cast<unsigned int>(format->nSamplesPerSec)));
				}

				if (format->wBitsPerSample > 0)
				{
					sampleSize = MarshalToValueRange<unsigned int>(
						System::Nullable<unsigned int>(
							static_cast<unsigned int>(format->wBitsPerSample)));
				}

				if (format->nChannels > 0)
				{
					channelCount = MarshalToValueRange<unsigned int>(
						System::Nullable<unsigned int>(format->nChannels));
				}

				// WebRTC's software APM always provides AGC and noise suppression regardless of hardware.
				// This mirrors Blink's input_device_info.cc which hardcodes {true, false} for both.
				auto alwaysSupported = gcnew Collections::Generic::List<bool>();
				alwaysSupported->Add(true);
				alwaysSupported->Add(false);

				// Software (WebRTC AEC3) is always available. System mode requires OS/driver support
				// and is added separately when WASAPI device effects are queried (not yet implemented).
				// false = disabled is always a valid option.
				auto echoCancellation = gcnew Collections::Generic::List<EchoCancellationValue>();
				echoCancellation->Add(EchoCancellationValue(EchoCancellationMode::Software));
				echoCancellation->Add(EchoCancellationValue(false));

				return MediaTrackCapabilities::Create(
					nullptr,
					nullptr,
					nullptr,
					nullptr,
					nullptr,
					nullptr,
					sampleRate,
					sampleSize,
					echoCancellation,
					nullptr,
					alwaysSupported,
					alwaysSupported,
					nullptr,
					channelCount,
					endpointId,
					String::Empty);
			}
			finally
			{
				if (format != nullptr)
					CoTaskMemFree(format);
				if (audioClient != nullptr)
					audioClient->Release();
			}
		}
	}

	AudioCapabilityQuery::AudioCapabilityQuery(String^ endpointId)
		: endpoint_id_(endpointId)
	{
	}

	MediaTrackCapabilities^ AudioCapabilityQuery::Query()
	{
		using namespace msclr::interop;

		auto hr = CoInitializeEx(nullptr, COINIT_MULTITHREADED);
		const auto uninitialize = SUCCEEDED(hr) && hr != S_FALSE;

		IMMDeviceEnumerator* enumerator = nullptr;
		IMMDevice* device = nullptr;

		try
		{
			hr = CoCreateInstance(
				__uuidof(MMDeviceEnumerator),
				nullptr,
				CLSCTX_INPROC_SERVER,
				__uuidof(IMMDeviceEnumerator),
				reinterpret_cast<void**>(&enumerator));
			if (InteropHResult::LogIfFailed(hr, "CoCreateInstance(MMDeviceEnumerator)", "Interop.Media.Audio.Capabilities")
				|| enumerator == nullptr)
				return MediaTrackCapabilities::CreateIdentity(endpoint_id_);

			String^ endpointId = endpoint_id_;
			const auto nativeId = marshal_as<std::wstring>(endpointId);
			hr = enumerator->GetDevice(nativeId.c_str(), &device);
			if (InteropHResult::LogIfFailed(hr, "IMMDeviceEnumerator::GetDevice", "Interop.Media.Audio.Capabilities")
				|| device == nullptr)
				return MediaTrackCapabilities::CreateIdentity(endpoint_id_);

			return QueryWithDevice(endpoint_id_, device);
		}
		finally
		{
			if (device != nullptr)
				device->Release();
			if (enumerator != nullptr)
				enumerator->Release();
			if (uninitialize)
				CoUninitialize();
		}
	}
}
