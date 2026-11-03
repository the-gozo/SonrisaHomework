# Create an alert rule — approved decisions

## D1 — First functionality

- Date: 2026-11-02
- Decision: Specify creating an alert rule as the first functionality.
- Rationale: Establish how users express what matters to them and where they want notifications before specifying dependent behavior.
- Approval: The user answered “Yes” to the recommendation to start with creating an alert rule, then asked about its minimum information.

## D2 — One rule supports both channels

- Date: 2026-11-03
- Decision: Use one rule record with `userId`, an array of event-type strings, an email boolean, and a Slack boolean.
- Rationale: Express both channel selections in one rule without duplicating its event selection in separate channel-specific rules.
- Approval: The user explicitly requested a single rule record for email and Slack and specified these fields, rejecting the assistant's one-destination-per-rule proposal.
- Unresolved: Exact boolean names, event-type semantics, destination configuration, identity, ownership enforcement, and validation.

## D3 — Predefined event types

- Date: 2026-11-03
- Decision: Provide a predefined list from which rule event types are selected.
- Rationale: Give event-type selections a shared vocabulary rather than leaving them as arbitrary strings.
- Approval: The user said, “Yes, create a predefined list of event types.”
- Unresolved: The initial list, matching semantics, and validation behavior. Destination configuration was not approved; the user requested an explanation of that question.

## D4 — Personal email and Slack identity

- Date: 2026-11-03
- Decision: Each user has their own email address and Slack user identity, used as that user's notification destinations.
- Rationale: Notifications target the individual user identified by the rule's `userId`.
- Approval: In response to the destination question, the user clarified, “No, each user has its own email address and slack user.”
- Unresolved: How these details are configured and stored, Slack identity representation, and whether Slack notifications are direct messages. Do not infer a shared channel or destination for multiple users.

## D5 — Initial event-type list and Slack direct messages

- Date: 2026-11-03
- Decision: Use `breaking-news`, `market-movement`, and `natural-disaster` as the initial predefined event types. Slack notifications are direct messages to the user's Slack identity.
- Rationale: The event types cover the three examples in the brief; direct messages target the individual user specified in D4.
- Approval: The user answered “q4: yes” to direct messages and “q5: yes” to the proposed initial list.
- Unresolved: Matching semantics, validation, user rule cardinality, creation interface, and how destinations and user identity are configured. Delivery implementation is not implied by these destination semantics.

## D6 — Matching and minimum validity

- Date: 2026-11-03
- Decision: Match any selected event type. Require at least one predefined event type and at least one enabled channel; reject unknown event types.
- Rationale: Selections express alternative interests, and every saved rule must express an actionable preference.
- Approval: The user answered yes to Q6 and Q7.

## D7 — One updatable rule per user

- Date: 2026-11-03
- Decision: Each user can have at most one rule and can update it.
- Rationale: Keep the user's event selection and channel preferences in one record, following the user's requested model.
- Approval: The user rejected multiple rules in Q8: “one user with one rule. there is an option to update its rule”.
- Unresolved: Initial absence of a rule, behavior of repeat creation, and update failure behavior.

## D8 — Defer the interaction interface

- Date: 2026-11-03
- Decision: Address the creation/update interface later in a separate spec; do not select a UI or API in this functional interview.
- Rationale: Resolve the functionality before its presentation and implementation details.
- Approval: In response to Q9, the user explicitly called it an implementation detail for a later, separate spec.

Discussion context: [discussion notes](create-alert-rule_DISCUSSION_NOTES.md).

## D9 — First test at the rule-instance boundary

- Date: 2026-11-03
- Decision: Start with AC5 through the rule's public constructor or factory, without requiring a user record.
- Rationale: Exercise rule-instance validation before user functionality exists.
- Approval: The user requested an AC that does not require a user, specified rule-instance validation, and then instructed implementing the proposed AC5 test.
- Scope: One failing test; validation implementation is a subsequent step. Exact exception type was not explicitly approved.
