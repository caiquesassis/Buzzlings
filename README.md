# Buzzlings 🐝
A web-based simulation where you manage a dynamic colony of creatures called Buzzlings, balancing their needs and behaviors through creation and maintenance via CRUD operations.

**Live Demo**: [Link to Azure Deployment]

**API Documentation**: Accessible via **Scalar** at `/scalar/v1`

---

**📖 Overview**

In this simulation, you create and manage a colony of creatures, each with distinct roles. By using CRUD operations, you ensure the hive stays balanced and functional, addressing challenges like internal conflict and resource management.

**Note:** This project is intentionally framed as a backend-oriented technical demo, not a game. The simulation is intentionally simple and random-based, and this was a conscious decision to prioritize a meaningful context to showcase the CRUD operations over complex simulation aspects.

---

**🛠️ Tech Stack**
* **Backend:** .NET 9 (ASP.NET Core MVC & Web API)
* **Frontend:** Razor Views, jQuery (AJAX Polling)
* **ORM:** Entity Framework Core (SQL Server)
* **Auth:** ASP.NET Core Identity
* **Architecture:** Repository Pattern, Unit of Work, Dependency Injection
* **Testing:** xUnit, NSubstitute

---

**🧠 Technical Decisions**

* **Repository + Unit of Work:** Implemented as an abstraction over EF Core to keep business services persistence-agnostic and simplify unit testing.
* **API Layer:** The application exposes a single read-only endpoint for external consumers to demonstrate API design, DTO usage, and separation of concerns without introducing unnecessary internal HTTP calls.
* **Health Checks:** Implemented a `/health` endpoint that monitors both application liveness and EF Core Database connectivity.
* **Backend-First UI:** Priority was given to server-side logic and clean architecture. The UI is intentionally simple and minimal to emphasize the underlying backend design.

---

**🖥️ Getting Started**

**Prerequisites**
* .NET 9 SDK
* SQL Server

**Installation**
1. Clone the repository.
2. Database Configuration: Update the `DefaultConnection` string in `appsettings.json` with your local SQL Server instance. (using **User Secrets** is recommended for security)
3. Migrations: Run `Update-Database` via Package Manager Console (target the Data project).
4. Run: Execute the `Buzzlings.Web` project.
