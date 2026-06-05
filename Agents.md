
## 1. Think Before Coding

**Don't assume. Don't hide confusion. Surface tradeoffs.**

Before implementing:
- State your assumptions explicitly. If uncertain, ask.
- If multiple interpretations exist, present them - don't pick silently.
- If a simpler approach exists, say so. Push back when warranted.
- If something is unclear, stop. Name what's confusing. Ask.

## 2. Simplicity First

**Minimum code that solves the problem. Nothing speculative.**

- No features beyond what was asked.
- No abstractions for single-use code.
- No "flexibility" or "configurability" that wasn't requested.
- No error handling for impossible scenarios.
- If you write 200 lines and it could be 50, rewrite it.

Ask yourself: "Would a senior engineer say this is overcomplicated?" If yes, simplify.

## 3. Surgical Changes

**Touch only what you must. Clean up only your own mess.**

When editing existing code:
- Don't "improve" adjacent code, comments, or formatting.
- Don't refactor things that aren't broken.
- Match existing style, even if you'd do it differently.
- If you notice unrelated dead code, mention it - don't delete it.

When your changes create orphans:
- Remove imports/variables/functions that YOUR changes made unused.
- Don't remove pre-existing dead code unless asked, but do surface these orphans to me.

The test: Every changed line should trace directly to the user's request.

## 4. Goal-Driven Execution

**Define success criteria. Loop until verified.**

Transform tasks into verifiable goals:
- "Add validation" → "Write tests for invalid inputs, then make them pass"
- "Fix the bug" → "Write a test that reproduces it, then make it pass"
- "Refactor X" → "Ensure tests pass before and after"

For multi-step tasks, state a brief plan:
```
1. [Step] → verify: [check]
2. [Step] → verify: [check]
3. [Step] → verify: [check]
```

Strong success criteria let you loop independently. Weak criteria ("make it work") require constant clarification.

## 5. Standards and Source Alignment (AI-Specific)

### 5.0 Instruction precedence and scope

- If guidance conflicts, apply this order:
	1. higher-level runtime/system constraints
	2. this `Agents.md`
	3. path- or tool-specific guidance
	4. ad hoc workflow suggestions
- `Agents.md` defines AI execution behavior and guardrails.
- Human contributor workflow requirements live in `CONTRIBUTING.md`.

### 5.1 Authority and implementation order

- **MUST** treat W3C specs as normative intent for behavior and semantics.
- **MUST** treat Google implementations as the primary implementation reference.
- **MUST** use this lookup order when implementing behavior:
	1. `libwebrtc` API/implementation
	2. Blink bindings/behavior glue
	3. Chromium tests and call sites
- **MAY** adapt for .NET/runtime constraints, but adaptations **MUST** be minimal and **MUST NOT** change required observable behavior unless explicitly documented.

### 5.2 Web-compatibility obligations

- Changes **MUST** preserve web-expected behavior across:
	- state machine transitions
	- promise/task resolution timing
	- event ordering
	- error taxonomy
- If exact parity is not feasible, the deviation **MUST** be explicit and justified.

### 5.3 .NET API mapping rules

- Public .NET APIs **MUST** use standard .NET naming/casing.
- XML docs **MUST** map terms back to relevant W3C concepts when names differ.
- Promise-style APIs **MUST** map to `Task`/`ValueTask`.
- Event callbacks **MUST** map to .NET events with `EventArgs`.
- `CancellationToken` **SHOULD** be used where cancellation/timeout semantics apply.
- Legacy callback-style `navigator.getUserMedia(...)` support is **OUT OF SCOPE**.

### 5.4 Media Capture scope for this project

- Media Capture scope and priority policy are defined in `CONTRIBUTING.md`.

### 5.5 Divergence, failures, and traceability

- When diverging from Google reference behavior, changes **MUST** include:
	- upstream Google reference (path + revision context)
	- what differs
	- why .NET/runtime needs require it
	- which web-observable semantics are preserved
	- test evidence for preserved behavior
- Implementations **MUST NOT** use silent no-ops or success-shaped stubs for unsupported features.
- Unsupported features **MUST** fail explicitly.
- Keep concise source intent hints in code/XML docs; keep fuller rationale in PR/issue documentation.