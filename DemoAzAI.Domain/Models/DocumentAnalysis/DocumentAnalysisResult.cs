namespace DemoAzAI.Domain.Models.DocumentAnalysis
{
    public class DocumentAnalysisResult
    {
        public string ModelName { get; set; } = string.Empty;
        public string ApiVersion { get; set; } = string.Empty;
        public string DocContent { get; set; } = string.Empty;
        public string DocFormat { get; set; } = string.Empty;

        public int PageCount { get; set; }
        public int ParagraphCount { get; set; }
        public int SectionCount { get; set; }
        public int TableCount { get; set; }
        public int FigureCount { get; set; }
        public int LanguagesCount { get; set; }

        public List<DocumentAnalyzed> Documents { get; set; } = new();

    }
}
