using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgentAI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    SessionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Action = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    EntityType = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    EntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    OldValues = table.Column<string>(type: "character varying(20000)", maxLength: 20000, nullable: true),
                    NewValues = table.Column<string>(type: "character varying(20000)", maxLength: 20000, nullable: true),
                    IpAddress = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    UserAgent = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AdditionalData = table.Column<string>(type: "character varying(10000)", maxLength: 10000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Repositories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Path = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    GitUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    DefaultBranch = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, defaultValue: "main"),
                    LastIndexed = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    TotalFiles = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    IndexedFiles = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repositories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    FullName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CodeFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RepositoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    FilePath = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    FileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FileExtension = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    FileHash = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    LineCount = table.Column<int>(type: "integer", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastIndexed = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsIndexed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    ChunkCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodeFiles_Repositories_RepositoryId",
                        column: x => x.RepositoryId,
                        principalTable: "Repositories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemoryItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    RepositoryPath = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Key = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Value = table.Column<string>(type: "character varying(20000)", maxLength: 20000, nullable: false),
                    Category = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemoryItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemoryItems_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    RepositoryPath = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Branch = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastActivityAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MessageCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sessions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PromptRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserPrompt = table.Column<string>(type: "character varying(10000)", maxLength: 10000, nullable: false),
                    DetectedIntent = table.Column<int>(type: "integer", nullable: false),
                    AssistantResponse = table.Column<string>(type: "character varying(50000)", maxLength: 50000, nullable: true),
                    TokensUsed = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    ContextChunksRetrieved = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    ResponseTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    WasSuccessful = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    ErrorMessage = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromptRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromptRecords_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RetrievalChunks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CodeFileId = table.Column<Guid>(type: "uuid", nullable: true),
                    FilePath = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    FileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FileExtension = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ChunkContent = table.Column<string>(type: "character varying(20000)", maxLength: 20000, nullable: false),
                    StartLine = table.Column<int>(type: "integer", nullable: false),
                    EndLine = table.Column<int>(type: "integer", nullable: false),
                    ChunkType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ClassName = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    MethodName = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Embedding = table.Column<float[]>(type: "real[]", nullable: false),
                    RepositoryPath = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Branch = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    IndexedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FileLastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    PromptRecordId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RetrievalChunks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RetrievalChunks_CodeFiles_CodeFileId",
                        column: x => x.CodeFileId,
                        principalTable: "CodeFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_RetrievalChunks_PromptRecords_PromptRecordId",
                        column: x => x.PromptRecordId,
                        principalTable: "PromptRecords",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ToolExecutions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    PromptRecordId = table.Column<Guid>(type: "uuid", nullable: true),
                    ToolType = table.Column<int>(type: "integer", nullable: false),
                    ToolName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Action = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    InputParameters = table.Column<string>(type: "character varying(10000)", maxLength: 10000, nullable: false),
                    Output = table.Column<string>(type: "character varying(50000)", maxLength: 50000, nullable: true),
                    ApprovalStatus = table.Column<int>(type: "integer", nullable: false),
                    RiskLevel = table.Column<int>(type: "integer", nullable: false),
                    RequestedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ApprovedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExecutedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExecutionDuration = table.Column<TimeSpan>(type: "interval", nullable: true),
                    WasSuccessful = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    ErrorMessage = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ApprovedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToolExecutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToolExecutions_PromptRecords_PromptRecordId",
                        column: x => x.PromptRecordId,
                        principalTable: "PromptRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ToolExecutions_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_Action",
                table: "AuditLogs",
                column: "Action");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_EntityId",
                table: "AuditLogs",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_EntityType",
                table: "AuditLogs",
                column: "EntityType");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_SessionId",
                table: "AuditLogs",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_Timestamp",
                table: "AuditLogs",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_UserId",
                table: "AuditLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeFiles_FileHash",
                table: "CodeFiles",
                column: "FileHash");

            migrationBuilder.CreateIndex(
                name: "IX_CodeFiles_FilePath",
                table: "CodeFiles",
                column: "FilePath");

            migrationBuilder.CreateIndex(
                name: "IX_CodeFiles_IsIndexed",
                table: "CodeFiles",
                column: "IsIndexed");

            migrationBuilder.CreateIndex(
                name: "IX_CodeFiles_LastIndexed",
                table: "CodeFiles",
                column: "LastIndexed");

            migrationBuilder.CreateIndex(
                name: "IX_CodeFiles_RepositoryId",
                table: "CodeFiles",
                column: "RepositoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeFiles_RepositoryId_FilePath",
                table: "CodeFiles",
                columns: new[] { "RepositoryId", "FilePath" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MemoryItems_Category",
                table: "MemoryItems",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_MemoryItems_CreatedAt",
                table: "MemoryItems",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_MemoryItems_Priority",
                table: "MemoryItems",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_MemoryItems_RepositoryPath",
                table: "MemoryItems",
                column: "RepositoryPath");

            migrationBuilder.CreateIndex(
                name: "IX_MemoryItems_UserId",
                table: "MemoryItems",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MemoryItems_UserId_RepositoryPath_Key",
                table: "MemoryItems",
                columns: new[] { "UserId", "RepositoryPath", "Key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PromptRecords_CreatedAt",
                table: "PromptRecords",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_PromptRecords_DetectedIntent",
                table: "PromptRecords",
                column: "DetectedIntent");

            migrationBuilder.CreateIndex(
                name: "IX_PromptRecords_SessionId",
                table: "PromptRecords",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_PromptRecords_WasSuccessful",
                table: "PromptRecords",
                column: "WasSuccessful");

            migrationBuilder.CreateIndex(
                name: "IX_Repositories_IsActive",
                table: "Repositories",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Repositories_LastIndexed",
                table: "Repositories",
                column: "LastIndexed");

            migrationBuilder.CreateIndex(
                name: "IX_Repositories_Name",
                table: "Repositories",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Repositories_Path",
                table: "Repositories",
                column: "Path",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RetrievalChunks_ChunkType",
                table: "RetrievalChunks",
                column: "ChunkType");

            migrationBuilder.CreateIndex(
                name: "IX_RetrievalChunks_CodeFileId",
                table: "RetrievalChunks",
                column: "CodeFileId");

            migrationBuilder.CreateIndex(
                name: "IX_RetrievalChunks_FileName",
                table: "RetrievalChunks",
                column: "FileName");

            migrationBuilder.CreateIndex(
                name: "IX_RetrievalChunks_FilePath",
                table: "RetrievalChunks",
                column: "FilePath");

            migrationBuilder.CreateIndex(
                name: "IX_RetrievalChunks_IndexedAt",
                table: "RetrievalChunks",
                column: "IndexedAt");

            migrationBuilder.CreateIndex(
                name: "IX_RetrievalChunks_PromptRecordId",
                table: "RetrievalChunks",
                column: "PromptRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_CreatedAt",
                table: "Sessions",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_LastActivityAt",
                table: "Sessions",
                column: "LastActivityAt");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_Status",
                table: "Sessions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_UserId",
                table: "Sessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ToolExecutions_ApprovalStatus",
                table: "ToolExecutions",
                column: "ApprovalStatus");

            migrationBuilder.CreateIndex(
                name: "IX_ToolExecutions_PromptRecordId",
                table: "ToolExecutions",
                column: "PromptRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_ToolExecutions_RequestedAt",
                table: "ToolExecutions",
                column: "RequestedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ToolExecutions_RiskLevel",
                table: "ToolExecutions",
                column: "RiskLevel");

            migrationBuilder.CreateIndex(
                name: "IX_ToolExecutions_SessionId",
                table: "ToolExecutions",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ToolExecutions_ToolType",
                table: "ToolExecutions",
                column: "ToolType");

            migrationBuilder.CreateIndex(
                name: "IX_ToolExecutions_WasSuccessful",
                table: "ToolExecutions",
                column: "WasSuccessful");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedAt",
                table: "Users",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "MemoryItems");

            migrationBuilder.DropTable(
                name: "RetrievalChunks");

            migrationBuilder.DropTable(
                name: "ToolExecutions");

            migrationBuilder.DropTable(
                name: "CodeFiles");

            migrationBuilder.DropTable(
                name: "PromptRecords");

            migrationBuilder.DropTable(
                name: "Repositories");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
