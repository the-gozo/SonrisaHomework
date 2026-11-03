# User

Status: AC1 implemented following the user's TDD request and integer-ID clarification on 2026-11-03. Email validation in AC3 and AC4 remains unimplemented.

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
- Use `int` for `Id` in both `User` and `AlertRule`, as requested on 2026-11-03. This increment accepts caller-supplied IDs; automatic assignment and email-change behavior remain deferred.
- Basic validation accepts `alice@example.com` and rejects null, blank, `alice`, and `alice@`.
- Email uniqueness checks and rule assignment/update operations are deferred. This increment covers the three properties and email validation.

## Acceptance criteria

| ID | Scenario | Expected behavior |
| --- | --- | --- |
| AC1 | Inspect the user class's intended data. | It represents a person with an integer `Id`, `Email`, and an optional reference to one alert rule. Automatic identifier assignment remains deferred. |
| AC2 | Create a user with `alice@example.com` and no rule. | The email is accepted and the absence of a rule is allowed. |
| AC3 | Attempt creation with a null, empty, or whitespace-only email. | The email is rejected. |
| AC4 | Attempt creation with `alice` or `alice@`. | The malformed email is rejected. |

These examples establish the agreed minimum validation boundary; they do not specify a comprehensive email-format standard or an exception/API failure contract.

## Deferred work

- Automatic identifier assignment.
- Email changes and uniqueness checks.
- Rule assignment/update operations beyond the optional reference.
- Slack identity property and integration.
- Login, registration flows, administrator permissions, persistence, and interaction interfaces.

The overall project still requires personal Slack destinations and one updatable rule per user. This class increment does not implement those workflows.

## Implementation plan

AC1 increment: test through the public constructors and properties. Retain caller-supplied integer IDs in both classes. Construct `User` with an email and an optional `AlertRule` reference, exposing read-only properties. Work one failing test and minimal implementation at a time. Keep email validation for AC3–AC4 and rule-update operations deferred. Preserve existing alert-rule behavior and run the backend build and test project.

## Verification

AC1 red steps (2026-11-03): targeted tests built and failed for the missing user implementation (`NotImplementedException`) and then the missing supplied rule reference (actual `null`). The related `AlertRule` identifier test first failed with expected `17`, actual `0`, then passed after retaining the supplied ID. Final verification is recorded below.

AC1 green step (2026-11-03): `dotnet test src/notif_be_tests/notif_be_tests.csproj` passed all 11 cases. `dotnet build src/notif_be/notif_be.csproj` succeeded with zero warnings and errors. Tests observe caller-supplied IDs, the email, and absent/present optional rule references through public properties. The valid-email/no-rule case also exercises AC2; rejection criteria AC3–AC4 remain pending.

## Records

- [Discussion notes](user_DISCUSSION_NOTES.md)
- [Approved decisions](user.DECISIONS.md)
