# Call Center BI - Control Desk

Business intelligence dashboard for call center operational control. This project collects realtime and daily KPIs (answered calls, disconnected calls, TNA, TMA, TME, TMO, idle time, pauses, operator status) and displays them in a web dashboard for supervisors.

## Why it matters
- Real time visibility into operator status, call load, and campaign health.
- Operational control with alerts/highlights for out-of-range TMA and pause limits.
- Consolidated KPIs across campaigns and groups for quick decisions on staffing and routing.

## What is included
- `ControlDesk.Coletor`: WinForms data collector that logs into the call center system and scrapes reports to capture KPIs. Uses a timer-driven workflow and HTML parsing to persist metrics into SQL Server.
- `ControlDesk.Dominio`: Domain and data access layer (ADO.NET + stored procedures). Encapsulates entities like `Atividade`, `Mailing`, `Pausa`, `Grupo`, and `Operador`.
- `ControlDesk.WebApplication`: ASP.NET MVC 4 dashboard with partial views and client-side scripts (jQuery, Knockout) to visualize KPIs in tables.
- `ControlDesk.WebApplication.Tests`: MSTest project scaffold for tests.

## Key features
- Dashboard with per-campaign and per-operator metrics (answered, disconnected, TNA, TMA, TME, TMO, idle time).
- Realtime operator activity panel (status, time in state, campaign).
- Pause control with time limits per hour and visual flags when exceeded.
- Capacity vs logged-in operators by group.

## Architecture and data flow
1) **Collector (WinForms)** logs in to the call center reporting system and pulls HTML reports.
2) **HTML parsing** uses HtmlAgilityPack to extract metrics.
3) **Domain layer** persists data via SQL Server stored procedures.
4) **Web dashboard** reads domain data and renders partial views for KPI tables.

This separation keeps data ingestion isolated from presentation, while the domain layer centralizes business rules and database access.

## Tech stack
- .NET Framework 4.5, C#
- ASP.NET MVC 4 + Razor
- ADO.NET + SQL Server stored procedures
- WinForms for data collection automation
- HtmlAgilityPack for HTML parsing
- jQuery / jQuery UI / Knockout

## Database notes
The project relies on a SQL Server database (`TotalIP`) and stored procedures such as `SPI*`, `SPU*`, `SPS*`, `SPD*` for CRUD and refresh. Connection strings are currently in `ControlDesk.WebApplication\\Web.config` and `ControlDesk.Coletor\\App.config`.

Before publishing this repo, replace any real credentials with environment-specific values (or remove secrets entirely).

## How to run (high level)
1) Open `ControlDesk.sln` in Visual Studio 2013+.
2) Restore NuGet packages.
3) Configure connection strings for your SQL Server instance.
4) Ensure stored procedures exist in the `TotalIP` database.
5) Run `ControlDesk.Coletor` to collect data.
6) Start `ControlDesk.WebApplication` to view the dashboard.

## Screens and behavior
- `Views/Home/Index.cshtml` composes the dashboard from `Mailing`, `Atividade`, and `Logados` panels.
- KPI rows highlight performance outliers (e.g., TMA deviations, pause limits).

## Notes for recruiters
This project demonstrates:
- Practical BI for a live operations environment.
- Multi-project solution architecture (collector + domain + web dashboard).
- Data ingestion from legacy systems and normalization into a reporting database.
- End-to-end ownership from data extraction through presentation.
