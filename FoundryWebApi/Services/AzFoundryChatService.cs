using Azure.AI.Projects;
using OpenAI.Responses;
using SharedDTO;
using System.ClientModel;

#pragma warning disable OPENAI001 // pour using OpenAI.Responses;

namespace WebApi.Services
{
    public class AzFoundryChatService : IAzFoundryChatService
    {
        private readonly IConfiguration _config;
        private readonly AIProjectClient _projectClient;
        private readonly string? _modelName;
        private readonly ILogger<AzFoundryChatService> _logger;

        public AzFoundryChatService(
            IConfiguration config, 
            AIProjectClient projectClient,
            ILogger<AzFoundryChatService> logger)
        {
            _config = config;
            _projectClient = projectClient;
            _logger = logger;

            _modelName = _config["Foundry-Project-01:ModelName"]
                ?? throw new InvalidOperationException("ModelName not configured");

        }

        public async Task<ChatResponse> GetResponseAsync(ChatRequest request, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Sending prompt to Azure AI Foundry");

            var systemPrompt = _config["Foundry-Project-01:SystemPrompt"] ?? string.Empty;

            var options = new CreateResponseOptions
            {
                Instructions = systemPrompt
            };  

            options.InputItems.Add(
                ResponseItem.CreateUserMessageItem(request.Request));

            var client = _projectClient
                .ProjectOpenAIClient
                .GetProjectResponsesClientForModel(_modelName);


            ClientResult<ResponseResult> result = await client.CreateResponseAsync(
                options, cancellationToken);

            var response = result.Value.GetOutputText();

            return new ChatResponse
            {
                Response = response
            };
        }
    }
}

