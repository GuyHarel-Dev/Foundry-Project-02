using DemoAzAI.Application.AI;

namespace DemoAzAI.Contracts.Api.Requests
{
    public class DocumentKnowledgeRequestDto
    {
        public string SystemPrompt { get; set; } = AiPrompts.DocumentKnowledgeExtraction;
        public string FileName { get; set; } = string.Empty;
        public string TextToChunk { get; set; } = string.Empty;
        public bool SaveToStore { get; set; } = false;

    }
}
