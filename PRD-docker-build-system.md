# PRD: Docker-Based Build System for WebRtcNet

## Problem Statement

Currently, building WebRtcNet requires:
- **2+ hours of setup** on a Windows machine (installing Visual Studio, Windows SDK, depot_tools, fetching ~10GB of WebRTC source)
- **60-90 minute rebuild cycles** (each build fetches, generates, and compiles WebRTC native dependencies)
- **High barrier to entry** for new contributors or developers without Windows machines
- **Expensive CI/CD costs** (~$60/year rebuilding WebRTC on every commit)
- **No support for Codespaces** (impossible to develop in cloud environment)

These friction points discourage community contributions, increase time-to-first-build for new developers, and waste money rebuilding the same WebRTC artifacts repeatedly.

## Solution

Implement a **two-tier Docker-based build system** that separates WebRTC compilation (expensive, infrequent) from WebRtcNet compilation (cheap, frequent):

1. **Tier 1: WebRTC Builder Image** (published monthly or on-demand)
   - Builds native WebRTC library once, publishes to GitHub Container Registry
   - Cost: $0.45 per build (45 minutes on GitHub Actions Windows runner)
   - Artifact size: ~250 MB (webrtc.lib + headers for x86/x64 debug/release)
   - Reusable by any downstream project

2. **Tier 2: WebRtcNet Builder Image** (per-commit in CI/CD, or developer machines)
   - Uses published WebRTC image as base layer
   - Builds only C++/CLI interop + managed C# code
   - Cost: $0.05-0.10 per build (10 minutes, mostly layer cache hits)
   - Enables Codespaces development (15-20 minute full build)

### Expected Outcomes

- **Developer setup**: 10 minutes (pull Docker image) vs. 2+ hours
- **Local rebuild**: 15 minutes vs. 60+ minutes (6-9x faster)
- **CI costs**: $1.45/month vs. $5.00/month (71% savings)
- **Codespaces support**: ✅ Now viable for full development
- **New contributors**: Can build on Mac/Linux/Windows (Docker only)

---

## User Stories

1. As a **new contributor cloning the repo**, I want to build WebRtcNet with a single command, so that I don't spend 2 hours setting up my dev machine

2. As a **Windows developer**, I want to rebuild WebRtcNet in 15 minutes instead of 60+, so that I can iterate faster on managed code changes

3. As a **project maintainer**, I want to reduce CI/CD costs, so that I can reinvest savings into other infrastructure

4. As a **developer on Mac/Linux**, I want to develop WebRtcNet locally without a Windows machine, so that I can contribute from any platform

5. As a **GitHub Codespaces user**, I want to open a Codespace and have a fully functioning build environment in 15-20 minutes, so that I can code entirely in the browser

6. As a **CI/CD pipeline owner**, I want WebRTC to build once monthly and be reused across hundreds of builds, so that I don't waste compute time on redundant compilation

7. As a **NuGet package maintainer**, I want to publish WebRtcNet packages directly from the build artifacts, so that the release process is automated and reproducible

8. As a **security-conscious developer**, I want to verify build artifacts are bit-reproducible from published Docker images, so that I can trust binaries haven't been tampered with

9. As a **team lead**, I want to onboard new developers in 15 minutes (pull image, build, done), so that time-to-first-PR is minimized

10. As a **enterprise user**, I want to cache Docker images in our private registry, so that developers don't re-download WebRTC artifacts across multiple machines

11. As a **release engineer**, I want separate Docker images for different WebRTC versions (7778, main branch), so that we can support multiple versions simultaneously

12. As a **developer in a bandwidth-constrained environment**, I want to build WebRtcNet from a cached Docker layer instead of downloading 10GB each time, so that I can develop reliably on slow connections

13. As a **CI/CD platform maintainer**, I want a single GitHub Actions workflow that detects changes and only rebuilds what's necessary, so that I don't trigger expensive WebRTC builds on managed code-only changes

14. As a **open-source contributor**, I want the build system documented clearly in CONTRIBUTING.md, so that I know exactly how to build locally vs. in Docker vs. in Codespaces

15. As a **quality assurance engineer**, I want automated test execution inside Docker containers, so that build results are reproducible across developers and CI platforms

16. As a **DevOps engineer**, I want to set up image retention policies and automatic cleanup, so that GHCR storage costs remain predictable and minimal

17. As a **compliance officer**, I want build artifacts traceable to exact source commits, so that we can audit and verify what went into each release

18. As a **international developer**, I want to use Codespaces without regional bandwidth restrictions, so that build times are consistent regardless of location

19. As a **Windows Subsystem for Linux (WSL) user**, I want to build WebRtcNet using Linux Docker images (via WSL 2), so that I can work in a familiar Linux environment

20. As a **project contributor**, I want clear documentation of Docker image versioning strategy, so that I understand which WebRTC version each build uses

---

## Implementation Decisions

### Module Structure

The implementation spans three key areas:

1. **Build Orchestration Layer** (new)
   - GitHub Actions workflows to detect changes and coordinate multi-tier builds
   - Decides whether to trigger expensive WebRTC build or fast WebRtcNet build based on file paths
   - Manages image tagging, retention policies, and artifact publication

2. **WebRTC Builder Module** (new)
   - Multi-stage Dockerfile that compiles WebRTC from source
   - Builds all four platform configurations (Debug/Release × x86/x64)
   - Publishes minimal artifact image to GHCR (~250 MB)
   - Runs on schedule (monthly) or manual trigger

3. **WebRtcNet Builder Module** (modified)
   - Updated Dockerfile to inherit from published WebRTC image
   - Layers only C++/CLI interop + managed C# build on top
   - Supports both local development (docker build) and Codespaces integration
   - Extracts NuGet packages as artifacts for downstream publication

### Architecture Decisions

- **Container Registry**: Use GitHub Container Registry (GHCR) as primary registry
  - Rationale: Integrated with GitHub Actions, free tier covers expected usage
  - Alternative considered: Docker Hub (discoverability, but additional cost)
  - Decision: GHCR as primary; can mirror to Docker Hub later if needed

- **Build Caching Strategy**: Use Docker BuildKit with inline caching
  - Rationale: Reduces rebuild time by caching layers aggressively
  - Enables faster iteration on both WebRTC and WebRtcNet

- **Image Tagging Scheme**:
  - WebRTC: `ghcr.io/general-fault/webrtc:7778-artifacts-full`, `ghcr.io/general-fault/webrtc:latest`
  - WebRtcNet: `ghcr.io/general-fault/webrtcnet:latest`, `ghcr.io/general-fault/webrtcnet:{version}`, `ghcr.io/general-fault/webrtcnet:{commit-sha}`
  - Rationale: Multiple tags enable both "latest" fast updates and reproducible pinned versions

- **Windows vs. Linux Container Decision**:
  - Use Windows Server 2022 containers (ltsc2022) for both tiers
  - Rationale: WebRTC build requires MSVC and Windows SDK; cannot cross-compile from Linux
  - Accept storage overhead; GHCR supports Windows images via foreign layers

- **Minimal vs. Full Artifact Set**:
  - Publish "artifacts-full" image with all 4 configurations (Debug/Release × x86/x64)
  - Also maintain "artifacts-minimal" with only Release/x64 for lean use cases
  - Rationale: Full set supports both development and production; minimal for bandwidth-constrained scenarios

- **Incremental WebRTC Updates**:
  - Rebuild WebRTC monthly on schedule, or on-demand if submodule pointer changes
  - Do NOT rebuild if only managed code (WebRtcNet/) changes
  - Rationale: WebRTC is stable branch (7778, Chrome M148); monthly refresh sufficient for security patches

### Interfaces and Contracts

**GitHub Actions Workflow Contract**:
- Input: Commit with changes to WebRtcNet/* or WebRtcInterop/* or Dockerfile.webrtcnet
- Process: Detect changes, skip WebRTC build, fast WebRtcNet build
- Output: Publish `ghcr.io/general-fault/webrtcnet:{sha}` and `ghcr.io/general-fault/webrtcnet:latest`

**WebRTC Image Contract**:
- Provides: `/artifacts/out/Release_x64/obj/webrtc.lib`, `/artifacts/out/Release_Win32/obj/webrtc.lib`, etc., plus `/artifacts/src/` (headers)
- Guarantees: Built from branch-heads/7778, all 4 configurations included, ready for immediate linking
- Stability: Image tag immutable; rebuilds on new tag only

**WebRtcNet Build Input**:
- `ARG WEBRTC_IMAGE=ghcr.io/general-fault/webrtc:latest`
- Copies `/artifacts` from WebRTC image into `C:\opt\webrtc`
- References via environment variables in MSBuild (`WEBRTC_SRC_PATH`, etc.)

**NuGet Artifact Output**:
- Extracts `.nupkg` files from `/out/nuget/` in WebRtcNet container
- Published to NuGet.org if desired (optional, requires NUGET_API_KEY secret)

### Environment Variables & Configuration

**In Dockerfile.webrtc**:
- `WEBRTC_BRANCH`: refs/branch-heads/7778 (hardcoded, can parameterize later)
- `GN_ARGS`: Per-configuration parameters (target_cpu, is_debug, optimization flags)

**In Dockerfile.webrtcnet**:
- `WEBRTC_IMAGE`: Pulled from GitHub Actions ARG, defaults to `ghcr.io/general-fault/webrtc:latest`
- `WEBRTC_SRC_PATH`: `/opt/webrtc/src` (where WebRTC headers copied)
- `WEBRTC_OUT_PATH`: `/opt/webrtc/out` (where webrtc.lib artifacts copied)

**GitHub Actions Secrets**:
- `NUGET_API_KEY`: Optional, for publishing packages to NuGet.org

### CI/CD Workflow Design

**Workflow 1: Build WebRTC (monthly + manual trigger)**
- Triggered: First of month at 2 AM UTC, or `workflow_dispatch` manual button
- Runs on: `windows-2022` GitHub Actions runner
- Steps: Checkout (with submodules), Docker build, push to GHCR
- Time: ~45 minutes
- Cost: $0.45 per build

**Workflow 2: Detect Changes (every push)**
- Triggered: Every push to main/develop
- Runs on: `ubuntu-latest` (cheap, fast)
- Detects if WebRTC-related files changed vs. WebRtcNet-only changes
- Outputs: Boolean flags (webrtc-changed, webrtcnet-changed)

**Workflow 3: Build WebRtcNet (per-commit, conditional)**
- Triggered: After Workflow 2 if WebRtcNet code changed
- Depends on: Workflow 1 (or uses published image if already available)
- Runs on: `windows-2022`
- Steps: Checkout, Docker pull (cached), Docker build, push to GHCR, extract NuGet packages
- Time: ~10-15 minutes
- Cost: $0.10-0.15 per build

**Failure Handling**:
- If WebRTC build fails: Notify via GitHub, do NOT publish broken image
- If WebRtcNet build fails: Only WebRtcNet build marked failed; previous WebRTC image still valid for next attempt
- Retry logic: Workflows can be manually re-triggered

### Local Development Integration

**For Docker Desktop users (Windows/Mac/Linux)**:
```bash
# One-time: Pull WebRTC base image
docker pull ghcr.io/general-fault/webrtc:7778-artifacts-full

# Build WebRtcNet locally
docker build -f Dockerfile.webrtcnet \
  --build-arg WEBRTC_IMAGE=ghcr.io/general-fault/webrtc:7778-artifacts-full \
  -t webrtcnet:local .

# Extract compiled binaries
docker run --rm -v ./output:C:/output webrtcnet:local powershell -Command "Copy-Item C:/app/* C:/output/"
```

**For GitHub Codespaces**:
- Codespaces machine downloads and caches Docker images
- User runs same `docker build` command as above
- Build completes in ~15-20 minutes
- Compiled WebRtcNet DLLs available for testing

**For native Windows development** (optional, not requiring Docker):
- README documents traditional setup path (Visual Studio, depot_tools, etc.)
- Docker approach is alternative, not replacement
- Both paths supported for backward compatibility

### Documentation Updates

1. **README.md**: Add "Quick Start with Docker" section above manual setup instructions
2. **CONTRIBUTING.md**: Create with three paths (Docker, Local Windows, Codespaces)
3. **.github/DOCKER.md**: Detailed Docker image versioning, retention policies
4. **Dockerfile.webrtc** and **Dockerfile.webrtcnet**: Inline comments explaining each stage

---

## Testing Decisions

### Testing Strategy

Good tests for this system verify:
- ✅ **Published Docker images contain expected artifacts** (webrtc.lib exists, headers accessible)
- ✅ **WebRtcNet can successfully link against WebRTC artifacts** (actual compilation succeeds)
- ✅ **Compiled binaries are executable and pass unit tests** (functional verification)
- ❌ **NOT** internal Dockerfile implementation details (e.g., exact layer count, specific RUN commands)

### Modules to Test

1. **WebRTC Artifact Image (Dockerfile.webrtc)**
   - Test: Build image, verify 4 `webrtc.lib` files exist in expected paths
   - Test: Verify all public headers are readable from `/artifacts/src/`
   - Test: Run WebRTC's own `webrtc_lib_link_test` if available

2. **WebRtcNet Builder Image (Dockerfile.webrtcnet)**
   - Test: Pull WebRTC image, build WebRtcNet image
   - Test: Extract NuGet packages from image, verify `.nupkg` files exist
   - Test: Run WebRtcNet unit tests inside container

3. **GitHub Actions Workflows**
   - Test: Trigger change-detection workflow with WebRTC-only changes → should skip WebRTC build
   - Test: Trigger change-detection workflow with WebRtcNet-only changes → should skip WebRTC build
   - Test: Trigger change-detection workflow with both changes → should trigger both builds

### Prior Art

- Similar multi-stage Docker tests in Chromium CI (builds toolchain, tests against it)
- GitHub Actions workflows tested by dry-run (workflow_dispatch with explicit flags)
- Unit tests inside containers (existing WebRtcNet test structure, runs in container context)

### Test Execution Plan

**Phase 1**: Manual testing by maintainer
- Build WebRTC image locally, verify artifacts
- Build WebRtcNet image using WebRTC base, verify linking succeeds
- Extract and inspect NuGet packages

**Phase 2**: Automated in GitHub Actions
- Add test jobs to workflows that verify published images
- Run WebRtcNet unit tests inside container to ensure no regressions

**Phase 3**: Developer validation
- Solicit feedback from new contributors using Docker approach
- Measure time-to-first-build; target < 20 minutes

---

## Out of Scope

1. **GPU support in Docker images** — WebRTC optional GPU codecs out-of-scope; can add later if needed
2. **Cross-compilation from Linux** — Windows-only WebRtcNet for now; future work to support .NET on Linux would enable this
3. **Publishing WebRTC images to Docker Hub** — GHCR sufficient; Docker Hub mirror optional future work
4. **Automatic image retention policies** — Manual deletion for now; can automate later with `ghcr.io` API
5. **Pre-built NuGet packages on NuGet.org** — Out-of-scope; CONTRIBUTING.md documents manual publish if desired
6. **Arm/ARM64 support** — Current WebRtcNet has no GN configuration for these; defer to future work
7. **Other build systems (CMake, Bazel)** — GN/Ninja required by WebRTC; no abstraction needed
8. **macOS/iOS WebRTC targets** — Windows Desktop only for now
9. **Legacy .NET Framework (< 4.7)** — Supporting .NET 6+ and .NET Framework 4.7+; older versions out-of-scope
10. **Real-time monitoring dashboards** — Cost tracking and performance metrics; future infrastructure work

---

## Further Notes

### Cost Savings Validation

Baseline scenario: 10 WebRtcNet builds/month (active development)
- **Single-container approach**: 10 builds × 45 min × $0.010/min = $4.50/month
- **Two-container approach**: 1 WebRTC build (45 min) + 10 WebRtcNet builds (10 min each)
  - Cost: (45 × $0.010) + (100 × $0.010) + storage (~$0.07) = ~$1.52/month
  - **Savings: 66% ($1.45 vs. $4.50)**

Enterprise scenario: 50 builds/month
- **Single**: $22.50/month
- **Two-container**: ~$5.40/month
- **Annual savings**: $207

### Potential Blockers & Mitigations

1. **GHCR Windows image size (~5 GB)** → Acceptable; mirrors typical Windows base image footprint
2. **First docker pull takes time** → Expected; cached on subsequent pulls; document in CONTRIBUTING.md
3. **GitHub Actions Windows runners expensive** → Offset by 78% savings on WebRtcNet builds per month
4. **Developers unfamiliar with Docker** → Provide copy-paste commands in CONTRIBUTING.md
5. **Windows Firewall/antivirus interference** → Users can use traditional setup as fallback

### Future Enhancement Opportunities

1. **Self-hosted runners** — For teams, set up on-premises Windows runner to save costs further
2. **Image mirror to Docker Hub** — Improve discoverability for open-source community
3. **ARM64 support** — Add GN args and Azure Pipelines ARM runner when Windows ARM builds are tested
4. **Automatic dependency updates** — Monitor WebRTC branch for new patches, auto-build and notify
5. **Build time analytics** — Track trends in build times; identify optimization opportunities
6. **Multi-version support** — Maintain pre-built images for 7778 (current stable) and main branch in parallel

### Success Metrics

- Time-to-first-build for new contributors: < 20 minutes (target)
- Local rebuild time (after dependencies cached): < 15 minutes (target)
- CI cost per WebRtcNet build: < $0.15 (target)
- Codespaces build success rate: > 95% (target)
- New contributor onboarding: 1-2 hours saved per person (estimate)

