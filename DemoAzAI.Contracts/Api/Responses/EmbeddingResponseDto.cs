using DemoAzAI.Domain;
using System.Text;

namespace DemoAzAI.Contracts.Api.Responses
{
    public class EmbeddingResponseDto
    {
        public List<EmbeddedChunk> TextEmbeddings { get; set; } = new List<EmbeddedChunk>();

        public string GetSummary()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Received {TextEmbeddings.Count} embeddings:");
            sb.AppendLine();

            foreach (var embedding in TextEmbeddings)
            {
                sb.AppendLine($"{embedding.Text}");
                sb.AppendLine($"[{string.Join(", ", embedding.EmbeddingVector.Take(5))}...]");
                sb.AppendLine();

            }
            return sb.ToString();
        }
    }
}
