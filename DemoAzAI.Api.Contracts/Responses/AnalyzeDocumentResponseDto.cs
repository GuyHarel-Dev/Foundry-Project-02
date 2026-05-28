using Azure.AI.DocumentIntelligence;
using DemoAzAI.Domain;

namespace DemoAzAI.Api.Contracts.Responses
{
    public class AnalyzeDocumentResponseDto
    {

        public string? ModelId { get; set; }

        public string DocContent { get; set; } = string.Empty;

        public DocumentContentFormat? DocFormat { get; set; }

        public int PageCount { get; set; }

        public int ParagraphCount { get; set; }

        public int TableCount { get; set; }

        public int FigureCount { get; set; }
        public int SectionCount { get; set; }
        public int LanguagesCount { get; set; }

        public List<DocumentAnalysisResult> Documents { get; set; } = [];
    }
}
