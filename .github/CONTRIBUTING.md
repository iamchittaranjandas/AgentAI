# Contributing to AgentAI

Thank you for your interest in contributing to AgentAI! This document provides guidelines and instructions for contributing.

## 🎯 Code of Conduct

- Be respectful and inclusive
- Follow enterprise coding standards
- Write clean, maintainable code
- Document your changes

## 🏗️ Architecture Guidelines

### Clean Architecture Principles

1. **Domain Layer** (No dependencies)
   - Pure business logic
   - Domain entities and events
   - Domain interfaces
   - No external dependencies

2. **Application Layer** (Depends on Domain)
   - CQRS Commands and Queries
   - MediatR handlers
   - FluentValidation validators
   - Application interfaces
   - DTOs

3. **Infrastructure Layer** (Implements Application)
   - Repository implementations
   - Service implementations
   - Database context
   - External integrations

4. **API Layer** (Depends on Application)
   - Controllers (thin, delegate to MediatR)
   - Middleware
   - Configuration

### SOLID Principles

- **Single Responsibility**: Each class has one reason to change
- **Open/Closed**: Open for extension, closed for modification
- **Liskov Substitution**: Subtypes must be substitutable for base types
- **Interface Segregation**: Many specific interfaces over one general
- **Dependency Inversion**: Depend on abstractions, not concretions

## 📝 Coding Standards

### C# Conventions

```csharp
// Use PascalCase for classes, methods, properties
public class SessionService
{
    // Use camelCase for private fields with underscore prefix
    private readonly ISessionRepository _sessionRepository;
    
    // Use PascalCase for public properties
    public string Title { get; set; }
    
    // Use async/await for all I/O operations
    public async Task<Result<SessionDto>> CreateSessionAsync(CreateSessionCommand command)
    {
        // Implementation
    }
}
```

### CQRS Pattern

**Commands** (Write operations):
```csharp
// Command
public class CreateSessionCommand : IRequest<Result<SessionDto>>
{
    public Guid UserId { get; set; }
    public string Title { get; set; }
}

// Handler
public class CreateSessionCommandHandler : IRequestHandler<CreateSessionCommand, Result<SessionDto>>
{
    // Implementation
}

// Validator
public class CreateSessionCommandValidator : AbstractValidator<CreateSessionCommand>
{
    // Validation rules
}
```

**Queries** (Read operations):
```csharp
// Query
public class GetSessionByIdQuery : IRequest<Result<SessionDto>>
{
    public Guid SessionId { get; set; }
}

// Handler
public class GetSessionByIdQueryHandler : IRequestHandler<GetSessionByIdQuery, Result<SessionDto>>
{
    // Implementation
}
```

### Domain Events

```csharp
// Domain Event
public class SessionCreatedEvent : IDomainEvent
{
    public Guid SessionId { get; }
    public DateTime OccurredOn { get; }
}

// Event Handler
public class SessionCreatedEventHandler : INotificationHandler<SessionCreatedEvent>
{
    public Task Handle(SessionCreatedEvent notification, CancellationToken cancellationToken)
    {
        // Handle event
    }
}
```

## 🔄 Development Workflow

### 1. Fork and Clone

```bash
git clone https://github.com/your-username/AgentAI.git
cd AgentAI
```

### 2. Create Feature Branch

```bash
git checkout -b feature/your-feature-name
```

### 3. Make Changes

Follow the architecture guidelines and coding standards above.

### 4. Write Tests

```csharp
[Fact]
public async Task CreateSession_WithValidData_ReturnsSuccess()
{
    // Arrange
    var command = new CreateSessionCommand { /* ... */ };
    
    // Act
    var result = await _handler.Handle(command, CancellationToken.None);
    
    // Assert
    Assert.True(result.Success);
    Assert.NotNull(result.Data);
}
```

### 5. Run Tests

```bash
dotnet test
```

### 6. Build and Verify

```bash
dotnet build
dotnet run --project AgentAI.API
```

### 7. Commit Changes

```bash
git add .
git commit -m "feat: add session creation feature"
```

**Commit Message Format**:
- `feat:` - New feature
- `fix:` - Bug fix
- `docs:` - Documentation changes
- `refactor:` - Code refactoring
- `test:` - Adding tests
- `chore:` - Maintenance tasks

### 8. Push and Create PR

```bash
git push origin feature/your-feature-name
```

Create a Pull Request on GitHub with:
- Clear description of changes
- Reference to related issues
- Screenshots (if UI changes)
- Test results

## 🧪 Testing Guidelines

### Unit Tests
- Test individual components in isolation
- Mock dependencies
- Test edge cases and error conditions

### Integration Tests
- Test database interactions
- Test service integrations
- Use test database

### API Tests
- Test endpoints end-to-end
- Test authentication and authorization
- Test error responses

## 📚 Documentation

- Update README.md for major changes
- Add XML comments to public APIs
- Document complex algorithms
- Update API documentation

## 🐛 Bug Reports

Include:
- Clear description of the issue
- Steps to reproduce
- Expected vs actual behavior
- Environment details (OS, .NET version)
- Error messages and stack traces

## 💡 Feature Requests

Include:
- Clear description of the feature
- Use cases and benefits
- Proposed implementation (if any)
- Mockups or examples (if applicable)

## ✅ Pull Request Checklist

- [ ] Code follows architecture guidelines
- [ ] SOLID principles applied
- [ ] All tests pass
- [ ] New tests added for new features
- [ ] Documentation updated
- [ ] No sensitive information in code
- [ ] Commit messages follow convention
- [ ] Code builds without warnings
- [ ] No breaking changes (or documented)

## 🔐 Security

- Never commit API keys or credentials
- Use appsettings.Example.json for examples
- Follow .gitignore rules
- Report security issues privately

## 📞 Questions?

- Open an issue for questions
- Check existing issues first
- Be specific and provide context

## 🙏 Thank You!

Your contributions make AgentAI better for everyone!
