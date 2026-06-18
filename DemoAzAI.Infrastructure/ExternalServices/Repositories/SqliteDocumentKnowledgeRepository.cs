using Dapper;
using DemoAzAI.Domain.Entities;
using DemoAzAI.Infrastructure.ExternalServices.Repositories.Interfaces;
using Microsoft.Data.Sqlite;
using System.Text.Json;

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
                CREATE TABLE IF NOT EXISTS DocumentEmbedding (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    ChunkIndex INTEGER NOT NULL,
                    ChunkText TEXT NOT NULL,
                    EmbeddingVectorJson TEXT NOT NULL,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY(DocumentId)
                    REFERENCES Documents(Id)
                );
                """);

            initialized = true;
        }

        public async Task<bool> SaveDocumentChunkEntityAsync(DocumentChunkEntity document)
        {
            using var connection = new SqliteConnection(connectionString);

            EnsureInitialized();

            var fileName = document.FileName;

            var fileId = await connection.ExecuteScalarAsync<long>(
            """
            INSERT INTO Document
            (
                FileName,
                CreatedAt
            )
            VALUES
            (
                @FileName,
                datetime('now')
            );

            SELECT last_insert_rowid();

            """,
                fileName);

            var chunks = document.EmbeddedChunks;

            if (chunks != null && chunks.Any())
            {
                var index = 0;
                foreach (var chunk in chunks)
                {
                    var test = await connection.ExecuteAsync(
                        """
                        INSERT INTO DocumentEmbedding
                        (
                            ChunkIndex,
                            ChunkText,
                            EmbeddingVectorJson,
                            CreatedAt
                        )
                        VALUES
                        (
                            @ChunkIndex,
                            @ChunkText,
                            @EmbeddingVectorJson,
                            datetime('now')
                        );

                        SELECT last_insert_rowid();

                        """,
                        new
                        {
                            ChunkIndex = index,
                            ChunkText = chunk.Text,
                            EmbeddingVectorJson = JsonSerializer.Serialize(chunk.EmbeddingVector),
                            CreatedAt = DateTime.UtcNow
                        });

                    index += 1;
                }
            }


            return fileId > 0;
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
