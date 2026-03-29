# Software Requirements Specification (SRS)

## Agentic AI Developer Assistant (Copilot-like)

**Version:** 2.0 (Enhanced)
**Status:** Implementation-Ready
**Target Stack:** .NET 10, Semantic Kernel, RAG, Vector Database, Clean Architecture
**Document Type:** Software Requirements Specification
**Database:** PostgreSQL (Default), SQL Server, MySQL (Configurable)
**Architecture Pattern:** Clean Architecture with Domain-Driven Design
**Implementation Approach:** Bottom-Up, Dependency-First, Layer-by-Layer

---

## 1. Introduction

### 1.1 Purpose

This document defines the detailed software requirements for an **Agentic AI Developer Assistant**, a Copilot-like system that helps software developers understand code, generate code, fix bugs, refactor modules, and execute safe development tools with human approval.

The system is intended to support developers by combining:

* Large Language Model (LLM) reasoning
* Retrieval-Augmented Generation (RAG)
* Tool execution
* Project-aware context management
* Safe human-in-the-loop automation

### 1.2 Scope

The product will provide an AI-driven development workspace that can:

* Understand natural language developer prompts
* Retrieve relevant project context from source code and documentation
* Generate code, explanations, fixes, tests, and refactoring suggestions
* Execute approved tools such as file operations, build, test, and Git actions
* Maintain conversation and project context across sessions
* Apply guardrails to reduce unsafe or destructive behavior

The initial release will be a **single-agent system** with RAG and tool support. The design must remain extensible for multi-agent workflows in future releases.

### 1.3 Product Vision

The product aims to become a practical engineering assistant that is more than a chatbot. It should behave like a task-oriented coding partner that can reason over a codebase, act on files and tools, and provide structured help throughout the development lifecycle.

### 1.4 Objectives

The system shall:

* Reduce time spent on repetitive engineering tasks
* Improve developer productivity and code quality
* Provide codebase-aware answers instead of generic responses
* Help developers safely automate common development operations
* Keep users in control of any change that modifies code or environment state

### 1.5 Definitions, Acronyms, and Abbreviations

* **AI**: Artificial Intelligence
* **LLM**: Large Language Model
* **RAG**: Retrieval-Augmented Generation
* **Agent**: An AI workflow that can plan, reason, and invoke tools
* **Tool**: A controlled function that performs an external action
* **Vector DB**: Database used to store and search embeddings
* **Embedding**: Numeric representation of text for semantic search
* **Semantic Kernel**: Microsoft AI orchestration SDK
* **AOT**: Ahead-Of-Time compilation
* **MVP**: Minimum Viable Product
* **Human-in-the-loop**: User approval before sensitive actions

### 1.6 References

* .NET 10 / ASP.NET Core documentation
* Semantic Kernel documentation
* OpenAI / Azure OpenAI documentation
* General software engineering and secure AI design practices

### 1.7 Document Overview

This SRS defines product scope, users, use cases, architecture, functional and non-functional requirements, data requirements, security requirements, deployment expectations, and acceptance criteria.

---

## 2. Overall Description

### 2.1 Product Perspective

The system is a new product designed as an AI-powered development assistant. It can be used as:

* A web application
* A backend service for IDE extensions
* A future VS Code or editor integration layer

The first version should be implemented as a modular application with a clean service boundary so it can later support richer interfaces.

### 2.2 Product Functions

At a high level, the system shall:

* Accept developer prompts in plain language
* Detect user intent
* Retrieve relevant project code and knowledge
* Generate an answer, code change, or action plan
* Optionally execute approved tools
* Store session history and action logs

### 2.3 User Classes and Characteristics

#### 2.3.1 Developer

A developer uses the system to get help with coding, debugging, refactoring, explanation, and test generation.

#### 2.3.2 Tech Lead / Reviewer

A senior reviewer uses the system to inspect proposed changes, confirm quality, and review generated output.

#### 2.3.3 Admin

An admin manages access, settings, model configuration, logging policies, and tool permissions.

#### 2.3.4 DevOps / Platform Engineer

A platform user manages deployment, monitoring, secrets, runtime configuration, and resource scaling.

### 2.4 Operating Environment

The solution is expected to run in:

* Cloud or on-premises environments
* Docker containers
* Kubernetes-ready infrastructure
* Windows and Linux server environments
* A browser-based UI and/or editor integration layer

### 2.5 Constraints

* The system shall use .NET 10 for the backend platform
* Sensitive actions shall require explicit approval
* Code modifications shall be traceable
* The system shall support codebase indexing and semantic retrieval
* The system shall be extensible for future multi-agent expansion

### 2.6 Assumptions and Dependencies

* Access to an LLM provider or local model endpoint is available
* A vector database or equivalent semantic index is available
* Source code access is permitted for indexing and retrieval
* Users have permission to analyze the repository they connect
* The initial release focuses on a single project or repository scope

---

## 3. Product Scope and Goals

### 3.1 Business Goals

* Improve developer productivity
* Reduce repetitive manual coding effort
* Speed up debugging and implementation tasks
* Make codebase knowledge accessible through natural language
* Support enterprise-ready AI workflows with auditability

### 3.2 Success Criteria

The product is successful if it can:

* Answer repository-specific developer questions correctly
* Generate useful code with minimal manual correction
* Complete common tasks with tool support under user supervision
* Maintain safe execution boundaries
* Reduce time spent on routine development operations

---

## 4. System Context

### 4.1 External Actors

* End user / developer
* LLM provider
* Vector database
* File system or repository storage
* Git provider or local Git repository
* CI/build runner
* Authentication service
* Logging and monitoring services

### 4.2 System Boundary

Inside the system:

* Prompt handling
* Agent orchestration
* Retrieval pipeline
* Tool orchestration
* Policy enforcement
* Conversation state management
* Audit logging

Outside the system:

* LLM hosting
* Source repository storage
* External tool execution infrastructure
* Identity provider
* Telemetry platform

---

## 5. Use Cases

### 5.1 Use Case UC-01: Ask Questions About the Codebase

**Actor:** Developer
**Goal:** Understand how a part of the codebase works.

**Main Flow:**

1. Developer enters a question.
2. System identifies the relevant repository and context.
3. System retrieves matching code and documentation.
4. System generates an explanation.
5. Developer reviews the response.

**Outcome:** Developer gets a project-aware answer.

### 5.2 Use Case UC-02: Generate New Code

**Actor:** Developer
**Goal:** Generate a class, method, API endpoint, or component.

**Main Flow:**

1. Developer describes required functionality.
2. System retrieves similar code patterns.
3. System drafts code based on project conventions.
4. System presents the code for review.
5. Developer approves or requests changes.

### 5.3 Use Case UC-03: Fix a Bug

**Actor:** Developer
**Goal:** Diagnose and fix a defect.

**Main Flow:**

1. Developer submits error details or stack trace.
2. System retrieves related code and logs.
3. System identifies likely root causes.
4. System proposes a fix.
5. User approves and applies the fix.

### 5.4 Use Case UC-04: Refactor Existing Code

**Actor:** Developer or Tech Lead
**Goal:** Improve readability, structure, or maintainability.

### 5.5 Use Case UC-05: Generate Tests

**Actor:** Developer
**Goal:** Create unit or integration tests for code.

### 5.6 Use Case UC-06: Execute Safe Tools

**Actor:** Developer
**Goal:** Run build, test, Git diff, or file operations through the agent.

### 5.7 Use Case UC-07: Audit Past Actions

**Actor:** Admin / Tech Lead
**Goal:** Review what the AI changed, suggested, or executed.

---

## 6. Functional Requirements

### 6.1 Prompt Handling

**FR-001** The system shall accept natural language prompts from the user.
**FR-002** The system shall support follow-up prompts within the same session.
**FR-003** The system shall classify intent such as explain, generate, fix, refactor, test, search, or execute.
**FR-004** The system shall preserve conversation context for the active session.

### 6.2 Context Retrieval

**FR-010** The system shall index project files into a retrievable knowledge base.
**FR-011** The system shall chunk source code using meaningful boundaries such as class, method, function, and document sections.
**FR-012** The system shall generate embeddings for indexed chunks.
**FR-013** The system shall retrieve top relevant results based on semantic similarity.
**FR-014** The system shall inject only relevant context into the model prompt.
**FR-015** The system shall support filtering by repository, branch, project area, file type, and recency.

### 6.3 Response Generation

**FR-020** The system shall generate explanations, code snippets, implementation plans, or fix suggestions.
**FR-021** The system shall provide structured outputs when requested.
**FR-022** The system shall explain assumptions made in generated responses.
**FR-023** The system shall highlight uncertain or inferred parts when confidence is low.

### 6.4 Code Generation and Modification

**FR-030** The system shall generate code following project conventions where available.
**FR-031** The system shall produce code changes as diffs or file updates, not only as plain text.
**FR-032** The system shall not apply destructive file changes without approval.
**FR-033** The system shall validate that generated code is syntactically reasonable before proposing it.
**FR-034** The system shall support partial file edits and full file generation.

### 6.5 Tool Invocation

**FR-040** The system shall support controlled tool execution through a tool registry.
**FR-041** The system shall expose file read operations through a File Tool.
**FR-042** The system shall expose file write operations only with explicit approval or policy clearance.
**FR-043** The system shall expose build execution through a Build Tool.
**FR-044** The system shall expose test execution through a Test Tool.
**FR-045** The system shall expose Git diff and commit support through a Git Tool.
**FR-046** The system shall log every tool invocation.

### 6.6 Safety and Approval

**FR-050** The system shall require explicit user approval before modifying source files.
**FR-051** The system shall require approval before running commands that may affect the environment, repository state, or data integrity.
**FR-052** The system shall block or warn against destructive operations.
**FR-053** The system shall detect and reject unsafe prompt-injection attempts when possible.

### 6.7 Session and Memory

**FR-060** The system shall store session history for ongoing conversations.
**FR-061** The system shall store tool execution history.
**FR-062** The system shall optionally store durable project memory such as coding conventions, architecture notes, and user preferences.
**FR-063** The system shall allow memory entries to be updated or removed by authorized users.

### 6.8 Administration

**FR-070** The system shall support role-based access control.
**FR-071** The system shall allow administrators to configure models, tool permissions, and retrieval settings.
**FR-072** The system shall provide logs and traceability for audits.
**FR-073** The system shall allow feature flags for future capabilities.

---

## 7. Non-Functional Requirements

### 7.1 Performance

**NFR-001** The system shall return an initial assistant response within an acceptable interactive timeframe for developer use.
**NFR-002** Retrieval operations shall complete quickly enough to support conversational usage.
**NFR-003** The system shall support caching for repeated queries and frequently accessed context.

### 7.2 Scalability

**NFR-010** The system shall support multiple repositories and multiple users.
**NFR-011** The system shall be designed for horizontal scaling of stateless services.
**NFR-012** The retrieval layer shall support growing codebase size.

### 7.3 Availability

**NFR-020** The system shall be designed for high availability in hosted environments.
**NFR-021** The system shall degrade gracefully if the model provider is temporarily unavailable.

### 7.4 Maintainability

**NFR-030** The codebase shall follow Clean Architecture principles.
**NFR-031** Business logic shall remain independent of UI and infrastructure concerns.
**NFR-032** Tool modules shall be independently testable.

### 7.5 Security

**NFR-040** The system shall authenticate users before allowing access.
**NFR-041** The system shall authorize sensitive actions based on roles and permissions.
**NFR-042** The system shall protect secrets and API keys using secure configuration mechanisms.
**NFR-043** The system shall log security-sensitive actions.

### 7.6 Reliability

**NFR-050** The system shall prevent tool failures from crashing the whole application.
**NFR-051** The system shall recover from transient failures where possible.
**NFR-052** The system shall preserve conversation state across recoverable interruptions.

### 7.7 Observability

**NFR-060** The system shall provide structured logs.
**NFR-061** The system shall support distributed tracing.
**NFR-062** The system shall expose metrics for model calls, retrieval latency, tool usage, and error rates.

### 7.8 Compliance and Auditability

**NFR-070** The system shall maintain audit records for prompts, tool usage, and file changes.
**NFR-071** The system shall provide traceability from user request to system action.
**NFR-072** The system shall support retention policies for logs and memory.

---

## 8. Proposed Technical Architecture

### 8.1 Architectural Style

* Clean Architecture
* Modular Monolith for MVP
* Service boundaries suitable for future microservices
* Single-agent orchestration for the first release

### 8.2 Backend Stack

**Core Framework:**
* .NET 10 SDK
* ASP.NET Core Web API (Controller-based)
* C# 12 with nullable reference types enabled

**AI & Orchestration:**
* Microsoft.SemanticKernel (latest stable)
* Microsoft.SemanticKernel.Connectors.OpenAI
* Microsoft.SemanticKernel.Plugins.Core

**Data Access:**
* Entity Framework Core 9.0
* Npgsql.EntityFrameworkCore.PostgreSQL (default)
* Microsoft.EntityFrameworkCore.SqlServer (optional)
* Dapper for high-performance queries

**Vector Database:**
* Pgvector extension for PostgreSQL (recommended)
* OR Qdrant vector database
* OR Azure AI Search

**Caching & Performance:**
* Microsoft.Extensions.Caching.Memory
* StackExchange.Redis (optional)
* Microsoft.AspNetCore.OutputCaching

**Validation & Mapping:**
* FluentValidation.AspNetCore
* Mapster OR AutoMapper

**Logging & Monitoring:**
* Serilog.AspNetCore
* Serilog.Sinks.Console
* Serilog.Sinks.File
* Serilog.Sinks.PostgreSQL (optional)

**Authentication & Security:**
* Microsoft.AspNetCore.Authentication.JwtBearer
* Microsoft.AspNetCore.Identity.EntityFrameworkCore

**API Documentation:**
* Swashbuckle.AspNetCore (Swagger/OpenAPI)

**Testing:**
* xUnit
* Moq
* FluentAssertions
* Microsoft.AspNetCore.Mvc.Testing

### 8.3 AI and Retrieval Stack

* LLM provider through API or local endpoint
* Embedding generation for chunk indexing
* Vector database for semantic search
* Prompt templates and structured tool calling
* Memory store for durable knowledge

### 8.4 Layer Responsibilities

#### Domain Layer (AgentAI.Domain)

**Purpose:** Core business entities and domain logic, framework-independent

**Contains:**
* **Entities/**: Domain entities with business logic
  - User.cs
  - Session.cs
  - PromptRecord.cs
  - RetrievalChunk.cs
  - ToolExecution.cs
  - AuditLog.cs
  - MemoryItem.cs
  - CodeFile.cs
  - Repository.cs
* **Enums/**: Domain enumerations
  - IntentType.cs
  - ToolType.cs
  - ApprovalStatus.cs
  - RiskLevel.cs
  - SessionStatus.cs
  - UserRole.cs
* **ValueObjects/**: Immutable value objects
  - FilePath.cs
  - CodeChunk.cs
  - EmbeddingVector.cs
* **Interfaces/**: Domain service contracts
  - IEntity.cs
  - IAuditableEntity.cs
* **Exceptions/**: Domain-specific exceptions
  - DomainException.cs
  - ValidationException.cs

**Dependencies:** None (pure domain logic)

#### Application Layer (AgentAI.Application)

**Purpose:** Use cases, business workflows, and application services

**Contains:**
* **DTOs/**: Data transfer objects
  - Chat/
    * ChatRequest.cs
    * ChatResponse.cs
    * MessageDto.cs
  - Session/
    * CreateSessionRequest.cs
    * SessionDto.cs
  - Retrieval/
    * RetrievalRequest.cs
    * RetrievalResultDto.cs
  - Tool/
    * ToolExecutionRequest.cs
    * ToolExecutionResultDto.cs
  - Indexing/
    * IndexingRequest.cs
    * IndexingStatusDto.cs
* **Interfaces/**: Application service contracts
  - Services/
    * IAgentOrchestrationService.cs
    * IRetrievalService.cs
    * IToolExecutionService.cs
    * ISessionService.cs
    * IIndexingService.cs
    * IEmbeddingService.cs
  - Repositories/
    * ISessionRepository.cs
    * IPromptRecordRepository.cs
    * IRetrievalChunkRepository.cs
    * IToolExecutionRepository.cs
    * IAuditLogRepository.cs
    * IMemoryItemRepository.cs
  - Infrastructure/
    * ILLMProvider.cs
    * IVectorStore.cs
    * IFileSystemService.cs
    * IGitService.cs
* **UseCases/**: CQRS-style use case handlers
  - Chat/
    * ProcessChatCommandHandler.cs
  - Retrieval/
    * SearchCodeQueryHandler.cs
  - Tool/
    * ExecuteToolCommandHandler.cs
* **Validators/**: FluentValidation validators
  - ChatRequestValidator.cs
  - ToolExecutionRequestValidator.cs
* **Mappings/**: DTO mappings
  - MappingProfile.cs
* **Common/**: Shared application logic
  - Result.cs
  - PaginatedList.cs
  - ApiResponse.cs

**Dependencies:** Domain layer only

#### Infrastructure Layer (AgentAI.Infrastructure)

**Purpose:** External integrations and technical implementations

**Contains:**
* **Persistence/**: Database implementation
  - ApplicationDbContext.cs
  - Configurations/
    * UserConfiguration.cs
    * SessionConfiguration.cs
    * PromptRecordConfiguration.cs
    * RetrievalChunkConfiguration.cs
    * ToolExecutionConfiguration.cs
  - Repositories/
    * SessionRepository.cs
    * PromptRecordRepository.cs
    * RetrievalChunkRepository.cs
    * ToolExecutionRepository.cs
    * AuditLogRepository.cs
    * MemoryItemRepository.cs
  - Migrations/
* **AI/**: AI service implementations
  - SemanticKernel/
    * SemanticKernelService.cs
    * KernelBuilder.cs
    * PromptTemplates.cs
  - LLMProviders/
    * OpenAIProvider.cs
    * AzureOpenAIProvider.cs
  - Embeddings/
    * EmbeddingService.cs
* **VectorStore/**: Vector database implementations
  - PgVector/
    * PgVectorStore.cs
  - Qdrant/
    * QdrantVectorStore.cs
* **Services/**: Application service implementations
  - AgentOrchestrationService.cs
  - RetrievalService.cs
  - ToolExecutionService.cs
  - SessionService.cs
  - IndexingService.cs
* **Tools/**: Tool implementations
  - FileTool.cs
  - GitTool.cs
  - BuildTool.cs
  - TestTool.cs
  - DatabaseTool.cs
* **FileSystem/**: File operations
  - FileSystemService.cs
  - CodeParser.cs
  - ChunkingService.cs
* **Git/**: Git operations
  - GitService.cs
* **Caching/**: Caching implementations
  - CacheService.cs
* **Logging/**: Logging setup
  - SerilogConfiguration.cs

**Dependencies:** Domain, Application layers

#### Web/API Layer (AgentAI.API)

**Purpose:** HTTP API endpoints and presentation logic

**Contains:**
* **Controllers/**: API controllers
  - ChatController.cs
  - SessionController.cs
  - RetrievalController.cs
  - ToolController.cs
  - IndexingController.cs
  - AdminController.cs
* **Middleware/**: Custom middleware
  - ExceptionHandlingMiddleware.cs
  - RequestLoggingMiddleware.cs
  - ApprovalMiddleware.cs
* **Filters/**: Action filters
  - ValidationFilter.cs
  - AuthorizationFilter.cs
* **Extensions/**: Service registration
  - ServiceCollectionExtensions.cs
  - ApplicationBuilderExtensions.cs
* **Configuration/**: Startup configuration
  - Program.cs
  - appsettings.json
  - appsettings.Development.json
  - appsettings.Production.json

**Dependencies:** Application, Infrastructure layers

---

## 9. Agent Design Specification

### 9.1 Agent Type

The MVP shall use a **single agent** that can perform task planning, retrieval, reasoning, and tool calling.

### 9.2 Agent Capabilities

* Intent classification
* Retrieval planning
* Context summarization
* Step-by-step reasoning
* Tool selection
* Code drafting
* Change proposal generation
* Safety checking

### 9.3 Agent Workflow

1. Receive user prompt
2. Identify intent and task type
3. Determine whether retrieval is needed
4. Query the vector index and/or project metadata
5. Summarize the retrieved context
6. Decide whether a tool call is needed
7. Request approval for sensitive actions
8. Execute approved tool action
9. Produce the final response
10. Store the interaction and logs

### 9.4 Agent Constraints

* The agent shall not bypass approval policies
* The agent shall not directly execute unsafe commands
* The agent shall prefer retrieval over guessing
* The agent shall clearly state uncertainty when needed

---

## 10. RAG Specification

### 10.1 Retrieval Goals

The retrieval system shall provide project-specific, accurate, and context-rich information to reduce hallucination and improve relevance.

### 10.2 Chunking Strategy

Code and documentation should be split using meaningful units such as:

* File-level metadata
* Class-level blocks
* Method-level blocks
* Markdown sections
* Configuration blocks

### 10.3 Embedding Strategy

The system shall generate embeddings for chunks and metadata using a compatible embedding model. The embedding strategy shall support updates when files change.

### 10.4 Retrieval Strategy

The system shall:

* Perform semantic search
* Prefer top-k relevant chunks
* Support filtering and reranking
* Optionally combine semantic and keyword search

### 10.5 Context Assembly

The system shall assemble retrieved content into a prompt-ready context window while respecting token limits and prioritizing the most relevant information.

### 10.6 Index Refresh

The system shall support incremental re-indexing when files are added, changed, or deleted.

---

## 11. Tool System Specification

### 11.1 Tool Registry

The system shall maintain a registry of tools with metadata such as name, description, permissions, and allowed actions.

### 11.2 Required Tools

#### File Tool

* Read file
* List files
* Write file with approval
* Patch file with approval

#### Git Tool

* View diff
* Show changed files
* Commit changes with approval

#### Build Tool

* Run project build
* Return build errors and warnings

#### Test Tool

* Run selected tests
* Run all tests
* Return failures and stack traces

#### Database Tool

* Execute safe read-only queries
* Optionally support migrations or writes with strict approval

### 11.3 Tool Safety Rules

* Every tool call shall be logged
* High-risk tools shall require approval
* Tool outputs shall be validated before use
* Commands shall be run in restricted, controlled environments where possible

---

## 12. User Interface Requirements

### 12.1 Primary UI Modes

The system may be delivered through:

* Web application
* IDE extension
* Internal admin console

### 12.2 Required UI Features

* Prompt input box
* Conversation panel
* Context preview panel
* File diff viewer
* Approval dialog for actions
* Tool execution history
* Session history
* Settings panel

### 12.3 UX Requirements

* Responses shall be easy to read and structured
* Code blocks shall be clearly separated
* Diffs shall highlight additions and removals
* Sensitive actions shall be clearly labeled before approval
* The user shall always know when the system is using code context or tools

---

## 13. API Requirements

### 13.1 Core APIs

#### Chat API

**Base Route:** `/api/v1/chat`

**Endpoints:**

**1. POST /api/v1/chat/send**
- **Purpose:** Send a prompt and receive agent response
- **Authentication:** Required (JWT)
- **Request:**
```csharp
public class ChatRequest
{
    [Required]
    [MaxLength(10000)]
    public string Prompt { get; set; }
    
    [Required]
    public Guid SessionId { get; set; }
    
    public bool IncludeContext { get; set; } = true;
    
    public int MaxContextChunks { get; set; } = 5;
    
    public string? RepositoryPath { get; set; }
    
    public string? Branch { get; set; }
}
```
- **Response:**
```csharp
public class ChatResponse
{
    public Guid PromptRecordId { get; set; }
    public string Response { get; set; }
    public IntentType DetectedIntent { get; set; }
    public int TokensUsed { get; set; }
    public int ContextChunksRetrieved { get; set; }
    public TimeSpan ResponseTime { get; set; }
    public List<RetrievalChunkDto>? RetrievedChunks { get; set; }
    public List<ToolExecutionDto>? SuggestedTools { get; set; }
}
```
- **Status Codes:** 200 OK, 400 Bad Request, 401 Unauthorized, 500 Internal Server Error

**2. POST /api/v1/chat/stream**
- **Purpose:** Stream agent response using Server-Sent Events (SSE)
- **Authentication:** Required (JWT)
- **Request:** Same as ChatRequest
- **Response:** SSE stream with partial responses
- **Status Codes:** 200 OK, 400 Bad Request, 401 Unauthorized

#### Session API

**Base Route:** `/api/v1/sessions`

**Endpoints:**

**1. POST /api/v1/sessions**
- **Purpose:** Create a new conversation session
- **Authentication:** Required (JWT)
- **Request:**
```csharp
public class CreateSessionRequest
{
    [MaxLength(500)]
    public string? Title { get; set; }
    
    [MaxLength(1000)]
    public string? RepositoryPath { get; set; }
    
    [MaxLength(200)]
    public string? Branch { get; set; }
}
```
- **Response:**
```csharp
public class SessionDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public SessionStatus Status { get; set; }
    public string? RepositoryPath { get; set; }
    public string? Branch { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime LastActivityAt { get; set; }
    public int MessageCount { get; set; }
}
```
- **Status Codes:** 201 Created, 400 Bad Request, 401 Unauthorized

**2. GET /api/v1/sessions**
- **Purpose:** Get all sessions for current user
- **Authentication:** Required (JWT)
- **Query Parameters:** `status`, `page`, `pageSize`, `sortBy`
- **Response:** `PaginatedList<SessionDto>`
- **Status Codes:** 200 OK, 401 Unauthorized

**3. GET /api/v1/sessions/{id}**
- **Purpose:** Get session details with message history
- **Authentication:** Required (JWT)
- **Response:**
```csharp
public class SessionDetailDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public SessionStatus Status { get; set; }
    public string? RepositoryPath { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime LastActivityAt { get; set; }
    public int MessageCount { get; set; }
    public List<MessageDto> Messages { get; set; }
}
```
- **Status Codes:** 200 OK, 404 Not Found, 401 Unauthorized

**4. PUT /api/v1/sessions/{id}**
- **Purpose:** Update session (title, status)
- **Authentication:** Required (JWT)
- **Request:**
```csharp
public class UpdateSessionRequest
{
    [MaxLength(500)]
    public string? Title { get; set; }
    
    public SessionStatus? Status { get; set; }
}
```
- **Response:** `SessionDto`
- **Status Codes:** 200 OK, 404 Not Found, 400 Bad Request

**5. DELETE /api/v1/sessions/{id}**
- **Purpose:** Archive or delete a session
- **Authentication:** Required (JWT)
- **Status Codes:** 204 No Content, 404 Not Found

#### Retrieval API

**Base Route:** `/api/v1/retrieval`

**Endpoints:**

**1. POST /api/v1/retrieval/search**
- **Purpose:** Search code and documentation semantically
- **Authentication:** Required (JWT)
- **Request:**
```csharp
public class RetrievalRequest
{
    [Required]
    [MaxLength(5000)]
    public string Query { get; set; }
    
    public string? RepositoryPath { get; set; }
    
    public string? Branch { get; set; }
    
    public List<string>? FileExtensions { get; set; }
    
    public string? ChunkType { get; set; }
    
    public int MaxResults { get; set; } = 10;
    
    public float MinSimilarity { get; set; } = 0.7f;
}
```
- **Response:**
```csharp
public class RetrievalResultDto
{
    public List<RetrievalChunkDto> Chunks { get; set; }
    public int TotalResults { get; set; }
    public TimeSpan SearchDuration { get; set; }
}

public class RetrievalChunkDto
{
    public Guid Id { get; set; }
    public string FilePath { get; set; }
    public string FileName { get; set; }
    public string ChunkContent { get; set; }
    public int StartLine { get; set; }
    public int EndLine { get; set; }
    public string ChunkType { get; set; }
    public string? ClassName { get; set; }
    public string? MethodName { get; set; }
    public float SimilarityScore { get; set; }
}
```
- **Status Codes:** 200 OK, 400 Bad Request, 401 Unauthorized

#### Tool Execution API

**Base Route:** `/api/v1/tools`

**Endpoints:**

**1. POST /api/v1/tools/execute**
- **Purpose:** Request tool execution (requires approval for high-risk)
- **Authentication:** Required (JWT)
- **Request:**
```csharp
public class ToolExecutionRequest
{
    [Required]
    public Guid SessionId { get; set; }
    
    [Required]
    public ToolType ToolType { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Action { get; set; }
    
    [Required]
    public Dictionary<string, object> Parameters { get; set; }
    
    public bool AutoApprove { get; set; } = false;
}
```
- **Response:**
```csharp
public class ToolExecutionResultDto
{
    public Guid ExecutionId { get; set; }
    public ApprovalStatus ApprovalStatus { get; set; }
    public RiskLevel RiskLevel { get; set; }
    public bool RequiresApproval { get; set; }
    public string? Output { get; set; }
    public bool WasSuccessful { get; set; }
    public string? ErrorMessage { get; set; }
    public TimeSpan? ExecutionDuration { get; set; }
}
```
- **Status Codes:** 200 OK, 202 Accepted (pending approval), 400 Bad Request, 403 Forbidden

**2. POST /api/v1/tools/{executionId}/approve**
- **Purpose:** Approve pending tool execution
- **Authentication:** Required (JWT, appropriate role)
- **Status Codes:** 200 OK, 404 Not Found, 403 Forbidden

**3. POST /api/v1/tools/{executionId}/reject**
- **Purpose:** Reject pending tool execution
- **Authentication:** Required (JWT)
- **Request:**
```csharp
public class RejectToolRequest
{
    [MaxLength(1000)]
    public string? Reason { get; set; }
}
```
- **Status Codes:** 200 OK, 404 Not Found

**4. GET /api/v1/tools/history**
- **Purpose:** Get tool execution history
- **Authentication:** Required (JWT)
- **Query Parameters:** `sessionId`, `toolType`, `status`, `page`, `pageSize`
- **Response:** `PaginatedList<ToolExecutionDto>`
- **Status Codes:** 200 OK, 401 Unauthorized

#### Indexing API

**Base Route:** `/api/v1/indexing`

**Endpoints:**

**1. POST /api/v1/indexing/start**
- **Purpose:** Start indexing a repository
- **Authentication:** Required (JWT, Admin role)
- **Request:**
```csharp
public class IndexingRequest
{
    [Required]
    [MaxLength(1000)]
    public string RepositoryPath { get; set; }
    
    [MaxLength(200)]
    public string? Branch { get; set; }
    
    public bool FullReindex { get; set; } = false;
    
    public List<string>? FileExtensions { get; set; }
    
    public List<string>? ExcludePatterns { get; set; }
}
```
- **Response:**
```csharp
public class IndexingStatusDto
{
    public Guid JobId { get; set; }
    public string Status { get; set; }
    public int TotalFiles { get; set; }
    public int ProcessedFiles { get; set; }
    public int TotalChunks { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? ErrorMessage { get; set; }
}
```
- **Status Codes:** 202 Accepted, 400 Bad Request, 403 Forbidden

**2. GET /api/v1/indexing/status/{jobId}**
- **Purpose:** Get indexing job status
- **Authentication:** Required (JWT)
- **Response:** `IndexingStatusDto`
- **Status Codes:** 200 OK, 404 Not Found

**3. GET /api/v1/indexing/repositories**
- **Purpose:** List indexed repositories
- **Authentication:** Required (JWT)
- **Response:** `List<RepositoryDto>`
- **Status Codes:** 200 OK

#### Admin API

**Base Route:** `/api/v1/admin`

**Endpoints:**

**1. GET /api/v1/admin/audit-logs**
- **Purpose:** Retrieve audit logs
- **Authentication:** Required (JWT, Admin role)
- **Query Parameters:** `userId`, `action`, `entityType`, `startDate`, `endDate`, `page`, `pageSize`
- **Response:** `PaginatedList<AuditLogDto>`
- **Status Codes:** 200 OK, 403 Forbidden

**2. GET /api/v1/admin/metrics**
- **Purpose:** Get system metrics
- **Authentication:** Required (JWT, Admin role)
- **Response:**
```csharp
public class SystemMetricsDto
{
    public int TotalUsers { get; set; }
    public int ActiveSessions { get; set; }
    public int TotalPrompts { get; set; }
    public int TotalToolExecutions { get; set; }
    public int IndexedRepositories { get; set; }
    public int TotalChunks { get; set; }
    public Dictionary<string, int> PromptsByIntent { get; set; }
    public Dictionary<string, int> ToolExecutionsByType { get; set; }
}
```
- **Status Codes:** 200 OK, 403 Forbidden

**3. POST /api/v1/admin/users**
- **Purpose:** Create new user
- **Authentication:** Required (JWT, Admin role)
- **Request:**
```csharp
public class CreateUserRequest
{
    [Required]
    [EmailAddress]
    [MaxLength(256)]
    public string Email { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string FullName { get; set; }
    
    [Required]
    public UserRole Role { get; set; }
    
    [Required]
    [MinLength(8)]
    public string Password { get; set; }
}
```
- **Response:** `UserDto`
- **Status Codes:** 201 Created, 400 Bad Request, 403 Forbidden

### 13.2 API Characteristics

* RESTful by default
* Versioned endpoints (v1, v2, etc.)
* Authenticated access using JWT Bearer tokens
* Structured request and response contracts
* Error handling with meaningful status codes
* Consistent response format:
```csharp
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    public List<string>? Errors { get; set; }
    public DateTime Timestamp { get; set; }
}
```
* Pagination support:
```csharp
public class PaginatedList<T>
{
    public List<T> Items { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public bool HasPrevious { get; set; }
    public bool HasNext { get; set; }
}
```
* Rate limiting: 100 requests per minute per user
* CORS enabled for configured origins
* Request/Response logging via middleware
* OpenAPI/Swagger documentation at `/swagger`

---

## 14. Data Requirements

### 14.1 Core Entities

#### User Entity

**Purpose:** Represents a person who interacts with the system.

**Properties:**
```csharp
public class User : IAuditableEntity
{
    public Guid Id { get; set; }
    public string Email { get; set; } // Required, unique, max 256
    public string FullName { get; set; } // Required, max 200
    public string PasswordHash { get; set; } // Required
    public UserRole Role { get; set; } // Enum: Developer, TechLead, Admin, DevOps
    public bool IsActive { get; set; } // Default: true
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    
    // Navigation Properties
    public ICollection<Session> Sessions { get; set; }
    public ICollection<MemoryItem> MemoryItems { get; set; }
}
```

**Database Mapping:**
- Table: Users
- Primary Key: Id (Guid)
- Indexes: Email (Unique), CreatedAt
- Constraints: Email unique, FullName required

#### Session Entity

**Purpose:** Represents a conversation thread or task thread.

**Properties:**
```csharp
public class Session : IAuditableEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } // Max 500, auto-generated from first prompt
    public SessionStatus Status { get; set; } // Enum: Active, Paused, Completed, Archived
    public string? RepositoryPath { get; set; } // Max 1000
    public string? Branch { get; set; } // Max 200
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public DateTime LastActivityAt { get; set; }
    public int MessageCount { get; set; } // Default: 0
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    
    // Navigation Properties
    public User User { get; set; }
    public ICollection<PromptRecord> PromptRecords { get; set; }
    public ICollection<ToolExecution> ToolExecutions { get; set; }
}
```

**Database Mapping:**
- Table: Sessions
- Primary Key: Id (Guid)
- Foreign Keys: UserId -> Users.Id (Cascade)
- Indexes: UserId, Status, LastActivityAt, CreatedAt

#### PromptRecord Entity

**Purpose:** Stores a user prompt and model response metadata.

**Properties:**
```csharp
public class PromptRecord : IAuditableEntity
{
    public Guid Id { get; set; }
    public Guid SessionId { get; set; }
    public string UserPrompt { get; set; } // Required, max 10000
    public IntentType DetectedIntent { get; set; } // Enum: Explain, Generate, Fix, Refactor, Test, Search, Execute
    public string? AssistantResponse { get; set; } // Max 50000
    public int TokensUsed { get; set; } // Default: 0
    public int ContextChunksRetrieved { get; set; } // Default: 0
    public TimeSpan ResponseTime { get; set; }
    public bool WasSuccessful { get; set; } // Default: true
    public string? ErrorMessage { get; set; } // Max 2000
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    
    // Navigation Properties
    public Session Session { get; set; }
    public ICollection<RetrievalChunk> RetrievedChunks { get; set; }
}
```

**Database Mapping:**
- Table: PromptRecords
- Primary Key: Id (Guid)
- Foreign Keys: SessionId -> Sessions.Id (Cascade)
- Indexes: SessionId, DetectedIntent, CreatedAt, WasSuccessful

#### RetrievalChunk Entity

**Purpose:** Stores indexed code or document segments.

**Properties:**
```csharp
public class RetrievalChunk : IAuditableEntity
{
    public Guid Id { get; set; }
    public Guid? CodeFileId { get; set; }
    public string FilePath { get; set; } // Required, max 1000
    public string FileName { get; set; } // Required, max 255
    public string FileExtension { get; set; } // Max 50
    public string ChunkContent { get; set; } // Required, max 20000
    public int StartLine { get; set; }
    public int EndLine { get; set; }
    public string ChunkType { get; set; } // Class, Method, Function, Documentation, Configuration
    public string? ClassName { get; set; } // Max 500
    public string? MethodName { get; set; } // Max 500
    public float[] Embedding { get; set; } // Vector embedding (1536 dimensions for OpenAI)
    public string? RepositoryPath { get; set; } // Max 1000
    public string? Branch { get; set; } // Max 200
    public DateTime IndexedAt { get; set; }
    public DateTime FileLastModified { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    
    // Navigation Properties
    public CodeFile? CodeFile { get; set; }
}
```

**Database Mapping:**
- Table: RetrievalChunks
- Primary Key: Id (Guid)
- Foreign Keys: CodeFileId -> CodeFiles.Id (Set Null)
- Indexes: FilePath, FileName, ChunkType, IndexedAt
- Vector Index: Embedding (using pgvector or equivalent)

#### ToolExecution Entity

**Purpose:** Stores tool name, input, output, timestamp, and approval status.

**Properties:**
```csharp
public class ToolExecution : IAuditableEntity
{
    public Guid Id { get; set; }
    public Guid SessionId { get; set; }
    public Guid? PromptRecordId { get; set; }
    public ToolType ToolType { get; set; } // Enum: File, Git, Build, Test, Database
    public string ToolName { get; set; } // Required, max 200
    public string Action { get; set; } // Required, max 200 (e.g., ReadFile, WriteFile, GitDiff)
    public string InputParameters { get; set; } // JSON, max 10000
    public string? Output { get; set; } // Max 50000
    public ApprovalStatus ApprovalStatus { get; set; } // Enum: Pending, Approved, Rejected, AutoApproved
    public RiskLevel RiskLevel { get; set; } // Enum: Low, Medium, High, Critical
    public DateTime RequestedAt { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime? ExecutedAt { get; set; }
    public TimeSpan? ExecutionDuration { get; set; }
    public bool WasSuccessful { get; set; } // Default: false
    public string? ErrorMessage { get; set; } // Max 2000
    public string? ApprovedBy { get; set; } // Max 256
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    
    // Navigation Properties
    public Session Session { get; set; }
    public PromptRecord? PromptRecord { get; set; }
}
```

**Database Mapping:**
- Table: ToolExecutions
- Primary Key: Id (Guid)
- Foreign Keys: SessionId -> Sessions.Id (Cascade), PromptRecordId -> PromptRecords.Id (Set Null)
- Indexes: SessionId, ToolType, ApprovalStatus, RiskLevel, RequestedAt, WasSuccessful

#### AuditLog Entity

**Purpose:** Stores action history for compliance and debugging.

**Properties:**
```csharp
public class AuditLog
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public Guid? SessionId { get; set; }
    public string Action { get; set; } // Required, max 500
    public string EntityType { get; set; } // Required, max 200
    public Guid? EntityId { get; set; }
    public string? OldValues { get; set; } // JSON, max 20000
    public string? NewValues { get; set; } // JSON, max 20000
    public string? IpAddress { get; set; } // Max 50
    public string? UserAgent { get; set; } // Max 500
    public DateTime Timestamp { get; set; }
    public string? AdditionalData { get; set; } // JSON, max 10000
}
```

**Database Mapping:**
- Table: AuditLogs
- Primary Key: Id (Guid)
- Indexes: UserId, SessionId, EntityType, EntityId, Timestamp, Action
- Note: No foreign keys (immutable audit trail)

#### MemoryItem Entity

**Purpose:** Stores durable user or project memory.

**Properties:**
```csharp
public class MemoryItem : IAuditableEntity
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public string? RepositoryPath { get; set; } // Max 1000
    public string Key { get; set; } // Required, max 500 (e.g., "coding_conventions", "architecture_notes")
    public string Value { get; set; } // Required, max 20000
    public string Category { get; set; } // Required, max 200 (e.g., "Convention", "Preference", "Note")
    public int Priority { get; set; } // Default: 0 (higher = more important)
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    
    // Navigation Properties
    public User? User { get; set; }
}
```

**Database Mapping:**
- Table: MemoryItems
- Primary Key: Id (Guid)
- Foreign Keys: UserId -> Users.Id (Set Null)
- Indexes: UserId, RepositoryPath, Category, Priority, CreatedAt
- Unique Constraint: (UserId, RepositoryPath, Key)

#### CodeFile Entity

**Purpose:** Tracks indexed code files.

**Properties:**
```csharp
public class CodeFile : IAuditableEntity
{
    public Guid Id { get; set; }
    public Guid RepositoryId { get; set; }
    public string FilePath { get; set; } // Required, max 1000
    public string FileName { get; set; } // Required, max 255
    public string FileExtension { get; set; } // Max 50
    public long FileSize { get; set; } // In bytes
    public string FileHash { get; set; } // SHA256, max 64
    public int LineCount { get; set; }
    public DateTime LastModified { get; set; }
    public DateTime LastIndexed { get; set; }
    public bool IsIndexed { get; set; } // Default: false
    public int ChunkCount { get; set; } // Default: 0
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    
    // Navigation Properties
    public Repository Repository { get; set; }
    public ICollection<RetrievalChunk> Chunks { get; set; }
}
```

**Database Mapping:**
- Table: CodeFiles
- Primary Key: Id (Guid)
- Foreign Keys: RepositoryId -> Repositories.Id (Cascade)
- Indexes: RepositoryId, FilePath, FileHash, LastIndexed, IsIndexed
- Unique Constraint: (RepositoryId, FilePath)

#### Repository Entity

**Purpose:** Represents a code repository being analyzed.

**Properties:**
```csharp
public class Repository : IAuditableEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } // Required, max 500
    public string Path { get; set; } // Required, max 1000
    public string? GitUrl { get; set; } // Max 1000
    public string? DefaultBranch { get; set; } // Max 200, default: "main"
    public DateTime? LastIndexed { get; set; }
    public bool IsActive { get; set; } // Default: true
    public int TotalFiles { get; set; } // Default: 0
    public int IndexedFiles { get; set; } // Default: 0
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    
    // Navigation Properties
    public ICollection<CodeFile> CodeFiles { get; set; }
}
```

**Database Mapping:**
- Table: Repositories
- Primary Key: Id (Guid)
- Indexes: Path (Unique), Name, IsActive, LastIndexed
- Constraints: Path unique

### 14.2 Domain Enumerations

#### UserRole Enum
```csharp
public enum UserRole
{
    Developer = 1,
    TechLead = 2,
    Admin = 3,
    DevOps = 4
}
```

#### SessionStatus Enum
```csharp
public enum SessionStatus
{
    Active = 1,
    Paused = 2,
    Completed = 3,
    Archived = 4
}
```

#### IntentType Enum
```csharp
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

#### ToolType Enum
```csharp
public enum ToolType
{
    File = 1,
    Git = 2,
    Build = 3,
    Test = 4,
    Database = 5
}
```

#### ApprovalStatus Enum
```csharp
public enum ApprovalStatus
{
    Pending = 1,
    Approved = 2,
    Rejected = 3,
    AutoApproved = 4
}
```

#### RiskLevel Enum
```csharp
public enum RiskLevel
{
    Low = 1,
    Medium = 2,
    High = 3,
    Critical = 4
}
```

### 14.3 Data Retention

* Session and log retention shall be configurable
* Sensitive data shall be handled according to access policy
* Deleted memory and audit records shall follow retention rules

### 14.3 Data Integrity

* The system shall preserve referential integrity where relational storage is used
* Tool execution records shall be immutable after finalization unless corrected through an audit process

---

## 15. Security Requirements

### 15.1 Authentication

* The system shall authenticate users before granting access
* The system shall support secure identity integration

### 15.2 Authorization

* The system shall enforce role-based and permission-based access control
* Administrative operations shall be restricted

### 15.3 Prompt Injection Protection

* The system shall detect and limit untrusted instructions embedded in retrieved content where possible
* The system shall treat external content as data, not instructions

### 15.4 Tool Guardrails

* The system shall classify tool risk levels
* The system shall require explicit confirmation for destructive or environment-changing actions
* The system shall provide the user with a summary before execution

### 15.5 Secret Management

* Secrets shall not be stored in plain text
* API keys shall be kept in secure configuration stores or secret managers

### 15.6 Audit and Traceability

* The system shall log who requested what, what data was retrieved, what tools were used, and what changes were made

---

## 16. Logging, Monitoring, and Observability

### 16.1 Logging

The system shall log:

* Prompts
* Retrieval decisions
* Tool invocations
* Errors
* Approvals
* File change summaries

### 16.2 Metrics

The system shall expose metrics such as:

* Request count
* Response latency
* Retrieval latency
* Tool execution time
* Error rate
* Model call count
* Token usage

### 16.3 Tracing

The system shall support request correlation across retrieval, model invocation, and tool execution.

---

## 17. Deployment Requirements

### 17.1 Deployment Model

The application shall support containerized deployment.

### 17.2 Environments

* Development
* Testing
* Staging
* Production

### 17.3 Delivery and Release

* CI/CD pipeline support
* Environment-based configuration
* Versioned deployments
* Rollback capability

### 17.4 Runtime Considerations

* .NET 10 runtime support
* Optional Native AOT support where appropriate
* Output caching and rate limiting where useful

---

## 18. Testing Requirements

### 18.1 Unit Testing

* Domain rules
* Application services
* Tool policy checks
* Retrieval helpers

### 18.2 Integration Testing

* Model provider integration
* Vector DB integration
* Database integration
* File and Git tool integration

### 18.3 End-to-End Testing

* Prompt to response workflow
* Prompt to retrieval workflow
* Prompt to approved file change workflow

### 18.4 Security Testing

* Authorization checks
* Prompt injection resistance tests
* Unsafe action prevention tests

### 18.5 Performance Testing

* Retrieval latency
* Tool execution responsiveness
* Concurrency handling

---

## 19. Risks and Mitigations

### 19.1 Hallucination Risk

**Risk:** The model may generate incorrect outputs.
**Mitigation:** Use RAG, structured prompting, and explicit uncertainty handling.

### 19.2 Unsafe Tool Execution

**Risk:** The system may execute harmful actions.
**Mitigation:** Approval gates, policy engine, restricted tool permissions.

### 19.3 Context Overload

**Risk:** Too much context may reduce response quality.
**Mitigation:** Chunking, ranking, summarization, and token budgeting.

### 19.4 Cost Growth

**Risk:** Model usage and indexing may become expensive.
**Mitigation:** Caching, selective retrieval, and usage controls.

### 19.5 Prompt Injection

**Risk:** Malicious content may alter agent behavior.
**Mitigation:** Treat external text as data, apply content isolation, and validate tool actions.

---

## 20. Release Plan

### 20.1 Phase 1: MVP

* Single agent
* Chat interface
* RAG retrieval
* Code explanation
* Safe tool usage
* Session and audit logging

### 20.2 Phase 2: Developer Productivity Expansion

* Code generation
* Bug fixing workflow
* Refactoring workflow
* Test generation
* Diff viewer

### 20.3 Phase 3: Advanced Automation

* Rich planning chains
* Multiple tool coordination
* IDE integration
* Project memory improvements
* Team collaboration features

### 20.4 Phase 4: Multi-Agent Evolution

* Planner agent
* Coder agent
* Reviewer agent
* Tester agent
* Policy and orchestration improvements

---

## 21. Acceptance Criteria

The system shall be considered acceptable when:

* It can answer repository-specific questions accurately using retrieval
* It can generate code aligned with project conventions
* It can execute approved safe tools and log those executions
* It blocks or warns on unsafe actions
* It supports auditability and session continuity
* It operates successfully on the target .NET 10 architecture

---

## 22. Future Enhancements

* Multi-agent orchestration
* IDE-native extension support
* Project-wide automated refactoring
* Advanced test planning
* Repository trend analysis
* Fine-grained memory personalization
* Policy-based enterprise governance

---

## 23. Glossary

* **Agentic AI**: AI that can plan and act using tools
* **RAG**: Retrieval-Augmented Generation
* **Embedding**: Vector representation of text
* **Vector Search**: Semantic similarity search over embeddings
* **Tool Call**: Programmatic action executed by the agent
* **Guardrail**: Rule that prevents unsafe behavior
* **Approval Gate**: User confirmation before an action is performed

---

## 24. Project Structure and Organization

### 24.1 Solution Structure

```
AgentAI/
├── AgentAI.sln
├── src/
│   ├── AgentAI.Domain/
│   │   ├── Entities/
│   │   │   ├── User.cs
│   │   │   ├── Session.cs
│   │   │   ├── PromptRecord.cs
│   │   │   ├── RetrievalChunk.cs
│   │   │   ├── ToolExecution.cs
│   │   │   ├── AuditLog.cs
│   │   │   ├── MemoryItem.cs
│   │   │   ├── CodeFile.cs
│   │   │   └── Repository.cs
│   │   ├── Enums/
│   │   │   ├── UserRole.cs
│   │   │   ├── SessionStatus.cs
│   │   │   ├── IntentType.cs
│   │   │   ├── ToolType.cs
│   │   │   ├── ApprovalStatus.cs
│   │   │   └── RiskLevel.cs
│   │   ├── ValueObjects/
│   │   │   ├── FilePath.cs
│   │   │   ├── CodeChunk.cs
│   │   │   └── EmbeddingVector.cs
│   │   ├── Interfaces/
│   │   │   ├── IEntity.cs
│   │   │   └── IAuditableEntity.cs
│   │   ├── Exceptions/
│   │   │   ├── DomainException.cs
│   │   │   └── ValidationException.cs
│   │   └── AgentAI.Domain.csproj
│   ├── AgentAI.Application/
│   │   ├── DTOs/
│   │   │   ├── Chat/
│   │   │   │   ├── ChatRequest.cs
│   │   │   │   ├── ChatResponse.cs
│   │   │   │   └── MessageDto.cs
│   │   │   ├── Session/
│   │   │   │   ├── CreateSessionRequest.cs
│   │   │   │   ├── SessionDto.cs
│   │   │   │   ├── SessionDetailDto.cs
│   │   │   │   └── UpdateSessionRequest.cs
│   │   │   ├── Retrieval/
│   │   │   │   ├── RetrievalRequest.cs
│   │   │   │   ├── RetrievalResultDto.cs
│   │   │   │   └── RetrievalChunkDto.cs
│   │   │   ├── Tool/
│   │   │   │   ├── ToolExecutionRequest.cs
│   │   │   │   ├── ToolExecutionResultDto.cs
│   │   │   │   ├── ToolExecutionDto.cs
│   │   │   │   └── RejectToolRequest.cs
│   │   │   ├── Indexing/
│   │   │   │   ├── IndexingRequest.cs
│   │   │   │   └── IndexingStatusDto.cs
│   │   │   └── Admin/
│   │   │       ├── AuditLogDto.cs
│   │   │       ├── SystemMetricsDto.cs
│   │   │       ├── CreateUserRequest.cs
│   │   │       ├── UserDto.cs
│   │   │       └── RepositoryDto.cs
│   │   ├── Interfaces/
│   │   │   ├── Services/
│   │   │   │   ├── IAgentOrchestrationService.cs
│   │   │   │   ├── IRetrievalService.cs
│   │   │   │   ├── IToolExecutionService.cs
│   │   │   │   ├── ISessionService.cs
│   │   │   │   ├── IIndexingService.cs
│   │   │   │   └── IEmbeddingService.cs
│   │   │   ├── Repositories/
│   │   │   │   ├── ISessionRepository.cs
│   │   │   │   ├── IPromptRecordRepository.cs
│   │   │   │   ├── IRetrievalChunkRepository.cs
│   │   │   │   ├── IToolExecutionRepository.cs
│   │   │   │   ├── IAuditLogRepository.cs
│   │   │   │   ├── IMemoryItemRepository.cs
│   │   │   │   ├── IRepositoryRepository.cs
│   │   │   │   └── ICodeFileRepository.cs
│   │   │   └── Infrastructure/
│   │   │       ├── ILLMProvider.cs
│   │   │       ├── IVectorStore.cs
│   │   │       ├── IFileSystemService.cs
│   │   │       └── IGitService.cs
│   │   ├── Validators/
│   │   │   ├── ChatRequestValidator.cs
│   │   │   ├── ToolExecutionRequestValidator.cs
│   │   │   ├── CreateSessionRequestValidator.cs
│   │   │   ├── RetrievalRequestValidator.cs
│   │   │   └── IndexingRequestValidator.cs
│   │   ├── Mappings/
│   │   │   └── MappingProfile.cs
│   │   ├── Common/
│   │   │   ├── Result.cs
│   │   │   ├── ApiResponse.cs
│   │   │   └── PaginatedList.cs
│   │   └── AgentAI.Application.csproj
│   ├── AgentAI.Infrastructure/
│   │   ├── Persistence/
│   │   │   ├── ApplicationDbContext.cs
│   │   │   ├── Configurations/
│   │   │   │   ├── UserConfiguration.cs
│   │   │   │   ├── SessionConfiguration.cs
│   │   │   │   ├── PromptRecordConfiguration.cs
│   │   │   │   ├── RetrievalChunkConfiguration.cs
│   │   │   │   ├── ToolExecutionConfiguration.cs
│   │   │   │   ├── AuditLogConfiguration.cs
│   │   │   │   ├── MemoryItemConfiguration.cs
│   │   │   │   ├── CodeFileConfiguration.cs
│   │   │   │   └── RepositoryConfiguration.cs
│   │   │   ├── Repositories/
│   │   │   │   ├── SessionRepository.cs
│   │   │   │   ├── PromptRecordRepository.cs
│   │   │   │   ├── RetrievalChunkRepository.cs
│   │   │   │   ├── ToolExecutionRepository.cs
│   │   │   │   ├── AuditLogRepository.cs
│   │   │   │   ├── MemoryItemRepository.cs
│   │   │   │   ├── RepositoryRepository.cs
│   │   │   │   └── CodeFileRepository.cs
│   │   │   └── Migrations/
│   │   ├── AI/
│   │   │   ├── SemanticKernel/
│   │   │   │   ├── SemanticKernelService.cs
│   │   │   │   ├── KernelBuilder.cs
│   │   │   │   └── PromptTemplates.cs
│   │   │   ├── LLMProviders/
│   │   │   │   ├── OpenAIProvider.cs
│   │   │   │   └── AzureOpenAIProvider.cs
│   │   │   └── Embeddings/
│   │   │       └── EmbeddingService.cs
│   │   ├── VectorStore/
│   │   │   ├── PgVector/
│   │   │   │   └── PgVectorStore.cs
│   │   │   └── Qdrant/
│   │   │       └── QdrantVectorStore.cs
│   │   ├── Services/
│   │   │   ├── AgentOrchestrationService.cs
│   │   │   ├── RetrievalService.cs
│   │   │   ├── ToolExecutionService.cs
│   │   │   ├── SessionService.cs
│   │   │   └── IndexingService.cs
│   │   ├── Tools/
│   │   │   ├── FileTool.cs
│   │   │   ├── GitTool.cs
│   │   │   ├── BuildTool.cs
│   │   │   ├── TestTool.cs
│   │   │   └── DatabaseTool.cs
│   │   ├── FileSystem/
│   │   │   ├── FileSystemService.cs
│   │   │   ├── CodeParser.cs
│   │   │   └── ChunkingService.cs
│   │   ├── Git/
│   │   │   └── GitService.cs
│   │   ├── Caching/
│   │   │   └── CacheService.cs
│   │   ├── Logging/
│   │   │   └── SerilogConfiguration.cs
│   │   └── AgentAI.Infrastructure.csproj
│   └── AgentAI.API/
│       ├── Controllers/
│       │   ├── ChatController.cs
│       │   ├── SessionController.cs
│       │   ├── RetrievalController.cs
│       │   ├── ToolController.cs
│       │   ├── IndexingController.cs
│       │   └── AdminController.cs
│       ├── Middleware/
│       │   ├── ExceptionHandlingMiddleware.cs
│       │   ├── RequestLoggingMiddleware.cs
│       │   └── ApprovalMiddleware.cs
│       ├── Filters/
│       │   ├── ValidationFilter.cs
│       │   └── AuthorizationFilter.cs
│       ├── Extensions/
│       │   ├── ServiceCollectionExtensions.cs
│       │   └── ApplicationBuilderExtensions.cs
│       ├── Program.cs
│       ├── appsettings.json
│       ├── appsettings.Development.json
│       ├── appsettings.Production.json
│       └── AgentAI.API.csproj
├── tests/
│   ├── AgentAI.Domain.Tests/
│   ├── AgentAI.Application.Tests/
│   ├── AgentAI.Infrastructure.Tests/
│   └── AgentAI.API.Tests/
├── docs/
│   ├── architecture.md
│   ├── api-documentation.md
│   └── deployment-guide.md
├── .github/
│   ├── instructions/
│   │   ├── srs.instructions.md
│   │   └── implementation-workflow.md
│   └── prompts/
│       └── promt.prompt.md
├── docker-compose.yml
├── Dockerfile
├── .gitignore
└── README.md
```

### 24.2 Naming Conventions

**Classes and Interfaces:**
- PascalCase for all class names
- Interfaces prefixed with `I`
- DTOs suffixed with `Dto` or `Request`/`Response`
- Services suffixed with `Service`
- Repositories suffixed with `Repository`
- Controllers suffixed with `Controller`
- Validators suffixed with `Validator`

**Properties and Methods:**
- PascalCase for public properties and methods
- camelCase for private fields (prefix with `_`)
- Async methods suffixed with `Async`
- Boolean properties prefixed with `Is`, `Has`, `Can`, `Should`

**Files and Folders:**
- One class per file
- File name matches class name exactly
- Folder names in PascalCase
- Organize by feature/domain concept

**Namespaces:**
- Follow folder structure exactly
- Format: `AgentAI.{Layer}.{Feature}.{SubFeature}`
- Examples:
  - `AgentAI.Domain.Entities`
  - `AgentAI.Application.DTOs.Chat`
  - `AgentAI.Infrastructure.Persistence.Repositories`

### 24.3 Configuration Management

**appsettings.json structure:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=agentai;Username=postgres;Password=***"
  },
  "AI": {
    "Provider": "OpenAI",
    "ApiKey": "***",
    "Model": "gpt-4",
    "EmbeddingModel": "text-embedding-ada-002",
    "MaxTokens": 4000,
    "Temperature": 0.7
  },
  "VectorStore": {
    "Provider": "PgVector",
    "Dimensions": 1536,
    "SimilarityFunction": "Cosine"
  },
  "Retrieval": {
    "MaxChunks": 10,
    "MinSimilarity": 0.7,
    "ChunkSize": 1000,
    "ChunkOverlap": 200
  },
  "Tools": {
    "AutoApproveRiskLevel": "Low",
    "ExecutionTimeout": 30000
  },
  "Jwt": {
    "Secret": "***",
    "Issuer": "AgentAI",
    "Audience": "AgentAI.API",
    "ExpirationMinutes": 60
  },
  "RateLimit": {
    "RequestsPerMinute": 100
  },
  "Serilog": {
    "MinimumLevel": "Information",
    "WriteTo": [
      { "Name": "Console" },
      { "Name": "File", "Args": { "path": "logs/log-.txt", "rollingInterval": "Day" } }
    ]
  }
}
```

### 24.4 Dependency Injection Registration Pattern

**ServiceCollectionExtensions.cs:**
```csharp
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register validators
        services.AddValidatorsFromAssemblyContaining<ChatRequestValidator>();
        
        // Register Mapster
        services.AddMapster();
        
        return services;
    }
    
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        // Database
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.UseVector()));
        
        // Repositories
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<IPromptRecordRepository, PromptRecordRepository>();
        // ... other repositories
        
        // Services
        services.AddScoped<IAgentOrchestrationService, AgentOrchestrationService>();
        services.AddScoped<IRetrievalService, RetrievalService>();
        // ... other services
        
        // AI Services
        services.AddSingleton<ILLMProvider, OpenAIProvider>();
        services.AddSingleton<IEmbeddingService, EmbeddingService>();
        services.AddSingleton<IVectorStore, PgVectorStore>();
        
        // Caching
        services.AddMemoryCache();
        services.AddSingleton<ICacheService, CacheService>();
        
        return services;
    }
}
```

### 24.5 Error Handling Pattern

**Consistent error responses:**
```csharp
public class ErrorResponse
{
    public string Type { get; set; }
    public string Title { get; set; }
    public int Status { get; set; }
    public string Detail { get; set; }
    public string Instance { get; set; }
    public Dictionary<string, string[]>? Errors { get; set; }
}
```

**Exception handling middleware:**
- Catches all unhandled exceptions
- Logs with correlation ID
- Returns consistent error format
- Hides sensitive details in production

### 24.6 Logging Standards

**Structured logging with Serilog:**
```csharp
_logger.LogInformation(
    "Processing chat request for session {SessionId} with intent {Intent}",
    sessionId,
    detectedIntent);

_logger.LogWarning(
    "Tool execution {ExecutionId} requires approval due to {RiskLevel} risk",
    executionId,
    riskLevel);

_logger.LogError(
    exception,
    "Failed to retrieve chunks for query {Query} in repository {Repository}",
    query,
    repositoryPath);
```

**Log levels:**
- **Trace:** Very detailed, development only
- **Debug:** Diagnostic information, development/staging
- **Information:** General flow, production
- **Warning:** Unexpected but handled, production
- **Error:** Failures, production
- **Critical:** System failures, production

### 24.7 Testing Strategy

**Unit Tests:**
- Test domain logic in isolation
- Mock all dependencies
- Use xUnit, Moq, FluentAssertions
- Naming: `MethodName_Scenario_ExpectedResult`

**Integration Tests:**
- Test with real database (test container)
- Test API endpoints end-to-end
- Use WebApplicationFactory

**Test Organization:**
```
AgentAI.Domain.Tests/
├── Entities/
│   ├── UserTests.cs
│   └── SessionTests.cs
└── ValueObjects/
    └── FilePathTests.cs

AgentAI.Application.Tests/
├── Services/
│   ├── SessionServiceTests.cs
│   └── RetrievalServiceTests.cs
└── Validators/
    └── ChatRequestValidatorTests.cs

AgentAI.Infrastructure.Tests/
├── Repositories/
│   └── SessionRepositoryTests.cs
└── AI/
    └── EmbeddingServiceTests.cs

AgentAI.API.Tests/
├── Controllers/
│   ├── ChatControllerTests.cs
│   └── SessionControllerTests.cs
└── Integration/
    └── ChatWorkflowTests.cs
```

---

## 25. Implementation Workflow

### 25.1 Development Phases

**Phase 1: Foundation (Week 1)**
- Set up solution structure
- Implement Domain layer (entities, enums)
- Implement Application layer (DTOs, interfaces)
- Create database schema
- Initial migration

**Phase 2: Core Infrastructure (Week 2)**
- Implement repositories
- Set up DbContext and configurations
- Implement basic services
- Set up authentication/authorization

**Phase 3: AI Integration (Week 3)**
- Integrate Semantic Kernel
- Implement LLM provider
- Implement embedding service
- Set up vector store
- Implement retrieval service

**Phase 4: Agent Orchestration (Week 4)**
- Implement agent orchestration service
- Implement intent detection
- Implement context assembly
- Implement response generation

**Phase 5: Tool System (Week 5)**
- Implement tool registry
- Implement file tool
- Implement Git tool
- Implement build/test tools
- Implement approval workflow

**Phase 6: API Layer (Week 6)**
- Implement controllers
- Set up middleware
- Configure Swagger
- Implement validation
- Set up logging

**Phase 7: Testing & Refinement (Week 7-8)**
- Write unit tests
- Write integration tests
- Performance testing
- Security testing
- Bug fixes and optimization

### 25.2 Critical Implementation Rules

**ALWAYS:**
✅ Follow dependency-first approach
✅ Build bottom-up (Domain → Application → Infrastructure → API)
✅ Run `dotnet build` after each file creation
✅ Fix compilation errors immediately
✅ Verify dependencies exist before creating files
✅ Use async/await for all I/O operations
✅ Apply proper error handling
✅ Add XML documentation comments
✅ Follow naming conventions strictly
✅ Keep methods focused and small

**NEVER:**
❌ Create files with missing dependencies
❌ Accumulate build errors
❌ Skip verification steps
❌ Use hardcoded values
❌ Ignore null safety
❌ Skip validation
❌ Bypass approval for high-risk operations
❌ Log sensitive information
❌ Create circular dependencies
❌ Mix concerns across layers

### 25.3 Code Quality Standards

**SOLID Principles:**
- Single Responsibility: One class, one purpose
- Open/Closed: Open for extension, closed for modification
- Liskov Substitution: Derived classes must be substitutable
- Interface Segregation: Many specific interfaces over one general
- Dependency Inversion: Depend on abstractions, not concretions

**Clean Code:**
- Meaningful names
- Small functions (< 20 lines)
- Clear intent
- No magic numbers
- Proper abstraction levels
- DRY (Don't Repeat Yourself)

**Performance:**
- Use async/await properly
- Avoid N+1 queries
- Use pagination for large datasets
- Implement caching where appropriate
- Use connection pooling
- Optimize database queries

---

## 26. Conclusion

This enhanced SRS defines a complete, implementation-ready foundation for a **Copilot-like Agentic AI Developer Assistant** built on **.NET 10**, **Semantic Kernel**, and **RAG**. 

The specification includes:
- ✅ Detailed technical architecture with specific NuGet packages
- ✅ Complete entity definitions with properties and relationships
- ✅ Comprehensive API endpoint specifications with request/response contracts
- ✅ Domain enumerations with specific values
- ✅ Database mapping and indexing strategies
- ✅ Project structure and file organization
- ✅ Naming conventions and coding standards
- ✅ Configuration management patterns
- ✅ Dependency injection setup
- ✅ Error handling and logging standards
- ✅ Testing strategy and organization
- ✅ Phase-by-phase implementation workflow
- ✅ Critical implementation rules and quality standards

The design emphasizes:
- **Practical developer support** with context-aware reasoning
- **Safe tool usage** with approval workflows and risk assessment
- **Enterprise-grade traceability** with comprehensive audit logging
- **Clean Architecture** with clear separation of concerns
- **Dependency-first development** to ensure zero compilation errors
- **Production-ready code** following SOLID principles and best practices

This SRS is now ready for immediate implementation following the bottom-up, layer-by-layer approach defined in the accompanying implementation workflow guide.
