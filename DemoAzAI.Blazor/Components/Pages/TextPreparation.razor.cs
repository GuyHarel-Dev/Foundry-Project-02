using DemoAzAI.Application;
using DemoAzAI.Blazor.Services.Api;
using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain.Models.DocumentAnalysis;
using Microsoft.AspNetCore.Components;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        protected override async Task OnInitializedAsync()
        {
            await GetDocumentResultsFromStore();
        }

        private async Task OnDocumentSelected(ChangeEventArgs e)
        {
            if (e.Value == null)
            {
                return;
            }

            var docId = e.Value.ToString();

            DocumentAnalysisText = storedDocuments.First(doc => doc.Id == docId).AnalysisResult.DocumentsFieldsContent;
            DocumentAnalysisTextChunk = string.Empty;
        }

        private async Task SendTextPreparationRequest()
        {
            try
            {

                sendTextPreparationRequestBusy = true;

                var request = new ChatRequestDto()
                {
                    SystemPrompt = "Just supply the text required, no dialog or explanation",
                    Prompt = $"I need the following text to be prepared (chunked) for vectorisation:{DocumentAnalysisText}"
                };

                var result = await apiClient.PostAsync<ChatRequestDto, ChatResponseDto>("api/chat/sendprompt", request);

                if (ApiClientResulIsOk(logger, result))
                {

                    var response = result.Value;

                    DocumentAnalysisTextChunk = response!.ChatResponse;

                    if (response.ModelStatistics != null)
                    {
                        DocumentAnalysisTextChunkStatistics = response.ModelStatistics.ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while getting document results from store.");
            }
            finally
            {
                EndSendTextPreparationProcess();
            }

        }

        private void BeginSendTextPreparationProcess()
        {
            sendTextPreparationRequestBusy = true;
            //AddExecutionStep(executionStepTracer, $"Starting analysis for document {SelectedDocument.FileName} using model {SelectedModel}");
        }

        private void EndSendTextPreparationProcess()
        {
            sendTextPreparationRequestBusy = false;
            //AddExecutionStep(executionStepTracer, $"Analysis completed for document {SelectedDocument.FileName} using model {SelectedModel}");
        }


        private async Task GetDocumentResultsFromStore()
        {
            try
            {

                getDocumentResultsFromStoreIsBusy = true;

                var result = await apiClient.GetAsync<IEnumerable<DocumentAnalysisStoredResult>>("api/store/document/analysis/results");

                if (v(logger, result))
                {
                    storedDocuments = result.Value!.ToList();
                    DocumentAnalysisText = storedDocuments.First().AnalysisResult.DocumentsFieldsContent;
                }

                //if (result == null || result.Value is null)
                //{
                //    Logger.LogWarning($"No document retreived from store");
                //    return;
                //}




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

