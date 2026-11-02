The Workflow

I am going to work on this problem through iterations.
Every iteration produces a deliverable, adding more functionality to the previous one.

The steps will be:
 - Defining specs based on the known functional requirements 
	- These do not have to be exhaustive, since the brief isn't complete either
	- As the project develops, new specs will appear, and those will be added to iterations as well
	- Specs can be updated during development to add more context
	- Every spec serves as the "source of truth" about the feature. It contains the requirements, the acceptance criteria, the implementation plan, and the verification
	- Tests are generated based on the acceptance criteria
 
 - Organizing specs into iterations
	- The first few iterations should produce a walking skeleton that can later be extended with more functionality
	- The scope of an iteration can change during development

- Giving the agent workflow instructions through AGENTS.md and SKILL.md files, such as:
	- Summarize the task before implementing
	- Prefer TDD
	- Produce small, reviewable sets of changes
	- Do not overengineer. Prefer making decisions at the last responsible moment
	- Ask if blocked

- Instructing the agent to start implementing the specs in order
	- Reviewing the changes as a human
	- Making corrections and suggestions and adding those to skills for future use
	- Asking the agent for suggestions as the project develops
 
The workflow is subject to change. Please check the Git history.
