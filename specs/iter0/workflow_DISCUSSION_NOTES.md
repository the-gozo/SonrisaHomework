# Workflow discussion notes

## 2026-11-02 - Establishing the project workflow

### Context

The project starts from an intentionally underspecified alerting brief in [AGENTS.md](../../AGENTS.md): notify users about important world events through email and Slack, support additional channels later, and provide an admin view. Our discussion so far has focused on how to organize development and retain its rationale.

### Discussion and reasoning

- Created root agent guidance based on [workflow.md](../../workflow.md): work incrementally, prefer TDD, derive tests from acceptance criteria, keep changes reviewable, and avoid premature abstractions.
- Clarified that specs come from the underspecified project description in `AGENTS.md`, rather than from individual tasks. Gaps should be identified and requirements clarified as needed.
- Agreed to represent specs as Markdown files grouped into sequential iteration folders under `specs/`. Early iterations should establish a working skeleton that can be extended.
- Explored available curated skills and conversation archiving. Discussed a custom notes skill and transcript export as possible approaches; neither was selected or implemented.
- Chose per-spec discussion notes and decision records. Notes summarize discussions rather than reproduce complete transcripts. Append dated entries after substantive discussions and preserve history by recording corrections in new entries.
- Distinguished proposals from approved technical choices. Decision records require explicit user approval and retain the rationale; relevant discussion entries link to those records.
- Added these workflow and recordkeeping instructions to `AGENTS.md` at the user's request.
- Discussed Git support, including the `yeet` skill. No skill installation was requested. An attempted commit and push stopped when the Git permission request was declined; that attempt created no commit and performed no push.
- The user then requested the first iteration as `iter0` and these two records with a `workflow_` prefix. Accordingly, this pair uses `workflow_DISCUSSION_NOTES.md` and `workflow_DECISIONS.md`; the general per-spec naming convention remains documented in `AGENTS.md`.

### Approved decisions

See [approved workflow decisions](workflow_DECISIONS.md):

- [Derive specs from the project brief](workflow_DECISIONS.md#d1---derive-specs-from-the-project-brief).
- [Organize specs into iterations starting at iter0](workflow_DECISIONS.md#d2---organize-specs-into-iterations-starting-at-iter0).
- [Maintain discussion and decision records](workflow_DECISIONS.md#d3---maintain-discussion-and-decision-records).

### Open questions

- The feature requirements, acceptance criteria, and implementation scope for the initial application increment remain to be defined.
- No application architecture, framework, persistence approach, event source, or delivery integration has been approved in this discussion.

### Next steps

- Refine the brief into initial feature specs with requirements, acceptance criteria, implementation plans, and verification.
- Discuss unresolved technical choices and record them as decisions only after explicit approval.
- Continue appending dated notes as the workflow evolves.
