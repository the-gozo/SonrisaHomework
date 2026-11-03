# Create an alert rule — discussion notes

## 2026-11-02 — Functionality selected and minimum information proposed

### Context

After redirecting the interview from iteration scope to the first functionality, the user accepted creating an alert rule as that functionality. They then asked what minimum information a rule has.

### Proposals and reasoning

- Start with a matching condition and notification destination: these express when the user wants a notification and where it should go.
- Distinguish user-supplied information from system-assigned identity and ownership. User identification has not been decided; this does not imply a login feature or authentication technology.
- Defer optional rule metadata and controls until justified by an agreed scenario.
- The earlier scope recommendations remain unanswered and unapproved.

### Approved decisions

The user approved the selection of the first functionality. See [D1](create-alert-rule.DECISIONS.md#d1--first-functionality). No technical choices or rule fields have been approved.

### Open questions and next steps

Agree on the minimum rule information, then explore the matching condition, destination configuration, ownership, validation, and creation behavior through dependent interview questions.

## 2026-11-03 — One rule with event types and channel flags

### Context and correction

The user requested one rule record for both email and Slack, containing `userId`, an array of event-type strings, and a boolean for each channel. This replaces the assistant's earlier proposal of one destination per rule; that earlier proposal was never approved.

### Reasoning and proposals

- A single rule avoids maintaining duplicate event selections for email and Slack.
- Proposed boolean names are `emailEnabled` and `slackEnabled`; exact names are not approved yet.
- Boolean flags select channels but do not identify destination addresses. Recommend resolving destinations from configuration associated with the user, subject to user agreement.
- Distinguish predefined event types from arbitrary keyword matching. Recommend matching any selected type from a predefined list, subject to user agreement.
- Explicit flags are adequate for the two current channels. Adding another channel would require a new field and corresponding behavior; the channel representation can be revisited when that requirement becomes concrete.

### Approved decisions

See [D2](create-alert-rule.DECISIONS.md#d2--one-rule-supports-both-channels).

### Open questions and next steps

Resolve event-type meaning and where channel destinations are configured, then specify validation, ownership enforcement, record identity, and observable creation behavior. No application implementation has begun.

## 2026-11-03 — Predefined types accepted; destinations need explanation

### Context

The user agreed to creating a predefined event-type list and said they did not understand the question about destination configuration.

### Proposals and reasoning

- Propose `breaking-news`, `market-movement`, and `natural-disaster` as the initial list, reflecting the examples in the project brief. These values are not approved yet.
- Explain destination configuration with two rules belonging to the same user: both can use a shared email address and Slack destination, while their event selections and channel flags differ.
- Clarify that choosing a channel with a boolean is distinct from supplying its address. User-level destination configuration remains a proposal, not a settled requirement or a login/profile implementation decision.

### Approved decisions

See [D3](create-alert-rule.DECISIONS.md#d3--predefined-event-types).

### Open questions and next steps

Confirm the proposed initial list and explain the destination choice before seeking an answer. Matching semantics and validation remain to be confirmed.

## 2026-11-03 — Clarification: destinations identify each individual user

### Context and correction

The user responded to Q4: “No, each user has its own email address and slack user.” The previous example of a Slack channel did not establish the intended destination. The requirement is personal email and a Slack user identity per user; do not interpret this response as requiring destinations to be entered separately on each rule.

### Approved decisions

See [D4](create-alert-rule.DECISIONS.md#d4--personal-email-and-slack-identity).

### Open questions and next steps

Confirm whether notifications to the Slack user mean direct messages. Q5 about the proposed three event types remains unanswered. Configuration, storage, matching, and validation still require discussion.

## 2026-11-03 — Event list and Slack direct messages confirmed

### Context and approved decisions

The user answered yes to Q4 (Slack direct messages) and Q5 (the three proposed event types). See [D5](create-alert-rule.DECISIONS.md#d5--initial-event-type-list-and-slack-direct-messages). These answers resolve the questions left open in the previous entry.

### Proposals and reasoning

- Recommend matching any selected event type, rather than requiring all selected types.
- Recommend requiring at least one known event type and at least one enabled channel when creating a rule.
- Recommend allowing multiple rules per user, so different event selections can use different channel combinations without separate records for email and Slack within a single rule.
- Recommend a small user-facing creation form as the entry point, subject to agreement; no framework or technology is selected by that proposal.

### Open questions and next steps

Ask about matching, minimum validity, multiple rules per user, and the creation interface. Derive subsequent questions about duplicate rules, ownership, identity, persistence, and destination readiness from the answers. No application code has been implemented.

## 2026-11-03 — Matching, validity, and one updatable rule confirmed

### Context and corrections

The user approved any-selected-type matching and the proposed minimum validity requirements. They rejected multiple rules per user, requiring one rule with an update option. They also directed that the interaction interface be addressed later in a separate spec, rather than in this functional interview. The previous proposals of multiple rules and a web form were not approved.

### Approved decisions

- [D6 — Matching and minimum validity](create-alert-rule.DECISIONS.md#d6--matching-and-minimum-validity).
- [D7 — One updatable rule per user](create-alert-rule.DECISIONS.md#d7--one-updatable-rule-per-user).
- [D8 — Defer the interaction interface](create-alert-rule.DECISIONS.md#d8--defer-the-interaction-interface).

### Open questions and next steps

Keep questions functional: what happens before a rule exists, when creation is attempted again, when an update is invalid, who can change the rule, and whether a missing notification destination prevents enabling its channel. Defer presentation and implementation technology. No application implementation has begun.

## 2026-11-03 — Interview stopped; draft consolidated

### Context

The user explicitly stopped the grilling and stated that enough information exists for a draft. No answers were given to Q10–Q13.

### Result

- Consolidated approved requirements into the feature draft and added traceable acceptance scenarios and a future implementation plan.
- Retained lifecycle, invalid-update, ownership, and missing-destination behavior as open questions. The assistant's recommendations for Q10–Q13 are not approved decisions.
- Kept interface design deferred to a separate spec and made no application implementation changes.
- Preserved the discussion history and existing approved-decision entries. The draft does not imply final approval of the complete spec.

### Next steps

The draft is available for review. Do not continue the interview unless the user requests it.

## 2026-11-03 — First failing test at the rule-instance boundary

The user rejected starting with AC1 because no user exists yet and requested rule-instance validation. Proposed AC5: an empty event-type selection is rejected through the rule's public constructor or factory. The user then explicitly requested implementing that test, confirming this seam. See [D9](create-alert-rule.DECISIONS.md#d9--first-test-at-the-rule-instance-boundary).

Added one constructor test with email enabled and Slack disabled, a backend project reference, and an empty `AlertRule` constructor scaffold. The test expects `ArgumentException`; this is an implementation choice for this test, not a separately approved failure contract. Replaced the template test. The targeted test built and failed because no exception was thrown. Next step: implement only the validation needed to pass AC5 when requested. User identity and HTTP contracts remain deferred.

## 2026-11-03 — AC5 green step

The user requested implementing functionality, followed by committing and pushing. Added a constructor guard rejecting an empty event-type array with `ArgumentException`. Ran the full test project: both projects built and the single test passed. Remaining acceptance criteria stay outside this increment.

## 2026-11-03 — AC6 red step

The user requested the next acceptance criterion in the same manner, with the red test first. Continued at the agreed public rule-constructor seam, without users or persistence. Added one theory covering an unknown event type alone and alongside a known type, as AC6 requires. Both cases expect `ArgumentException`, following the existing test contract. The full test run built successfully: AC5 passed and both AC6 cases failed because no exception was thrown. Production code remains unchanged. Next step is unknown-type validation when requested.

## 2026-11-03 — AC6 green step

The user requested making the AC6 test green. Added a constructor check that rejects any event type outside `breaking-news`, `market-movement`, and `natural-disaster`, throwing `ArgumentException` as required by the existing test. The full test project built successfully and all 3 cases passed. No additional acceptance criteria were implemented; the next increment remains to be selected.

## 2026-11-03 — AC7 red step

The user requested a red test for AC7. Continued at the agreed public rule-constructor seam. The test uses `breaking-news` to satisfy event-type validation, disables both channels, and expects `ArgumentException`, following the existing validation test contract. The full test run built successfully: AC7 failed because no exception was thrown; all 3 AC5/AC6 cases passed. Production code is unchanged. Next step: implement channel validation when requested.

## 2026-11-03 — AC7 green step

The user requested implementation. Added a constructor guard throwing `ArgumentException` when both email and Slack are disabled. Ran the full test project: both projects built and all 4 cases passed. No additional acceptance criteria were implemented.

## 2026-11-03 — Remaining criteria without users: AC3 and AC4 red tests

The user explicitly requested failing tests for all remaining acceptance criteria that do not require a user, after reviewing the AC3–AC7 list and discussing AC4 matching. Continued at the public rule-instance boundary: channel properties expose both preferences on one instance (AC3), and `Matches` exposes any-selected-type matching (AC4). This catches rule behavior without testing users, persistence, or delivery.

Added an AC3 test for both channel flags enabled and an AC4 theory with the spec's three literal event-type/expected-result pairs. Added only public member scaffolding so the tests compile: unassigned channel properties and a matching method throwing `NotImplementedException`. Exact member names are implementation choices, not separately approved technical decisions. This request explicitly asks for both red criteria together; implementation remains deferred rather than proceeding through the normal one-slice green step.

Ran the full test project: both projects built, the 4 new cases failed for missing behavior, and the 4 existing cases passed. Next step: implement AC3 and AC4 when requested.

## 2026-11-03 — AC3/AC4 green step

The user requested implementation. The constructor now retains both channel flags and copies the validated event-type array. `Matches` returns whether the incoming event type is in that selection. Ran the full test project: both projects built and all 8 cases passed. User-dependent behavior, persistence, and delivery remain outside this increment.
