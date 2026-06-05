# Spec Index Artifacts

This folder contains cross-spec metadata.

Canonical files:

- `spec-map.json`: Machine-readable map from API symbols to spec anchors.
- `spec-map.md`: Human-readable map (derived from `spec-map.json`).
- `update-log.md`: Refresh history and source metadata.

## Source-of-truth policy

- `spec-map.json` is the source of truth for API symbol to W3C anchor mapping.
- `spec-map.md` should be kept in sync with `spec-map.json`.
- `documents\crosswalk\webrtcnet-api-to-spec.md` captures implementation status and adaptation notes.

## Status vocabulary

Use one of the following values for mapping and implementation status:

- `seeded`
- `partial`
- `implemented`
- `todo`
