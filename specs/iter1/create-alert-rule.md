# Create and update a user's alert rule

Status: Draft for review. The user ended the interview on 2026-11-03; unresolved behavior is listed explicitly below. Rule-instance behavior for AC3–AC7 is implemented with passing tests after observed failures. User-dependent behavior remains deferred.

## Goal

Allow a user to create and update their single alert rule describing which event types they want notifications about and which personal notification channels to use.

## Confirmed requirements

- Creating an alert rule is the first functionality to specify, confirmed by the user on 2026-11-02.
- The original task produced specifications; on 2026-11-03 the user authorized implementing the first failing test for AC5 at the rule-instance boundary.
- Each user can have at most one rule, and can update that rule.
- An event matches if its type is any one of the rule's selected event types.
- A valid rule selects at least one predefined event type and enables at least one notification channel. Unknown event types are rejected.
- The creation/update interface is an implementation detail to address later in a separate spec. This spec describes functional behavior without selecting a form, API, or framework.

## Requested rule structure

The user specified one rule record supporting both email and Slack, rather than separate records for each channel:

- `userId`: the user associated with the rule; the user ID type is `int` under the user spec. This association and identifier assignment remain unimplemented.
- `Id`: an integer identifier for the rule itself, explicitly requested on 2026-11-03. The constructor accepts this ID from its caller; automatic assignment is deferred. This is distinct from the still-unimplemented user association above.
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

Current increment: following the observed red tests and the user's request to implement AC3/AC4, retain the channel flags in read-only `EmailEnabled`/`SlackEnabled` properties. Keep a copy of the validated event-type selection and implement `Matches(string eventType)` by testing membership in that selection. These check channel retention and any-selected-type matching without persistence or delivery.

AC7 current increment: following the observed red test and the user's request to implement, reject construction when both notification channels are disabled with `ArgumentException`. No other rule behavior is added.

Current increment: test AC5 through the public `AlertRule` constructor, without a user record. Constructing with an empty event-type array, email enabled, and Slack disabled throws `ArgumentException`. After observing red, the user requested functionality; the constructor now rejects an empty array. The exception type is the initial test contract, not a separately approved API failure contract.

For later implementation, after review and resolution of any open questions needed by the chosen increment:

AC6 increment: following its observed red test and the user's request to make it green, validate each selected event type against the three predefined values. Throw `ArgumentException` if any selected type is unknown, including in a mixed selection. No other rule behavior is added in this increment.

1. Derive behavior tests from AC1–AC10 without coupling them to a UI or API.
2. Observe the relevant failures before implementing rule creation, predefined-type validation, and channel validation.
3. Implement the one-rule-per-user constraint and valid updates according to the agreed lifecycle behavior.
4. Verify any-selected-type matching against all three predefined types.
5. Add the interaction interface according to its separate spec and run the applicable build and tests.

## Verification

Integer-ID increment (2026-11-03): the new public-constructor/property test failed with expected `17`, actual `0` before retaining the supplied integer ID. Updated all existing constructor calls to supply an ID. Final `dotnet test src/notif_be_tests/notif_be_tests.csproj` passed all 11 cases, including the existing rule behavior and new user tests. `dotnet build src/notif_be/notif_be.csproj` succeeded with zero warnings and errors. See the [user ID decision](user.DECISIONS.md#d10--integer-identifiers).

AC3/AC4 green step (2026-11-03): retained channel preferences and implemented selected-event-type matching. Ran `dotnet test src/notif_be_tests/notif_be_tests.csproj`: both projects built and all 8 cases passed, covering rule-instance behavior for AC3–AC7.

AC3/AC4 red step (2026-11-03): ran `dotnet test src/notif_be_tests/notif_be_tests.csproj`. Both projects built. AC3 failed because the channel properties returned `(false, false)` instead of `(true, true)`. All 3 AC4 cases failed with `NotImplementedException`: `breaking-news` and `natural-disaster` should match, while `market-movement` should not. The 4 existing AC5/AC6/AC7 cases passed (4 failed, 4 passed). No channel-retention or matching implementation was added.

AC7 green step (2026-11-03): added the constructor guard for both channels disabled. Ran `dotnet test src/notif_be_tests/notif_be_tests.csproj`: both projects built and all 4 cases passed (AC5, AC6, and AC7).

AC7 red step (2026-11-03): ran `dotnet test src/notif_be_tests/notif_be_tests.csproj`. Both projects built. `Alert_rule_rejects_both_notification_channels_disabled` failed as intended because no `ArgumentException` was thrown. The 3 existing AC5/AC6 cases passed (1 failed, 3 passed). Production code is unchanged.

AC6 red step (2026-11-03): added `Alert_rule_rejects_an_unknown_event_type` at the same public constructor seam, with an unknown type alone and alongside `breaking-news`. Ran `dotnet test src/notif_be_tests/notif_be_tests.csproj`: both projects built; both AC6 cases failed because no `ArgumentException` was thrown, and AC5 passed (2 failed, 1 passed). Production code is unchanged; unknown-event-type validation awaits the green step.

AC6 green step (2026-11-03): added predefined-event-type validation in the constructor. Ran `dotnet test src/notif_be_tests/notif_be_tests.csproj`: both projects built and all 3 cases passed (AC5 and both AC6 cases).

On 2026-11-03, ran `dotnet test src/notif_be_tests/notif_be_tests.csproj --filter FullyQualifiedName~Alert_rule_rejects_an_empty_event_type_selection`. Both projects built successfully. The single AC5 test failed as intended: `Assert.Throws() Failure: No exception was thrown` (expected `ArgumentException`). After adding empty-array validation, ran `dotnet test src/notif_be_tests/notif_be_tests.csproj`: both projects built and the one test passed. Other acceptance criteria have not been tested or implemented.

## Records

- [Discussion notes](create-alert-rule_DISCUSSION_NOTES.md)
- [Approved decisions](create-alert-rule.DECISIONS.md)
