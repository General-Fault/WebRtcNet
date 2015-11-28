// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently,
// but are changed infrequently

#pragma once

#include <string>
#include <msclr\marshal.h>
#include <msclr\marshal_cppstd.h>
using namespace msclr::interop;

#define ManagedRtc_Start namespace WebRtcInterop { namespace Rtc {
#define managed_rtc WebRtcInterop::Rtc
#define ManagedRtc_End }}


#define ManagedCricket_Start namespace WebRtcInterop { namespace Cricket {
#define managed_cricket WebRtcInterop::Cricket
#define ManagedCricket_End }}


#define WebRtcObservers_Start namespace WebRtcInterop { namespace Observers {
#define webrtc_observers WebRtcInterop::Observers
#define WebRtcObservers_End }}

#define WebRtcAdapters_Start namespace WebRtcInterop { namespace Adapters {
#define webrtc_adapters WebRtcInterop::Adapters
#define WebRtcAdapters_End }}

#define WebRtcHandles_Start namespace WebRtcInterop { namespace Handles {
#define webrtc_handles WebRtcInterop::Handles
#define WebRtcHandles_End }}
