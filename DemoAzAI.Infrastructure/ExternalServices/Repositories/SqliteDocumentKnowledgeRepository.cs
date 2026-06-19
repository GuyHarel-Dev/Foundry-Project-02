using Dapper;
using DemoAzAI.Domain.Entities;
using DemoAzAI.Domain.Models;
using DemoAzAI.Infrastructure.ExternalServices.Repositories.Interfaces;
using Microsoft.Data.Sqlite;
using System.Reflection.Metadata;
using System.Text.Json;

namespace DemoAzAI.Infrastructure.ExternalServices.Repositories
{
    public class SqliteDocumentKnowledgeRepository : IDocumentKnowledgeRepository
    {
        private readonly string connectionString = string.Empty;

        private bool initialized = false;

        public SqliteDocumentKnowledgeRepository(string repositoryPath)
        {
            connectionString = $"Data Source={repositoryPath}/document_knowledge.db";
        }

        private void EnsureInitialized()
        {
            if (initialized)
                return;

            using var connection =
                new SqliteConnection(connectionString);

            connection.Execute(
                """
                CREATE TABLE IF NOT EXISTS Documents (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    FileName TEXT NOT NULL UNIQUE,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                );

                CREATE TABLE IF NOT EXISTS DocumentEmbeddings (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,

                    DocumentId INTEGER NOT NULL,

                    ChunkIndex INTEGER NOT NULL,

                    ChunkText TEXT NOT NULL,

                    EmbeddingVectorJson TEXT NOT NULL,

                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,

                    FOREIGN KEY(DocumentId)
                        REFERENCES Documents(Id)
                        ON DELETE CASCADE,

                    UNIQUE(DocumentId, ChunkIndex)
                );

                CREATE INDEX IF NOT EXISTS IX_DocumentEmbeddings_DocumentId
                ON DocumentEmbeddings(DocumentId);
                """);

            initialized = true;
        }

        public async Task<bool> DocumentKnowledgeExistsAsync(string fileName)
        {
            EnsureInitialized();

            await using var connection = new SqliteConnection(connectionString);

            const string sql = """
                SELECT COUNT(1)
                FROM Documents
                WHERE FileName = @fileName;
                """;

            var count = await connection.ExecuteScalarAsync<long>(
                sql,
                new { fileName = fileName });

            return count > 0;
        }

        public async Task<bool> DeleteDocumentKnowledgeAsync(string fileName)
        {

            EnsureInitialized();

            using var connection =
                new SqliteConnection(connectionString);

            string sql = """
                    DELETE FROM Documents
                    WHERE FileName = @fileName;
                    """;

            var status = await connection.ExecuteAsync(
                sql,
                new
                {
                    fileName = fileName
                });

            return status > 0;


        }
        public async Task<bool> SaveDocumentKnowledgeAsync(DocumentKnowledgeEntity document)
        {
            EnsureInitialized();

            using var connection = new SqliteConnection(connectionString);

            await connection.OpenAsync();

            //await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                var documentId = await connection.ExecuteScalarAsync<long>(
                    """
                    INSERT INTO Documents
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
                     new { FileName = document.FileName });


                if (document.EmbeddedTexts != null && document.EmbeddedTexts.Any())
                {
                    var test = JsonSerializer.Serialize(document.EmbeddedTexts[0].Vector);

                    var index = 0;
                    foreach (var chunk in document.EmbeddedTexts)
                    {
                        var status = await connection.ExecuteAsync(
                        """
                        INSERT INTO DocumentEmbeddings
                        (
                            DocumentId,
                            ChunkIndex,
                            ChunkText,
                            EmbeddingVectorJson,
                            CreatedAt
                        )
                        VALUES
                        (
                            @DocumentId,
                            @ChunkIndex,
                            @ChunkText,
                            @EmbeddingVectorJson,
                            datetime('now')
                        );

                        """,
                            new
                            {
                                DocumentId = documentId,
                                ChunkIndex = index,
                                ChunkText = chunk.Text,
                                EmbeddingVectorJson = JsonSerializer.Serialize(chunk.Vector),
                                CreatedAt = DateTime.UtcNow
                            });

                        index += 1;
                    }
                }

                //await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                //await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<DocumentKnowledgeEntity?> GetDocumentKnowledgeAsync(string fileName)
        {
            EnsureInitialized();

            using var connection = new SqliteConnection(connectionString);

            await connection.OpenAsync();

            string sql = """
                SELECT
                    Id,
                    FileName,
                    CreatedAt
                FROM Documents
                WHERE FileName = @FileName
                """;

            var entity = await connection.QuerySingleOrDefaultAsync<DocumentKnowledgeEntity>(
                sql,
                new { FileName = fileName });

            if (entity == null)
            {
                return null;
            }

            var chunks = await connection.QueryAsync<DocumentKnowledgeEntity>(
                """
                SELECT
                    ChunkIndex,
                    ChunkText,
                    EmbeddingVectorJson
                FROM DocumentEmbeddings
                WHERE DocumentId = @DocumentId
                ORDER BY ChunkIndex
                """,
                new { DocumentId = entity.DocumentId });


            return entity;
        }

    }
}
