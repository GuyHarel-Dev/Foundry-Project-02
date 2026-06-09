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
            foreach (var embedding in TextEmbeddings)
            {
                sb.AppendLine($"{embedding.Text}: [{string.Join(", ", embedding.EmbeddingVector.Take(5))}...]");

            }
            return sb.ToString();
        }
    }
}
