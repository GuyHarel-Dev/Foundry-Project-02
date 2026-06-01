namespace DemoAzAI.Domain
{
    public class DocumentAnalysisResult
    {
        public string Hash { get; set; } = string.Empty;
        public string? DocumentType { get; set; }

        public float? Confidence { get; set; }

        public List<DocumentAnalysisResultField> Fields { get; set; } = [];
    }
}
