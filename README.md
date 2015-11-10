# WebRtcNet
A .Net implementation of the WebRTC standard built using the WebRTC Project native client. This was originally developed as the Google PeerConnection native API. WebRtcNet is not endorsed by or affiliated with Google or the WebRTC Project in any way.

See the WebRTC 1.0: Real-time Communication Between Browsers - W3C Editor's Draft 06 October 2015 for more documentation on the WebRtc interface. 
http://w3c.github.io/webrtc-pc/

If building WebRtcNet, you will need to fetch and build the WebRTC native client. Information on how to do that can be found here.
http://www.webrtc.org/native-code/development

The short version is this:

Install the Windows10 SDK
Install Visual Studio 2015 (The free Community Edition is sufficient)

Download and install Chromium deopt_tools See "http://dev.chromium.org/developers/how-tos/install-depot-tools" for details.
	Download from "https://src.chromium.org/svn/trunk/tools/depot_tools.zip". 
	Extract depot_tools to [Source Directory]\Google\depot_tools
	Add environment variable for the Google depot tools path. 
		GOOGLE_DEPOT_TOOLS=[Source Directory]\Google\depot_tools
	Add depot_tools to your environment path variable. Append ";GOOGLE_DEPOT_TOOLS%" (without quotes) to the path variable. 

Set up your git client:
Start cmd and execute:

	gclient.bat
	git config --global user.name "My Name"
	git config --global user.email "name@email"
	git config --global core.autocrlf false
	git config --global core.filemode false
	git config --global branch.autosetuprebase always
	git config --global color.ui true


Set required environment variables for this build.

	set DEPOT_TOOLS_WIN_TOOLCHAIN=0
	set GYP_DEFINES=component=shared_library
	set GYP_GENERATORS=ninja,msvs-ninja
	set GYP_MSVS_VERSION=2015

Download the WebRtc source.

	mkdir [WebRtc Working Directory]\src\Google\src
	cd [WebRtc Working Directory]\src\Google\src
	fetch webrtc

To build:
	ninja -C out\Debug

	
As of this time , WebRtc does not build with Visual Studio 2015. There are a small handful of changes that you will need to make 

gflags changes: https://codereview.appspot.com/270690043/
These will not be necessary when issue https://code.google.com/p/webrtc/issues/detail?id=5185 is fixed.

webrtc changes: https://codereview.webrtc.org/1412653006
These will not be necessary when issue https://code.google.com/p/webrtc/issues/detail?id=5183 is fixed.