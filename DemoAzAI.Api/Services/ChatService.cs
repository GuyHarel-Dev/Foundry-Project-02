using Azure.AI.Extensions.OpenAI;
using DemoAzAI.Api.Services.Interfaces;
using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain;
using OpenAI.Responses;
using System.ClientModel;
using WebApi.Extensions;

#pragma warning disable OPENAI001 // pour using OpenAI.Responses;

namespace DemoAzAI.Api.Services
{
    public class ChatService(
            IConfiguration config,
            ProjectResponsesClient projectClient,
            ILogger<ChatService> logger) 
        : IChatService
    {
        public async Task<ChatResponseDto> GetPromptResponseAsync(ChatRequestDto request, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Sending prompt to Azure AI Foundry");

            CreateResponseOptions options = CreateResponseOptions(request);

            ClientResult<ResponseResult> result = await projectClient.CreateResponseAsync(
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

            logger.LogInformation("returning response");

            return response;
        }

        private CreateResponseOptions CreateResponseOptions(ChatRequestDto request)
        {
            var systemPrompt = config["FoundryPrj01:SystemPrompt"] ?? string.Empty;

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

