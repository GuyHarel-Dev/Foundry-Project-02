using DemoAzAI.Domain;
using System.Text;

namespace DemoAzAI.Api.Contracts.Responses
{
    public class EmbeddingTextResponseDto
    {
        public List<TextEmbedding> TextEmbeddings { get; set; } = new List<TextEmbedding>();
        public string GetSummary()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Received {TextEmbeddings.Count} embeddings:");
            sb.AppendLine();

            foreach (var embedding in TextEmbeddings)
            {
                sb.AppendLine($"{embedding.Text}");
                sb.AppendLine($"[{string.Join(", ", embedding.Vector.Take(5))}...]");
                sb.AppendLine();

            }
            return sb.ToString();
        }
    }
}
