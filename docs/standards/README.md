# Standards

This folder contains repository-local reference material used to support standards-alignment work in `WebRtcNet.Api`.

## Goals

- Keep canonical standards references local and stable.
- Reduce repeated web-fetching during implementation sessions.
- Provide Copilot-friendly derived artifacts for fast retrieval.

## Structure

- `specs/`: W3C specifications, indexes, and licensing metadata.
- `crosswalk/`: Mapping from WebRtcNet.Api surface area to spec sections.

See `specs/README.md` for naming conventions and artifact expectations.

## Maintenance Triggers

Refresh local spec artifacts when:

1. A month has passed since the last refresh.
2. `WebRtcNet.Api` contracts change in a way that touches W3C-aligned behavior.
3. A chunk explicitly calls for standards parity review (for example, issue #14 follow-through).
