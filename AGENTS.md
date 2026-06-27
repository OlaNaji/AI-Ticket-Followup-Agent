# AGENTS.md

## Project

AI Ticket & Project Follow-up Agent is a small, portfolio-quality ASP.NET Core business workflow application.

The goal is to prove professional backend engineering habits while using Codex as an AI coding partner: clear architecture, database design, tests, Git workflow, documentation, CI/CD, and safe AI-agent behavior.

## Coaching mode

- Treat the developer as an active learner, not a passive passenger.
- Before substantial changes, ask the learner to predict the design or behavior.
- Prefer small, reviewable changes over large rewrites.
- Explain the plan before editing.
- After editing, summarize the diff and require the learner to inspect and explain it before moving on.
- Do not create broad changes without explicit approval.
- Keep the project small, finishable, and portfolio-focused.

## Architecture

Use a compact modular monolith with clean boundaries.

Planned structure:

```text
src/
  FollowUpAgent.Api/
  FollowUpAgent.Application/
  FollowUpAgent.Domain/
  FollowUpAgent.Infrastructure/
tests/
  FollowUpAgent.Application.Tests/
  FollowUpAgent.Api.IntegrationTests/
docs/
```

Project responsibilities:

- `FollowUpAgent.Api`: HTTP endpoints, authentication setup, dependency injection, configuration, and request/response boundaries.
- `FollowUpAgent.Application`: use cases, DTOs, interfaces, validation orchestration, and workflow coordination.
- `FollowUpAgent.Domain`: entities, value objects, enums, domain rules, and business invariants.
- `FollowUpAgent.Infrastructure`: EF Core persistence, database configuration, external AI provider implementations, audit persistence, and other external integrations.
- `tests`: unit and integration tests.
- `docs`: architecture decisions, diagrams, API notes, and demo material.

Dependency rule:

```text
Api -> Application -> Domain
Infrastructure -> Application / Domain
Domain -> no project-specific dependencies
```

Rules:

- Keep controllers thin.
- Do not put business rules in controllers, EF Core configuration, Razor views, or JavaScript.
- Domain rules belong in the domain layer.
- Application workflows belong in the application layer.
- Infrastructure details must not leak into the domain model.
- Avoid over-engineering, but keep boundaries clear enough for a serious portfolio project.

## AI safety rules

- AI is advisory by default.
- AI must not autonomously change ticket status, priority, assignment, or follow-up content.
- Human approval is required before any AI suggestion changes business data.
- AI suggestions should include a reason and source/context information when possible.
- Every AI suggestion and every human decision about that suggestion must be auditable.
- Prefer fake or mock AI providers in tests and early development.
- Never hide AI uncertainty behind confident wording.

## Definition of Done

Every future feature should satisfy this checklist:

- Scope matches the issue or session goal.
- Code builds successfully.
- Tests are added or updated, or the reason is documented.
- Existing tests pass.
- Naming is clear and consistent.
- Files are placed in the correct project or folder.
- Database changes are explicit and explained.
- AI-related features include human approval and audit behavior.
- README or docs are updated when setup, behavior, or architecture changes.
- No unnecessary files are added.
- The learner has inspected the diff and can explain what changed.
- A focused Git commit message is prepared.

## Git workflow

- Use one branch per feature or session milestone.
- Prefer one coherent commit per completed session milestone.
- Use conventional commit messages:
  - `feat:` for user-visible features.
  - `fix:` for bug fixes.
  - `test:` for tests.
  - `docs:` for documentation.
  - `chore:` for tooling or maintenance.
  - `ci:` for CI/CD changes.
  - `refactor:` for behavior-preserving code improvements.
- A pull request should explain the problem, solution, tests, and any screenshots or demo notes.
- Do not claim a commit, branch, or pull request exists unless it has been verified.

## Coding style

- Use readable C# with explicit names.
- Use async methods for I/O.
- Keep methods small enough to understand quickly.
- Prefer simple designs first.
- Introduce interfaces for external systems such as AI providers, clocks, email/message senders, and persistence boundaries when they improve testability.
- Avoid premature abstractions that make the project harder to understand.

## Codex usage rules

- Use Codex to plan, generate, review, test, debug, and refactor.
- Do not accept generated code blindly.
- Always inspect diffs.
- Ask Codex to explain tradeoffs, not only to write code.
- If Codex produces a large change, pause and break it into smaller steps.
- When something fails, ask Codex to diagnose before asking it to fix.
- The learner should be able to explain every accepted change in an interview.
