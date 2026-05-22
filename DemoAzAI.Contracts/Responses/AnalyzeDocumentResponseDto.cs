using Azure.AI.DocumentIntelligence;
using DemoAzAI.Contracts.Dtos;

namespace DemoAzAI.Contracts.Responses
{
    public class AnalyzeDocumentResponseDto
    {
        public AnalyzeDocumentResponseDto(List<StepTracerStepDto> steps)
        {
            Steps.AddRange(steps);
        }

        public string? ModelId { get; set; }

        public string DocContent { get; set; } = string.Empty;

        public DocumentContentFormat? DocFormat { get; set; }

        public int PageCount { get; set; }

        public int ParagraphCount { get; set; }

        public int TableCount { get; set; }

        public int FigureCount { get; set; }
        public int SectionCount { get; set; }
        public int LanguagesCount { get; set; }

        public List<DocumentDto> Documents { get; set; } = [];

        public List<StepTracerStepDto> Steps { get; set; } = new();
    }
}
