# Email delivery

Status: First email-dispatch slice implemented and verified through red -> green -> refactor review. AC1–AC3 are complete for the approved email-enabled-user scenario. Additional channel-selection behavior and real delivery remain deferred.

## Goal

Define the email delivery boundary as `IEmailService`, similar to the existing notification-service boundary. An in-memory implementation in tests is sufficient for this increment.

## Confirmed scope

- Introduce an `IEmailService` contract.
- Use an in-memory test implementation; a real email provider is not required for this increment.
- See [D1](email-delivery.DECISIONS.md#d1--email-service-boundary-and-test-implementation).
- Accept explicit recipient email address, subject, and body inputs. The caller maps event title/body unchanged; rule matching and channel selection remain outside this boundary ([D2](email-delivery.DECISIONS.md#d2--explicit-email-inputs)).
- Connect this boundary to a concrete notification service in a separate increment ([D3](email-delivery.DECISIONS.md#d3--separate-email-contract-from-notification-service-integration)).
- Correction: The user subsequently approved bringing the first email-enabled-user dispatch slice into scope at `INotifyService.NotifyAsync` ([D7](email-delivery.DECISIONS.md#d7--first-email-dispatch-test-at-the-notification-service-boundary)). Other notification-service integration behavior remains deferred.
- Use `Task SendAsync(string recipientEmail, string subject, string body)`. Completion represents completion of the send request rather than confirmed inbox receipt; failures use exceptions, without automatic retries ([D4](email-delivery.DECISIONS.md#d4--asynchronous-send-contract)).
- Defer invalid-address and blank-content validation to production implementation work. The in-memory test implementation records values unchanged; existing `User.Email` validation remains unfinished ([D5](email-delivery.DECISIONS.md#d5--defer-email-input-validation)).

- Use a recording in-memory implementation in tests. Do not add a standalone test that verifies the recorder stores supplied values; later caller tests can assert against recorded requests ([D6](email-delivery.DECISIONS.md#d6--recording-test-implementation-without-a-storage-self-test)).

## Deferred scope

Real email providers, notification-service integration beyond AC3, input validation, automatic retries, configurable failure/delay test doubles, and delivery receipts remain outside this increment. AC3 exercises event-to-email mapping at the caller boundary.

## Acceptance criteria

| ID | Scenario | Expected behavior |
| --- | --- | --- |
| AC1 | A caller references the email service contract. | `IEmailService` exposes `Task SendAsync(string recipientEmail, string subject, string body)`, without requiring a User or Event. |
| AC2 | Test code needs to observe email requests. | A test-only in-memory implementation records recipient, subject, and body and returns a completed task. No standalone storage-preservation test is required. |
| AC3 | Notify an email-enabled user through `INotifyService.NotifyAsync`. | Request one email addressed to that user's email, with the event title as subject and event body as body. |

Successful completion represents send-request completion rather than inbox receipt. Failures are communicated through exceptions; production failure handling and its tests belong with the future caller/provider, not the recorder.

## Implementation plan

1. Obtain confirmation of shared understanding before implementing, as required by the grilling skill.
2. Exercise the approved `INotifyService.NotifyAsync` seam with one email-enabled-user test and compile scaffolding. Observe its expected failure before implementing dispatch or recording behavior.
3. Retain the injected `IEmailService`, forward recipient and event title/body, and return its task. Implement the private recorder only once the behavior test is red. Use tuples rather than introducing a new domain model.
4. Run the targeted test, review production code and tests for refactoring, then run the full test project and backend build. Record evidence for all three TDD steps.

Do not implement a recorder before a failing behavior test needs it. The approved seam is `INotifyService.NotifyAsync`: for an email-enabled user, request email delivery through `IEmailService`. This verifies dispatch, not real inbox delivery. No standalone recorder storage test is required.

## Verification

### 2026-11-03 — Contract and recording implementation

- The user confirmed shared understanding with "yes" after the final scope summary.
- AC1: Added `src/notif_be/Services/IEmailService.cs` with the approved asynchronous three-string contract.
- AC2: Added internal test-only `RecordingEmailService` in the test project. It appends a recipient/subject/body tuple to `Requests` and returns `Task.CompletedTask`. With no caller test yet, an internal helper avoids an otherwise empty test class while keeping the recorder available to future tests.
- Red/green: Not applicable to this contract/test-scaffolding increment; no executable production behavior or standalone recorder test was introduced, consistent with D6. No failing behavior test is claimed.
- Review/refactor assessment: Reviewed the interface and recorder for naming, duplication, clarity, and unnecessary abstractions. No refactoring warranted: a direct tuple list and completed task suffice; no model, provider dependency, or failure simulation was introduced.
- `dotnet build src/notif_be/notif_be.csproj`: passed, 0 warnings and 0 errors.
- `dotnet test src/notif_be_tests/notif_be_tests.csproj`: passed all 40 tests, 0 failed and 0 skipped. These are existing regression tests; they do not verify real email delivery or caller integration.
- `git diff --check`: passed.

### 2026-11-03 — Test-first correction

- The user rejected the implementation-first recorder. The earlier red/green exemption was incorrect and is superseded by this correction; the earlier verification above is historical, not completion evidence for the reopened work.
- Removed `RecordingEmailService.cs`. The interface remains; AC2 is pending.
- No new test has been written at an unconfirmed seam. No red/green/refactor cycle is claimed. The next step is seam agreement followed by one failing behavior test, before recorder or production behavior implementation.

### 2026-11-03 — AC3 red at the approved seam

- The user approved testing email dispatch through `INotifyService.NotifyAsync`; see D7.
- Added `NotifyServiceTests.Notifying_an_email_enabled_user_requests_email_delivery`. It calls the public notification interface with an email-enabled user and checks for one request with the literal expected recipient, subject, and body. This tests caller dispatch/mapping, not the recorder's storage behavior independently.
- Compile scaffolding: `NotifyService` accepts `IEmailService` but does not use it; `NotifyAsync` returns a completed task without dispatch. A private recorder scaffold exposes an empty request list; its `SendAsync` still throws `NotImplementedException`. Recording behavior is not implemented.
- Red command: `dotnet test src/notif_be_tests/notif_be_tests.csproj --filter FullyQualifiedName~NotifyServiceTests.Notifying_an_email_enabled_user_requests_email_delivery`.
- Observed result: built successfully, then failed 1/1 test at `Assert.Single` because the request collection was empty. This is the expected missing-dispatch failure, not a compilation error.
- Stopped at red as requested. Green dispatch/recording implementation and refactor review remain pending. No later slice has started; no full-suite pass is claimed for this state.

### 2026-11-03 — AC3 green and refactor review

- The user requested "make it green", authorizing completion of the existing red slice.
- Green: `NotifyService` retains its injected email service and returns `SendAsync(user.Email, worldEvent.Title, worldEvent.Body)`. The private recording test implementation now appends the supplied tuple and returns `Task.CompletedTask`.
- The same targeted command recorded above passed: 1 passed, 0 failed/skipped. The test was not weakened or changed; only the previously unimplemented recorder method changed in test code.
- Refactor assessment: Reviewed production dispatch and test code for duplication, naming, clarity, and design improvements. No refactoring warranted. Direct task forwarding avoids an unnecessary async wrapper, and the private tuple recorder needs no additional abstraction. This completes the current slice's review before any further slice.
- `dotnet test src/notif_be_tests/notif_be_tests.csproj`: all 41 passed, 0 failed/skipped.
- `dotnet build src/notif_be/notif_be.csproj`: passed with 0 warnings and 0 errors.
- Scope limit: This minimal dispatch implementation sends email whenever called. Email-disabled users, rule-less users, Slack dispatch, and caller-level async/failure scenarios have not been implemented or verified in this slice. `NotifyService` is not registered in application startup. Real delivery remains deferred.

### 2026-11-03 — Primary-constructor refactor

- Applied the user's review feedback: `NotifyService(IEmailService emailService)` replaces the explicit constructor and backing field, with dispatch unchanged. Added the primary-constructor convention to `AGENTS.md`; see D8.
- Reviewed the resulting class: the primary-constructor parameter is used directly for the injected email service, removing redundant boilerplate. No test changes were needed for this behavior-preserving refactor.
- `dotnet test src/notif_be_tests/notif_be_tests.csproj`: all 41 passed, 0 failed/skipped.
- `dotnet build src/notif_be/notif_be.csproj`: passed with 0 warnings/errors.
- `git diff --check`: passed.
