# User

Status: Draft awaiting confirmation of shared understanding. No application implementation authorized by this interview.

## Goal

Define the user entity needed by iteration 1's alert-rule behavior.

## Existing constraints

The approved [alert-rule decisions](create-alert-rule.DECISIONS.md) establish that each user has at most one updatable alert rule, a personal email address, and a Slack identity used for direct messages. Destination configuration and user identification remain unresolved. The interaction interface is deferred to a separate spec.

## Confirmed requirements

- A user is a human individual: a person.
- This increment defines a user class with an email and a reference to an alert rule.
- Add a stable `Id` independent of the email address.
- A user can exist without an alert rule; the rule reference is optional.
- Defer the Slack identity property until Slack integration work. The overall personal Slack destination requirement remains in force.
- Require a nonblank email with basic address-format validation. This does not confirm ownership or deliverability.
- Identifier type/assignment and email-change behavior are deferred at the user's request; no automatic `Guid` generation or email-update behavior is approved.
- Basic validation accepts `alice@example.com` and rejects null, blank, `alice`, and `alice@`.
- Email uniqueness checks and rule assignment/update operations are deferred. This increment covers the three properties and email validation.

## Acceptance criteria

| ID | Scenario | Expected behavior |
| --- | --- | --- |
| AC1 | Inspect the user class's intended data. | It represents a person with `Id`, `Email`, and an optional reference to one alert rule. Identifier representation and assignment remain deferred. |
| AC2 | Create a user with `alice@example.com` and no rule. | The email is accepted and the absence of a rule is allowed. |
| AC3 | Attempt creation with a null, empty, or whitespace-only email. | The email is rejected. |
| AC4 | Attempt creation with `alice` or `alice@`. | The malformed email is rejected. |

These examples establish the agreed minimum validation boundary; they do not specify a comprehensive email-format standard or an exception/API failure contract.

## Deferred work

- Identifier type and assignment.
- Email changes and uniqueness checks.
- Rule assignment/update operations beyond the optional reference.
- Slack identity property and integration.
- Login, registration flows, administrator permissions, persistence, and interaction interfaces.

The overall project still requires personal Slack destinations and one updatable rule per user. This class increment does not implement those workflows.

## Implementation plan

After confirmation of shared understanding and authorization to implement, derive focused tests from AC2–AC4, observe their failures, then implement only the agreed class behavior. Resolve identifier representation before an implementation step that requires it; its deferral does not authorize choosing a type silently. Preserve the existing alert-rule behavior and run the backend build and test project for any code change.

## Verification

Documentation only; no application checks run for this interview. Reviewed the draft against the approved decision record. Application behavior remains unimplemented.

## Records

- [Discussion notes](user_DISCUSSION_NOTES.md)
- [Approved decisions](user.DECISIONS.md)
