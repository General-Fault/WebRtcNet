---
name: w3c-specs
description: Reviews WebRtcNet's local W3C spec corpus and produces implementation guidance with citations. Use when the user asks to compare or validate WebRtcNet.Api behavior against local spec files, requests gap analysis, or wants chunk-focused standards review using documents/specs and documents/crosswalk.
---

# W3C Specs

## Quick start

Use this skill when the prompt provides:

1. **Focus area** (what to inspect)
2. **Expected output** (what to do with findings)

Example input:

`Focus: MediaTrackConstraints exact/ideal parity. Output: list API gaps + concrete code change checklist with file paths.`

## Workflow

1. Read local sources first, in this order:
	1. `documents/specs/index/spec-map.md` (or `.json`)
	2. `documents/crosswalk/webrtcnet-api-to-spec.md`
	3. Relevant TR snapshots in `documents/specs/**/TR-*.html`
	4. Relevant summaries and `*-idl.webidl` files
2. Restrict analysis to the requested focus area.
3. Map findings to concrete repository targets (`WebRtcNet.Api/**`, tests, docs) when requested.
4. Treat local corpus as source of truth unless a section is missing.

## Output contract

Always return:

1. **Findings** tied to spec anchors and local file paths.
2. **Gaps/Drift** between current API and spec intent.
3. **Actionable next steps** matching the requested output type.
4. **Citations** for substantive claims.

## Citation rules

- Prefer local file citations first.
- Include anchor IDs when available (example: `#dom-rtcpeerconnection`).
- If a required section is missing locally, state that explicitly before using external sources.

## Boundaries

- Do not broaden scope beyond the prompt focus.
- Do not rewrite standards text; summarize and map to implementation impact.
- Keep C# adaptation notes explicit where WebIDL/JS semantics differ.
