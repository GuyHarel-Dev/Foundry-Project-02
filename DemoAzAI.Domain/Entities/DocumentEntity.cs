namespace DemoAzAI.Domain.Entities
{
    public class DocumentEntity
    {
        public string Id { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public string BytesHash { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{FileName}";
        }
    }
}
