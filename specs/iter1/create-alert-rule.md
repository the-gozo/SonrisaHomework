# Create and update a user's alert rule

Status: Draft for review. The user ended the interview on 2026-11-03; unresolved behavior is listed explicitly below. Application implementation has not started.

## Goal

Allow a user to create and update their single alert rule describing which event types they want notifications about and which personal notification channels to use.

## Confirmed requirements

- Creating an alert rule is the first functionality to specify, confirmed by the user on 2026-11-02.
- This task produces specifications for later implementation, not application code.
- Each user can have at most one rule, and can update that rule.
- An event matches if its type is any one of the rule's selected event types.
- A valid rule selects at least one predefined event type and enables at least one notification channel. Unknown event types are rejected.
- The creation/update interface is an implementation detail to address later in a separate spec. This spec describes functional behavior without selecting a form, API, or framework.

## Requested rule structure

The user specified one rule record supporting both email and Slack, rather than separate records for each channel:

- `userId`: the user associated with the rule; identifier type and assignment remain unresolved.
- `eventTypes`: an array of strings selecting from the predefined values `breaking-news`, `market-movement`, and `natural-disaster`. Matching any selected type is sufficient.
- An email boolean and a Slack boolean. Proposed names: `emailEnabled` and `slackEnabled`.

Each user has at most one rule containing both channel flags. The rule can be updated. This supersedes the assistant's proposals of separate channel-specific rules and multiple rules per user.

Channel destination configuration, ownership enforcement, and creation/update failure behavior still need agreement. User-entered names, descriptions, schedules, priorities, and pause controls have not been approved.

The channel booleans select whether a channel is used. Each user has their own email address and Slack user identity; destinations belong to the user rather than being entered on each rule. Slack notifications are direct messages to that user. How these details are configured or stored remains unresolved.

## Event types

| Value | Meaning |
| --- | --- |
| `breaking-news` | A news event classified as breaking news |
| `market-movement` | A reported financial market movement |
| `natural-disaster` | A reported natural disaster |

These values select categories; they do not express keywords, market thresholds, or geographic filters. Event ingestion and classification behavior have not been specified.

## Acceptance criteria

| ID | Scenario | Expected behavior |
| --- | --- | --- |
| AC1 | A user without a rule creates one with a predefined event type and email enabled. | The user's rule records that selection and channel preference. |
| AC2 | A user without a rule creates one with a predefined event type and Slack enabled. | The user's rule records that selection and channel preference. |
| AC3 | A user selects both email and Slack. | Both flags are stored in the same rule record. |
| AC4 | A rule selects `breaking-news` and `natural-disaster`. | Either selected type matches; `market-movement` does not. |
| AC5 | Creation is attempted with an empty event-type list. | The rule is rejected. |
| AC6 | Creation is attempted with an unknown event type, including alongside a known type. | The rule is rejected. |
| AC7 | Creation is attempted with both channel flags false. | The rule is rejected. |
| AC8 | A user already has a rule. | The system never stores a second rule for that user; the exact repeat-creation response remains open. |
| AC9 | A user updates their rule to a different valid event selection and channel combination. | The rule reflects the new preferences and remains the user's single rule. |
| AC10 | A rule refers to a user's email or Slack destination. | Email targets that user's email address; Slack targets that user through a direct message. Actual delivery is not an acceptance test for rule creation. |

## Open questions

These are unresolved, not implicitly accepted recommendations. No further interview is requested now.

- Whether users start without a rule or receive one automatically.
- Whether repeat creation fails, updates the existing rule, or has another outcome consistent with one rule per user.
- Invalid-update behavior, including whether the existing rule remains unchanged.
- Who can create or update a rule on behalf of a user, including administrator permissions.
- Whether enabling a channel requires its destination to be configured already.
- Exact boolean field names, `userId` representation, and behavior for missing fields or repeated event-type entries.

## Deferred work

- A separate spec for the interaction interface, including any UI or API contract.
- User identification, authentication, and destination configuration.
- Event ingestion, classification, and actual email/Slack delivery.
- Administrator functionality and further channels.
- Technology, persistence, deployment, and detailed failure contracts.

The brief still requires delivery, extensibility, and an admin view for the overall project. This draft does not specify those features.

## Implementation plan

For later implementation, after review and resolution of any open questions needed by the chosen increment:

1. Derive behavior tests from AC1–AC10 without coupling them to a UI or API.
2. Observe the relevant failures before implementing rule creation, predefined-type validation, and channel validation.
3. Implement the one-rule-per-user constraint and valid updates according to the agreed lifecycle behavior.
4. Verify any-selected-type matching against all three predefined types.
5. Add the interaction interface according to its separate spec and run the applicable build and tests.

## Verification

Documentation only. Requirements and acceptance scenarios were checked against the user's recorded answers. Companion-file links were checked locally. No application code, build, or application tests exist for this feature; the scenarios above have not been executed.

## Records

- [Discussion notes](create-alert-rule_DISCUSSION_NOTES.md)
- [Approved decisions](create-alert-rule.DECISIONS.md)
