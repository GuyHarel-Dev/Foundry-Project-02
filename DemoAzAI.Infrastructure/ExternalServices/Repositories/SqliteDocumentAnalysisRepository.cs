using Dapper;
using DemoAzAI.Domain.Entities;
using DemoAzAI.Domain.Models;
using DemoAzAI.Infrastructure.ExternalServices.Repositories.Interfaces;
using Microsoft.Data.Sqlite;
using System.Text.Json;

namespace DemoAzAI.Infrastructure.ExternalServices.Repositories
{
    public class SqliteDocumentAnalysisRepository : IDocumentAnalysisRepository
    {
        private readonly string connectionString = string.Empty;

        private bool initialized = false;

        public SqliteDocumentAnalysisRepository(string repositoryPath)
        {
            connectionString = $"Data Source={repositoryPath}/document_analysis.db";
        }
        public async Task<DocumentAnalysisEntity?> GetDocumentAnalysisByIdAsync(string id)
        {
            EnsureInitialized();

            using var connection =
                new SqliteConnection(connectionString);

            string sql = """
                SELECT
                    Id,
                    FileName,
                    BytesHash,
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

            var doc =  new DocumentAnalysisEntity
            {
                Id = row.Id,
                FileName = row.FileName,
                BytesHash = row.BytesHash,
                CreatedAt = row.CreatedAt,
                DocumentAnalysisResultJson = row.DocumentAnalysisResultJson,
                DocumentAnalysisResult = JsonSerializer.Deserialize<DocumentAnalysisResult>(row.DocumentAnalysisResultJson)
            };

            return doc;

        }
        public async Task<DocumentAnalysisEntity?> GetDocumentAnalysisByNameAsync(string fileName)
        {
            EnsureInitialized();

            using var connection =
                new SqliteConnection(connectionString);

            string sql = """
                SELECT
                    Id,
                    FileName,
                    BytesHash,
                    CreatedAt,
                    AnalysisResultJson
                FROM DocumentAnalysis
                WHERE FileName = @IdParam
                """;

            var row = await connection.QuerySingleOrDefaultAsync<DocumentAnalysisEntity>(
                sql,
                new { IdParam = fileName });

            if (row == null)
                return null;

            return new DocumentAnalysisEntity
            {
                Id = row.Id,
                FileName = row.FileName,
                BytesHash = row.BytesHash,
                CreatedAt = row.CreatedAt,
                DocumentAnalysisResult =
                    JsonSerializer.Deserialize<DocumentAnalysisResult>(row.DocumentAnalysisResultJson)!
            };
        }

        public async Task<IEnumerable<DocumentInfo>> GetAllDocumentAnalysisAsync()
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
                await connection.QueryAsync<DocumentInfo>(sql);

            return rows.Select(r => new DocumentInfo
            {
                Id = r.Id.ToString(),
                FileName = r.FileName
            });
        }

        public async Task<bool> DocumentAnalysisExistsAsync(string fileName)
        {
            EnsureInitialized();

            await using var connection = new SqliteConnection(connectionString);

            const string sql = """
                SELECT COUNT(1)
                FROM DocumentAnalysis
                WHERE FileName = @fileName;
                """;

            var count = await connection.ExecuteScalarAsync<long>(
                sql,
                new { fileName = fileName });

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
                    BytesHash TEXT NOT NULL UNIQUE,
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
                    (FileName, BytesHash, AnalysisResultJson, CreatedAt)
                    VALUES
                    (@FileName, @BytesHash, @AnalysisResultJson, @CreatedAt);
                    """;

            var status = await connection.ExecuteAsync(
                sql,
                new
                {
                    FileName = fileName,
                    BytesHash = bytesHash,
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
