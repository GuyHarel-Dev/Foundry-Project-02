using Dapper;
using DemoAzAI.Abstractions;
using DemoAzAI.Domain.Models.DocumentAnalysis;
using Microsoft.Data.Sqlite;
using System.Data.Common;
using System.Text.Json;

namespace DemoAzAI.Infrastructure.Persistence
{
    public class SqliteDocumentAnalysisStore : IDocumentAnalysisStoreService
    {
        private readonly string connectionString = "Data Source=document_analysis.db";

        private bool initialized;

        public async Task<IEnumerable<DocumentAnalysisHistoryItem>> GetAllAsync()
        {
            EnsureInitialized();

            using var connection =
                new SqliteConnection(connectionString);

            string sql = """
                SELECT
                    Id,
                    Hash,
                    CreatedAt,
                    AnalysisResult
                FROM DocumentAnalysis
                ORDER BY Id DESC;
                """;

            var rows =
                await connection.QueryAsync<DocumentAnalysisEntity>(sql);

            return rows.Select(r => new DocumentAnalysisHistoryItem
            {
                Id = r.Id,
                Hash = r.Hash,
                Result = JsonSerializer.Deserialize<DocumentAnalysisResult>(
                    r.AnalysisResult)!
            });
        }

        public Task<bool> ExistsAsync(string hash)
        {
            throw new NotImplementedException();
        }

        private void EnsureInitialized()
        {
            if (initialized)
                return;

            using var connection =
                new SqliteConnection(connectionString);

            connection.Execute("""
            CREATE TABLE IF NOT EXISTS DocumentAnalysis (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Hash TEXT NOT NULL UNIQUE,
                AnalysisResult TEXT NOT NULL,
                CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
            );
            """);

            initialized = true;
        }

        public async Task SaveAsync(string hash, DocumentAnalysisResult result)
        {
                EnsureInitialized();

                using var connection =
                    new SqliteConnection(connectionString);

                string sql = """
                    INSERT INTO DocumentAnalysis
                    (Hash, AnalysisResult, CreatedAt)
                    VALUES
                    (@Hash, @AnalysisResult, @CreatedAt);
                    """;

                var status = await connection.ExecuteAsync(
                    sql,
                    new
                    {
                        Hash = hash,
                        AnalysisResult = JsonSerializer.Serialize(result),
                        CreatedAt = DateTime.UtcNow
                    });

                var test = 123;

        }


        Task<DocumentAnalysisResult?> IDocumentAnalysisStoreService.GetByHashAsync(string hash)
        {
            throw new NotImplementedException();
        }
    }
}
