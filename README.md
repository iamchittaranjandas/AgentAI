# AgentAI - Enterprise-Grade AI Developer Assistant

A production-ready enterprise AI coding assistant built with .NET 10, implementing complete Clean Architecture and all enterprise patterns from [dotnet-enterprise-api](https://github.com/iamchittaranjandas/dotnet-enterprise-api).

## 🎯 Overview

AgentAI is a comprehensive AI-powered development assistant that combines Semantic Kernel, OpenRouter multi-model support, and enterprise-grade architectural patterns to provide intelligent code assistance with human-in-the-loop safety controls.

## 🏗️ Architecture Patterns

### Clean Architecture ✅
Four-layer architecture with clear separation of concerns:
- **Domain Layer**: Core business logic, entities, domain events (no dependencies)
- **Application Layer**: Use cases, CQRS, interfaces, DTOs, validators
- **Infrastructure Layer**: External integrations (EF Core, AI, PostgreSQL)
- **API Layer**: REST controllers, middleware, configuration

**Dependency Flow**: API → Application → Domain ← Infrastructure

### CQRS with MediatR ✅
- **Commands**: Write operations (CreateSessionCommand)
- **Queries**: Read operations (GetSessionByIdQuery)
- **Handlers**: Separate handlers for each command/query
- **Validators**: FluentValidation for each request

### Repository Pattern ✅
8 repositories with interface abstraction:
- ISessionRepository, IPromptRecordRepository, IRetrievalChunkRepository
- IToolExecutionRepository, IAuditLogRepository, IMemoryItemRepository
- IRepositoryRepository, ICodeFileRepository

### Unit of Work Pattern ✅
Transaction management with:
- `SaveChangesAsync()` - Persist changes
- `BeginTransactionAsync()` - Start transaction
- `CommitTransactionAsync()` - Commit with rollback on failure
- `RollbackTransactionAsync()` - Manual rollback

### Domain Events ✅
Event-driven architecture:
- `BaseEntity` with domain event collection
- `IDomainEvent` interface extending MediatR INotification
- `DomainEventDispatcher` - Automatic event dispatching on SaveChanges
- Event handlers (e.g., `SessionCreatedEventHandler`)

### Pipeline Behaviors ✅
Automatic cross-cutting concerns:
- **ValidationBehaviour**: FluentValidation pipeline
- **LoggingBehaviour**: Request/response logging
- **PerformanceBehaviour**: Monitors requests >500ms

### Result Pattern ✅
Standardized responses:
```csharp
Result<T>.SuccessResult(data, "Success message")
Result<T>.FailureResult("Error message", "Error details")
```

### SOLID Principles ✅
- **SRP**: Each handler handles one command/query
- **OCP**: Pipeline behaviors extend without modification
- **LSP**: Interface-based design allows substitution
- **ISP**: Small, focused interfaces
- **DIP**: Dependency injection throughout

## 🚀 Key Features

### Core Features
- ✅ **CQRS with MediatR** - Commands and Queries separation
- ✅ **FluentValidation** - Automatic request validation pipeline
- ✅ **Pipeline Behaviors** - Logging, Performance monitoring, Validation
- ✅ **Domain Events** - Event-driven architecture support
- ✅ **Global Exception Handling** - Centralized error management
- ✅ **Request Logging Middleware** - Comprehensive request/response logging
- ✅ **AutoMapper** - Object mapping with profiles
- ✅ **Extension Methods** - Clean Program.cs with service and middleware extensions

### Enterprise Features
- ✅ **Health Checks** - Database health monitoring at `/health`
- ✅ **Rate Limiting** - Fixed window (200 req/min per user/IP)
- ✅ **API Versioning** - Query string and header-based version control
- ✅ **Cursor-Based Pagination** - Efficient pagination for large datasets
- ✅ **Unit of Work** - Transaction management with rollback
- ✅ **Repository Pattern** - Data access abstraction
- ✅ **Result Pattern** - Standardized response handling

### AI Features
- ✅ **Multi-Model Support** - OpenRouter gateway (GPT-4, Claude 3, Gemini, Llama 3)
- ✅ **Semantic Kernel** - AI orchestration framework
- ✅ **RAG Pipeline** - Code retrieval with embeddings
- ✅ **Vector Search** - Cosine similarity for code chunks
- ✅ **Human-in-the-Loop** - Tool execution approval workflow

## 📦 Technology Stack

### Backend
- **.NET 10** - Latest .NET framework
- **ASP.NET Core** - Web API
- **Entity Framework Core 10** - ORM with PostgreSQL
- **MediatR 12.4.1** - CQRS implementation
- **FluentValidation 11.11.0** - Request validation
- **AutoMapper 12.0.1** - Object mapping
- **Semantic Kernel 1.74** - AI orchestration
- **Serilog** - Structured logging
- **Swagger/OpenAPI** - API documentation

### Enterprise Packages
- **AspNetCore.HealthChecks.Npgsql 9.0.0** - Health monitoring
- **Asp.Versioning.Mvc 8.1.1** - API versioning
- **System.Threading.RateLimiting 10.0.0** - Rate limiting
- **Microsoft.Extensions.Logging.Abstractions 10.0.5** - Logging

### Database
- **PostgreSQL 14+** - Primary database
- **Npgsql 10.0.2** - PostgreSQL provider

### AI/ML
- **OpenRouter** - Unified API for multiple LLM providers
- **OpenAI Embeddings** - Text embeddings via OpenRouter

## 📋 Prerequisites

- .NET 10 SDK
- PostgreSQL 14+
- OpenRouter API Key ([Get one here](https://openrouter.ai/))

## 🚀 Quick Start

### 1. Clone and Build

```bash
git clone <repository-url>
cd AgentAI
dotnet restore
dotnet build
```

### 2. Configure Database

Update `AgentAI.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=AgentAI;Username=postgres;Password=your_password"
  }
}
```

### 3. Configure OpenRouter

Add your API key in `AgentAI.API/appsettings.json`:

```json
{
  "OpenRouter": {
    "ApiKey": "your-openrouter-api-key",
    "Model": "qwen/qwq-32b-preview",
    "EmbeddingModel": "text-embedding-3-small",
    "Endpoint": "https://openrouter.ai/api/v1",
    "SiteUrl": "http://localhost:5000",
    "SiteName": "AgentAI"
  }
}
```

**Available Models**: GPT-4, Claude 3, Gemini Pro, Llama 3, Qwen, and more at [OpenRouter Models](https://openrouter.ai/models)

### 4. Create Database

```bash
dotnet ef database update --project AgentAI.Infrastructure --startup-project AgentAI.API
```

### 5. Run the Application

```bash
cd AgentAI.API
dotnet run
```

**Endpoints**:
- HTTP: http://localhost:5000
- HTTPS: https://localhost:5001
- Swagger: https://localhost:5001/swagger
- Health: https://localhost:5001/health

## 🔌 API Endpoints

### Health & Monitoring
- `GET /health` - Database and API health status

### Chat API
- `POST /api/chat` - Send chat message
- `POST /api/chat/stream` - Stream chat response

### Session API (CQRS Examples)
- `POST /api/session` - Create session (Command)
- `GET /api/session/{id}` - Get session (Query)
- `GET /api/session/{id}/detail` - Get session with messages
- `GET /api/session/user/{userId}` - Get user sessions
- `PUT /api/session/{id}` - Update session
- `DELETE /api/session/{id}` - Delete session

### Retrieval API
- `POST /api/retrieval/search` - Search code chunks with RAG

### Tool API
- `POST /api/tool/execute` - Execute tool
- `POST /api/tool/{executionId}/approve` - Approve execution
- `POST /api/tool/{executionId}/reject` - Reject execution
- `GET /api/tool/history` - Get execution history

### Indexing API
- `POST /api/indexing/start` - Start repository indexing
- `GET /api/indexing/status/{jobId}` - Get indexing status
- `GET /api/indexing/repositories` - Get indexed repositories

## 📁 Project Structure

```
AgentAI/
├── AgentAI.Domain/
│   ├── Common/
│   │   ├── BaseEntity.cs              # Domain events support
│   │   ├── BaseAuditableEntity.cs     # Audit fields
│   │   └── IDomainEvent.cs            # Event interface
│   ├── Entities/                      # 9 domain entities
│   ├── Enums/                         # 6 enumerations
│   ├── Events/
│   │   └── SessionCreatedEvent.cs     # Domain event example
│   └── Interfaces/
│
├── AgentAI.Application/
│   ├── Common/
│   │   ├── Behaviours/
│   │   │   ├── ValidationBehaviour.cs
│   │   │   ├── LoggingBehaviour.cs
│   │   │   └── PerformanceBehaviour.cs
│   │   ├── Exceptions/
│   │   │   ├── ValidationException.cs
│   │   │   └── NotFoundException.cs
│   │   ├── Interfaces/
│   │   │   └── IUnitOfWork.cs
│   │   ├── Mappings/
│   │   │   └── MappingProfile.cs      # AutoMapper profiles
│   │   └── Models/
│   │       ├── Result.cs              # Result pattern
│   │       └── CursorPaginatedList.cs # Cursor pagination
│   ├── Features/
│   │   └── Sessions/
│   │       ├── Commands/              # CQRS Commands
│   │       ├── Queries/               # CQRS Queries
│   │       └── EventHandlers/         # Domain event handlers
│   ├── DTOs/                          # 21 DTOs
│   ├── Validators/                    # 6+ FluentValidation validators
│   └── DependencyInjection.cs         # Application DI
│
├── AgentAI.Infrastructure/
│   ├── Common/
│   │   └── DomainEventDispatcher.cs   # Event dispatcher
│   ├── Persistence/
│   │   ├── ApplicationDbContext.cs    # DbContext with events
│   │   ├── UnitOfWork.cs              # Transaction management
│   │   └── Configurations/            # 9 entity configurations
│   ├── Repositories/                  # 8 repository implementations
│   ├── Services/                      # 6 service implementations
│   ├── AI/
│   │   └── SemanticKernelLLMProvider.cs
│   └── DependencyInjection.cs
│
└── AgentAI.API/
    ├── Controllers/                   # 5 API controllers
    ├── Extensions/
    │   ├── ServiceExtensions.cs       # Service registration
    │   └── MiddlewareExtensions.cs    # Middleware pipeline
    ├── Middleware/
    │   ├── ExceptionHandlingMiddleware.cs
    │   └── RequestLoggingMiddleware.cs
    └── Program.cs                     # Clean startup
```

## 🔄 Request Flow

```
HTTP Request
  ↓
RequestLoggingMiddleware (logs request)
  ↓
ExceptionHandlingMiddleware (catches errors)
  ↓
Rate Limiter (enforces 200 req/min)
  ↓
Controller → MediatR.Send(Command/Query)
  ↓
ValidationBehaviour (validates request)
  ↓
LoggingBehaviour (logs execution)
  ↓
PerformanceBehaviour (monitors >500ms)
  ↓
Handler (uses Repository + UnitOfWork)
  ↓
Domain Events Dispatched (on SaveChanges)
  ↓
Result<T> → ApiResponse<T> → HTTP Response
```

## 📊 Database Schema

### Core Entities
- **Users**: User accounts with roles (Developer, TechLead, Admin, DevOps)
- **Sessions**: Chat sessions with repository context
- **PromptRecords**: User prompts and AI responses
- **RetrievalChunks**: Code chunks with embeddings for RAG
- **ToolExecutions**: Tool execution requests with approval workflow
- **AuditLogs**: Comprehensive audit trail
- **MemoryItems**: User and repository-specific memory
- **CodeFiles**: Indexed code files
- **Repositories**: Tracked repositories

## 💡 Usage Examples

### CQRS Command Example

```csharp
// In Controller
var command = new CreateSessionCommand
{
    UserId = userId,
    Title = "New Session",
    RepositoryPath = "/path/to/repo"
};

var result = await _mediator.Send(command, cancellationToken);

if (result.Success)
{
    return Ok(new ApiResponse<SessionDto>
    {
        Success = true,
        Data = result.Data,
        Message = result.Message
    });
}
```

### CQRS Query Example

```csharp
// In Controller
var query = new GetSessionByIdQuery(sessionId);
var result = await _mediator.Send(query, cancellationToken);

if (!result.Success)
{
    return NotFound(new ApiResponse<SessionDto>
    {
        Success = false,
        Errors = result.Errors
    });
}
```

### Domain Event Example

```csharp
// In Entity
public class Session : BaseEntity
{
    public void Create()
    {
        // Business logic
        AddDomainEvent(new SessionCreatedEvent(Id, UserId, Title));
    }
}

// Events automatically dispatched on SaveChangesAsync
```

### Unit of Work Example

```csharp
// In Handler
await _unitOfWork.BeginTransactionAsync(cancellationToken);

try
{
    await _sessionRepository.AddAsync(session, cancellationToken);
    await _auditLogRepository.AddAsync(auditLog, cancellationToken);
    
    await _unitOfWork.CommitTransactionAsync(cancellationToken);
}
catch
{
    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
    throw;
}
```

## 🔐 Security Features

- **Human-in-the-Loop**: Tool executions require approval based on risk level
- **Risk Assessment**: Automatic risk determination (Low, Medium, High, Critical)
- **Audit Logging**: Comprehensive audit trail for all actions
- **Input Validation**: FluentValidation for all requests
- **Error Handling**: Global exception handling middleware
- **Rate Limiting**: 200 requests per minute per user/IP
- **API Versioning**: Version control for backward compatibility

## 🧪 Testing

The application is designed with testability in mind:

- **Unit Tests**: Test individual components in isolation
- **Integration Tests**: Test database and service interactions
- **API Tests**: Test API endpoints end-to-end

Run tests:
```bash
dotnet test
```

## 📝 Development Workflow

1. **Domain First**: Define entities and business rules
2. **Application Layer**: Define use cases, commands, queries, and interfaces
3. **Infrastructure**: Implement repositories and services
4. **API Layer**: Expose functionality via REST API
5. **Testing**: Write tests at each layer
6. **Migration**: Create and apply database migrations

## 🎯 Enterprise Pattern Compliance

| Pattern | Status | Implementation |
|---------|--------|----------------|
| Clean Architecture | ✅ | 4 layers with dependency inversion |
| CQRS with MediatR | ✅ | Commands, Queries, Handlers |
| Repository Pattern | ✅ | 8 repositories with interfaces |
| Unit of Work | ✅ | Transaction management |
| Result Pattern | ✅ | Standardized responses |
| Domain Events | ✅ | Event-driven architecture |
| Pipeline Behaviors | ✅ | Validation, Logging, Performance |
| FluentValidation | ✅ | 6+ validators |
| AutoMapper | ✅ | Mapping profiles |
| Exception Handling | ✅ | Global middleware |
| Request Logging | ✅ | Comprehensive logging |
| Health Checks | ✅ | Database monitoring |
| Rate Limiting | ✅ | 200 req/min |
| API Versioning | ✅ | Query string & header |
| Cursor Pagination | ✅ | Efficient pagination |
| Extension Methods | ✅ | Clean DI registration |
| BaseEntity | ✅ | Domain event support |
| BaseAuditableEntity | ✅ | Audit fields |
| SOLID Principles | ✅ | All 5 principles |

**Pattern Compliance: 100%**

## 🚧 Implementation Status

✅ **Completed**:
- Complete Clean Architecture with all enterprise patterns
- CQRS with MediatR (Commands, Queries, Handlers, Validators)
- Domain Events with automatic dispatching
- Unit of Work with transaction management
- Pipeline Behaviors (Validation, Logging, Performance)
- Repository Pattern (8 repositories)
- Result Pattern for standardized responses
- FluentValidation with automatic pipeline
- AutoMapper with mapping profiles
- Global Exception Handling
- Request Logging Middleware
- Health Checks (PostgreSQL)
- Rate Limiting (200 req/min)
- API Versioning (v1.0)
- Cursor-Based Pagination
- Extension Methods for clean DI
- BaseEntity and BaseAuditableEntity
- Custom Exceptions (ValidationException, NotFoundException)
- Database schema and migrations
- Swagger/OpenAPI documentation
- Semantic Kernel AI integration
- OpenRouter multi-model support

⚠️ **Pending** (Simulated/Placeholder):
- Actual tool execution implementations
- Repository indexing service
- File system operations
- Git integration
- Full RAG pipeline optimization

## 🆘 Troubleshooting

### Database Connection Issues
- Ensure PostgreSQL is running: `pg_ctl status`
- Verify connection string in appsettings.json
- Check database user permissions
- Test connection: `psql -U postgres -d AgentAI`

### OpenRouter API Issues
- Verify API key at https://openrouter.ai/keys
- Check API credits and billing
- Ensure network connectivity
- Try different models if one is unavailable
- Check model availability at https://openrouter.ai/models

### Build Errors
- Run `dotnet restore` to restore NuGet packages
- Ensure .NET 10 SDK is installed: `dotnet --version`
- Clean and rebuild: `dotnet clean && dotnet build`
- Check for missing dependencies

### Migration Issues
- Delete existing database: `DROP DATABASE AgentAI;`
- Recreate database: `CREATE DATABASE AgentAI;`
- Apply migrations: `dotnet ef database update`

## 🤝 Contributing

This is an enterprise-grade application following strict coding standards:

- **SOLID Principles**: All 5 principles implemented
- **Clean Code**: Meaningful names, small functions, clear intent
- **DRY**: Don't Repeat Yourself
- **CQRS**: Separate read and write operations
- **Domain-Driven Design**: Rich domain models with domain events
- **Test-Driven Development**: Write tests first

## 📄 License

This project is for educational and demonstration purposes.

## 🙏 Acknowledgments

- Enterprise patterns inspired by [dotnet-enterprise-api](https://github.com/iamchittaranjandas/dotnet-enterprise-api)
- Clean Architecture by Robert C. Martin
- Domain-Driven Design by Eric Evans
- CQRS Pattern
- ASP.NET Core Team
- Semantic Kernel Team

## 📞 Support

For issues and questions:
- Check the troubleshooting section above
- Review API documentation at `/swagger`
- Check health endpoint at `/health`
- Review logs in console output

---

**Built with ❤️ using Clean Architecture and Enterprise Patterns**

**Framework**: .NET 10 | **Database**: PostgreSQL | **AI**: OpenRouter + Semantic Kernel | **Architecture**: Clean Architecture + CQRS + Domain Events
