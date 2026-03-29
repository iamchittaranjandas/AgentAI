# Security Policy

## 🔐 Reporting Security Vulnerabilities

If you discover a security vulnerability in AgentAI, please report it responsibly:

1. **Do NOT** open a public issue
2. Email the maintainers directly (if available)
3. Provide detailed information about the vulnerability
4. Allow time for the issue to be addressed before public disclosure

## 🛡️ Security Best Practices

### API Keys and Credentials

**NEVER commit sensitive information:**
- ❌ API keys (OpenRouter, OpenAI, etc.)
- ❌ Database passwords
- ❌ Connection strings with credentials
- ❌ Private keys or certificates
- ❌ OAuth tokens
- ❌ Any secrets or credentials

**Use environment variables or secure configuration:**
```json
// ✅ Good: appsettings.Example.json (template)
{
  "OpenRouter": {
    "ApiKey": "YOUR_OPENROUTER_API_KEY_HERE"
  }
}

// ❌ Bad: appsettings.json (committed with real key)
{
  "OpenRouter": {
    "ApiKey": "sk-or-v1-abc123..."
  }
}
```

### Configuration Management

1. **Use appsettings.Example.json** as a template
2. **Copy to appsettings.json** and add real credentials
3. **Ensure appsettings.json is in .gitignore**
4. **Use environment variables** for production

### Database Security

- Use strong passwords
- Limit database user permissions
- Use SSL/TLS for database connections
- Never expose database directly to the internet
- Regular backups with encryption

### API Security

- Rate limiting enabled (200 req/min per user/IP)
- Input validation with FluentValidation
- Global exception handling (no sensitive data in errors)
- HTTPS enforced in production
- CORS configured appropriately

### Authentication & Authorization

- Human-in-the-loop for tool executions
- Risk-based approval workflow
- Audit logging for all actions
- Role-based access control (when implemented)

## 🔍 Security Features

### Built-in Security

- ✅ **Rate Limiting**: 200 requests per minute per user/IP
- ✅ **Input Validation**: FluentValidation on all requests
- ✅ **Global Exception Handling**: No sensitive data in error responses
- ✅ **Audit Logging**: Comprehensive audit trail
- ✅ **Human-in-the-Loop**: Tool execution approval workflow
- ✅ **Risk Assessment**: Automatic risk level determination

### Recommended Additional Security

- [ ] JWT Authentication (implement as needed)
- [ ] Role-Based Authorization (implement as needed)
- [ ] API Key Authentication (implement as needed)
- [ ] Request signing (implement as needed)
- [ ] IP whitelisting (configure as needed)

## 🚨 Common Security Issues

### 1. Exposed API Keys

**Problem**: API keys committed to Git
**Solution**: 
- Remove from Git history: `git filter-branch` or BFG Repo-Cleaner
- Revoke compromised keys immediately
- Generate new keys
- Update .gitignore

### 2. SQL Injection

**Protection**: 
- Using Entity Framework Core with parameterized queries
- Input validation with FluentValidation
- No raw SQL queries without parameters

### 3. Cross-Site Scripting (XSS)

**Protection**:
- API returns JSON (not HTML)
- Input validation
- Output encoding (if rendering HTML)

### 4. Cross-Site Request Forgery (CSRF)

**Protection**:
- CORS configuration
- API tokens (when implemented)
- SameSite cookies (when using cookies)

## 📋 Security Checklist

Before deploying to production:

- [ ] All API keys in environment variables or secure vault
- [ ] Database credentials secured
- [ ] HTTPS enforced
- [ ] Rate limiting configured
- [ ] CORS properly configured
- [ ] Error messages don't expose sensitive data
- [ ] Audit logging enabled
- [ ] Database backups configured
- [ ] Security headers configured
- [ ] Dependencies up to date
- [ ] Vulnerability scanning performed

## 🔄 Dependency Security

### Regular Updates

```bash
# Check for outdated packages
dotnet list package --outdated

# Update packages
dotnet add package <PackageName> --version <NewVersion>
```

### Known Vulnerabilities

- AutoMapper 12.0.1 has a known vulnerability (non-critical)
- Monitor NuGet advisories: https://github.com/advisories

## 🛠️ Security Tools

### Recommended Tools

- **OWASP Dependency-Check**: Scan for vulnerable dependencies
- **SonarQube**: Static code analysis
- **Snyk**: Vulnerability scanning
- **GitHub Dependabot**: Automated dependency updates

## 📞 Contact

For security concerns:
- Open a private security advisory on GitHub
- Email maintainers directly (if contact info available)
- Do not discuss security issues in public forums

## 🙏 Responsible Disclosure

We appreciate responsible disclosure of security vulnerabilities. We will:
- Acknowledge receipt within 48 hours
- Provide regular updates on progress
- Credit researchers (if desired)
- Coordinate public disclosure timing

Thank you for helping keep AgentAI secure!
