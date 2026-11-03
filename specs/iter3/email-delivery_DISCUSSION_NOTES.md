# Email delivery discussion notes

## 2026-11-03 — Initial scope and first round

The user requested a grill-with-docs interview about email delivery, starting with `IEmailService` and an in-memory implementation in tests. Recorded the explicitly requested boundary as [D1](email-delivery.DECISIONS.md#d1--email-service-boundary-and-test-implementation).

Iteration 2 defines `INotifyService.NotifyAsync(User, Event)` returning `Task`, without a production implementation. Rule matching happens outside that service. Event title and body are forwarded unchanged; successful completion means service-call completion rather than confirmed receipt. Earlier plans assigned real email/Slack providers to iteration 3; the user's current request narrows this email increment to an interface and test implementation.

First-round proposals, pending approval:

- Q1: Give the email boundary explicit recipient address, subject, and body inputs. Channel selection and rule matching remain outside it; map event title/body to subject/body unchanged.
- Q2: Limit this increment to the email contract and test implementation. Discuss the concrete notification service and its connection to email separately.

Next: Resolve these independent scope and responsibility questions, then discuss the dependent method contract, completion/failure semantics, and verification boundary. Repository fact finding is in progress. No production code or tests changed.

## 2026-11-03 — First-round approvals and second round

The user answered "both yes", approving explicit email inputs ([D2](email-delivery.DECISIONS.md#d2--explicit-email-inputs)) and deferring notification-service integration to a separate increment ([D3](email-delivery.DECISIONS.md#d3--separate-email-contract-from-notification-service-integration)).

Repository fact finding confirmed that `User.Email` currently has no validation; iteration 1 still tracks this as pending. Event title/body validation exists and preserves valid content unchanged. Existing notification tests use private recording/failing/blocking test doubles. There is no production notification-service implementation or DI registration to extend. No new glossary term or ADR is needed for the approved boundary: the existing Delivery attempt term already covers an attempt through email.

Second-round proposals, pending approval:

- Q3: Use `Task SendAsync(string recipientEmail, string subject, string body)`. Completion means the implementation accepted/completed the send request, not confirmed inbox receipt; failures use exceptions, with no retry in this increment.
- Q4: Defer invalid-address and blank-content validation for this contract-only increment. The recording test implementation accepts and records supplied values unchanged; this does not assume that `User.Email` is validated. Revisit validation when introducing the production implementation.

Next: Resolve method semantics and validation scope, then settle the test implementation's verification boundary. No production code or tests changed; no checks run.

## 2026-11-03 — Second-round approvals and verification boundary

The user answered "both yes" to Q3 and Q4. Recorded the asynchronous send contract as [D4](email-delivery.DECISIONS.md#d4--asynchronous-send-contract) and validation deferral as [D5](email-delivery.DECISIONS.md#d5--defer-email-input-validation).

Remaining proposal, pending approval:

- Q5: Use a private recording implementation inside the email service tests, exercised through `IEmailService`. One minimal contract test awaits a send and checks that recipient, subject, and body were retained exactly, including whitespace and line breaks. Defer configurable failures and delayed-completion test doubles until a production caller needs them. This verifies interface usability and the test helper only; it does not demonstrate real email delivery or production caller behavior.

Next: Resolve Q5, consolidate acceptance criteria and the TDD plan, then seek confirmation of shared understanding before implementation. No production code or tests changed; no checks run.

## 2026-11-03 — Recording scope correction and shared understanding

The user accepted recording but declined the proposed data-preservation check: "Recording is fine, but no data preservation check is required, since we simply store it and asserting on it later." Recorded [D6](email-delivery.DECISIONS.md#d6--recording-test-implementation-without-a-storage-self-test). This corrects Q5's proposal: no standalone recorder self-test will be added. Recorded data can support later caller tests.

Consolidated the spec with AC1–AC2, deferred scope, and a compilation/regression verification plan. No executable production behavior is introduced by declaring an interface and supplying test scaffolding, so no artificial red/green test of the test helper is planned. Future caller/provider behavior remains subject to the full repository TDD cycle.

The interview frontier is empty for this increment. Next: confirm shared understanding before implementation, as explicitly required by the grilling skill. No production code or tests changed; no checks run. No new domain term or architecture trade-off warrants a glossary entry or ADR.

## 2026-11-03 — Shared understanding confirmed and increment completed

The user answered "yes" to the final scope summary, confirming shared understanding. Added the approved `IEmailService` contract and an internal recording implementation in the test project. The recorder uses tuples rather than a new model; no standalone recorder self-test was added, as requested. No production email delivery or notification-service integration was introduced.

Reviewed both additions for clarity, duplication, and unnecessary complexity; no refactoring was warranted. This contract/test-scaffolding change introduces no executable production behavior, so no red/green evidence is claimed. Backend build passed with 0 warnings/errors; all 40 existing tests passed with 0 failures/skips; `git diff --check` passed. Detailed verification is recorded in the spec. Caller/provider work remains a separate increment. Changes are uncommitted.

## 2026-11-03 — Correction: failing test before recorder implementation

The user explicitly rejected adding `RecordingEmailService` without a preceding failing test. Removed the prematurely implemented recorder and reopened the spec. The earlier assumption that test scaffolding could be implemented ahead of a failing test is superseded. The interface remains; no new test or implementation was added.

The prior instruction against standalone recorder data-preservation checks remains in force. Proposed first seam, not approved: `INotifyService.NotifyAsync` for an email-enabled user requests email delivery through `IEmailService`. This catches missing email dispatch, but not real provider delivery. It brings a small caller integration slice into the increment, so seek explicit agreement before writing the test. The TDD skill requires pre-agreed seams. Once agreed, write and run one failing test before implementing dispatch or the recording test double. Do not claim a historical failing run retroactively.

## 2026-11-03 — Seam approval and red test

The user answered "yes" to the proposed notification-service seam and first email-dispatch slice. Recorded [D7](email-delivery.DECISIONS.md#d7--first-email-dispatch-test-at-the-notification-service-boundary), superseding the earlier integration deferral for this slice only.

Added one test through `INotifyService.NotifyAsync`, expecting one email request with the user's email and event title/body. Added only compile scaffolding: a no-op notification service and a private recorder whose send method throws `NotImplementedException`. The recorder does not yet store requests.

Ran the targeted test: it compiled and failed at `Assert.Single` because no email request was recorded (1 failed, 0 passed). Stopped at red as requested. Dispatch, recording behavior, green verification, and refactor review remain pending; no further slice started. See the spec for the exact command and evidence.
