using Azure.AI.DocumentIntelligence;

namespace AzureAI.Contracts.Responses
{
    public class AnalyzeDocumentResponseDto
    {
        public string DocContent { get; set; } = string.Empty;

        public DocumentContentFormat? DocFormat { get; set; }

        public int PageCount { get; set; }

        public int ParagraphCount { get; set; }

        public int SectionCount { get; set; }

        public int TableCount { get; set; }

    }
}
