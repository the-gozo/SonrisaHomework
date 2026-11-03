# Notification system — approved decisions

## D1 — Notification interface in iteration 2; implementations in iteration 3

- Date: 2026-11-03
- Decision: Iteration 2 processes a supplied event, identifies matching users, and requests notifications through enabled channels by programming against `INotifyService`. Do not add a concrete implementation of this interface in iteration 2. Email and Slack implementations are due in iteration 3.
- Rationale: Establish notification behavior and its service boundary before integrating actual delivery channels.
- Approval: The user answered Q1: "yes, but only program against an INotifyService interface with no implementation. Email and Slack implementation is due in iter3".
- Open: Interface responsibilities, method contract, event intake, recipient source, and failure behavior.

## D2 — Distinguish events, notifications, and delivery attempts

- Date: 2026-11-03
- Decision: An Event is a reported world occurrence; a Notification is its message intended for one user; a Delivery attempt is one try to send that notification through one channel. Email and Slack can carry the same notification through separate attempts.
- Rationale: Distinguish what happened, whom the message concerns, and the individual sends, including partial channel failure.
- Approval: The user answered Q2: "yes".
- Terminology: Recorded in the [glossary](../../GLOSSARY.md).

Discussion context: [round 1 answers](notification-system_DISCUSSION_NOTES.md#2026-11-03--round-1-answers-and-round-2-frontier).

## D3 — Matching outside the notification service

- Date: 2026-11-03
- Decision: Keep rule matching outside `INotifyService`. Call the interface once per matching user with that user's notification and selected channels. When both channels are enabled, one call carries both selections.
- Rationale: Separate recipient selection from requesting notification delivery.
- Approval: The user answered "q3: yes".

## D4 — Caller supplies an already classified event

- Date: 2026-11-03
- Decision: Start processing from an already classified event supplied by a caller. External feeds, polling, and classification are outside iteration 2.
- Rationale: Exercise the notification flow without requiring external event-source integration.
- Approval: The user answered "q4: yes".

Discussion context: [round 2 answers](notification-system_DISCUSSION_NOTES.md#2026-11-03--round-2-partial-answers-and-q5-clarification).

## D5 — Workflow retrieves candidate users

- Date: 2026-11-03
- Decision: The caller supplies only the event. The notification workflow retrieves candidate users itself through a user lookup dependency, then checks their rules and requests notifications for matches.
- Rationale: The user selected this boundary to minimize the call hierarchy and avoid requiring callers to retrieve and pass users.
- Approval: In response to the Q5 alternatives, the user said "2nd option for minimizing the call hierarchy".
- Alternative rejected: Caller supplies both the event and candidate users.
- Open: User lookup contract, concrete storage, and retrieval failure behavior.

Discussion context: [Q5 answer](notification-system_DISCUSSION_NOTES.md#2026-11-03--q5-answer-and-round-3-frontier).

## D6 — User repository with an in-memory implementation

- Date: 2026-11-03
- Decision: Inject `IUserRepository` into the workflow, retrieve all users, and match their rules in the workflow. Implement the repository with a simple in-memory collection at this stage.
- Rationale: Provide user lookup for the event-only entry point without requiring database storage.
- Approval: The user answered Q6: "yes, exactly! a simple inmemory collection will do at this stage."
- Scope clarification: The earlier recommendation to defer concrete storage is replaced by the user's in-memory choice. The no-implementation constraint on `INotifyService` remains in force.

## D7 — Event fields

- Date: 2026-11-03
- Decision: An event has an integer `Id`, one predefined event type, a title, and a message body.
- Rationale: Identify the occurrence and provide the content to communicate.
- Approval: The user answered Q7: "yes".
- Open: Field validation, identifier assignment, and notification-content mapping.

Discussion context: [round 3 partial answers](notification-system_DISCUSSION_NOTES.md#2026-11-03--round-3-partial-answers).

## D8 — Admin view in iteration 3

- Date: 2026-11-03
- Decision: Defer the admin view to iteration 3.
- Rationale: Keep iteration 2 focused on event processing and notification requests.
- Approval: The user answered Q8: "defer it to iter3".

Discussion context: [Q8 answer](notification-system_DISCUSSION_NOTES.md#2026-11-03--q8-answer-and-round-4-frontier).

## D9 — Unchanged event content

- Date: 2026-11-03
- Decision: Use the event's title and body unchanged as notification content, without personalization or templates.
- Rationale: Establish direct event-to-notification content mapping in the initial workflow.
- Approval: The user answered Q9: "yes".

## D10 — Immediate asynchronous processing

- Date: 2026-11-03
- Decision: Start processing when called and complete after awaiting all `INotifyService` calls. No scheduler or background queue in iteration 2. Completion does not prove user receipt.
- Rationale: Give callers a clear processing completion boundary without introducing background infrastructure.
- Approval: The user answered Q10: "yes".

## D11 — Per-user failure isolation without retries

- Date: 2026-11-03
- Decision: Continue processing other matching users when one user's service call fails. Return a per-user success/failure summary. No automatic retries in iteration 2.
- Rationale: A failed request for one user must not prevent requests for others.
- Approval: The user answered Q11: "yes".
- Open: Exact summary contract and error representation. Channel-specific outcomes are not implied by this per-user contract.

## D12 — No duplicate suppression in iteration 2

- Date: 2026-11-03
- Decision: Process repeated submissions of the same event independently, potentially requesting notifications again for the same users.
- Rationale: Defer duplicate tracking in the initial increment, accepting that whole-operation retries can repeat successful requests.
- Approval: The user answered Q12: "yes".

Discussion context: [round 4 answers](notification-system_DISCUSSION_NOTES.md#2026-11-03--round-4-answers-and-round-5-frontier).

## D13 — Notification service reads channel selections from the user's rule

- Date: 2026-11-03
- Decision: `INotifyService.NotifyAsync` receives the recipient `User` and the event and returns `Task`. Read email/Slack flags from `User.AlertRule` rather than explicit channel parameters. Retain the proposed completion/exception outcome contract and avoid a separate Notification model for this increment.
- Rationale: The existing rule reference already supplies channel selections; explicit parameters would duplicate that data.
- Approval: In response to Q13, the user said "User already has an AlertRule reference whitch contains email/slack flags, use that instead of eplicit input params".

## D14 — Event validation and caller-supplied IDs

- Date: 2026-11-03
- Decision: Accept caller-supplied integer event IDs. Require a known event type and nonblank title/body. Reject invalid events before user lookup or notification requests. Automatic ID allocation remains deferred.
- Rationale: Ensure processing has a classified event with usable content before requesting notifications.
- Approval: The user answered Q14: "yes".

## D15 — Seeded in-memory repository without management endpoints

- Date: 2026-11-03
- Decision: Initialize the in-memory repository from a supplied collection of users and expose retrieval of all users. Do not add user-management endpoints or database persistence in iteration 2.
- Rationale: Supply candidate users directly for this processing increment and its tests.
- Approval: The user answered Q15: "yes".

## D16 — Defer user retrieval error handling

- Date: 2026-11-03
- Decision: Defer user retrieval error handling. Support an empty user list as a valid minimum case. Do not add a lookup-error recovery contract in iteration 2.
- Rationale: Keep this increment focused on the simple in-memory user source.
- Approval: In response to Q16, the user said "skip user retreival error for now. use empy list at minimum."
- Scope clarification: This does not require catching repository exceptions and replacing them with an empty list; retrieval failures are outside the current specified behavior.

Discussion context: [round 5 answers](notification-system_DISCUSSION_NOTES.md#2026-11-03--round-5-answers-and-round-6-frontier).

## D17 — Per-matching-user result entries

- Date: 2026-11-03
- Decision: Return a user ID and success/failure for each matching user's service call. Skip users without a rule or with a nonmatching rule and omit them from results. Empty input or no matches yields an empty result. Defer detailed error messages and per-channel outcomes.
- Rationale: Expose the agreed per-user outcomes without expanding delivery reporting.
- Approval: The user answered "yes to all" to Q17 and Q18.

## D18 — Sequential notification requests

- Date: 2026-11-03
- Decision: Process matching users sequentially, awaiting each notification-service call before starting the next.
- Rationale: Keep execution simple while continuing after individual failures.
- Approval: The user answered "yes to all" to Q17 and Q18.

Discussion context: [round 6 answers](notification-system_DISCUSSION_NOTES.md#2026-11-03--round-6-answers-and-consolidated-spec).

## D19 — Event-only increment and constructor/property test boundary

- Date: 2026-11-03
- Decision: Implement Event only for the current increment. Test through its public constructor and properties; defer agreement on the processing boundary until that increment begins.
- Rationale: Deliver the event data and validation independently of recipient lookup and notification processing.
- Approval: After clarification recommending boundary 1 only for the Event increment, the user replied: "Agree. Stick to Event for now."

Discussion context: [Event-only approval and implementation](notification-system_DISCUSSION_NOTES.md#2026-11-03--event-only-approval-and-implementation).

## D20 — Repository retrieval test boundary for AC3

- Date: 2026-11-03
- Decision: Test AC3 through the repository's public retrieval method using the real in-memory implementation. Cover populated and empty collections in separate red -> green -> refactor slices; processing and delivery remain outside this boundary.
- Rationale: Verify that seeded users are retrievable without coupling tests to internal storage.
- Approval: In response to the proposed boundary and populated-first sequence, the user replied: "OK. Red test then," authorizing only the first red stage for now.

Discussion context: [AC3 retrieval boundary approved and first red test](notification-system_DISCUSSION_NOTES.md#2026-11-03--ac3-retrieval-boundary-approved-and-first-red-test).

## D21 — Public processing boundary for AC4

- Date: 2026-11-03
- Decision: Exercise the public processing method using a real in-memory repository and a recording INotifyService test double. The first example includes matching, nonmatching, and rule-less users and observes only the matching user's notification request with the supplied event.
- Rationale: Verify recipient selection through observable notification requests without testing internal helpers or sending actual messages.
- Approval: After the boundary was explained, the user requested: "Make a red test". Only the red stage is authorized for this increment.

Discussion context: [AC4 processing boundary approved and red test added](notification-system_DISCUSSION_NOTES.md#2026-11-03--ac4-processing-boundary-approved-and-red-test-added).
