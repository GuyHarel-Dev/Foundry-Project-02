using Azure.AI.DocumentIntelligence;
using AzureAI.Contracts.Dtos;

namespace AzureAI.Contracts.Responses
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

        public List<DocumentFieldDto> Fields { get; set; } = [];

        public object JsonObjectResult { get; set; } = string.Empty;
    }
}
