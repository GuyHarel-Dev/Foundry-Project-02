using DemoAzAI.Domain.Entities;
using System.Text;

namespace DemoAzAI.Contracts.Api.Responses
{
    public class EmbeddingTextLinesResponseDto
    {
        public List<DocumentEmbeddingEntity>? EmbeddedTexts { get; set; }

    }
}
