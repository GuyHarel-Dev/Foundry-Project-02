namespace DemoAzAI.Domain.Models.DocumentAnalysis
{
    public class DocumentAnalysisStoredResult
    {
        public int Id { get; set; }

        public string Hash { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public string AnalysisResultJson { get; set; } = string.Empty;
        public DocumentAnalysisResult AnalysisResult { get; set; } = default!;

        public override string ToString()
        {
            return $"{Id}-{FileName}";
        }
    }
}
