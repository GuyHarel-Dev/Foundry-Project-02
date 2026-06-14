using DemoAzAI.Application;
using DemoAzAI.Blazor.Services.Api;
using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain.Entities;
using DemoAzAI.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace DemoAzAI.Blazor.Components.Pages
{
    public partial class TextPreparation
    {

        [Inject]
        private ApiClient apiClient { get; set; } = default!;

        [Inject]
        private IExecutionStepTracer executionStepTracer { get; set; } = default!;

        [Inject]
        ILogger<TextPreparation> logger { get; set; } = default!;


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await GetDocumentResultsAsync();
                StateHasChanged();
            }

        }

        private async Task GetDocumentResultsAsync()
        {
            try
            {

                getDocumentResultsFromStoreIsBusy = true;

                var result = await apiClient.GetAsync<IEnumerable<DocumentInfo>>("api/document/analysis", logger);

                if (result is not null)
                {
                    storedDocuments = result.Value!.ToList();
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while getting document results from store.");
            }
            finally
            {
                getDocumentResultsFromStoreIsBusy = false;
            }


        }

        private async Task OnDocumentSelected(ChangeEventArgs e)
        {
            if (e.Value == null)
            {
                return;
            }

            var docId = e.Value.ToString();

            if (docId != null)
            {
                var doc = await GetDocumentAnalysisByIdAsync(docId);

                documentAnalysisText = doc?.AnalysisResult.DocContent;

            }

        }

        private async Task<DocumentAnalysisEntity?> GetDocumentAnalysisByIdAsync(string docId)
        {
            try
            {
                var result = await apiClient.GetAsync<DocumentAnalysisEntity>($"api/document/analysis/{docId}", logger);

                if (result is not null)
                {
                    var response = result.Value;

                    return response;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while getting document analysis from store.");
            }
            finally
            {

            }

            return null;
        }

        private async Task SendTextPreparationRequest()
        {
            try
            {

                sendTextPreparationRequestBusy = true;

                var request = new ChatRequestDto()
                {
                    SystemPrompt = "Just supply the text required, no dialog or explanation",
                    Prompt = $"I need the following text to be prepared (chunked) for vectorisation:{documentAnalysisText}"
                };

                var result = await apiClient.PostAsync<ChatRequestDto, ChatResponseDto>("api/chat/prompt/response", request, logger);

                if (result is not null)
                {
                    var response = result.Value;

                    documentAnalysisTextChunk = response!.ChatResponse;

                    if (response.ModelStatistics != null)
                    {
                        documentAnalysisTextChunkStatistics = response.ModelStatistics.ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while getting document results from store.");
            }
            finally
            {
                sendTextPreparationRequestBusy = false;
            }

        }




        private async Task EmbedTextLines()
        {

            //try
            //{
            //    isBusy = true;

            //    embeddedText = string.Empty;
            //    executionSteps = string.Empty;

            //    executionSteps = AddUiStepperStep(StepTracer, "BlazorUi", $"{nameof(EmbedTextLines)}: Starting embedding text...", clearSteps: true);

            //    await RefreshUI();

            //    List<string> lines = textToEmbed
            //        .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
            //        .ToList();

            //    var request = new EmbeddingRequestDto
            //    {
            //        AddEmbeddingsToStore = true,
            //        TextLines = lines
            //    };

            //    var result = await apiClient.PostAsync<EmbeddingRequestDto, EmbeddingResponseDto>("api/embedding/embed/chunks", request);

            //    if (!result.IsSuccess)
            //    {
            //        executionSteps = "API returned no data.";
            //        Logger.LogInformation(result.ErrorMessage);
            //        return;

            //    }

            //}
            //catch (Exception ex)
            //{
            //    Logger.LogError(ex, "Error while embedding text lines.");
            //}
            //finally
            //{
            //    isBusy = false;
            //}

        }



    }


}

