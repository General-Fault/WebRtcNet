# Contributing to WebRtcNet

This document defines contributor workflow requirements for this repository.

## 1. Behavioral alignment requirements

When a change affects WebRTC or Media Capture behavior, contributors **MUST**:

1. Reference the relevant W3C requirement(s).
2. Reference upstream Google implementation behavior.
3. Demonstrate expected observable behavior in tests.

Upstream references **MUST** be pinned to a revision in PR/issue notes (path + commit/branch context) so behavior can be re-validated later.

## 2. Required PR evidence block

PR descriptions for behavioral changes **MUST** include:

1. **W3C reference**: section link(s)
2. **Google source reference**: libwebrtc/Blink/Chromium path(s) and revision context
3. **Behavior summary**: what observable behavior is intended and why
4. **Test evidence**: tests added/updated and what they prove

For `WebRtcNet.Api` behavior changes, PRs **MUST** also update or validate:

- `documents\specs\index\spec-map.json` (canonical API symbol to spec anchor map)
- `documents\specs\index\spec-map.md` (human-readable mirror)
- `documents\crosswalk\webrtcnet-api-to-spec.md` (implementation status/adaptation notes)

## 3. Test gates by change area

- API-only managed changes **MUST** run managed NUnit tests.
- Interop/marshaling/native-boundary changes **MUST** also run interop unit tests when environment support is available.
- If a required environment is unavailable, PRs **MUST** explicitly call that out and identify what was run instead.

## 4. Scope priorities for Media Capture

- Prioritize WebRTC-critical Media Capture functionality first.
- `P0`: `getUserMedia`, track lifecycle/events, and constraints needed for call setup quality.
- `P1`: device enumeration/change and high-value constraints/settings (including `backgroundBlur`).
- Legacy callback `navigator.getUserMedia(success, error)` is out of scope.
- Extended constraints beyond immediate needs are in near-term scope and should be added incrementally.
