using Azure.AI.Extensions.OpenAI;
using Azure.AI.Projects;
using AzureAI.Common;
using AzureAI.Contracts.Dtos;
using AzureAI.Contracts.Requests;
using AzureAI.Contracts.Responses;
using OpenAI.Responses;
using System.ClientModel;
using WebApi.Services.Interfaces;

#pragma warning disable OPENAI001 // pour using OpenAI.Responses;

namespace AzureAI.Api.Services
{
    public class AzFoundryEmbeddingService : IAzFoundryEmbeddingService

    {
        private readonly IConfiguration _config;
        private readonly AIProjectClient _projectClient;
        private readonly ILogger<AzFoundryEmbeddingService> _logger;

        public AzFoundryEmbeddingService(
            IConfiguration config,
            AIProjectClient projectClient,
            ILogger<AzFoundryEmbeddingService> logger)
        {
            _config = config;
            _projectClient = projectClient;
            _logger = logger;

        }


        public async Task<EmbeddingResponseDto> GetEmbeddingAsync(EmbeddingRequestDto request, CancellationToken cancellationToken = default)
        {
            ProjectResponsesClient client = CreateResponseClient();

            //var result = await client.CreateResponseAsync("user text", "previous resp id", cancellationToken);

            var response = new EmbeddingResponseDto { RequestId = "123" };

            return response;
        }

        //        public async Task<EmbeddingResponseDto> GetPromptResponseAsync(EmbeddingRequestDto request, CancellationToken cancellationToken = default)
        //        {
        //            _logger.LogInformation("Sending text do embed to Azure AI Foundry");

        //            //CreateResponseOptions options = CreateResponseOptions(request);
        //            ProjectResponsesClient client = CreateResponseClient();

        //            ClientResult<ResponseResult> result = await client.CreateResponseAsync("user text", "previous resp id", cancellationToken);

        //            var response = new EmbeddingResponseDto();



        ///*            response.ModelStatistics = new ModelStatisticsDto
        //            {
        //                Model = result.Value.Model,
        //                InputTokens = result.Value.Usage?.InputTokenCount,
        //                OutputTokens = result.Value.Usage?.OutputTokenCount,
        //                TotalTokens = result.Value.Usage?.TotalTokenCount,
        //                AnswerLength = response.ChatResponse.Length
        //            };

        //            StepTracer.AddStep(response.Steps, $"{nameof(AzFoundryChatService)}", "Received model answer");*/

        //            return response;
        //        }

        private ProjectResponsesClient CreateResponseClient()
        {

            var modelName = _config["FoundryPrj01:Model:TextEmbedding"]
                ?? throw new InvalidOperationException("ModelName not configured");


            return _projectClient
                .ProjectOpenAIClient
                .GetProjectResponsesClientForModel(modelName);
        }

/*        private CreateResponseOptions CreateResponseOptions(ChatRequestDto request)
        {
            var systemPrompt = _config["FoundryPrj01:SystemPrompt"] ?? string.Empty;

            var options = new CreateResponseOptions
            {
                Instructions = systemPrompt
            };

            options.InputItems.Add(
                ResponseItem.CreateUserMessageItem(request.Prompt));
            return options;
        }*/
    }
}

