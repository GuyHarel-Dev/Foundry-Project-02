
using AzureAI.Contracts.Dtos;

namespace AzureAI.Contracts.Requests
{
    public class ChatRequestDto
    {
        public string Prompt { get; set; } = string.Empty;
    }
}
