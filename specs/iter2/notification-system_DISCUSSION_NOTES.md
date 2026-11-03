# Notification system discussion notes

## 2026-11-03 — Interview opened

Context: The user requested starting iteration 2 by grilling them about the notification system using grill-with-docs, which composes the grilling and domain-modeling skills.

Repository findings: Iteration 1 implements user properties and rule-instance validation/matching. No event intake or notification delivery exists. Email validation, rule ownership/assignment/update workflows, and Slack identity storage remain incomplete or deferred. Starting iteration 2 discovery does not imply iteration 1 is complete.

Round 1 proposals, awaiting user answers:

- Q1: Make the iteration deliverable processing a supplied event, selecting matching users, and requesting delivery through enabled channels. Resolve intake, provider realism, administration, and failure behavior in subsequent rounds.
- Q2: Use Event for a reported world occurrence, Notification for the message about that event intended for one user, and Delivery attempt for one try to send that notification through one channel. Under this proposal, one user notified by email and Slack receives one notification through two channel deliveries.

Reasoning: Agree the increment's outcome and distinguish domain concepts before selecting interfaces or infrastructure. Existing Alert rule terminology remains in force.

Open questions: Both proposals are unapproved. The downstream branches are tracked in the draft spec.

Next steps: Wait for this round's answers, capture approved decisions/terms, and advance the design frontier. No new technical decisions or ADRs have been approved.

## 2026-11-03 — Round 1 answers and round 2 frontier

The user approved Q1 with an explicit scope constraint: program against `INotifyService`, without an implementation in iteration 2. Email and Slack implementations are due in iteration 3. Recorded as [D1](notification-system.DECISIONS.md#d1--notification-interface-in-iteration-2-implementations-in-iteration-3). This supersedes the initial round's open question about actual channel integrations in iteration 2.

The user approved Q2's Event, Notification, and Delivery attempt definitions. Recorded as [D2](notification-system.DECISIONS.md#d2--distinguish-events-notifications-and-delivery-attempts) and added immediately to the glossary.

Round 2 proposals, awaiting approval:

- Q3: Keep rule matching outside `INotifyService`; call it once per matching user with that user's notification and selected channels. If both channels are enabled, make one service call carrying both selections.
- Q4: Start processing from an already classified event supplied by a caller. External feeds, polling, and classification are outside this increment.
- Q5: Supply candidate users to the processing operation; skip users with no rule or a nonmatching rule. User lookup and persistence are outside this increment.

Reasoning: Settle independent boundaries for notification requests, event input, and candidate recipients before choosing detailed data contracts. These remain proposals. Concrete delivery outcomes and retry semantics depend on the service boundary and will be addressed after it is settled.

Next steps: Wait for round 2 answers, then resolve data and observable behavior. No implementation or new ADR created.

## 2026-11-03 — Round 2 partial answers and Q5 clarification

The user approved Q3 and Q4. Recorded as [D3](notification-system.DECISIONS.md#d3--matching-outside-the-notification-service) and [D4](notification-system.DECISIONS.md#d4--caller-supplies-an-already-classified-event).

The user requested elaboration on Q5; no recipient-source decision has been approved. The proposed boundary is that the caller supplies users to consider, and processing checks their rules. For example, given a natural-disaster event and Alice (matching rule), Bob (market-movement only), and Carol (no rule), processing calls `INotifyService` for Alice only. This contrasts with the alternative in which processing obtains users itself through a separate lookup dependency.

Reasoning for the recommendation: Caller-supplied users allow a small, directly testable matching increment without selecting storage or user-lookup behavior. Supplying candidates is not supplying preselected recipients; rule matching still belongs to this workflow. How a future caller obtains a complete candidate set remains future work if this boundary is approved.

Next step: Resolve Q5 before advancing to the next interview round. No production code changed.

## 2026-11-03 — Q5 answer and round 3 frontier

The user selected option 2: the workflow retrieves users itself, with the rationale "for minimizing the call hierarchy". Recorded as [D5](notification-system.DECISIONS.md#d5--workflow-retrieves-candidate-users). This rejects the earlier caller-supplied-user recommendation; the caller supplies only an event.

Round 3 proposals, awaiting approval:

- Q6: Retrieve all users through an injected `IUserRepository` interface and perform rule matching in the workflow. Defer concrete storage and use a test double for verification.
- Q7: Define the supplied event with an integer identifier, one predefined event type, a title, and a message body. The event is already classified; geographic and market-threshold filtering remain outside the agreed category matching.
- Q8: Defer the admin view to a later iteration, retaining it as an overall project requirement.

Reasoning: Establish the new lookup dependency without choosing storage; establish the minimum data to identify an event and compose its message; keep iteration 2's scope reviewable. Exact contracts, validation, delivery request content, failures, duplicates, and timing remain to be discussed after the relevant prerequisites are settled.

Next steps: Wait for round 3 answers and advance the frontier. No implementation or ADR created.

## 2026-11-03 — Round 3 partial answers

The user approved Q6 and explicitly requested a simple in-memory collection at this stage. Recorded as [D6](notification-system.DECISIONS.md#d6--user-repository-with-an-in-memory-implementation). This replaces the proposal to defer concrete storage: `IUserRepository` will have an in-memory implementation. It does not authorize a concrete `INotifyService` implementation, which remains deferred to iteration 3.

The user approved Q7's event fields: integer identifier, one predefined event type, title, and message body. Recorded as [D7](notification-system.DECISIONS.md#d7--event-fields). The existing Event glossary definition remains sufficient; these data-contract details belong in the spec rather than the glossary.

Q8 received no answer and remains open. No deferral of the admin view has been approved. Re-present Q8 before advancing to the next round.

Next step: Settle admin scope, then continue with detailed behavior and contracts. No production implementation or tests added.

## 2026-11-03 — Q8 answer and round 4 frontier

The user explicitly deferred the admin view to iteration 3. Recorded as [D8](notification-system.DECISIONS.md#d8--admin-view-in-iteration-3). Iteration 3 now includes both concrete email/Slack implementations and the admin view; their detailed requirements remain future work.

Round 4 proposals, awaiting approval:

- Q9: For each matching user, use the event title and message body unchanged as the notification content, with no templates, summaries, or personalization in this iteration.
- Q10: Process immediately when called, with an asynchronous operation that completes after awaiting all notification-service calls; no scheduler or background queue in this iteration. Completion means requests completed, not proof that a person received a message.
- Q11: If `INotifyService` fails for one user, continue with other matching users and return a per-user success/failure summary. Do not retry automatically in iteration 2. The one-call-per-user boundary means channel-specific outcomes are not inferred from a successful service call.
- Q12: Treat repeated submissions of the same event as separate processing requests, with no duplicate suppression in iteration 2. Reprocessing can request another notification for the same user.

Reasoning: These independent decisions establish message mapping, invocation timing, failure isolation, and repeated-event behavior without choosing real delivery providers. Each is a proposal, not an accepted requirement. Detailed contracts and validation remain to be resolved after this round.

Next steps: Wait for round 4 answers. No production code or tests changed.

## 2026-11-03 — Round 4 answers and round 5 frontier

The user approved Q9, Q10, Q11, and Q12. Recorded as [D9](notification-system.DECISIONS.md#d9--unchanged-event-content), [D10](notification-system.DECISIONS.md#d10--immediate-asynchronous-processing), [D11](notification-system.DECISIONS.md#d11--per-user-failure-isolation-without-retries), and [D12](notification-system.DECISIONS.md#d12--no-duplicate-suppression-in-iteration-2).

Round 5 proposals, awaiting approval:

- Q13: Define `INotifyService.NotifyAsync` to receive the recipient `User`, supplied event, and explicit email/Slack selections, returning `Task`. A completed call is a successful request; an exception is a failed request. Do not add a separate Notification model merely for the domain term in iteration 2.
- Q14: Accept caller-supplied integer event IDs, require one known event type and nonblank title/body, and reject invalid events before user lookup or notification requests. Automatic event ID allocation remains deferred.
- Q15: Initialize the in-memory repository from a supplied collection of users, expose retrieval of all users, and add no user-management endpoints or database persistence in this increment.
- Q16: If user lookup fails, fail the processing operation explicitly without notification requests, rather than returning a misleading empty success summary. An empty repository is a normal no-recipient outcome.

Reasoning: Make the already approved behavior implementable with minimal contracts and resolve input and lookup failure boundaries. These proposals are not approved. Exact result fields and detailed verification remain to be settled.

Next steps: Wait for round 5 answers. No implementation or tests added.

## 2026-11-03 — Round 5 answers and round 6 frontier

Q13 correction: The user requested reading email/Slack flags from the recipient's existing `AlertRule` reference instead of passing them explicitly. Recorded as [D13](notification-system.DECISIONS.md#d13--notification-service-reads-channel-selections-from-the-users-rule). The proposed User/event inputs, `Task` completion contract, and absence of a separate Notification model are retained with that correction.

The user approved event validation and the seeded in-memory repository in Q14 and Q15. Recorded as [D14](notification-system.DECISIONS.md#d14--event-validation-and-caller-supplied-ids) and [D15](notification-system.DECISIONS.md#d15--seeded-in-memory-repository-without-management-endpoints).

For Q16 the user requested deferring retrieval errors and using an empty list at minimum. Recorded as [D16](notification-system.DECISIONS.md#d16--defer-user-retrieval-error-handling). An empty repository is a supported normal case; no exception-to-empty-list fallback is inferred from this answer.

Round 6 proposals, awaiting approval:

- Q17: Return a result entry containing user ID and success/failure for each matched user's service call. Users without a rule or with a nonmatching rule are skipped and omitted. Empty/no-match input yields an empty result. Detailed exception text and per-channel outcomes are deferred.
- Q18: Process matching users sequentially, awaiting each service call before the next. Do not introduce parallel delivery requests in iteration 2.

Reasoning: Close the observable result and execution-order contracts without expanding delivery infrastructure. After these answers, consolidate acceptance criteria and verification into the draft spec and request confirmation of shared understanding before implementation.

Next steps: Wait for round 6 answers. No production implementation or tests added.

## 2026-11-03 — Round 6 answers and consolidated spec

The user answered "yes to all", approving Q17's per-matching-user results and Q18's sequential processing. Recorded as [D17](notification-system.DECISIONS.md#d17--per-matching-user-result-entries) and [D18](notification-system.DECISIONS.md#d18--sequential-notification-requests).

Consolidated the approved behavior into the source-of-truth spec, with acceptance criteria AC1–AC11, a small-increment implementation plan, planned verification, iteration 1 dependencies, and explicit deferred scope. The interview frontier is empty within the agreed iteration 2 scope. Iteration 3 retains the real delivery implementations and admin view; their details are deliberately deferred rather than silently specified here.

No ADR is needed for the readily reversible interface/in-memory choices. The glossary already captures the resolved domain terms without implementation details.

Next step: Ask the user to confirm shared understanding of the consolidated spec before acting on implementation, as required by the grilling skill. This confirmation has not yet been given. No production code changed, and build/tests were not run for this documentation increment.

## 2026-11-03 — Shared understanding confirmed

The user answered "yes" to the confirmation question for the consolidated iteration 2 spec. Shared understanding is confirmed; the interview is complete and the spec is ready for implementation.

The approved scope and decisions remain unchanged. This concludes the requested design interview; production implementation has not started. Build/tests were not run for these documentation changes.

## 2026-11-03 — Event implementation suggestions

Context: The user requested a review of the notification-system spec and suggestions for Event implementation using the TDD skill. Reviewed the spec, approved decisions, glossary, existing models, and AlertRule tests. This request is for recommendations; implementation has not started.

Proposals, awaiting approval:

- Add `Event : IModel` in `src/notif_be/Models/Event.cs`, with constructor `Event(int id, string eventType, string title, string body)`. Use a normal class consistent with User and AlertRule; the domain name needs no alternative C# name at present.
- Expose `Id` with get/set as required by IModel, and get-only `EventType`, `Title`, and `Body`. Constructor validation keeps event type and content valid after construction.
- Accept exactly the existing string values `breaking-news`, `market-movement`, and `natural-disaster`. Avoid introducing an enum or changing AlertRule's public contract in this slice. If extracting shared event-type validation, keep it small and preserve existing rule behavior.
- Reject unknown/null event types and null, empty, or whitespace-only title/body with ArgumentException identifying the parameter, consistent with AlertRule's validation style. Preserve valid title/body verbatim, including surrounding whitespace. Add no positive-ID restriction, length limits, or automatic ID assignment because the spec does not require them.
- Keep matching, recipient lookup, and notification requests outside the Event model. Add no persistence, delivery state, timestamps, or separate Notification model in this slice.

Proposed test seams, not yet confirmed:

1. Event's public constructor and properties (AC1–AC2): catches field retention, IModel compatibility, allowed event types, and rejected invalid content; does not exercise processing or delivery.
2. The eventual processing operation's public entry point (AC2, later workflow slice): catches rejection of a null event before lookup or notification requests; does not test transport binding or real providers. Non-null invalid model instances should already be prevented by the constructor.

Suggested TDD order after seam confirmation: one valid-event retention example, then individual invalid-input cases, each through a red/green cycle. Cover every known event type and preserve whitespace around otherwise nonblank content. Add workflow boundary verification when that workflow is implemented.

Open questions and next steps: Confirm the proposed test seams before writing tests, as required by the TDD skill. These implementation recommendations are not approved technical decisions and have not been added to the decisions file. No production code or tests changed; build/tests were not run for this documentation-only review.

## 2026-11-03 — Event-only approval and implementation

Context: The user asked to clarify the proposed test boundaries. Explained that the constructor/property boundary covers event retention and validation, while a future processing-method boundary covers matching and notification outcomes. Recommended approving only the first boundary for this increment. The user replied: "Agree. Stick to Event for now." Recorded the scope and test-boundary approval as [D19](notification-system.DECISIONS.md#d19--event-only-increment-and-constructorproperty-test-boundary).

Implemented `Event : IModel` with supplied ID, event type, title, and body. Event type and content are get-only and validated in the constructor; Id follows IModel's writable contract. Valid content is retained verbatim. Invalid types and blank content throw ArgumentException with the relevant parameter name. No additional ID rules or shared event-type abstraction were introduced.

Verification used only the approved constructor/property boundary. Observed the initial missing-model compilation failure, then separate red/green cycles for unknown event types, blank titles, and blank bodies. All 25 tests passed, including 14 Event cases. Backend build succeeded with 0 warnings and 0 errors. Results are also recorded in the spec.

Remaining work: Repository, notification-service interface, and processing workflow are pending. No processing-boundary tests were written. Revisit that boundary when the user proceeds to the next increment.

## 2026-11-03 — AC3 retrieval boundary approved and first red test

Context: Suggested AC3 as the next increment: a seeded in-memory user repository. Proposed testing through its public retrieval method with the real implementation, starting with populated input and then handling empty input in a later cycle. This boundary catches retrieval behavior and excludes processing and delivery. The user replied: "OK. Red test then," approving the boundary and requesting only the red stage. Recorded as [D20](notification-system.DECISIONS.md#d20--repository-retrieval-test-boundary-for-ac3).

Added one test that seeds a user with an alert rule and a user without one, retrieves through the proposed `IUserRepository.GetAll()` boundary, and checks identities, email addresses, and optional rule IDs. Sorting retrieved values avoids inventing an ordering requirement. Proposed routine API names are `InMemoryUserRepository` and namespace `notif_be.Repositories`; no production types have been added.

Verification: The filtered dotnet test command exited 1 with CS0234 for the missing `notif_be.Repositories` namespace. The backend built, but the test assembly failed compilation, so no assertions ran. This is the initial missing-API red state, not a passing test run.

Next steps: Stop at red as requested. Green implementation and refactor assessment remain pending; do not start an empty-collection test until this slice completes. No commit or push requested for these changes.

## 2026-11-03 — Correct red stage to compile and fail at runtime

The user pointed out that the test did not build because IUserRepository and InMemoryUserRepository were missing. Corrected the earlier interpretation of red: provide the minimal API scaffolding needed to run the test, leaving retrieval behavior unimplemented.

Added `IUserRepository.GetAll()` returning `IEnumerable<User>` and an InMemoryUserRepository stub accepting the supplied users. Its GetAll method throws NotImplementedException. The filtered test now compiles and runs, failing with that exception (1 failed test). Building the test project succeeds with 0 warnings and 0 errors, including the backend dependency.

This supersedes the prior noncompiling red state. Green and refactor remain pending; the current slice still stops at red. No additional test, commit, or push was performed.

## 2026-11-03 — Populated-collection retrieval implemented

Context: On resuming, explained that the next step was green for the existing populated-collection test, followed by a refactor assessment before starting the empty-collection case. The user requested: "Implement."

Re-ran the existing test and confirmed the expected NotImplementedException failure. Implemented the constructor by materializing supplied users into a private array, and GetAll by returning that collection. The filtered repository test then passed.

Refactor assessment: Reviewed production code and the public-boundary test for duplication, clarity, and design improvements. No refactoring was warranted for the small direct implementation. No additional abstraction or test was introduced.

Verification after review: All 26 tests passed; the backend build succeeded with 0 warnings and 0 errors. Recorded red, green, and refactor evidence in the spec. Next slice is explicit empty-collection verification; notification-service and processing work remain deferred. No commit or push was requested for this implementation.

## 2026-11-03 — AC3 empty-collection verification completed

Context: Explained that the empty-input test should already pass, and that an immediate pass would be recorded without manufacturing a failure or modifying production behavior. The user requested: "Go with Ac3 empty collection."

Added one test at the already approved repository retrieval boundary: seed with Array.Empty<User>(), call GetAll through IUserRepository, and assert an empty result. The test passed on its initial filtered run. No red state was observed and no production code changed.

Refactor assessment: Reviewed the repository implementation, interface, and both tests for duplication, clarity, and design improvements. No refactoring was warranted; the examples remain small and independently understandable.

Verification: All 27 tests passed, with no failures or skips. The backend build succeeded with 0 warnings and 0 errors. AC3 is now fully implemented and verified; processing work remains deferred pending agreement on its test boundary. No commit or push was requested.

## 2026-11-03 — AC4 processing boundary approved and red test added

Context: Proposed testing AC4 through the public processing method with the real in-memory repository and a recording notification-service double. Explained that the observable result is a request for only the matching user, with the supplied event; internal helpers and actual message delivery are outside this boundary. The user requested: "Make a red test". Approval is recorded as [D21](notification-system.DECISIONS.md#d21--public-processing-boundary-for-ac4).

Added one test with matching, nonmatching, and rule-less users. It awaits ProcessAsync, expects a single recorded notification request, checks the matching user ID, and verifies the supplied event is forwarded. The recording service exists only in tests. Added INotifyService.NotifyAsync(User, Event) returning Task and a minimal EventProcessor stub accepting the repository and notification service. ProcessAsync throws NotImplementedException; no processing behavior is implemented. Per-user result shape is deferred until its acceptance criteria are exercised.

Verification: The filtered EventProcessorTests run compiles and executes 1 test, which fails with NotImplementedException from ProcessAsync before notification assertions. Building the test project succeeds with 0 warnings and 0 errors, including the backend. Stopped at red; green and refactor are pending. No commit or push was requested.

## 2026-11-03 — AC4 made green and reviewed for refactoring

The user requested: "Make test green". Re-ran the existing test and confirmed the runtime NotImplementedException failure before changing production code. EventProcessor now retains its injected repository and notification service, retrieves all users, skips absent or nonmatching alert rules, and awaits a notification request for matching users with the supplied event.

The unchanged AC4 test passed. Reviewed the implementation, interface, and test for duplication, clarity, and design improvements; no refactoring was warranted. Reused the existing AlertRule.Matches behavior and kept the processor as a small direct loop. No new test, result model, or concrete notification implementation was added.

Verification: Full suite passed all 28 tests, with no failures or skips. Backend build succeeded with 0 warnings and 0 errors. Recorded red, green, and refactor evidence in the spec. AC4 is complete; AC2 processing-boundary checks and AC5–AC11 verification remain pending. No commit or push was requested.

## 2026-11-03 — EventProcessor primary constructor

The user requested: "Use primary constructor in EventProcessor". Converted the constructor to the class declaration and removed the explicit dependency fields and assignments. The processing method uses the captured constructor parameters directly. This supersedes the earlier assessment that no constructor simplification was warranted. Approved choice recorded as [D22](notification-system.DECISIONS.md#d22--eventprocessor-primary-constructor).

Verification after the refactor: all 28 tests passed; the backend build succeeded with 0 warnings and 0 errors. No behavior or test changes were needed. Changes remain uncommitted.
