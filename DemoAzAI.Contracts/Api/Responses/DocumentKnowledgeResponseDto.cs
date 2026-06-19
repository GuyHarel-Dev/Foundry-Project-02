using DemoAzAI.Domain.Entities;
using DemoAzAI.Domain.Models;

namespace DemoAzAI.Contracts.Api.Responses
{
    public class DocumentKnowledgeResponseDto
    {
        public DocumentEntity? DocumentEntity { get; set; }

        public List<DocumentEmbeddingEntity>? DocumentEmbeddingEntities { get; set; } 

        public ModelStatistics? ModelStatistics { get; set; }
    }
}
