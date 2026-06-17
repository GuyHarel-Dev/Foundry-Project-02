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

            var entity = await connection.QuerySingleOrDefaultAsync<DocumentAnalysisEntity>(
                sql,
                new { IdParam = id });

            if (entity != null && entity.AnalysisResultJson != null)
            {
                entity.DocumentAnalysisResult = JsonSerializer.Deserialize<DocumentAnalysisResult>(entity.AnalysisResultJson);
            }

            return entity;

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

            var entity = await connection.QuerySingleOrDefaultAsync<DocumentAnalysisEntity>(
                sql,
                new { IdParam = fileName });

            if (entity != null && entity.AnalysisResultJson != null)
            {
                entity.DocumentAnalysisResult = JsonSerializer.Deserialize<DocumentAnalysisResult>(entity.AnalysisResultJson);
            }

            return entity;
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

        public async Task<bool> SaveDocumentAnalysisAsync(string fileName, DocumentAnalysisResult? result, string bytesHash)
        {

            EnsureInitialized();

            if (await DocumentAnalysisExistsAsync(fileName))
            {
                await DeleteDocumentAnalysisAsync(fileName);
            }

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

        public async Task<bool> DeleteDocumentAnalysisAsync(string fileName)
        {

            EnsureInitialized();

            using var connection =
                new SqliteConnection(connectionString);

            string sql = """
                    DELETE FROM DocumentAnalysis
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

        Task<DocumentAnalysisEntity?> IDocumentAnalysisRepository.GetDocumentAnalysisByHashAsync(string hash)
        {
            throw new NotImplementedException();
        }


    }
}
