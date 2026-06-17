using Dapper;
using DemoAzAI.Domain.Entities;
using DemoAzAI.Domain.Models;
using DemoAzAI.Infrastructure.ExternalServices.Repositories.Interfaces;
using Microsoft.Data.Sqlite;
using System.Text.Json;

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
                    TextChunks TEXT NOT NULL UNIQUE,
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
    }
}
