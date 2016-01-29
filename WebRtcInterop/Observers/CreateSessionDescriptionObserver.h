#pragma once

#include "talk\app\webrtc\jsep.h"

#include <msclr\auto_gcroot.h>

using namespace System::Threading::Tasks;

WebRtcObservers_Start

private class CreateSessionDescriptionObserver : public webrtc::CreateSessionDescriptionObserver
{
public:
	~CreateSessionDescriptionObserver();

	// Inherited via CreateSessionDescriptionObserver
	virtual void OnSuccess(webrtc::SessionDescriptionInterface * desc) override;
	virtual void OnFailure(const std::string & error) override;

	CreateSessionDescriptionObserver();

	gcroot<Task<WebRtcNet::RtcSessionDescription> ^> CreateSessionTask();

private:
	gcroot<TaskCompletionSource<WebRtcNet::RtcSessionDescription> ^> _task;
};

WebRtcObservers_End