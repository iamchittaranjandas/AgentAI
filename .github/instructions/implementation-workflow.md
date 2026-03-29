# Implementation Workflow Guide

## Bottom-Up, Dependency-First Development

This guide provides the exact step-by-step workflow for implementing the Agentic AI Developer Assistant following Clean Architecture principles and dependency-first development.

---

## Phase 1: Project Setup

### Step 1.1: Create Solution Structure

```bash
# Create solution
dotnet new sln -n AgentAI

# Create projects
dotnet new classlib -n AgentAI.Domain -f net10.0
dotnet new classlib -n AgentAI.Application -f net10.0
dotnet new classlib -n AgentAI.Infrastructure -f net10.0
dotnet new webapi -n AgentAI.API -f net10.0

# Add projects to solution
dotnet sln add AgentAI.Domain/AgentAI.Domain.csproj
dotnet sln add AgentAI.Application/AgentAI.Application.csproj
dotnet sln add AgentAI.Infrastructure/AgentAI.Infrastructure.csproj
dotnet sln add AgentAI.API/AgentAI.API.csproj

# Set up project references (dependency flow)
dotnet add AgentAI.Application/AgentAI.Application.csproj reference AgentAI.Domain/AgentAI.Domain.csproj
dotnet add AgentAI.Infrastructure/AgentAI.Infrastructure.csproj reference AgentAI.Domain/AgentAI.Domain.csproj
dotnet add AgentAI.Infrastructure/AgentAI.Infrastructure.csproj reference AgentAI.Application/AgentAI.Application.csproj
dotnet add AgentAI.API/AgentAI.API.csproj reference AgentAI.Application/AgentAI.Application.csproj
dotnet add AgentAI.API/AgentAI.API.csproj reference AgentAI.Infrastructure/AgentAI.Infrastructure.csproj
```

### Step 1.2: Install NuGet Packages

**Domain Layer (AgentAI.Domain):**
```bash
cd AgentAI.Domain
# No external dependencies - pure domain logic
```

**Application Layer (AgentAI.Application):**
```bash
cd AgentAI.Application
dotnet add package FluentValidation
dotnet add package Mapster
dotnet add package Microsoft.Extensions.DependencyInjection.Abstractions
```

**Infrastructure Layer (AgentAI.Infrastructure):**
```bash
cd AgentAI.Infrastructure
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.SemanticKernel
dotnet add package Microsoft.SemanticKernel.Connectors.OpenAI
dotnet add package Dapper
dotnet add package Serilog.AspNetCore
dotnet add package Serilog.Sinks.Console
dotnet add package Serilog.Sinks.File
dotnet add package StackExchange.Redis
dotnet add package Pgvector.EntityFrameworkCore
```

**API Layer (AgentAI.API):**
```bash
cd AgentAI.API
dotnet add package Swashbuckle.AspNetCore
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Serilog.AspNetCore
dotnet add package FluentValidation.AspNetCore
```

### Step 1.3: Verify Build
```bash
dotnet build
# Expected: Build succeeded. 0 Error(s)
```

---

## Phase 2: Domain Layer Implementation

### Step 2.1: Create Domain Interfaces

**File: AgentAI.Domain/Interfaces/IEntity.cs**
```csharp
namespace AgentAI.Domain.Interfaces;

public interface IEntity
{
    Guid Id { get; set; }
}
```

**File: AgentAI.Domain/Interfaces/IAuditableEntity.cs**
```csharp
namespace AgentAI.Domain.Interfaces;

public interface IAuditableEntity : IEntity
{
    DateTime CreatedAt { get; set; }
    DateTime? UpdatedAt { get; set; }
    string? CreatedBy { get; set; }
    string? UpdatedBy { get; set; }
}
```

**Verify:**
```bash
dotnet build AgentAI.Domain/AgentAI.Domain.csproj
# Expected: Build succeeded. 0 Error(s)
```

### Step 2.2: Create Domain Enums

**File: AgentAI.Domain/Enums/UserRole.cs**
```csharp
namespace AgentAI.Domain.Enums;

public enum UserRole
{
    Developer = 1,
    TechLead = 2,
    Admin = 3,
    DevOps = 4
}
```

**File: AgentAI.Domain/Enums/SessionStatus.cs**
```csharp
namespace AgentAI.Domain.Enums;

public enum SessionStatus
{
    Active = 1,
    Paused = 2,
    Completed = 3,
    Archived = 4
}
```

**File: AgentAI.Domain/Enums/IntentType.cs**
```csharp
namespace AgentAI.Domain.Enums;

public enum IntentType
{
    Explain = 1,
    Generate = 2,
    Fix = 3,
    Refactor = 4,
    Test = 5,
    Search = 6,
    Execute = 7,
    Unknown = 99
}
```

**File: AgentAI.Domain/Enums/ToolType.cs**
```csharp
namespace AgentAI.Domain.Enums;

public enum ToolType
{
    File = 1,
    Git = 2,
    Build = 3,
    Test = 4,
    Database = 5
}
```

**File: AgentAI.Domain/Enums/ApprovalStatus.cs**
```csharp
namespace AgentAI.Domain.Enums;

public enum ApprovalStatus
{
    Pending = 1,
    Approved = 2,
    Rejected = 3,
    AutoApproved = 4
}
```

**File: AgentAI.Domain/Enums/RiskLevel.cs**
```csharp
namespace AgentAI.Domain.Enums;

public enum RiskLevel
{
    Low = 1,
    Medium = 2,
    High = 3,
    Critical = 4
}
```

**Verify:**
```bash
dotnet build AgentAI.Domain/AgentAI.Domain.csproj
# Expected: Build succeeded. 0 Error(s)
```

### Step 2.3: Create Domain Entities

Create entities in this order (respecting dependencies):

1. User.cs (no dependencies)
2. Repository.cs (no dependencies)
3. Session.cs (depends on User)
4. CodeFile.cs (depends on Repository)
5. PromptRecord.cs (depends on Session)
6. RetrievalChunk.cs (depends on CodeFile)
7. ToolExecution.cs (depends on Session, PromptRecord)
8. AuditLog.cs (no navigation properties)
9. MemoryItem.cs (depends on User)

**After creating each entity, run:**
```bash
dotnet build AgentAI.Domain/AgentAI.Domain.csproj
```

### Step 2.4: Create Domain Exceptions

**File: AgentAI.Domain/Exceptions/DomainException.cs**
**File: AgentAI.Domain/Exceptions/ValidationException.cs**

**Final Verification:**
```bash
dotnet build AgentAI.Domain/AgentAI.Domain.csproj
# Expected: Build succeeded. 0 Error(s)
```

---

## Phase 3: Application Layer Implementation

### Step 3.1: Create Common Classes

**File: AgentAI.Application/Common/Result.cs**
**File: AgentAI.Application/Common/ApiResponse.cs**
**File: AgentAI.Application/Common/PaginatedList.cs**

**Verify:**
```bash
dotnet build AgentAI.Application/AgentAI.Application.csproj
```

### Step 3.2: Create DTOs

Create DTOs in organized folders:
- Chat/ChatRequest.cs
- Chat/ChatResponse.cs
- Chat/MessageDto.cs
- Session/CreateSessionRequest.cs
- Session/SessionDto.cs
- Session/SessionDetailDto.cs
- Session/UpdateSessionRequest.cs
- Retrieval/RetrievalRequest.cs
- Retrieval/RetrievalResultDto.cs
- Retrieval/RetrievalChunkDto.cs
- Tool/ToolExecutionRequest.cs
- Tool/ToolExecutionResultDto.cs
- Tool/ToolExecutionDto.cs
- Indexing/IndexingRequest.cs
- Indexing/IndexingStatusDto.cs
- Admin/AuditLogDto.cs
- Admin/SystemMetricsDto.cs
- Admin/CreateUserRequest.cs
- Admin/UserDto.cs

**After each DTO, verify:**
```bash
dotnet build AgentAI.Application/AgentAI.Application.csproj
```

### Step 3.3: Create Service Interfaces

**File: AgentAI.Application/Interfaces/Services/IAgentOrchestrationService.cs**
**File: AgentAI.Application/Interfaces/Services/IRetrievalService.cs**
**File: AgentAI.Application/Interfaces/Services/IToolExecutionService.cs**
**File: AgentAI.Application/Interfaces/Services/ISessionService.cs**
**File: AgentAI.Application/Interfaces/Services/IIndexingService.cs**
**File: AgentAI.Application/Interfaces/Services/IEmbeddingService.cs**

### Step 3.4: Create Repository Interfaces

**File: AgentAI.Application/Interfaces/Repositories/ISessionRepository.cs**
**File: AgentAI.Application/Interfaces/Repositories/IPromptRecordRepository.cs**
**File: AgentAI.Application/Interfaces/Repositories/IRetrievalChunkRepository.cs**
**File: AgentAI.Application/Interfaces/Repositories/IToolExecutionRepository.cs**
**File: AgentAI.Application/Interfaces/Repositories/IAuditLogRepository.cs**
**File: AgentAI.Application/Interfaces/Repositories/IMemoryItemRepository.cs**
**File: AgentAI.Application/Interfaces/Repositories/IRepositoryRepository.cs**
**File: AgentAI.Application/Interfaces/Repositories/ICodeFileRepository.cs**

### Step 3.5: Create Infrastructure Interfaces

**File: AgentAI.Application/Interfaces/Infrastructure/ILLMProvider.cs**
**File: AgentAI.Application/Interfaces/Infrastructure/IVectorStore.cs**
**File: AgentAI.Application/Interfaces/Infrastructure/IFileSystemService.cs**
**File: AgentAI.Application/Interfaces/Infrastructure/IGitService.cs**

### Step 3.6: Create Validators

**File: AgentAI.Application/Validators/ChatRequestValidator.cs**
**File: AgentAI.Application/Validators/ToolExecutionRequestValidator.cs**
**File: AgentAI.Application/Validators/CreateSessionRequestValidator.cs**
**File: AgentAI.Application/Validators/RetrievalRequestValidator.cs**
**File: AgentAI.Application/Validators/IndexingRequestValidator.cs**

**Final Verification:**
```bash
dotnet build AgentAI.Application/AgentAI.Application.csproj
# Expected: Build succeeded. 0 Error(s)
```

---

## Phase 4: Infrastructure Layer Implementation

### Step 4.1: Create DbContext

**File: AgentAI.Infrastructure/Persistence/ApplicationDbContext.cs**

Include all DbSets for entities.

**Verify:**
```bash
dotnet build AgentAI.Infrastructure/AgentAI.Infrastructure.csproj
```

### Step 4.2: Create Entity Configurations

**File: AgentAI.Infrastructure/Persistence/Configurations/UserConfiguration.cs**
**File: AgentAI.Infrastructure/Persistence/Configurations/SessionConfiguration.cs**
**File: AgentAI.Infrastructure/Persistence/Configurations/PromptRecordConfiguration.cs**
**File: AgentAI.Infrastructure/Persistence/Configurations/RetrievalChunkConfiguration.cs**
**File: AgentAI.Infrastructure/Persistence/Configurations/ToolExecutionConfiguration.cs**
**File: AgentAI.Infrastructure/Persistence/Configurations/AuditLogConfiguration.cs**
**File: AgentAI.Infrastructure/Persistence/Configurations/MemoryItemConfiguration.cs**
**File: AgentAI.Infrastructure/Persistence/Configurations/CodeFileConfiguration.cs**
**File: AgentAI.Infrastructure/Persistence/Configurations/RepositoryConfiguration.cs**

### Step 4.3: Create Repositories

**File: AgentAI.Infrastructure/Persistence/Repositories/SessionRepository.cs**
**File: AgentAI.Infrastructure/Persistence/Repositories/PromptRecordRepository.cs**
**File: AgentAI.Infrastructure/Persistence/Repositories/RetrievalChunkRepository.cs**
**File: AgentAI.Infrastructure/Persistence/Repositories/ToolExecutionRepository.cs**
**File: AgentAI.Infrastructure/Persistence/Repositories/AuditLogRepository.cs**
**File: AgentAI.Infrastructure/Persistence/Repositories/MemoryItemRepository.cs**
**File: AgentAI.Infrastructure/Persistence/Repositories/RepositoryRepository.cs**
**File: AgentAI.Infrastructure/Persistence/Repositories/CodeFileRepository.cs**

### Step 4.4: Create Service Implementations

**File: AgentAI.Infrastructure/Services/AgentOrchestrationService.cs**
**File: AgentAI.Infrastructure/Services/RetrievalService.cs**
**File: AgentAI.Infrastructure/Services/ToolExecutionService.cs**
**File: AgentAI.Infrastructure/Services/SessionService.cs**
**File: AgentAI.Infrastructure/Services/IndexingService.cs**

### Step 4.5: Create AI Services

**File: AgentAI.Infrastructure/AI/SemanticKernel/SemanticKernelService.cs**
**File: AgentAI.Infrastructure/AI/SemanticKernel/KernelBuilder.cs**
**File: AgentAI.Infrastructure/AI/LLMProviders/OpenAIProvider.cs**
**File: AgentAI.Infrastructure/AI/Embeddings/EmbeddingService.cs**

### Step 4.6: Create Vector Store

**File: AgentAI.Infrastructure/VectorStore/PgVector/PgVectorStore.cs**

### Step 4.7: Create Tools

**File: AgentAI.Infrastructure/Tools/FileTool.cs**
**File: AgentAI.Infrastructure/Tools/GitTool.cs**
**File: AgentAI.Infrastructure/Tools/BuildTool.cs**
**File: AgentAI.Infrastructure/Tools/TestTool.cs**

**Final Verification:**
```bash
dotnet build AgentAI.Infrastructure/AgentAI.Infrastructure.csproj
# Expected: Build succeeded. 0 Error(s)
```

---

## Phase 5: API Layer Implementation

### Step 5.1: Create Controllers

**File: AgentAI.API/Controllers/ChatController.cs**
**File: AgentAI.API/Controllers/SessionController.cs**
**File: AgentAI.API/Controllers/RetrievalController.cs**
**File: AgentAI.API/Controllers/ToolController.cs**
**File: AgentAI.API/Controllers/IndexingController.cs**
**File: AgentAI.API/Controllers/AdminController.cs**

### Step 5.2: Create Middleware

**File: AgentAI.API/Middleware/ExceptionHandlingMiddleware.cs**
**File: AgentAI.API/Middleware/RequestLoggingMiddleware.cs**

### Step 5.3: Create Service Extensions

**File: AgentAI.API/Extensions/ServiceCollectionExtensions.cs**

Register all services, repositories, validators, and configurations.

### Step 5.4: Configure Program.cs

Update Program.cs with:
- Service registrations
- Middleware pipeline
- Swagger configuration
- Authentication/Authorization
- Serilog configuration
- CORS policy

### Step 5.5: Create Configuration Files

**File: AgentAI.API/appsettings.json**
**File: AgentAI.API/appsettings.Development.json**

**Final Verification:**
```bash
dotnet build
# Expected: Build succeeded. 0 Error(s)
```

---

## Phase 6: Database Setup

### Step 6.1: Create Initial Migration

```bash
cd AgentAI.Infrastructure
dotnet ef migrations add InitialCreate --startup-project ../AgentAI.API/AgentAI.API.csproj
```

### Step 6.2: Update Database

```bash
dotnet ef database update --startup-project ../AgentAI.API/AgentAI.API.csproj
```

---

## Phase 7: Testing and Verification

### Step 7.1: Run Application

```bash
cd AgentAI.API
dotnet run
```

### Step 7.2: Access Swagger

Navigate to: `https://localhost:5001/swagger`

### Step 7.3: Test Endpoints

Test each endpoint group:
1. Session creation
2. Chat interaction
3. Retrieval search
4. Tool execution
5. Indexing operations

---

## Dependency Verification Checklist

Before creating ANY file, verify:

✅ All referenced types exist
✅ All using statements are correct
✅ All namespaces match folder structure
✅ All dependencies compile successfully
✅ Run `dotnet build` after each file

---

## Common Build Error Solutions

**CS0246 - Type or namespace not found:**
- Add missing using statement
- Verify file exists in correct location
- Check namespace matches folder structure

**CS0103 - Name does not exist:**
- Verify variable/method is declared
- Check spelling
- Ensure using statements are present

**CS1061 - Type does not contain definition:**
- Verify method exists in referenced class
- Check if using correct type
- Ensure interface is implemented

---

## Project Structure Reference

```
AgentAI/
├── AgentAI.sln
├── AgentAI.Domain/
│   ├── Entities/
│   ├── Enums/
│   ├── ValueObjects/
│   ├── Interfaces/
│   └── Exceptions/
├── AgentAI.Application/
│   ├── DTOs/
│   ├── Interfaces/
│   ├── Validators/
│   ├── Mappings/
│   └── Common/
├── AgentAI.Infrastructure/
│   ├── Persistence/
│   ├── AI/
│   ├── VectorStore/
│   ├── Services/
│   ├── Tools/
│   ├── FileSystem/
│   └── Git/
└── AgentAI.API/
    ├── Controllers/
    ├── Middleware/
    ├── Extensions/
    ├── Filters/
    └── Configuration/
```
