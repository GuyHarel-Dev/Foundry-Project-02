using Azure;
using Azure.AI.Extensions.OpenAI;
using Azure.AI.Projects;
using DemoAzAI.Api.Services.Interfaces;
using DemoAzAI.Common;
using DemoAzAI.Contracts.Dtos;
using DemoAzAI.Contracts.Requests;
using DemoAzAI.Contracts.Responses;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OpenAI.Responses;
using System.ClientModel;

#pragma warning disable OPENAI001 // pour using OpenAI.Responses;

namespace DemoAzAI.Api.Services
{
    public class AzFoundryChatService : IAzFoundryChatService
    {
        private readonly IConfiguration _config;
        private readonly AIProjectClient _projectClient;
        private readonly ILogger<AzFoundryChatService> _logger;

        public AzFoundryChatService(
            IConfiguration config,
            AIProjectClient projectClient,
            ILogger<AzFoundryChatService> logger)
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

            //_traceStepper.AddStep(StepTracerTraceTypeEnum.WebApi, "Received response from Azure AI Foundry");

            var response = new ChatResponseDto();

            StepTracer.AddStep($"{nameof(AzFoundryChatService)}", "Sending system prompt and user prompt");

            response.ChatResponse = result.Value.GetOutputText();

            response.ModelStatistics = new ModelStatisticsDto
            {
                Model = result.Value.Model,
                InputTokens = result.Value.Usage?.InputTokenCount,
                OutputTokens = result.Value.Usage?.OutputTokenCount,
                TotalTokens = result.Value.Usage?.TotalTokenCount,
                AnswerLength = response.ChatResponse.Length
            };

            StepTracer.AddStep($"{nameof(AzFoundryChatService)}", "Received model answer");

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

