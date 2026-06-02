using Dapper;
using DemoAzAI.Abstractions;
using DemoAzAI.Domain;
using Microsoft.Data.Sqlite;
using System.Data.Common;
using System.Text.Json;

namespace DemoAzAI.Infrastructure.Persistence
{
    public class SqliteDocumentAnalysisStore : IDocumentAnalysisStoreService
    {
        private readonly string connectionString = "Data Source=document_analysis.db";

        private bool initialized;

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
                    (Hash, AnalysisResult)
                    VALUES
                    (@Hash, @AnalysisResult);
                    """;

            await connection.ExecuteAsync(
                sql,
                new
                {
                    Hash = hash,
                    AnalysisResult =
                        JsonSerializer.Serialize(result)
                });

        }


        Task<DocumentAnalysisResult?> IDocumentAnalysisStoreService.GetByHashAsync(string hash)
        {
            throw new NotImplementedException();
        }
    }
}
