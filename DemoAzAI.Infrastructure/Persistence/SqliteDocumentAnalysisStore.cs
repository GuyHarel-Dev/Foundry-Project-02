using Dapper;
using DemoAzAI.Abstractions;
using DemoAzAI.Domain;
using Microsoft.Data.Sqlite;

namespace DemoAzAI.Infrastructure.Persistence
{
    public class SqliteDocumentAnalysisStore
    : IDocumentAnalysisStoreService
    {
        public SqliteDocumentAnalysisStore()
        {

            using var connection = new SqliteConnection("Data Source=document_analysis.db");

            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS DocumentAnalysis (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Hash TEXT NOT NULL UNIQUE,
                    AnalysisResult TEXT NOT NULL,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                );";


            connection.Execute(createTableQuery);
        }

        public Task<bool> ExistsAsync(string hash)
        {
            throw new NotImplementedException();
        }

        public Task<DocumentAnalysisResult?> GetByHashAsync(string hash)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(string hash, DocumentAnalysisResult result)
        {
            throw new NotImplementedException();
        }
    }
}
