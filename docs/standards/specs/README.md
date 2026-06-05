# Local Spec Corpus

This directory stores local copies of standards content and derived working artifacts.

## Directory Layout

- `webrtc/`: WebRTC 1.0 specification artifacts.
- `mediacapture/`: Media Capture and Streams artifacts.
- `webrtc-stats/`: WebRTC stats specification artifacts.
- `licenses/`: License and notice files for redistributed content.
- `index/`: Global index, map, and update metadata.

## Canonical File Naming

Use these names consistently:

- `TR-<slug>.html`: Pinned W3C TR snapshot (canonical reference).
	- Example: `TR-webrtc.html`
- `ED-<slug>.html`: Editor's Draft snapshot/mirror (working reference).
	- Example: `ED-webrtc-pc.html`

## Derived Artifact Naming

- `<slug>-summary.md`: Concise human/Copilot-oriented summary.
- `<slug>-idl.webidl`: Extracted IDL blocks for quick lookup.
- `index/spec-map.json`: Machine-readable API-to-spec anchor map.
- `index/spec-map.md`: Human-readable map view.
- `index/update-log.md`: Source and refresh history.

## Phase 1 Baseline

Minimum required corpus:

1. `webrtc/TR-webrtc.html`
2. `mediacapture/TR-mediacapture-streams.html`
3. `webrtc-stats/TR-webrtc-stats.html`

## Refresh Workflow

Use:

```powershell
.\scripts\update-spec-docs.ps1
```

Optional Editor's Draft refresh:

```powershell
.\scripts\update-spec-docs.ps1 -IncludeEditorsDraft
```

Run refresh at least monthly and after W3C-aligned `WebRtcNet.Api` API changes.
