namespace DemoAzAI.Contracts.Api.Requests
{
    public class ChatQuestionRequestDto
    {
        public string SystemPrompt { get; set; } = string.Empty;
        public string Prompt { get; set; } = string.Empty;
    }
}
