## Summary

Describe the change and its motivation.

## Change Area (required)

- [ ] API-only managed change (`WebRtcNet.Api` / managed-only behavior)
- [ ] Interop / marshaling / native-boundary change
- [ ] Docs/infra only (no behavior change)

## Required Evidence (for behavioral/API changes)

If this change affects WebRTC or Media Capture behavior, complete all sections below.  
If not applicable, write `N/A - no behavioral change`.

### 1) W3C reference (required when applicable)

- Spec section link(s):
- Relevant requirement quote(s):

### 2) Google source reference (required when applicable)

Provide pinned references: path + revision context (commit SHA or branch/tag).

- Source path(s):
- Revision context (SHA/branch/tag):
- Notes on alignment:

### 3) Intended observable behavior (required when applicable)

Describe externally observable behavior (state transitions, event ordering, timing, errors).

- Behavior summary:
- Why this behavior is correct:

### 4) Divergence from Google reference (required if diverging)

- [ ] No divergence
- [ ] Divergence exists (explain below)
- Divergence details (what differs, why .NET/runtime needs require it, and which web-observable semantics are preserved):

## Test Evidence (required)

### Tests added/updated

- 

### Commands run + results

- 

### Change-area test gate confirmation

- [ ] API-only managed change: ran managed NUnit tests
- [ ] Interop/native-boundary change: ran interop unit tests (when environment available)
- [ ] Required environment unavailable (explain below)

### If environment unavailable (required when checked above)

- Missing environment/dependency:
- What was run instead:
- Residual risk:

## Documentation

- [ ] I updated documentation for externally visible behavior changes.
- [ ] No external docs impact.

## Spec/Crosswalk Review (required for W3C-aligned behavior changes)

- [ ] Reviewed `docs\standards\crosswalk\webrtcnet-api-to-spec.md`
- [ ] Reviewed `docs\standards\specs\index\spec-map.md`
- [ ] Refreshed/checked local spec snapshots (`.\scripts\update-spec-docs.ps1`) when needed
