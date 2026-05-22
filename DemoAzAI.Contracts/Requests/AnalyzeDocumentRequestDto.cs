using DemoAzAI.Contracts.Dtos;

namespace DemoAzAI.Contracts.Requests
{
    public class AnalyzeDocumentRequestDto
    {
        public AnalyzeDocumentRequestDto(List<StepTracerStepDto> steps)
        {
           Steps = steps;
        }
        public string ModelName { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public byte[] Bytes { get; set; } = Array.Empty<byte>();

        public List<StepTracerStepDto> Steps { get; set; } = new();

    }
}
