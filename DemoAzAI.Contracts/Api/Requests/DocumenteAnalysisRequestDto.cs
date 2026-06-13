
namespace DemoAzAI.Contracts.Api.Requests
{
    public class DocumentAnalysisRequestDto
    {
        public string ModelName { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public bool SaveToStore { get; set; } = false;

    }
}
