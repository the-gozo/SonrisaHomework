# User — approved decisions

## D1 — A user is a person

- Date: 2026-11-03
- Decision: A user represents a human individual.
- Rationale: Establish the entity's meaning before specifying identity and lifecycle behavior.
- Approval: The user explicitly stated, “No, user is a human individual. A person.”
- Scope: This resolves the term only; it does not approve lifecycle, authentication, or administrator behavior.

Discussion context: [discussion notes](user_DISCUSSION_NOTES.md#2026-11-03--user-means-a-person). Existing related constraints: [alert-rule decisions](create-alert-rule.DECISIONS.md).

## D2 — Minimal user class

- Date: 2026-11-03
- Decision: Define a user class with an email and a reference to an alert rule in this increment.
- Rationale: Follow the user's requested small entity scope rather than broadening into identity and account workflows.
- Approval: The user stated, “I only want a user class with reference to a rule, and an email,” and requested recommendations for other properties.
- Unresolved: Additional properties, identifier choices, rule optionality, email constraints, and Slack identity timing.

Discussion context: [minimal class scope](user_DISCUSSION_NOTES.md#2026-11-03--minimal-class-scope).

## D3 — Stable user identity

- Date: 2026-11-03
- Decision: Add a stable `Id` to the user class, independent of email.
- Rationale: Preserve the person's identity when their email changes and support the alert-rule spec's anticipated user association.
- Approval: The user answered “all yes” to Q3–Q5, including Q3 recommending a stable identifier.
- Unresolved: Identifier type and assignment.

## D4 — Defer Slack identity property

- Date: 2026-11-03
- Decision: Defer the Slack identity property until Slack integration work.
- Rationale: Keep this class increment focused; decide the destination representation when integration requires it.
- Approval: The user answered “all yes” to Q3–Q5, including Q4 recommending this deferral.
- Scope: The existing personal Slack direct-message requirement is retained.

## D5 — Optional alert rule

- Date: 2026-11-03
- Decision: A user may exist before creating an alert rule; the rule reference is optional.
- Rationale: Creating the person's record need not require choosing notification preferences.
- Approval: The user answered “all yes” to Q3–Q5, including Q5 recommending optionality.
- Unresolved: How a rule is attached and subsequently updated.

Discussion context for D3–D5: [identifier, Slack deferral, and optional rule](user_DISCUSSION_NOTES.md#2026-11-03--identifier-slack-deferral-and-optional-rule).

## D6 — Basic email validity

- Date: 2026-11-03
- Decision: Require a nonblank email with basic address-format validation when creating a user.
- Rationale: Reject missing or malformed addresses without introducing email ownership verification.
- Approval: The user answered “q7: yes.”
- Unresolved: The exact validation boundary and email uniqueness.

## D7 — Defer ID creation and email changes

- Date: 2026-11-03
- Decision: Defer identifier type/assignment and email-change behavior.
- Rationale: The user stated these are unnecessary at this stage.
- Approval: The user said, “q6, q8, defer these. not needed at this stage.”
- Scope: The stable `Id` property remains approved. Automatic `Guid` creation and mutable-email behavior were proposals and are not approved.

Discussion context for D6–D7: [email validation and deferred behavior](user_DISCUSSION_NOTES.md#2026-11-03--email-validation-and-deferred-behavior).

## D8 — Email validation examples

- Date: 2026-11-03
- Decision: Accept `alice@example.com`; reject null, blank, `alice`, and `alice@`. Do not add mailbox verification or provider-specific restrictions.
- Rationale: Make basic validation concrete without specifying an exhaustive email standard.
- Approval: The user answered “both yes” to Q9 and Q10, including Q9's validation examples.

## D9 — Defer uniqueness and rule operations

- Date: 2026-11-03
- Decision: Defer email uniqueness checks and rule assignment/update operations. Keep this increment to `Id`, `Email`, the optional rule reference, and email validation.
- Rationale: Match the user's requested small class scope.
- Approval: The user answered “both yes” to Q9 and Q10, including Q10's scope recommendation.

Discussion context for D8–D9: [final scope boundaries](user_DISCUSSION_NOTES.md#2026-11-03--final-scope-boundaries).

## D10 — Integer identifiers

- Date: 2026-11-03
- Decision: Use `int` for `Id` in both `User` and `AlertRule`.
- Rationale: Follow the user's explicit identifier representation for both entities.
- Approval: The user stated, “Use int as Id in both AlertRule and User.”
- Scope: Supersedes the type deferral in D7. Automatic identifier generation, allocation, persistence, and uniqueness remain unspecified.

Discussion context: [integer IDs and AC1 implementation](user_DISCUSSION_NOTES.md#2026-11-03--integer-ids-and-ac1-implementation).
