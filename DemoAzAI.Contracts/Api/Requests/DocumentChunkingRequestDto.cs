using DemoAzAI.Application.AI;

namespace DemoAzAI.Contracts.Api.Requests
{
    public class DocumentChunkingRequestDto : ChatQuestionRequestDto
    {
        public string FileName { get; set; } = string.Empty;
        public string TextChunks { get; set; } = string.Empty;
        public bool SaveToStore { get; set; } = false;

        public DocumentChunkingRequestDto()
        {
            Prompt = AiPrompts.DocumentKnowledgeExtraction;
        }
    }
}
