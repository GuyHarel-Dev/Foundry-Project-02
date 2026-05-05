using Azure.AI.Extensions.OpenAI;
using Azure.AI.Projects;
using OpenAI.Responses;
using Shared;
using Shared.DTOs;
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

        public async Task<ChatResponseDto> GetResponseAsync(ChatRequestDto request, CancellationToken cancellationToken = default)
        {
            string logEntry = CreateLogEntry("Sending prompt to Azure AI Foundry");

            _logger.LogInformation(logEntry);

            CreateResponseOptions options = CreateClientOptions(request);
            ProjectResponsesClient client = CreateClient();

            var stepper = new TraceStepper(request.TraceSteps);
            stepper.AddStep(logEntry);

            ChatResponseDto responseDto = new();

            ClientResult<ResponseResult> result = await client.CreateResponseAsync(
                options, cancellationToken);

            stepper.AddStep(CreateLogEntry("Received response from Azure AI Foundry"));

            responseDto.Response = result.Value.GetOutputText();
            responseDto.TraceSteps = stepper.Steps;
            responseDto.Stats = new AiResponseStatsDto
            {
                ResponseId = result.Value.Id,
                Model = result.Value.Model,
                InputTokens = result.Value.Usage?.InputTokenCount,
                OutputTokens = result.Value.Usage?.OutputTokenCount,
                TotalTokens = result.Value.Usage?.TotalTokenCount
            };
            return responseDto;


        }

        private string CreateLogEntry(string title)
        {
            return $"{nameof(AzFoundryChatService)}:{nameof(GetResponseAsync)} - {title}";
        }

        private ProjectResponsesClient CreateClient()
        {
            return _projectClient
                .ProjectOpenAIClient
                .GetProjectResponsesClientForModel(_modelName);
        }

        private CreateResponseOptions CreateClientOptions(ChatRequestDto request)
        {
            var systemPrompt = _config["Foundry-Project-01:SystemPrompt"] ?? string.Empty;

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

