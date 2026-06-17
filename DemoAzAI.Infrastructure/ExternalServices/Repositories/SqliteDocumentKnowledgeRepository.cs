using Dapper;
using DemoAzAI.Domain.Entities;
using DemoAzAI.Infrastructure.ExternalServices.Repositories.Interfaces;
using Microsoft.Data.Sqlite;

namespace DemoAzAI.Infrastructure.ExternalServices.Repositories
{
    public class SqliteDocumentKnowledgeRepository : IDocumentKnowledgeRepository
    {
        private readonly string connectionString = string.Empty;

        private bool initialized = false;

        public SqliteDocumentKnowledgeRepository(string repositoryPath)
        {
            connectionString = $"Data Source={repositoryPath}/document_chunks.db";
        }

        private void EnsureInitialized()
        {
            if (initialized)
                return;

            using var connection =
                new SqliteConnection(connectionString);

            connection.Execute("""
                CREATE TABLE IF NOT EXISTS Document (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    FileName TEXT NOT NULL UNIQUE,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                );
                """);

            connection.Execute("""
                CREATE TABLE IF NOT EXISTS DocumentKnowledge (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    DocumentId INTEGER NOT NULL,
                    ChunkIndex INTEGER NOT NULL,
                    ChunkText TEXT NOT NULL,
                    Embedding TEXT NOT NULL,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY(DocumentId)
                    REFERENCES Documents(Id)
                );
                """);

            initialized = true;
        }

        public async Task<bool> SaveAsync(DocumentChunkEntity document)
        {
            using var connection = new SqliteConnection(connectionString);

            EnsureInitialized();

            var rows = await connection.ExecuteAsync(
                """
            INSERT INTO DocumentChunks
            (
                FileName,
                TextChunks,
                CreatedAt
            )
            VALUES
            (
                @FileName,
                @TextChunks,
                datetime('now')
            )
            """,
                document);


            return rows > 0;
        }

        public async Task<DocumentChunkEntity?> GetDocumentChunksAsync(string fileName)
        {
            EnsureInitialized();

            using var connection =
                new SqliteConnection(connectionString);

            string sql = """
                SELECT
                    Id,
                    FileName,
                    TextChunks,
                    CreatedAt
                FROM DocumentChunks
                WHERE FileName = @FileName
                """;

            var entity = await connection.QuerySingleOrDefaultAsync<DocumentChunkEntity>(
                sql,
                new { FileName = fileName });


            return entity;
        }

        public Task<DocumentChunkEntity?> GetDocumentKnowledgeAsync(string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
