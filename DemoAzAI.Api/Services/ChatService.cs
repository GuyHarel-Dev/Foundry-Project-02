using Azure.AI.Extensions.OpenAI;
using DemoAzAI.Api.Services.Interfaces;
using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain.Entities;
using DemoAzAI.Domain.Models;
using DemoAzAI.Infrastructure.ExternalServices.Repositories.Interfaces;
using OpenAI.Responses;
using System.ClientModel;

#pragma warning disable OPENAI001 // pour using OpenAI.Responses;

namespace DemoAzAI.Api.Services
{
    public class ChatService(
            ProjectResponsesClient projectClient,
            IDocumentChunksRepository documentChunksRepository,
            ILogger<ChatService> logger) 
        : IChatService
    {
        public async Task<DocumentChunkingResponseDto> GetDocumentChunkingResponseAsync(DocumentChunkingRequestDto request, CancellationToken cancellationToken = default)
        {
           var chatResponse = await GetPromptResponseAsync(request, cancellationToken);

            if (request.SaveToStore)
            {
                var chunks = new DocumentChunkEntity
                {
                    FileName = request.FileName,
                    TextChunks = request.TextChunks
                };

                var status = await documentChunksRepository.SaveAsync(chunks);
            }

            return new DocumentChunkingResponseDto
            {
                ChatResponse = chatResponse.ChatResponse,
                ModelStatistics = chatResponse.ModelStatistics
            };
        }

        public async Task<ChatQuestionResponseDto> GetPromptResponseAsync(ChatQuestionRequestDto request, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Sending prompt to Azure AI Foundry");

            CreateResponseOptions options = CreateResponseOptions(request);

            ClientResult<ResponseResult> result = await projectClient.CreateResponseAsync(
                options, cancellationToken);

            var response = new ChatQuestionResponseDto();

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

        private CreateResponseOptions CreateResponseOptions(ChatQuestionRequestDto request)
        {
            var options = new CreateResponseOptions
            {
                Instructions = request.SystemPrompt
            };

            options.InputItems.Add(
                ResponseItem.CreateUserMessageItem(request.Prompt));
            return options;
        }
    }
}

