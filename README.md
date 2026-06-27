# AI Ticket & Project Follow-up Agent

A portfolio-quality ASP.NET Core project for learning professional AI-assisted software engineering with Codex.

## Problem

Small teams often lose track of tickets, tasks, project actions, quotations, invoices, visits, blockers, and follow-up messages.

This project explores a practical business workflow system where users can track work and request AI assistance without allowing AI to silently change business data.

## Product vision

Users can create project tickets, assign owners, track status and priority, and ask an AI assistant to suggest:

- A short ticket summary.
- A priority recommendation.
- Possible blockers.
- The next best action.
- A follow-up email or message.

AI suggestions are advisory. A human must approve changes before they affect ticket data.

## Core principles

- Backend-first and business-focused.
- Small enough to finish, polished enough to show.
- Human-approved AI decisions.
- Auditability for AI suggestions and human decisions.
- Clean boundaries between API, application logic, domain rules, and infrastructure.
- Tests and GitHub workflow treated as part of the product, not extras.

## Planned architecture

The project will start as a compact modular monolith:

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

Dependency direction:

```text
Api -> Application -> Domain
Infrastructure -> Application / Domain
Domain -> no project-specific dependencies
```

This keeps the project understandable while still showing real architecture discipline.

## Initial scope

Ticket statuses:

- New
- In Progress
- Waiting
- Blocked
- Done

Ticket priorities:

- Low
- Medium
- High
- Urgent

Planned MVP modules:

- Ticket creation and update.
- Assignment and status tracking.
- Filtering and dashboard counts.
- AI summary and priority suggestions.
- AI blocker and next-action suggestions.
- Human approval flow.
- Audit log for AI suggestions and decisions.
- Unit and integration tests.
- Docker support.
- GitHub Actions CI.
- README, screenshots, and short demo script.

## Out of scope for version 1

- Full multi-tenant SaaS.
- Payments.
- Complex frontend SPA.
- Autonomous AI writes.
- Production WhatsApp integration.
- Overly complex enterprise architecture.

These can be considered later only after the core portfolio project is strong.

## Planned tech stack

- C#
- ASP.NET Core
- EF Core
- SQLite first
- PostgreSQL or SQL Server later
- xUnit or similar test framework
- Docker
- GitHub Actions
- Optional Razor or lightweight dashboard UI
- AI provider abstraction with fake/mock provider for early development and tests

## Learning goals

This project is also a Codex training program. Each session should practice:

- Predicting what Codex will do.
- Asking Codex for a small scoped change.
- Inspecting the diff.
- Running tests or checks.
- Explaining the result.
- Improving the prompt.
- Preparing a clean commit message.

## Milestones

- [ ] Session 1: Foundation files, architecture rules, Definition of Done.
- [ ] Session 2: Domain model, ticket lifecycle, and first architecture decision record.
- [ ] Session 3: API project scaffold and basic health endpoint.
- [ ] Session 4: EF Core persistence and first CRUD flow.
- [ ] Session 5: Business rules, filters, and application tests.
- [ ] Session 6: Human approval and audit model.
- [ ] Session 7: Fake AI suggestion provider.
- [ ] Session 8: Read-only agent/tool-calling workflow.
- [ ] Session 9: Dashboard or reporting view.
- [ ] Session 10: Test hardening and debugging practice.
- [ ] Session 11: Docker and GitHub Actions CI.
- [ ] Session 12: README polish, screenshots, demo, and CV material.

## Session 1 Definition of Done

- `AGENTS.md` defines durable Codex rules.
- `README.md` explains the product vision and planned architecture.
- The learner can explain why this project starts as a compact modular monolith.
- The learner can explain the difference between ordinary documentation and durable Codex instructions.
- The learner has inspected the diff before moving to app code.

Suggested commit message after Session 1:

```text
docs: add project foundation and Codex workflow rules
```
