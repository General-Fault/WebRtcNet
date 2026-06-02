# BasicVideoChat

A peer-to-peer audio/video call example using the **WebRtcNet** API.

Two computers connect directly over TCP — no messaging server required. One peer acts
as the **Host** (listens for an incoming connection) and the other is the **Guest**
(dials by IP and port).

---

## Prerequisites

- Windows 10 or later
- .NET 10.0 **or** .NET Framework 4.8
- A built copy of `WebRtcInterop` (requires WebRTC native libraries — see the
  [repository README](../../README.md) for setup)

> **Note:** Because several `WebRtcInterop` methods are not yet implemented,
> this application will not run end-to-end today. It is intended as a working
> specification of the API surface that still needs to be wired up in the
> interop layer.

---

## How to run

### Host side

1. Launch the application.
2. Leave the **Host** radio button selected.
3. Set the **Port** (default: `7777`).
4. Click **Connect** — the app waits for a guest.

### Guest side

1. Launch the application on a second machine.
2. Select the **Guest** radio button.
3. Enter the **IP address** of the Host and match the **Port**.
4. Click **Connect**.

---

## ICE / STUN configuration

The application uses Google's public STUN server by default:

```
stun:stun.l.google.com:19302
```

This is hardcoded in `MainWindow.xaml.cs` as `DefaultConfiguration`. For LAN-only
use you can replace it with an empty `RtcConfiguration()`. For calls across NAT you
may want to add a TURN server. See the
[WebRTC getting-started guide](https://webrtc.org/getting-started/turn-server) for
details on running your own TURN server.

---

## Features

| Feature | Status |
|---|---|
| Audio + Video | Pending interop |
| Mute audio toggle | Pending interop |
| Camera off toggle | Pending interop |
| Direct TCP signaling (no server) | Implemented |
| ICE/STUN support | Implemented |

---

## Architecture notes

- **Signaling**: Newline-delimited JSON over a single persistent TCP connection.
  `TcpSignalingChannel` serialises/deserialises `SignalingMessage` objects
  (`Offer`, `Answer`, `Candidate`, `Bye`).
- **ICE candidate ordering**: The signaling read loop awaits the message handler
  before reading the next message, so `SetRemoteDescription` is always guaranteed
  to complete before any `AddIceCandidate` arrives.
- **WpfVideoRenderer**: A prototype `VideoRenderer` implementation using
  `WriteableBitmap`. Proper renderer assemblies for WPF, Windows Forms, and WinUI
  are tracked in [issue #36](https://github.com/General-Fault/WebRtcNet/issues/36).
