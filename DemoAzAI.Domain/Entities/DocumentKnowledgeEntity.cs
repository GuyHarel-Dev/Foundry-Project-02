using System.Text;

namespace DemoAzAI.Domain.Entities
{
    public class DocumentKnowledgeEntity
    {
        public string? DocumentId { get; set; }
        public string? FileName { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<DocumentEmbeddingEntity>? Embeddings { get; set; }

        public string CosineSimilaritiesToString(float threshold)
        {
            var sb = new StringBuilder();

            if (Embeddings != null)
            {
                foreach (var emb in Embeddings)
                {
                    sb.AppendLine($"{emb.CosineSimilarity.ToString("P0")} [{emb.ChunkText}]");
                }
            }

            // Sort
            var sortedLines = sb.ToString()
                                .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                                .OrderByDescending(line => line);

            sb.Clear();
            foreach (var line in sortedLines)
            {
                sb.AppendLine(line);
            }
            return sb.ToString();
        }


        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"DocumentId:{DocumentId}");
            sb.AppendLine($"FileName:{FileName}");
            
            if ( Embeddings != null )
            {
                foreach (var emb in Embeddings)
                { 
                    sb.AppendLine($"  {emb.ChunkText}, ");
                    sb.AppendLine($"  [{emb.EmbeddingVectorJson?.Substring(1, 128)}...]");
                    sb.AppendLine();
                }
            }
           
            return sb.ToString();
        }
    }
}
 