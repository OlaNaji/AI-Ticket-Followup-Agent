# ADR 0001: Use a compact modular monolith

## Status

Accepted

## Date

2026-06-28

## Context

AI Ticket & Project Follow-up Agent is a small portfolio project, not a large enterprise system.

The project still needs to demonstrate serious backend engineering skills:

- clear business rules;
- database design;
- tests;
- Git/GitHub workflow;
- documentation;
- CI/CD;
- safe AI-assisted workflow;
- human approval before AI suggestions change business data;
- auditability.

Starting with a single simple CRUD project would be fast, but it would make it easy to mix business logic, database code, controllers, and AI behavior.

Starting with microservices or a very heavy enterprise architecture would create unnecessary complexity before the project has real product weight.

## Decision

Use a compact modular monolith with clean boundaries:

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

## Rationale

This architecture gives the project enough structure to be professional without making it artificially complicated.

The domain layer should not depend on infrastructure because business rules should not care how data is stored or which external tools are used. For example, changing from SQLite to PostgreSQL or SQL Server should not require rewriting core ticket rules.

The application layer coordinates use cases such as creating a ticket, changing status, approving an AI suggestion, and writing audit records.

The infrastructure layer handles details such as EF Core, database configuration, external AI providers, and persistence.

The API layer exposes the application through HTTP endpoints and handles request/response concerns.

## Consequences

Positive:

- Business rules stay easier to test.
- The project can start small but remain maintainable.
- AI behavior can be isolated behind interfaces and tested with fake providers.
- Database technology can change later with less impact on business logic.
- The portfolio shows architecture discipline without over-engineering.

Tradeoffs:

- There are more projects/folders than a basic CRUD app.
- Some simple features require thinking about the correct layer.
- The developer must practice dependency direction and avoid placing business logic in controllers or EF Core code.

## Alternatives considered

### Simple CRUD app

Rejected for now because it would be too easy to mix concerns and produce a prototype-looking portfolio project.

### Full microservices

Rejected because the project is too small for distributed architecture. Microservices would add deployment, networking, data consistency, and operational complexity before they are justified.

### Heavy enterprise clean architecture

Rejected because too many abstractions would slow learning and make the code harder to explain in interviews.

## Review trigger

Revisit this decision only if the project grows enough to require independent deployment, separate scaling, or clear ownership boundaries between modules.
