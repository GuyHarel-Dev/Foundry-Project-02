using DemoAzAI.Domain.Models;

namespace DemoAzAI.Contracts.Api.Responses
{
    public class DocumentKnowledgeResponseDto
    {
        public List<string> TextChunks { get; set; } = new();
        public ModelStatistics? ModelStatistics { get; set; }
    }
}
