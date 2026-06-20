namespace DemoAzAI.Domain.Models
{
    public class DocumentInfo
    {
        public string Id { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public string FileUrl { get; set; } = string.Empty;
    }
}
