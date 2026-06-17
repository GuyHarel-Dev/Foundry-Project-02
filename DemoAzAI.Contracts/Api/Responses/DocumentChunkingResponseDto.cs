using DemoAzAI.Domain.Models;

namespace DemoAzAI.Contracts.Api.Responses
{
    public class DocumentKnowledgeResponseDto
    {
        public string TextChunks { get; set; } = string.Empty;
        public ModelStatistics? ModelStatistics { get; set; }
    }
}
