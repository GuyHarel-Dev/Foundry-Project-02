namespace DemoAzAI.Contracts.Api.Requests
{
    public class AnalyzeDocumentRequestDto
    {
        public string ModelName { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public byte[] Bytes { get; set; } = Array.Empty<byte>();

    }
}
