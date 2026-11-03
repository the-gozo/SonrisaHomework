# Notification system

Status: Ready for implementation. The user confirmed shared understanding of the consolidated spec on 2026-11-03. Implementation has not started.

## Goal

Define a small, reviewable notification-system increment building on iteration 1's users and alert rules.

## Existing constraints

- Each person can have at most one optional, updatable alert rule.
- A rule matches any selected predefined event type and enables email, Slack, or both.
- Email targets the user's personal address; Slack targets that user through a direct message.
- Other channels must be possible in the overall solution, and an admin view is required by the project brief.
- See [iteration 1 decisions](../iter1/create-alert-rule.DECISIONS.md) and the [glossary](../../GLOSSARY.md).

## Confirmed scope

Given an event, identify matching users and request delivery through their enabled channels. Program against an `INotifyService` interface, with no concrete implementation in iteration 2. Email and Slack implementations are due in iteration 3.

Rule matching happens outside `INotifyService`. Call the interface once per matching user with that user's notification and selected channels; both enabled channels are represented in one call.

Processing receives an already classified event from its caller. External feeds, polling, and event classification are outside iteration 2. The workflow retrieves all users through an injected `IUserRepository`, then performs matching itself; the caller supplies only the event. Use a simple in-memory collection as the repository implementation at this stage.

An event contains an integer `Id`, one predefined event type, a title, and a message body. Any new model must implement `IModel` under repository guidance. The domain word Event does not require a particular C# class name.

The admin view is deferred to iteration 3, alongside the email and Slack implementations.

For each matching user, use the event title and message body unchanged, without templates or personalization. Process immediately when called using an asynchronous operation that awaits all notification-service calls; no scheduler or background queue is included. Completion reflects completed service calls rather than confirmed receipt by a user.

If a notification-service call fails for one user, continue processing other matching users and return a per-user success/failure summary. Do not retry automatically. Repeated submissions of the same event are processed independently, potentially requesting repeat notifications; no duplicate suppression is included in iteration 2.

`INotifyService.NotifyAsync` receives the recipient `User` and the event, returning `Task`. Read channel selections from `User.AlertRule`; do not pass redundant email/Slack parameters. A completed call counts as a successful request and an exception as a failed request. No separate Notification model is required for this increment.

Events accept caller-supplied integer IDs, require one known event type and nonblank title/body, and must be rejected before user lookup or notification requests when invalid. Automatic event ID allocation remains deferred.

Initialize the in-memory user repository from a supplied collection and expose retrieval of all users. No user-management endpoints or database persistence are included. An empty collection is a valid no-recipient case. User retrieval error handling is deferred; no lookup-error recovery or simulated retrieval failures are required in iteration 2.

Use the approved Event, Notification, and Delivery attempt terms in the [glossary](../../GLOSSARY.md). One notification is intended for one user; sending through email and Slack involves separate delivery attempts.

Process matching users sequentially, awaiting each service call before starting the next. Return one result entry with user ID and success/failure per matching user. Users with no rule or a nonmatching rule are skipped and omitted from results. An empty user collection or no matches produces an empty result. Detailed error messages and per-channel results are deferred.

## Acceptance criteria

| ID | Scenario | Expected behavior |
| --- | --- | --- |
| AC1 | Construct a valid event. | Retain the supplied integer ID, one predefined event type, title, and body; implement `IModel`. |
| AC2 | Supply an unknown event type or a null, empty, or whitespace-only title/body. | Reject the invalid event before repository lookup or notification requests. |
| AC3 | Initialize the in-memory user repository with users. | Retrieval exposes all supplied users, including users with and without rules. An empty collection is supported. |
| AC4 | Process an event with a matching user, a nonmatching user, and a user with no rule in the repository. | Retrieve users within the workflow and request a notification only for the matching user. |
| AC5 | Matching users enable email only, Slack only, or both. | Call `INotifyService.NotifyAsync` once for each matching user with that user and the supplied event. Channel selections remain available through `User.AlertRule`, without duplicate flag parameters. |
| AC6 | Request a notification for a matching user. | Supply the event title and body unchanged. No separate Notification model or concrete `INotifyService` implementation is introduced. |
| AC7 | The first matching user's service call is incomplete. | Do not start the next call or complete processing until that call completes; process users sequentially. |
| AC8 | A user's notification-service call completes successfully. | Include a successful result for that user ID; this is service-call success rather than confirmed receipt. |
| AC9 | A user's notification-service call throws. | Include a failed result for that user ID, make no automatic retry, and continue to later matching users. |
| AC10 | There are no users or no matching users. | Make no notification-service calls and return an empty result. Skipped users never appear in the result. |
| AC11 | Submit the same event twice. | Process both submissions independently and request notifications again for matching users. |

## Deferred scope and iteration dependencies

- Iteration 3: concrete email and Slack implementations and admin view. Slack identity configuration, missing destinations, and provider-specific/channel-specific outcomes need discussion with those implementations. This iteration does not invent or validate Slack destinations.
- External event sources, classification, database persistence, user-management endpoints, automatic IDs, scheduling, queues, retries, duplicate suppression, and detailed error reporting are outside this increment. User retrieval error handling is explicitly deferred.
- Existing iteration 1 rule matching and user/rule references support this increment. Iteration 1's unfinished email validation and rule lifecycle workflows remain tracked there; this spec does not silently complete or supersede them. Seeded users can already reference rules without adding assignment/update workflows.
- No further product decision is outstanding within the agreed iteration 2 scope. Routine names and collection/result representations can follow repository conventions during implementation; any material behavior change must update this spec.

No ADR is proposed: the selected interface and in-memory boundaries are readily reversible and do not justify a separate architecture record under the domain-modeling skill.

## Implementation plan

After the user confirms shared understanding, work in small increments, using the approved decisions and acceptance criteria as the source of truth:

1. Add the event model and tests for retained fields and validation (AC1–AC2). Observe relevant failing tests before implementation. Follow `IModel` conventions and retain existing event-type values without expanding matching semantics.
2. Introduce `IUserRepository` and a simple seeded in-memory implementation (AC3). Verify populated and empty collections.
3. Declare `INotifyService.NotifyAsync(User, event)` returning `Task`, with no production implementation. Use a recording/controllable test double solely for verification.
4. Add the event-processing workflow, repository lookup, recipient matching, unchanged event forwarding, and per-user result entries (AC4–AC6, AC8, AC10).
5. Exercise incomplete and failing service calls to verify sequential awaiting and per-user failure isolation (AC7, AC9), then repeated event submissions (AC11). Add only the behavior needed by the failing criteria and refactor once green.
6. Run the backend build and full existing test project, recording actual results below. Keep interface/UI endpoints and concrete delivery providers out of this increment.

## Verification

Discovery only. No production code changed; build and tests have not been run for this documentation increment.

Planned commands after implementation:

- `dotnet test src/notif_be_tests/notif_be_tests.csproj`
- `dotnet build src/notif_be/notif_be.csproj`

Use behavior-focused tests with an in-memory repository and notification-service test double. Verify sequencing with a controllable incomplete task rather than elapsed-time sleeps. No actual email or Slack messages are required or sent by this iteration's verification.

## Records

- [Discussion notes](notification-system_DISCUSSION_NOTES.md)
- [Approved decisions](notification-system.DECISIONS.md)
