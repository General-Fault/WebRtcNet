#include "pch.h"

#include "VideoCapabilityQuery.h"
#include "Marshaling/MarshalMediaTrackCapabilities.h"

#include <mfapi.h>
#include <mfidl.h>
#include <modules/video_capture/video_capture_factory.h>

#pragma comment(lib, "mf.lib")
#pragma comment(lib, "mfplat.lib")

// MF_DEVSOURCE_ATTRIBUTE_SOURCE_TYPE_VIDCAP_PANEL_INFO and MF_CAMERA_FACING_DIRECTION
// are not present in all Windows SDK versions. Define them manually if needed.
#ifndef MF_DEVSOURCE_ATTRIBUTE_SOURCE_TYPE_VIDCAP_PANEL_INFO
EXTERN_GUID(MF_DEVSOURCE_ATTRIBUTE_SOURCE_TYPE_VIDCAP_PANEL_INFO,
	0xf9e8a569, 0x7f2c, 0x458f, 0xad, 0xcf, 0x38, 0xee, 0x7d, 0x8c, 0x63, 0x42);
#endif

#ifndef MF_CAMERA_FACING_DIRECTION_UNKNOWN
enum MF_CAMERA_FACING_DIRECTION
{
	MF_CAMERA_FACING_DIRECTION_UNKNOWN = 0,
	MF_CAMERA_FACING_DIRECTION_ENVIRONMENT = 1,
	MF_CAMERA_FACING_DIRECTION_USER = 2,
};
#endif

namespace WebRtcInterop::Media
{
	using namespace WebRtcNet;
	using namespace System::Collections::Generic;

	namespace
	{
		void ScanDirectShow(
			String^ deviceId,
			ValueRange<unsigned int>^% width,
			ValueRange<unsigned int>^% height,
			ValueRange<double>^% aspectRatio,
			ValueRange<double>^% frameRate)
		{
			using namespace msclr::interop;

			width = nullptr;
			height = nullptr;
			aspectRatio = nullptr;
			frameRate = nullptr;

			const std::unique_ptr<webrtc::VideoCaptureModule::DeviceInfo> deviceInfo(
				webrtc::VideoCaptureFactory::CreateDeviceInfo());

			const auto nativeDeviceId = marshal_as<std::string>(deviceId);
			const int32_t count = deviceInfo
				? deviceInfo->NumberOfCapabilities(nativeDeviceId.c_str())
				: 0;
			if (count <= 0)
				return;

			int32_t minWidth = INT_MAX;
			int32_t maxWidth = 0;
			int32_t minHeight = INT_MAX;
			int32_t maxHeight = 0;
			int32_t maxFps = 0;
			double minAspect = DBL_MAX;
			double maxAspect = 0.0;

			for (uint32_t i = 0; i < static_cast<uint32_t>(count); ++i)
			{
				webrtc::VideoCaptureCapability cap{};
				if (deviceInfo->GetCapability(nativeDeviceId.c_str(), i, cap) != 0)
					continue;

				if (cap.width > 0)
				{
					if (cap.width < minWidth)
						minWidth = cap.width;
					if (cap.width > maxWidth)
						maxWidth = cap.width;
				}

				if (cap.height > 0)
				{
					if (cap.height < minHeight)
						minHeight = cap.height;
					if (cap.height > maxHeight)
						maxHeight = cap.height;
				}

				if (cap.maxFPS > 0 && cap.maxFPS > maxFps)
					maxFps = cap.maxFPS;

				if (cap.width > 0 && cap.height > 0)
				{
					const auto ar = static_cast<double>(cap.width) / cap.height;
					if (ar < minAspect)
						minAspect = ar;
					if (ar > maxAspect)
						maxAspect = ar;
				}
			}

			if (maxWidth > 0)
			{
				width = MarshalToValueRange<unsigned int>(
					System::Nullable<unsigned int>(static_cast<unsigned int>(minWidth)),
					System::Nullable<unsigned int>(static_cast<unsigned int>(maxWidth)));
			}

			if (maxHeight > 0)
			{
				height = MarshalToValueRange<unsigned int>(
					System::Nullable<unsigned int>(static_cast<unsigned int>(minHeight)),
					System::Nullable<unsigned int>(static_cast<unsigned int>(maxHeight)));
			}

			if (maxFps > 0)
			{
				frameRate = MarshalToValueRange<double>(
					System::Nullable<double>(0.0),
					System::Nullable<double>(static_cast<double>(maxFps)));
			}

			if (maxAspect > 0.0)
			{
				aspectRatio = MarshalToValueRange<double>(
					System::Nullable<double>(minAspect),
					System::Nullable<double>(maxAspect));
			}
		}

		List<VideoFacingModeValue>^ QueryFacingMode(String^ deviceId)
		{
			using namespace msclr::interop;

			bool mfStarted = false;
			IMFAttributes* attributes = nullptr;
			IMFActivate** devices = nullptr;
			UINT32 deviceCount = 0;

			try
			{
				if (FAILED(MFStartup(MF_VERSION, MFSTARTUP_NOSOCKET)))
					return nullptr;
				mfStarted = true;

				if (FAILED(MFCreateAttributes(&attributes, 1)) || attributes == nullptr)
					return nullptr;

				if (FAILED(attributes->SetGUID(
					MF_DEVSOURCE_ATTRIBUTE_SOURCE_TYPE,
					MF_DEVSOURCE_ATTRIBUTE_SOURCE_TYPE_VIDCAP_GUID)))
				{
					return nullptr;
				}

				if (FAILED(MFEnumDeviceSources(attributes, &devices, &deviceCount)))
					return nullptr;

				for (UINT32 i = 0; i < deviceCount; ++i)
				{
					if (devices[i] == nullptr)
						continue;

					WCHAR* link = nullptr;
					UINT32 linkLen = 0;
					if (FAILED(devices[i]->GetAllocatedString(
						MF_DEVSOURCE_ATTRIBUTE_SOURCE_TYPE_VIDCAP_SYMBOLIC_LINK,
						&link,
						&linkLen)))
					{
						continue;
					}

					const auto matches = link != nullptr && String::Equals(
						marshal_as<String^>(link),
						deviceId,
						StringComparison::OrdinalIgnoreCase);
					CoTaskMemFree(link);

					if (!matches)
						continue;

					UINT32 panel = MF_CAMERA_FACING_DIRECTION_UNKNOWN;
					devices[i]->GetUINT32(
						MF_DEVSOURCE_ATTRIBUTE_SOURCE_TYPE_VIDCAP_PANEL_INFO,
						&panel);

					if (panel == MF_CAMERA_FACING_DIRECTION_USER)
					{
						auto modes = gcnew List<VideoFacingModeValue>();
						modes->Add(VideoFacingModeValue(VideoFacingModes::User));
						return modes;
					}

					if (panel == MF_CAMERA_FACING_DIRECTION_ENVIRONMENT)
					{
						auto modes = gcnew List<VideoFacingModeValue>();
						modes->Add(VideoFacingModeValue(VideoFacingModes::Environment));
						return modes;
					}

					return nullptr;
				}

				return nullptr;
			}
			finally
			{
				for (UINT32 i = 0; i < deviceCount; ++i)
					if (devices[i] != nullptr)
						devices[i]->Release();

				CoTaskMemFree(devices);

				if (attributes != nullptr)
					attributes->Release();

				if (mfStarted)
					MFShutdown();
			}
		}
	}

	VideoCapabilityQuery::VideoCapabilityQuery(String^ deviceId)
		: device_id_(deviceId)
	{
	}

	MediaTrackCapabilities^ VideoCapabilityQuery::Query()
	{
		ValueRange<unsigned int>^ width = nullptr;
		ValueRange<unsigned int>^ height = nullptr;
		ValueRange<double>^ aspectRatio = nullptr;
		ValueRange<double>^ frameRate = nullptr;
		ScanDirectShow(device_id_, width, height, aspectRatio, frameRate);

		auto facingMode = QueryFacingMode(device_id_);

		auto resizeMode = gcnew List<VideoResizeModeValue>();
		resizeMode->Add(VideoResizeModeValue(VideoResizeModes::None));
		resizeMode->Add(VideoResizeModeValue(VideoResizeModes::CropAndScale));

		return MediaTrackCapabilities::Create(
			width,
			height,
			aspectRatio,
			frameRate,
			facingMode,
			resizeMode,
			nullptr,
			nullptr,
			nullptr,
			nullptr,
			nullptr,
			nullptr,
			nullptr,
			nullptr,
			device_id_,
			String::Empty);
	}
}
