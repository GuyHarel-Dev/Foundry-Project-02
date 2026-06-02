namespace DemoAzAI.Domain.Models.DocumentAnalysis
{
    public class DocumentAnalysisHistoryItem
    {
        public int Id { get; set; }

        public string Hash { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DocumentAnalysisResult Result { get; set; } = default!;
    }
}
