using Dapper;
using DemoAzAI.Domain.Entities;
using Microsoft.Data.Sqlite;

namespace DemoAzAI.Infrastructure.ExternalServices.Repositories
{
    public class SqliteDocumentRepository : IDocumentRepository
    {
        private readonly string connectionString = "Data Source=document.db";
        private bool initialized;

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

        public async Task<bool> SaveAsync(StoreDocument document)
        {
            using var connection = new SqliteConnection(connectionString);

            var rows = await connection.ExecuteAsync(
                """
            INSERT INTO Documents
            (
                Id,
                FileName,
                DisplayName,
                FileType,
                RelativePath,
                CreatedUtc
            )
            VALUES
            (
                @Id,
                @FileName,
                @DisplayName,
                @FileType,
                @RelativePath,
                datetime('now')
            )
            """,
                document);


            return rows > 0;
        }

        public async Task<List<StoreDocument>> GetAllDocuments()
        {
            throw new NotImplementedException();
        }
    }
}
