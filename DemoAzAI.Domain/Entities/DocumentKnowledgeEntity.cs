using DemoAzAI.Domain.Models;

namespace DemoAzAI.Domain.Entities
{
    public class DocumentKnowledgeEntity
    {
        public string? DocumentId { get; set; }
        public string? FileName { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<EmbeddedText>? EmbeddedTexts { get; set; }
    }
}
 