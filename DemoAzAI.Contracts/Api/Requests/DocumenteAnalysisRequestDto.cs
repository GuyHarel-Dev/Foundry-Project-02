
namespace DemoAzAI.Contracts.Api.Requests
{
    public class DocumentAnalysisRequestDto
    {
        public bool AddDocumentToStore { get; set; } = false;

        public string ModelName { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public byte[] Bytes { get; set; } = Array.Empty<byte>();

        public string BytesHash { get; set; } = string.Empty;

    }
}
