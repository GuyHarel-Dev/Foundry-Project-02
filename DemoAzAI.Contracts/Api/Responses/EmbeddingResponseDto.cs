using DemoAzAI.Domain.Entities;
using DemoAzAI.Domain.Models;
using System.Text;

namespace DemoAzAI.Contracts.Api.Responses
{
    public class EmbeddingResponseDto
    {
        public DocumentChunkEntity DocumentChunkEntity { get; set; } = new();

        public string GetSummary()
        {
            var sb = new StringBuilder();
            foreach (var embedding in DocumentChunkEntity.EmbeddedChunks)
            {
                sb.AppendLine($"{embedding.Text}: [{string.Join(", ", embedding.EmbeddingVector.Take(5))}...]");

            }
            return sb.ToString();
        }
    }
}
