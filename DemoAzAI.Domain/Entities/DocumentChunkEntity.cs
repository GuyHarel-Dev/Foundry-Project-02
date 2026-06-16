namespace DemoAzAI.Domain.Entities
{
    public class DocumentChunkEntity : DocumentEntity
    {
        public List<string> Chunks { get; set; } = new();

    }
}
