using DemoAzAI.Domain.Entities;
using DemoAzAI.Domain.Models;
using System.Text;

namespace DemoAzAI.Contracts.Api.Responses
{
    public class EmbeddingResponseDto
    {
        public DocumentEmbeddingEntity DocumentEmbeddingEntity { get; set; } = new();

        public string GetSummary()
        {
            var sb = new StringBuilder();
            foreach (var embedding in DocumentEmbeddingEntity.EmbeddedChunks)
            {
                sb.AppendLine($"{embedding.ChunkText}: [{string.Join(", ", embedding.EmbeddingVector.Take(5))}...]");

            }
            return sb.ToString();
        }
    }
}
