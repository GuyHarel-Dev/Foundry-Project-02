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

        public async Task<DocumentAnalysisStoredResult?> GetByIdAsync(string id)
        {
           EnsureInitialized();

            using var connection =
                new SqliteConnection(connectionString);

            string sql = """
                SELECT
                    Id,
                    FileName,
                    Hash,
                    CreatedAt,
                    AnalysisResultJson
                FROM DocumentAnalysisResults
                WHERE Id = @IdParam
                """;

            var row = await connection.QuerySingleOrDefaultAsync<DocumentAnalysisStoredResult>(
                sql,
                new { IdParam = id });

            if (row == null)
                return null;

            return new DocumentAnalysisStoredResult
            {
                Id = row.Id,
                FileName = row.FileName,
                Hash = row.Hash,
                CreatedAt = row.CreatedAt,
                AnalysisResult =
                    JsonSerializer.Deserialize<DocumentAnalysisResult>(row.AnalysisResultJson)!
            };
        }

        public async Task<IEnumerable<DocumentAnalysisStoredResult>> GetAllAsync()
        {
            EnsureInitialized();

            using var connection =
                new SqliteConnection(connectionString);

            string sql = """
                SELECT
                    Id,
                    FileName,
                    Hash,
                    CreatedAt,
                    AnalysisResultJson
                FROM DocumentAnalysisResults
                ORDER BY Id DESC;
                """;

            var rows =
                await connection.QueryAsync<DocumentAnalysisEntity>(sql);

            return rows.Select(r => new DocumentAnalysisStoredResult
            {
                Id = r.Id,
                FileName = r.FileName,
                CreatedAt = r.CreatedAt,
                Hash = r.Hash,
                AnalysisResult = JsonSerializer.Deserialize<DocumentAnalysisResult>(
                    r.AnalysisResultJson)!
            });
        }

        public async Task<bool> ExistsAsync(string hash)
        {
            EnsureInitialized();

            await using var connection = new SqliteConnection(connectionString);

            const string sql = """
                SELECT COUNT(1)
                FROM DocumentAnalysisResults
                WHERE Hash = @Hash;
                """;

            var count = await connection.ExecuteScalarAsync<long>(
                sql,
                new { Hash = hash });

            return count > 0;
        }

        private void EnsureInitialized()
        {
            if (initialized)
                return;

            using var connection =
                new SqliteConnection(connectionString);

            connection.Execute("""
                CREATE TABLE IF NOT EXISTS DocumentAnalysisResults (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    FileName TEXT NOT NULL UNIQUE,
                    Hash TEXT NOT NULL UNIQUE,
                    AnalysisResultJson TEXT NOT NULL,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                );
                """);

            initialized = true;
        }

        public async Task<bool> SaveAsync(string fileName, string documentHash, DocumentAnalysisResult result)
        {
                EnsureInitialized();

                using var connection =
                    new SqliteConnection(connectionString);

                string sql = """
                    INSERT INTO DocumentAnalysisResults
                    (FileName, Hash, AnalysisResultJson, CreatedAt)
                    VALUES
                    (@FileName, @Hash, @AnalysisResultJson, @CreatedAt);
                    """;

                var status = await connection.ExecuteAsync(
                    sql,
                    new
                    {
                        FileName = fileName,
                        Hash = documentHash,
                        AnalysisResultJson = JsonSerializer.Serialize(result),
                        CreatedAt = DateTime.UtcNow
                    });

                return status > 0;
        }


        Task<DocumentAnalysisStoredResult?> IDocumentAnalysisStoreService.GetByHashAsync(string hash)
        {
            throw new NotImplementedException();
        }


    }
}
