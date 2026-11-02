# Approved workflow decisions

These entries record choices explicitly approved in the conversation. No application architecture or technology choices have been approved yet.

## D1 - Derive specs from the project brief

- Date: 2026-11-02
- Status: Approved
- Decision: Derive specs from the underspecified description in root `AGENTS.md`, identify gaps, and clarify requirements as needed. Each spec is a Markdown file and serves as the source of truth for its requirements, acceptance criteria, implementation plan, and verification.
- Rationale: Develop the incomplete brief into implementable requirements while allowing specs to evolve as more context becomes available.
- Approval: The user corrected the proposed task-based approach to a brief-based approach, then explicitly requested adding the revised instruction to `AGENTS.md`.

## D2 - Organize specs into iterations starting at iter0

- Date: 2026-11-02
- Status: Approved
- Decision: Group specs in sequential folders under `specs/`, beginning with `specs/iter0/`. Each iteration delivers a small, reviewable increment, with early iterations favoring a working skeleton.
- Rationale: Make progress incremental and reviewable while leaving room for later functionality and requirement changes.
- Approval: The user approved the iteration-folder instruction and subsequently explicitly requested the first iteration as `iter0`.

## D3 - Maintain discussion and decision records

- Date: 2026-11-02
- Status: Approved
- Decision: Keep two companion records for each spec in its iteration folder. Use `<spec-name>_DISCUSSION_NOTES.md` for dated summaries of context, proposals, reasoning, open questions, and next steps. Append entries after substantive discussions, preserve earlier entries, and record corrections in new entries. Use `<spec-name>.DECISIONS.md` for explicitly approved technical choices and their rationale, linked from the relevant discussion entries.
- Rationale: Preserve the reasoning behind evolving specs while distinguishing proposals from approved choices.
- Approval: The user explicitly requested adding the final combined instruction to `AGENTS.md`.
- Workflow record filenames: For this initial workflow pair, the user's later request for a `workflow_` prefix is applied as `workflow_DISCUSSION_NOTES.md` and `workflow_DECISIONS.md`.

Discussion context: [Workflow discussion notes](workflow_DISCUSSION_NOTES.md).
