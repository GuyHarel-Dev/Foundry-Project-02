using Azure.AI.Extensions.OpenAI;
using Azure.AI.Projects;
using AzureAI.Api.Services.Interfaces;
using AzureAI.Common;
using AzureAI.Contracts.Dtos;
using AzureAI.Contracts.Requests;
using AzureAI.Contracts.Responses;
using OpenAI.Responses;
using System.ClientModel;

#pragma warning disable OPENAI001 // pour using OpenAI.Responses;

namespace AzureAI.Api.Services
{
    public class AzFoundryChatService : IAzFoundryChatService
    {
        private readonly IConfiguration _config;
        private readonly AIProjectClient _projectClient;
        private readonly string _modelName;
        private readonly ILogger<AzFoundryChatService> _logger;
        private readonly IApiStepTracer _traceStepper;

        public AzFoundryChatService(
            IConfiguration config,
            AIProjectClient projectClient,
            ILogger<AzFoundryChatService> logger,
            IApiStepTracer traceStepper)
        {
            _config = config;
            _projectClient = projectClient;
            _logger = logger;
            _traceStepper = traceStepper;

            _modelName = _config["FoundryPrj01:Model:OpenAiChat"]
                ?? throw new InvalidOperationException("ModelName not configured");

        }

        public async Task<ChatResponseDto> GetPromptResponseAsync(ChatRequestDto request, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Sending prompt to Azure AI Foundry");

            CreateResponseOptions options = CreateResponseOptions(request);
            ProjectResponsesClient client = CreateResponseClient();

            ClientResult<ResponseResult> result = await client.CreateResponseAsync(
                options, cancellationToken);

            _traceStepper.AddStep(ApiStepTypeEnum.WebApi, "Received response from Azure AI Foundry");

            var responseDto = new ChatResponseDto
            {
                ChatResponse = result.Value.GetOutputText(),
                ModelStatistics = new ModelStatisticsDto
                {
                    ResponseId = result.Value.Id,
                    Model = result.Value.Model,
                    InputTokens = result.Value.Usage?.InputTokenCount,
                    OutputTokens = result.Value.Usage?.OutputTokenCount,
                    TotalTokens = result.Value.Usage?.TotalTokenCount
                }
            };
             
            return responseDto;
        }

        private ProjectResponsesClient CreateResponseClient()
        {
            return _projectClient
                .ProjectOpenAIClient
                .GetProjectResponsesClientForModel(_modelName);
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

