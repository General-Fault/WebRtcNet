#include "stdafx.h"

#include "CreateSessionDescriptionObserver.h"

#include "..\Marshaling\MarshalPeerConnection.h"

#include "msclr\marshal_cppstd.h"
using namespace msclr;

WebRtcObservers_Start

CreateSessionDescriptionObserver::CreateSessionDescriptionObserver()
	: _task(gcnew TaskCompletionSource<WebRtcNet::RtcSessionDescription>())
{
}

CreateSessionDescriptionObserver::~CreateSessionDescriptionObserver()
{
	_task->TrySetCanceled();
}

gcroot<Task<WebRtcNet::RtcSessionDescription> ^> CreateSessionDescriptionObserver::CreateSessionTask()
{
	return _task->Task;
}


void CreateSessionDescriptionObserver::OnSuccess(webrtc::SessionDescriptionInterface * desc)
{
	auto sessionDescription = marshal_as<WebRtcNet::RtcSessionDescription>(desc);
	_task->SetResult(sessionDescription);
}

void CreateSessionDescriptionObserver::OnFailure(const std::string & error)
{
	auto exception = gcnew WebRtcNet::CreateSessionDescriptionFailue(marshal_as<System::String^>(error));
	_task->SetException(exception);
}

WebRtcObservers_End