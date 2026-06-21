using DemoAzAI.Domain.Models;

namespace DemoAzAI.Contracts.Api.Responses
{
    public class DocumentKnowledgeResponseDto
    {
        public ModelStatistics? ChunkingStatistics { get; set; }
    }
}
