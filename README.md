# TaskManager Capstone API (.NET & EF Core)

**Level 4 of 4 · Capstone — The portfolio piece you show an employer**

A robust, enterprise-grade Task Management REST API built with **ASP.NET Core** and **Entity Framework Core**. Backed by a real relational database via code-first migrations, structured with a clean decoupled service layer, and featuring dynamic multi-parameter filtering, searching, and pagination.

---

## 🎯 Requirements Checklist

| Requirement | Implementation Details | Status |
| :--- | :--- | :---: |
| **Migrations, not a hand-made database** | Real EF Core code-first migrations generated and tracked in [`Migrations/`](file:///Users/abhishekyadav/internship%20project/level4-capstone-task-api/Migrations); auto-applied on startup | ✅ Met |
| **Service layer between controller & data** | Strict separation of concerns: [`TasksController`](file:///Users/abhishekyadav/internship%20project/level4-capstone-task-api/Controllers/TasksController.cs) -> [`ITaskService`](file:///Users/abhishekyadav/internship%20project/level4-capstone-task-api/Services/ITaskService.cs) -> [`AppDbContext`](file:///Users/abhishekyadav/internship%20project/level4-capstone-task-api/Data/AppDbContext.cs) | ✅ Met |
| **Filtering and paging on list endpoint** | `GET /api/tasks` supports `Status`, `Priority`, `Search`, `SortBy`, `SortDescending`, `PageNumber`, and `PageSize` with [`PagedResult<T>`](file:///Users/abhishekyadav/internship%20project/level4-capstone-task-api/Models/DTOs/PagedResult.cs) metadata | ✅ Met |
| **Correct HTTP status codes** | `200 OK`, `201 Created` *(with Location header)*, `204 No Content`, `400 Bad Request`, and `404 Not Found` | ✅ Met |
| **Model validation on input** | DataAnnotation validation on DTOs with RFC 9110 Problem Details for errors | ✅ Met |
| **Swagger / OpenAPI Documentation** | Interactive Swagger UI enabled at root (`/`) and `/swagger` | ✅ Met |
| **Builds & runs cleanly** | Verified on modern .NET SDK (`net10.0` / `net8.0`) | ✅ Met |

---

## 🏗️ Architecture & Layering

```
TaskManagerApi/
├── Controllers/
│   └── TasksController.cs          # HTTP transport, routing, status codes
├── Services/
│   ├── ITaskService.cs             # Application service contract
│   └── TaskService.cs              # Business logic, query composition, paging
├── Data/
│   ├── AppDbContext.cs             # EF Core DbContext with Fluent API & indexes
│   └── DbInitializer.cs            # Demo task seeding
├── Migrations/
│   ├── 20261027154414_InitialCreate.cs
│   └── AppDbContextModelSnapshot.cs
└── Models/
    ├── Entities/
    │   └── TaskItem.cs             # Relational database entity
    ├── Enums/
    │   ├── TaskStatus.cs           # Pending, InProgress, Completed, Cancelled
    │   └── TaskPriority.cs         # Low, Medium, High, Urgent
    └── DTOs/
        ├── CreateTaskDto.cs        # Creation input model + validation
        ├── UpdateTaskDto.cs        # Update input model + validation
        ├── TaskDto.cs              # Output model
        ├── TaskFilterParameters.cs # Filter & pagination query parameters
        └── PagedResult.cs          # Paged envelope with navigation metadata
```

---

## 📦 Setup & Database Migrations

### 1. Automated Migrations on Startup
When you launch the application, `Program.cs` automatically executes pending migrations using EF Core's `Database.MigrateAsync()` and seeds initial demo tasks if the database is newly initialized:
```csharp
using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
await db.Database.MigrateAsync();
await DbInitializer.SeedAsync(db);
```

### 2. Manual Migration Commands (CLI)
You can also inspect or manage migrations manually using the .NET EF CLI tool:

```bash
# Install the EF Core global tool (if not already installed)
dotnet tool install --global dotnet-ef

# Add a new migration (after modifying entities)
dotnet ef migrations add <MigrationName>

# Apply pending migrations to the SQLite database
dotnet ef database update

# Remove the latest unapplied migration
dotnet ef migrations remove
```

---

## 🚀 How to Run

### Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download) (version 8.0, 9.0, or 10.0)

### 1. Clone & Run
```bash
# 1. Clone the repository
git clone https://github.com/avi74600/capstone-task-api.git
cd capstone-task-api

# 2. Build the project
dotnet build

# 3. Launch the API server
dotnet run --launch-profile http
```

### 2. Interactive Swagger UI
Open your browser to:  
👉 **`http://localhost:5124/`** (or `http://localhost:5124/swagger`)

---

## 🧪 Exercising the API with `curl`

### 1. Paging (`PageNumber` and `PageSize`)
```bash
curl -i "http://localhost:5124/api/tasks?pageNumber=1&pageSize=3"
```
**Sample Response Envelope**:
```json
{
  "items": [ ... ],
  "totalCount": 7,
  "pageNumber": 1,
  "pageSize": 3,
  "totalPages": 3,
  "hasPreviousPage": false,
  "hasNextPage": true
}
```

### 2. Filtering by Status (`Pending`, `InProgress`, `Completed`, `Cancelled`)
```bash
curl -i "http://localhost:5124/api/tasks?status=InProgress"
```

### 3. Filtering by Priority (`Low`, `Medium`, `High`, `Urgent`)
```bash
curl -i "http://localhost:5124/api/tasks?priority=Urgent"
```

### 4. Search Filter (searches Title and Description)
```bash
curl -i "http://localhost:5124/api/tasks?search=migration"
```

### 5. Sorting by Field & Direction
```bash
curl -i "http://localhost:5124/api/tasks?sortBy=dueDate&sortDescending=false"
```

### 6. Combined Query (Status + Search + Paging)
```bash
curl -i "http://localhost:5124/api/tasks?status=Pending&search=test&pageNumber=1&pageSize=5"
```

### 7. GET Single Task by ID (`200 OK` or `404 Not Found`)
```bash
curl -i "http://localhost:5124/api/tasks/1"
```

### 8. POST Create Task (`201 Created` with Location header)
```bash
curl -i -X POST "http://localhost:5124/api/tasks" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Deploy to Azure App Service",
    "description": "Configure GitHub Actions deployment slot and production environment.",
    "status": "Pending",
    "priority": "High",
    "dueDate": "2026-11-15T18:00:00Z"
  }'
```

### 9. PUT Update Task (`200 OK`)
```bash
curl -i -X PUT "http://localhost:5124/api/tasks/1" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Design Database Schema (Approved)",
    "description": "Finalized entity relationship design with engineering team.",
    "status": "Completed",
    "priority": "High"
  }'
```

### 10. DELETE Task (`204 No Content`)
```bash
curl -i -X DELETE "http://localhost:5124/api/tasks/7"
```

---

## 💡 Key Architectural Decisions

1. **Service Layer Abstraction**:
   Controllers remain thin HTTP handlers responsible strictly for request validation, status code mapping, and response serialization. All querying, sorting, pagination logic, and database mutations are encapsulated in `TaskService`.

2. **Database Indexing**:
   The `AppDbContext` configures indexes on `Status`, `Priority`, and `CreatedAt` using EF Core Fluent API, ensuring fast lookup performance as data scales.

3. **String Enum Serialization**:
   Enums are converted to strings in JSON payloads using `JsonStringEnumConverter`, providing human-readable status values (`"InProgress"` instead of `1`) across API boundaries.

4. **Code-First Relational SQLite Database**:
   Utilizes a real relational database with transaction support, ACID guarantees, and automated schema migrations, making it zero-friction for reviewers to run without external database infrastructure.
