using Azure.AI.Extensions.OpenAI;
using Azure.AI.Projects;
using DemoAzAI.Api.Contracts.Requests;
using DemoAzAI.Api.Contracts.Responses;
using DemoAzAI.Api.Services.Interfaces;
using DemoAzAI.Domain;
using OpenAI.Responses;
using System.ClientModel;

#pragma warning disable OPENAI001 // pour using OpenAI.Responses;

namespace DemoAzAI.Api.Services
{
    public class ChatService : IChatService
    {
        private readonly IConfiguration _config;
        private readonly AIProjectClient _projectClient;
        private readonly ILogger<ChatService> _logger;

        public ChatService(
            IConfiguration config,
            AIProjectClient projectClient,
            ILogger<ChatService> logger)
       {
            _config = config;
            _projectClient = projectClient;
            _logger = logger;


        }

        public async Task<ChatResponseDto> GetPromptResponseAsync(ChatRequestDto request, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Sending prompt to Azure AI Foundry");

            CreateResponseOptions options = CreateResponseOptions(request);
            ProjectResponsesClient client = CreateResponseClient();

            ClientResult<ResponseResult> result = await client.CreateResponseAsync(
                options, cancellationToken);

            var response = new ChatResponseDto();

            response.ChatResponse = result.Value.GetOutputText();

            response.ModelStatistics = new ModelStatistics
            (
                result.Value.Model,
                result.Value.Usage?.InputTokenCount,
                result.Value.Usage?.OutputTokenCount,
                result.Value.Usage?.TotalTokenCount,
                response.ChatResponse.Length
            );


            return response;
        }

        private ProjectResponsesClient CreateResponseClient()
        {

            var modelName = _config["FoundryPrj01:Model:OpenAiChat"]
                ?? throw new InvalidOperationException("ModelName not configured");

            return _projectClient
                .ProjectOpenAIClient
                .GetProjectResponsesClientForModel(modelName);
        }

        private CreateResponseOptions CreateResponseOptions(ChatRequestDto request)
        {
            var systemPrompt = _config["FoundryPrj01:SystemPrompt"] ?? string.Empty;

            var options = new CreateResponseOptions
            {
                Instructions = systemPrompt
            };

            options.InputItems.Add(
                ResponseItem.CreateUserMessageItem(request.Prompt));
            return options;
        }
    }
}

