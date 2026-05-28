namespace DemoAzAI.Api.Contracts.Dtos
{
    public class DocumentDto
    {
        public string? DocumentType { get; set; }

        public float? Confidence { get; set; }

        public List<DocumentFieldDto> Fields { get; set; } = [];
    }
}
