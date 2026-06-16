using Dapper;
using DemoAzAI.Domain.Entities;
using DemoAzAI.Infrastructure.ExternalServices.Repositories.Interfaces;
using Microsoft.Data.Sqlite;

namespace DemoAzAI.Infrastructure.ExternalServices.Repositories
{
    public class SqliteDocumentChunksRepository : IDocumentChunksRepository
    {
        private readonly string connectionString = string.Empty;

        private bool initialized = false;

        public SqliteDocumentChunksRepository(string repositoryPath)
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
                CREATE TABLE IF NOT EXISTS DocumentChunks (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    FileName TEXT NOT NULL UNIQUE,
                    Chunks TEXT NOT NULL UNIQUE,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
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
                Id,
                FileName,
                Chunks,
                CreatedUtc
            )
            VALUES
            (
                @Id,
                @FileName,
                @Chunks,
                datetime('now')
            )
            """,
                document);


            return rows > 0;
        }

        public async Task<List<DocumentChunkEntity>> GetDocumentChunksAsync(string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
