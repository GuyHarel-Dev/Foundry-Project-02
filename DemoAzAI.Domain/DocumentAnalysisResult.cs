namespace DemoAzAI.Domain
{
    public class DocumentAnalysisResult
    {
        public string? DocumentType { get; set; }

        public float? Confidence { get; set; }

        public List<DocumentAnalysisResultField> Fields { get; set; } = [];
    }
}
