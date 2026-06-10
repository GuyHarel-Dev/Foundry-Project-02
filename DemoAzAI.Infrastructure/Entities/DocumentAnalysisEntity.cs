namespace DemoAzAI.Infrastructure.Entities
{
    public class DocumentAnalysisEntity
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string Hash { get; set; } = string.Empty;
        public string AnalysisResultJson { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
