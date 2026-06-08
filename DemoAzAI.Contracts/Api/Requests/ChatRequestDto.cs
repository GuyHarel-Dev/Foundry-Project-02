namespace DemoAzAI.Contracts.Api.Requests
{
    public class ChatRequestDto
    {
        public string SystemPrompt { get; set; } = string.Empty;
        public string Prompt { get; set; } = string.Empty;
    }
}
