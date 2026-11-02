# Repository guidance

## Brief

The task is to create a working solution for this requirement:

"We want users to be able to set up alerts so they get notified when something important happens in the world — like breaking news, market movements, natural disasters, that kind of thing. Should work for both email and Slack. Make it flexible enough that we can add more channels later. We need an admin view too."

This is underspecified on purpose. We need to work out a flexible solution by filling the gaps.

## Workflow

- Read `workflow.md` before starting work. It describes the project's iterative, specification-driven approach.
- Summarize the task and intended changes before implementing.
- Work through specs in their defined iteration order. Keep each deliverable small and reviewable.
- Treat each feature's spec as its source of truth for requirements, acceptance criteria, implementation plan, and verification.
- Update the relevant spec when requirements or implementation context change. Ask for clarification when a missing requirement blocks progress.
- Favor a working skeleton in early iterations, then extend it incrementally.

## Implementation

- Prefer TDD: derive tests from acceptance criteria, observe the relevant failure, then implement the behavior and refactor.
- Keep changes focused on the current task and preserve unrelated work.
- Follow existing code and naming conventions as the codebase develops.
- Avoid overengineering. Introduce abstractions and dependencies when current requirements justify them, and defer decisions until needed.
- Read applicable `SKILL.md` instructions when using repository skills.

## Verification and handoff

- Run the relevant tests and available build checks for the changed behavior. Document verification in the spec.
- Do not claim a check passed unless it was run successfully. Report checks that could not be run and explain why.
- Summarize what changed, how it was verified, and any unresolved issues for human review.
- When blocked, explain the blocker and ask a focused question.
- Update this guidance and relevant skills when requested to capture review feedback.

## Project setup

The repository currently has no application solution, build commands, or test suite. Once these exist, document the actual setup, build, and test commands here.
