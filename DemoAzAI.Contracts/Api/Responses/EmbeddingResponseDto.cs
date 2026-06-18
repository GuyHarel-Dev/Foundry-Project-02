using DemoAzAI.Domain.Models;
using System.Text;

namespace DemoAzAI.Contracts.Api.Responses
{
    public class EmbeddingResponseDto
    {
        public List<EmbeddedChunk> EmbeddedChunks { get; set; } = [];

        public string GetSummary()
        {
            var sb = new StringBuilder();
            foreach (var embedding in EmbeddedChunks)
            {
                sb.AppendLine($"{embedding.Text}: [{string.Join(", ", embedding.EmbeddingVector.Take(5))}...]");

            }
            return sb.ToString();
        }
    }
}
