using Dapper;
using Microsoft.Data.Sqlite;

namespace DemoAzAI.Infrastructure.Persistence
{
    public class SqliteDocumentAnalysisStore
    {
        private readonly string _connectionString = "Data Source=document_analysis.db";

        public SqliteDocumentAnalysisStore()
        {
            using var connection = new SqliteConnection(_connectionString);

            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS DocumentAnalysis (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    DocumentName TEXT NOT NULL,
                    AnalysisResult TEXT NOT NULL,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                )";

            connection.Execute(createTableQuery);
        }
    }
}
