# Media Capture and Streams Summary (TR)

Source: <https://www.w3.org/TR/mediacapture-streams/>

## High-Value Areas for WebRtcNet.Api

1. `MediaDevices` acquisition model (`getUserMedia`, `getDisplayMedia`, enumeration/events).
2. `MediaStream` and `MediaStreamTrack` lifecycle and event model.
3. Constraints pipeline:
	- `MediaTrackConstraints`
	- `MediaTrackConstraintSet`
	- `Constrain*` value shapes (`exact` / `ideal`)
4. Capabilities/settings dictionaries used for `getCapabilities` / `getSettings`.
5. Constraint processing rules (fitness distance + apply flow).

## Key Anchors

- `#dom-mediadevices-getusermedia`
- `#dom-mediastreamtrack-applyconstraints`
- `#dom-mediatrackconstraints`
- `#dom-mediatrackconstraintset`
- `#dom-mediatrackcapabilities`
- `#dom-mediatracksettings`
- `#constraints`
