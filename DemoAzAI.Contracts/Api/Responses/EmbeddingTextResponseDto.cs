using DemoAzAI.Domain.Entities;

namespace DemoAzAI.Contracts.Api.Responses
{
    public class EmbeddingTextResponseDto
    {
        public DocumentEmbeddingEntity? EmbeddedEntity { get; set; }

    }
}
