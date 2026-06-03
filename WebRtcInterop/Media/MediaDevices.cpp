#include "pch.h"

#include "MediaDevices.h"
#include "MediaStreamTrack.h"
#include "Marshaling/MarshalMedia.h"

#include <mmdeviceapi.h>
#include <Functiondiscoverykeys_devpkey.h>

#pragma comment(lib, "ole32.lib")
#pragma comment(lib, "mmdevapi.lib")

using namespace System::Collections::Generic;
using namespace System::Threading::Tasks;

namespace WebRtcInterop::Media
{
	// Helper to enumerate Windows audio devices
	static List<WebRtcNet::Media::MediaDeviceInfo^>^ EnumerateAudioDevices()
	{
		auto devices = gcnew List<WebRtcNet::Media::MediaDeviceInfo^>();

		try
		{
			// Initialize COM
			HRESULT hr = CoInitializeEx(nullptr, COINIT_MULTITHREADED);
			if (FAILED(hr) && hr != S_FALSE)
				return devices; // COM already initialized or failed

			{
				IMMDeviceEnumerator* pEnumerator = nullptr;
				hr = CoCreateInstance(__uuidof(MMDeviceEnumerator), nullptr, CLSCTX_INPROC_SERVER,
					__uuidof(IMMDeviceEnumerator), (void**)&pEnumerator);

				if (SUCCEEDED(hr) && pEnumerator != nullptr)
				{
					// Enumerate audio inputs
					{
						IMMDeviceCollection* pCollection = nullptr;
						hr = pEnumerator->EnumAudioEndpoints(eCapture, DEVICE_STATE_ACTIVE, &pCollection);
						if (SUCCEEDED(hr) && pCollection != nullptr)
						{
							UINT count = 0;
							pCollection->GetCount(&count);
							for (UINT i = 0; i < count; ++i)
							{
								IMMDevice* pDevice = nullptr;
								if (SUCCEEDED(pCollection->Item(i, &pDevice)) && pDevice != nullptr)
								{
									LPWSTR pwszId = nullptr;
									if (SUCCEEDED(pDevice->GetId(&pwszId)))
									{
										auto id = gcnew System::String(pwszId);
										auto kind = WebRtcNet::Media::MediaDeviceKind::AudioInput;
										auto label = gcnew System::String(L"Audio Input Device");
										auto groupId = gcnew System::String(L"");

										// Try to get friendly name
										IPropertyStore* pProps = nullptr;
										if (SUCCEEDED(pDevice->OpenPropertyStore(STGM_READ, &pProps)) && pProps != nullptr)
										{
											PROPVARIANT varName;
											PropVariantInit(&varName);
											if (SUCCEEDED(pProps->GetValue(PKEY_Device_FriendlyName, &varName)))
											{
												if (varName.vt == VT_LPWSTR)
													label = gcnew System::String(varName.pwszVal);
												PropVariantClear(&varName);
											}
											pProps->Release();
										}

										auto deviceInfo = WebRtcNet::Media::MediaDeviceInfo::Create(id, kind, label, groupId);
										devices->Add(deviceInfo);

										CoTaskMemFree(pwszId);
									}
									pDevice->Release();
								}
							}
							pCollection->Release();
						}
					}

					// Enumerate audio outputs
					{
						IMMDeviceCollection* pCollection = nullptr;
						hr = pEnumerator->EnumAudioEndpoints(eRender, DEVICE_STATE_ACTIVE, &pCollection);
						if (SUCCEEDED(hr) && pCollection != nullptr)
						{
							UINT count = 0;
							pCollection->GetCount(&count);
							for (UINT i = 0; i < count; ++i)
							{
								IMMDevice* pDevice = nullptr;
								if (SUCCEEDED(pCollection->Item(i, &pDevice)) && pDevice != nullptr)
								{
									LPWSTR pwszId = nullptr;
									if (SUCCEEDED(pDevice->GetId(&pwszId)))
									{
										auto id = gcnew System::String(pwszId);
										auto kind = WebRtcNet::Media::MediaDeviceKind::AudioOutput;
										auto label = gcnew System::String(L"Audio Output Device");
										auto groupId = gcnew System::String(L"");

										// Try to get friendly name
										IPropertyStore* pProps = nullptr;
										if (SUCCEEDED(pDevice->OpenPropertyStore(STGM_READ, &pProps)) && pProps != nullptr)
										{
											PROPVARIANT varName;
											PropVariantInit(&varName);
											if (SUCCEEDED(pProps->GetValue(PKEY_Device_FriendlyName, &varName)))
											{
												if (varName.vt == VT_LPWSTR)
													label = gcnew System::String(varName.pwszVal);
												PropVariantClear(&varName);
											}
											pProps->Release();
										}

										auto deviceInfo = WebRtcNet::Media::MediaDeviceInfo::Create(id, kind, label, groupId);
										devices->Add(deviceInfo);

										CoTaskMemFree(pwszId);
									}
									pDevice->Release();
								}
							}
							pCollection->Release();
						}
					}

					pEnumerator->Release();
				}
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
	static List<WebRtcNet::Media::MediaDeviceInfo^>^ EnumerateVideoDevices()
	{
		auto devices = gcnew List<WebRtcNet::Media::MediaDeviceInfo^>();

		try
		{
			// Initialize COM
			HRESULT hr = CoInitializeEx(nullptr, COINIT_MULTITHREADED);
			if (FAILED(hr) && hr != S_FALSE)
				return devices;

			{
				// TODO: Implement video device enumeration using DirectShow or WinRT
				// For now, return empty list - will be expanded in next iteration
			}

			CoUninitialize();
		}
		catch (...)
		{
			// Silently ignore errors
		}

		return devices;
	}

	Task<IEnumerable<WebRtcNet::Media::MediaDeviceInfo^>^>^ MediaDevices::EnumerateDevices()
	{
		try
		{
			auto allDevices = gcnew List<WebRtcNet::Media::MediaDeviceInfo^>();

			// Enumerate audio devices
			auto audioDevices = EnumerateAudioDevices();
			if (audioDevices != nullptr)
				allDevices->AddRange(audioDevices);

			// Enumerate video devices
			auto videoDevices = EnumerateVideoDevices();
			if (videoDevices != nullptr)
				allDevices->AddRange(videoDevices);

			return Task::FromResult<IEnumerable<WebRtcNet::Media::MediaDeviceInfo^>^>(allDevices);
		}
		catch (System::Exception^ ex)
		{
			return Task::FromException<IEnumerable<WebRtcNet::Media::MediaDeviceInfo^>^>(ex);
		}
	}

	WebRtcNet::Media::MediaTrackSupportedConstraints^ MediaDevices::GetSupportedConstraints()
	{
		return gcnew WebRtcNet::Media::MediaTrackSupportedConstraints();
	}

	Task<WebRtcNet::Media::MediaStream^>^ MediaDevices::GetUserMedia(WebRtcNet::Media::MediaStreamConstraints^ constraints)
	{
		try
		{
			// Simplified GetUserMedia: Create default audio/video tracks based on constraints
			// TODO: Implement full constraint validation and device selection
			
			if (constraints == nullptr)
			{
				return Task::FromException<WebRtcNet::Media::MediaStream^>(
					gcnew System::ArgumentNullException("constraints"));
			}

			// For now, create default streams if audio/video are requested
			// This is a simplified MVP implementation
			if (!constraints->Audio && !constraints->Video)
			{
				return Task::FromException<WebRtcNet::Media::MediaStream^>(
					gcnew WebRtcNet::Media::MediaStreamException("At least one of audio or video must be requested."));
			}

			// TODO: Implement actual audio/video source creation using WebRTC APIs
			// This will require access to the native PeerConnectionFactory and audio/video device enumeration
			return Task::FromException<WebRtcNet::Media::MediaStream^>(
				gcnew System::NotImplementedException("GetUserMedia device source creation not yet implemented. Awaiting WebRTC audio/video source API integration."));
		}
		catch (System::Exception^ ex)
		{
			return Task::FromException<WebRtcNet::Media::MediaStream^>(ex);
		}
	}
}
