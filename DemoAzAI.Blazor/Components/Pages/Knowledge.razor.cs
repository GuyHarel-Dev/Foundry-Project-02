using Azure;
using DemoAzAI.Application;
using DemoAzAI.Blazor.Services.Api;
using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain.Entities;
using DemoAzAI.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace DemoAzAI.Blazor.Components.Pages
{
    public partial class Knowledge
    {

        [Inject]
        ILogger<Knowledge> logger { get; set; } = default!;


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

                var result = await apiClient.GetAsync<IEnumerable<DocumentInfo>>("api/document-analysis", logger, pageLogger);

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
                var entity = await GetDocumentAnalysisByIdAsync(docId);

                documentText = entity?.DocumentAnalysisResult?.DocContent;

            }

        }

        private async Task<DocumentAnalysisEntity?> GetDocumentAnalysisByIdAsync(string docId)
        {
            try
            {
                var result = await apiClient.GetAsync<DocumentAnalysisResponseDto>($"api/document-analysis/id/{docId}", logger, pageLogger);
                
                return result?.Value?.DocumentAnalysisEntity;
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
                    SystemPrompt = """
                    You are a text chunking service. 
                    You analyse the given prompt and extract the semantically relevant name and value pairs.
                    Each name and value pair separated by a colon, on a separe line.
                    Just supply the answer, simple text line, no json, no dialog or explanation
                    """,

                    Prompt = $"{documentText}"
                };

                var result = await apiClient.PostAsync<ChatRequestDto, ChatResponseDto>("api/chat/prompt/response", request, logger);

                documentTextChunk = result?.Value?.ChatResponse;
                documentTextChunkStatistics = result?.Value?.ModelStatistics?.ToString();

                if (documentTextChunk != null && saveDocumentTextChunkToStore)
                {
                    await SaveDocumentTextChunkToStore(documentTextChunk);
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

        private async Task SaveDocumentTextChunkToStore(string documentTextChunk)
        {

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

