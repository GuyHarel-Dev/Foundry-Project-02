namespace DemoAzAI.Contracts.Api.Requests
{
    public class DocumentKnowledgeRequestDto
    {
        public string SystemPrompt { get; set; } = string.Empty;
        public string DocumentText { get; set; } = string.Empty;
        public bool SaveToStore { get; set; } = false;
    }
}
