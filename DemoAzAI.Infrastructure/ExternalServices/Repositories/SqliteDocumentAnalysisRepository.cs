using Dapper;
using DemoAzAI.Domain.Entities;
using DemoAzAI.Domain.Models;
using Microsoft.Data.Sqlite;
using System.Text.Json;

namespace DemoAzAI.Infrastructure.ExternalServices.Repositories
{
    public class SqliteDocumentAnalysisRepository : IDocumentAnalysisRepository
    {
        private readonly string connectionString = "Data Source=document_analysis.db";

        private bool initialized = false;


        public async Task<DocumentAnalysisEntity?> GetDocumentAnalysisByIdAsync(string id)
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
                FROM DocumentAnalysis
                WHERE Id = @IdParam
                """;

            var row = await connection.QuerySingleOrDefaultAsync<DocumentAnalysisEntity>(
                sql,
                new { IdParam = id });

            if (row == null)
                return null;

            return new DocumentAnalysisEntity
            {
                Id = row.Id,
                FileName = row.FileName,
                Hash = row.Hash,
                CreatedAt = row.CreatedAt,
                AnalysisResult =
                    JsonSerializer.Deserialize<DocumentAnalysisResult>(row.AnalysisResultJson)!
            };
        }

        public async Task<IEnumerable<DocumentAnalysisInfo>> GetDocumentAnalysisAsync()
        {
            EnsureInitialized();

            using var connection =
                new SqliteConnection(connectionString);

            string sql = """
                SELECT
                    Id,
                    FileName
                FROM DocumentAnalysis
                ORDER BY Id DESC;
                """;

            var rows =
                await connection.QueryAsync<DocumentAnalysisInfo>(sql);

            return rows.Select(r => new DocumentAnalysisInfo
            {
                Id = r.Id.ToString(),
                FileName = r.FileName
            });
        }

        public async Task<bool> DocumentAnalysisExistsAsync(string hash)
        {
            EnsureInitialized();

            await using var connection = new SqliteConnection(connectionString);

            const string sql = """
                SELECT COUNT(1)
                FROM DocumentAnalysis
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
                CREATE TABLE IF NOT EXISTS DocumentAnalysis (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    FileName TEXT NOT NULL UNIQUE,
                    Hash TEXT NOT NULL UNIQUE,
                    AnalysisResultJson TEXT NOT NULL,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                );
                """);

            initialized = true;
        }

        public async Task<bool> SaveDocumentAnalysisAsync(string fileName, DocumentAnalysisResult? result, string bytesHash)
        {

            EnsureInitialized();

            using var connection =
                new SqliteConnection(connectionString);

            string sql = """
                    INSERT INTO DocumentAnalysis
                    (FileName, Hash, AnalysisResultJson, CreatedAt)
                    VALUES
                    (@FileName, @Hash, @AnalysisResultJson, @CreatedAt);
                    """;

            var status = await connection.ExecuteAsync(
                sql,
                new
                {
                    FileName = fileName,
                    Hash = bytesHash,
                    AnalysisResultJson = JsonSerializer.Serialize(result),
                    CreatedAt = DateTime.UtcNow
                });

            return status > 0;
        }


        Task<DocumentAnalysisEntity?> IDocumentAnalysisRepository.GetDocumentAnalysisByHashAsync(string hash)
        {
            throw new NotImplementedException();
        }


    }
}
