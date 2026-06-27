# Codex Learning Progress — AI Ticket & Project Follow-up Agent

## Learner profile

- **Starting point:** .NET developer (1.5 years), confident modifying business CRUD systems; strengthening fundamentals, architecture, tests, Git/GitHub, Docker, CI/CD, and AI engineering.
- **Primary stack:** C#, ASP.NET Core, EF Core, SQL (SQLite first; PostgreSQL or SQL Server later), Razor or a small dashboard client, Docker, GitHub Actions.
- **Professional goal:** Backend / ASP.NET Core engineer who can build reliable AI-powered business tools without outsourcing judgment to the agent.
- **Codex surfaces:** IDE extension first, then CLI for local workflows; Codex app/web for planning, review, and larger offloaded work.
- **Portfolio product:** AI Ticket & Project Follow-up Agent — a human-approved workflow system that classifies, summarizes, prioritizes, and proposes follow-ups while recording all AI advice and human decisions.

## Non-negotiable engineering loop

For every Codex-assisted change:

1. State the outcome, constraints, and acceptance checks.
2. Predict the likely files and behavior before asking Codex to act.
3. Ask for a plan or a narrow change; keep the first request small.
4. Inspect every changed file and explain the diff in your own words.
5. Run the relevant tests, formatter/build, and one manual behavior check.
6. Improve the prompt with the gap you discovered; do not blindly accept a second attempt.
7. Commit one coherent, reviewable unit of work with evidence in the message or PR.

## Current curriculum (12 sessions)

| # | Session | Build milestone | Codex practice | Portfolio artifact |
|---|---|---|---|---|
| 1 | Professional setup and repo hygiene | Repository, solution, conventions, issue board | Surface selection, planning, diff review | Architecture one-pager + issue backlog |
| 2 | Domain and database design | Ticket, project, assignment, status-history schema | Clarifying prompts, schema review | ERD + ADR-001 |
| 3 | API foundation | Cleanly structured API with health endpoint and error handling | Narrow implementation prompts | Runnable API + API contract |
| 4 | Persistence and CRUD | EF Core migrations, ticket/project endpoints | Test-first prompt and generated diff critique | Migration + endpoint collection |
| 5 | Business rules and problem solving | Valid status transitions, priority rules, filtering | Ask Codex for options, choose and justify | Rule matrix + unit tests |
| 6 | Authentication, authorization, auditability | Roles, human approval workflow, audit log | Threat-model/review prompting | Security notes + audit trail demo |
| 7 | AI advisory workflow | Structured AI suggestions behind an interface; no autonomous writes | Prompt design, mock/fake provider, failure modes | AI decision contract + safety demo |
| 8 | Tool-calling agent workflow | Read-only tools for ticket context and summaries; approval gate remains | Agent plan/review, tool-schema inspection | Agent trace + tool contract |
| 9 | Dashboard and operational UX | Filters, counts, ticket detail, approval queue | UI task decomposition and browser/diff checks | Screenshots/GIF |
| 10 | Testing and debugging | Unit, integration, and regression tests; diagnostics | Reproduce-first debugging prompts | Test report + bug postmortem |
| 11 | Containers and CI/CD | Docker Compose and GitHub Actions build/test pipeline | Environment-aware prompts, CI log triage | Passing CI badge + deployment runbook |
| 12 | Professional handoff | README, architecture diagram, demo, PR-quality final release | Full review, refactoring, portfolio storytelling | v1.0 release, demo video, CV/LinkedIn copy |

## Adaptive curriculum rules

- At the end of every session, rate each objective: **Needs practice**, **Working knowledge**, or **Confident**.
- Mark a topic **Confident** only after you explain the trade-off, inspect a Codex diff, and pass relevant tests without step-by-step help.
- Reduce the next session's introduction for confident topics and replace the saved time with a harder extension: architecture trade-offs, edge cases, performance, security, observability, or a more independent implementation.
- If an objective needs practice, add one 20–30 minute repair exercise before the next milestone; do not pile on unrelated material.
- Record the exact change under **Curriculum changes** below. This file is updated after every session before beginning the next one.

## Session record

### Session 0 — Program design (2026-06-22)

- **What I learned:** Codex should be used as a scoped collaborator: plan, inspect, test, explain, revise, and commit.
- **What I struggled with:** Not assessed yet.
- **Codex features I used:** Codex coaching workflow; documentation-guided program design.
- **Artifacts I produced:** Personalized 12-session curriculum and this learning tracker.
- **What to study next:** Session 1 — professional setup and repo hygiene.
- **Curriculum changes:** Baseline curriculum created. No topics removed or accelerated yet.

### Session 1 — Professional setup and repo hygiene

- **What I learned:** The difference between durable Codex instructions and public project documentation; why project structure should be agreed before endpoints; the first version of a compact modular monolith for an ASP.NET Core portfolio project; why the domain layer should not depend on infrastructure.
- **What I struggled with:** The reason for choosing this exact architecture needed clarification, especially the direction of dependencies.
- **Codex features I used:** Guided prediction, small approved change, foundation-file generation, diff review prompt, progress tracking.
- **Artifacts I produced:** `AGENTS.md` and `README.md`.
- **Evidence (tests/build/diff):** Files were created and manually reviewed. No build/tests yet because no application code exists. Git command is not available from the current Codex environment.
- **Self-rating (Needs practice / Working knowledge / Confident):** Working knowledge for README vs AGENTS.md and dependency direction; needs practice applying architecture rules in code.
- **What to study next:** Architecture tradeoffs, solution scaffolding, and Git installation/PATH verification outside this environment.
- **Curriculum changes:** Keep Session 2 focused on architecture and domain modeling before database/code generation. Add a short architecture-decision exercise before scaffolding projects. Add a Git setup checkpoint before the first real commit.

### Session 2 — Domain and database design

- **What I learned:**
- **What I struggled with:**
- **Codex features I used:**
- **Artifacts I produced:**
- **Evidence (tests/build/diff):**
- **Self-rating (Needs practice / Working knowledge / Confident):**
- **What to study next:**
- **Curriculum changes:**

### Session 3 — API foundation

- **What I learned:**
- **What I struggled with:**
- **Codex features I used:**
- **Artifacts I produced:**
- **Evidence (tests/build/diff):**
- **Self-rating (Needs practice / Working knowledge / Confident):**
- **What to study next:**
- **Curriculum changes:**

### Session 4 — Persistence and CRUD

- **What I learned:**
- **What I struggled with:**
- **Codex features I used:**
- **Artifacts I produced:**
- **Evidence (tests/build/diff):**
- **Self-rating (Needs practice / Working knowledge / Confident):**
- **What to study next:**
- **Curriculum changes:**

### Session 5 — Business rules and problem solving

- **What I learned:**
- **What I struggled with:**
- **Codex features I used:**
- **Artifacts I produced:**
- **Evidence (tests/build/diff):**
- **Self-rating (Needs practice / Working knowledge / Confident):**
- **What to study next:**
- **Curriculum changes:**

### Session 6 — Authentication, authorization, auditability

- **What I learned:**
- **What I struggled with:**
- **Codex features I used:**
- **Artifacts I produced:**
- **Evidence (tests/build/diff):**
- **Self-rating (Needs practice / Working knowledge / Confident):**
- **What to study next:**
- **Curriculum changes:**

### Session 7 — AI advisory workflow

- **What I learned:**
- **What I struggled with:**
- **Codex features I used:**
- **Artifacts I produced:**
- **Evidence (tests/build/diff):**
- **Self-rating (Needs practice / Working knowledge / Confident):**
- **What to study next:**
- **Curriculum changes:**

### Session 8 — Tool-calling agent workflow

- **What I learned:**
- **What I struggled with:**
- **Codex features I used:**
- **Artifacts I produced:**
- **Evidence (tests/build/diff):**
- **Self-rating (Needs practice / Working knowledge / Confident):**
- **What to study next:**
- **Curriculum changes:**

### Session 9 — Dashboard and operational UX

- **What I learned:**
- **What I struggled with:**
- **Codex features I used:**
- **Artifacts I produced:**
- **Evidence (tests/build/diff):**
- **Self-rating (Needs practice / Working knowledge / Confident):**
- **What to study next:**
- **Curriculum changes:**

### Session 10 — Testing and debugging

- **What I learned:**
- **What I struggled with:**
- **Codex features I used:**
- **Artifacts I produced:**
- **Evidence (tests/build/diff):**
- **Self-rating (Needs practice / Working knowledge / Confident):**
- **What to study next:**
- **Curriculum changes:**

### Session 11 — Containers and CI/CD

- **What I learned:**
- **What I struggled with:**
- **Codex features I used:**
- **Artifacts I produced:**
- **Evidence (tests/build/diff):**
- **Self-rating (Needs practice / Working knowledge / Confident):**
- **What to study next:**
- **Curriculum changes:**

### Session 12 — Professional handoff

- **What I learned:**
- **What I struggled with:**
- **Codex features I used:**
- **Artifacts I produced:**
- **Evidence (tests/build/diff):**
- **Self-rating (Needs practice / Working knowledge / Confident):**
- **What to study next:**
- **Curriculum changes:**
