# WebRtcNet
A .Net implementation of the WebRTC standard built using the WebRTC Project native client. This was originally developed as the Google PeerConnection native API. WebRtcNet is not endorsed by or affiliated with Google or the WebRTC Project in any way.

See the WebRTC 1.0: Real-time Communication Between Browsers - W3C Recommendation 26 January 2021 for more documentation on the WebRtc interface. 
https://www.w3.org/TR/webrtc/

If building WebRtcNet, you will need to fetch and build the WebRTC native client. Information on how to do that can be found here.
https://webrtc.googlesource.com/src/+/main/docs/native-code/index.md

WebRtcNet currently uses WebRTC branch 5112 (refs/branch-heads/5112) which corosponds to Chromium milestone 104 (see https://chromiumdash.appspot.com/branches)
https://groups.google.com/g/discuss-webrtc/c/Zrsn2hi8FV0/m/KIbn0EZPBQAJ


It is highly recomended that you follow the instructions provided by google - they have changed several times since this project was created. However the short version is this:

Install Visual Studio 2019 or later (The free Community Edition is sufficient)
	 Install the “Desktop development with C++” component and the “MFC/ATL support” sub-components
	 Install the 10.0.20348.0 Windows 10 SDK

Download and install Chromium deopt_tools See "https://commondatastorage.googleapis.com/chrome-infra-docs/flat/depot_tools/docs/html/depot_tools_tutorial.html" for details.

Set up your git client:
Start cmd and execute:

	gclient
	git config --global user.name "My Name"
	git config --global user.email "name@email"
	git config --global core.autocrlf false
	git config --global core.filemode false
	git config --global color.ui true

	git config branch.autosetupmerge always
	git config branch.autosetuprebase always

Download the WebRtc source.

	cd [WebRtcNet Working Directory]\third-party\google\WebRtc
	fetch --nohooks webrtc
	git checkout -b webrtcnet_104 refs/remotes/branch-heads/5112
	gclient sync

This may take some time to complete. The sync command downloads the WebRtc native client from https://chromium.googlesource.com/external/webrtc.git including much of the chromium tree https://chromium.googlesource.com/chromium/src.git. These add up to nearly 10 GB.

Custom build steps in the WebRtcInterop projects will execute the "gn" code generation and execute ninja to build the webrtc dependencies in the required locations.
