# Email delivery decisions

## D1 — Email service boundary and test implementation

- Date: 2026-11-03.
- Decision: Start with `IEmailService`, similar to the notification service. An in-memory implementation in tests is sufficient; a real email provider is not required for this increment.
- Rationale: Establish a small, reviewable email boundary before introducing an external provider.
- Approval: The user requested: "Lets start with IEmailService, similar to notification service. In-mem implementation in tests is sufficient."
- Discussion: [Initial scope](email-delivery_DISCUSSION_NOTES.md#2026-11-03--initial-scope-and-first-round).

## D2 — Explicit email inputs

- Date: 2026-11-03.
- Decision: The email service accepts a recipient email address, subject, and body. The caller maps event title/body unchanged to subject/body. Rule matching and channel selection remain outside the email service.
- Rationale: Keep the email boundary independent of users, alert rules, and event classification.
- Approval: The user answered "both yes" to Q1 and Q2, accepting Q1's recommendation.
- Discussion: [First-round approvals](email-delivery_DISCUSSION_NOTES.md#2026-11-03--first-round-approvals-and-second-round).

## D4 — Asynchronous send contract

- Date: 2026-11-03.
- Decision: Use `Task SendAsync(string recipientEmail, string subject, string body)`. Successful task completion represents completion of the send request, not confirmed inbox receipt. Failures use exceptions. No automatic retries are included.
- Rationale: Follow the existing notification service's asynchronous completion and failure convention.
- Approval: The user answered "both yes" to Q3 and Q4, accepting Q3's recommendation.
- Discussion: [Second-round approvals](email-delivery_DISCUSSION_NOTES.md#2026-11-03--second-round-approvals-and-verification-boundary).

## D5 — Defer email input validation

- Date: 2026-11-03.
- Decision: Defer invalid-address and blank-content validation until production email implementation work. The in-memory test implementation records supplied values unchanged. This does not assume that `User.Email` is validated.
- Rationale: Keep this increment focused on defining the contract and recording requests in tests.
- Approval: The user answered "both yes" to Q3 and Q4, accepting Q4's recommendation.
- Discussion: [Second-round approvals](email-delivery_DISCUSSION_NOTES.md#2026-11-03--second-round-approvals-and-verification-boundary).

## D3 — Separate email contract from notification-service integration

- Date: 2026-11-03.
- Decision: Define and test the email contract in this increment. Defer its connection to a concrete `INotifyService` implementation to a separate increment.
- Clarification: D6 supersedes the proposed standalone recorder test; this increment verifies compilation and existing checks instead.
- Rationale: Settle the small email boundary before defining email/Slack orchestration.
- Approval: The user answered "both yes" to Q1 and Q2, accepting Q2's recommendation.
- Discussion: [First-round approvals](email-delivery_DISCUSSION_NOTES.md#2026-11-03--first-round-approvals-and-second-round).

## D6 — Recording test implementation without a storage self-test

- Date: 2026-11-03.
- Decision: Use a recording implementation in tests, without a standalone data-preservation check. Retain recorded requests for assertions in later caller tests.
- Rationale: A test that only proves the test helper stores its inputs adds no useful production-behavior coverage.
- Approval: The user said: "Recording is fine, but no data preservation check is required, since we simply store it and asserting on it later."
- Discussion: [Recording scope correction](email-delivery_DISCUSSION_NOTES.md#2026-11-03--recording-scope-correction-and-shared-understanding).

## D7 — First email dispatch test at the notification-service boundary

- Date: 2026-11-03.
- Decision: Test `INotifyService.NotifyAsync` with an email-enabled user, expecting an email request through `IEmailService`. Bring this small caller-integration slice into scope, superseding D3's deferral for this slice only. Start with a failing test before implementing dispatch or recording behavior.
- Rationale: Exercise useful caller behavior rather than testing the recorder against itself.
- Approval: The user answered "yes" to the proposed first test and scope change after requiring failing-test-first development.
- Discussion: [Seam approval and red test](email-delivery_DISCUSSION_NOTES.md#2026-11-03--seam-approval-and-red-test).

## D8 — Primary constructor for notification-service injection

- Date: 2026-11-03.
- Decision: Use a primary constructor for `NotifyService` and prefer primary constructors where applicable in repository guidance.
- Rationale: Remove the explicit constructor and backing-field boilerplate for this simple injected dependency.
- Approval: The user requested: "use primary constructor in NotifyService. add an instruction to agents.md, to use primary ctor where applicable".
- Discussion: [Primary-constructor feedback](email-delivery_DISCUSSION_NOTES.md#2026-11-03--primary-constructor-feedback).
