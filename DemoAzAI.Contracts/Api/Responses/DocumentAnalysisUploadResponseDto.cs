namespace DemoAzAI.Contracts.Api.Responses
{
    public class DocumentAnalysisUploadResponseDto
    {
        public string FileName { get; set; } = string.Empty;
        public string Hash { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
