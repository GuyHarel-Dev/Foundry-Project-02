using Azure;
using DemoAzAI.Application;
using DemoAzAI.Blazor.Services.Api;
using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain.Entities;
using Microsoft.AspNetCore.Components;

namespace DemoAzAI.Blazor.Components.Pages
{
    public partial class SemanticAnalysis
    {
        [Inject]
        private ApiClient apiClient { get; set; } = default!;

        [Inject]
        private IExecutionStepTracer executionStepTracer { get; set; } = default!;

        [Inject]
        private ILogger<SemanticAnalysis> logger { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            await GetDocumentResultsFromStore();
        }

        private async Task SendTextEmbeddingRequest()
        {
            try
            {

                sendTextEmbeddingRequestBusy = true;

                var request = new EmbeddingRequestDto()
                {
                    TextLines = new List<string>()
                    {
                        "The cat is sleeping on the sofa",
                        "A kitten is resting on a couch",
                        "The dog is playing in the garden",
                        "The stock market closed higher today",
                        "Interest rates were increased by the central bank",
                        "A recipe for chocolate cake requires eggs and flour"
                    }
                };

                var result = await apiClient.PostAsync<EmbeddingRequestDto, EmbeddingResponseDto>("api/embedding/embed/textlines", request, logger);

                if (result is not null)
                {
                    DocumentAnalysisTextChunkVectorized = result.Value!.GetSummary();
                }

  
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error while getting document results from store.");
            }
            finally
            {
                sendTextEmbeddingRequestBusy = false;
            }

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

                var result = await apiClient.PostAsync<ChatRequestDto, ChatResponseDto>("api/chat/prompt/response", request, logger);

                if (result is not null)
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
                Logger.LogError(ex, "Error while getting document results from store.");
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

        private void BeginSendTextEmbeddingProcess()
        {
            sendTextEmbeddingRequestBusy = true;
            //AddExecutionStep(executionStepTracer, $"Starting analysis for document {SelectedDocument.FileName} using model {SelectedModel}");
        }

        private void EndSendTextEmbeddingProcess()
        {
            sendTextEmbeddingRequestBusy = false;
            //AddExecutionStep(executionStepTracer, $"Analysis completed for document {SelectedDocument.FileName} using model {SelectedModel}");
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

        private async Task GetDocumentResultsFromStore()
        {
            try
            {

                getDocumentResultsFromStoreIsBusy = true;

                var result = await apiClient.GetAsync<IEnumerable<DocumentAnalysisEntity>>("api/store/document/analysis/results", logger);

                if (result is not null) 
                {
                    storedDocuments = result.Value!.ToList();
                    DocumentAnalysisText = storedDocuments.First().AnalysisResult.DocumentsFieldsContent;
                }
    

                }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error while getting document results from store.");
            }
            finally
            {
                getDocumentResultsFromStoreIsBusy = false;
            }

        }

        private async Task SemanticSearch()
        {
            //if (string.IsNullOrWhiteSpace(semanticQuestion))
            //{
            //    semanticQuestion = "Please enter a semantic question...";
            //    return;
            //}

            //searchSteps = AddUiStepperStep(StepTracer, "BlazorUi", $"{nameof(SemanticSearch)}: Searching embedding store...");

            //var request = new EmbeddingRequestDto
            //{
            //    AddEmbeddingsToStore = false,
            //    TextLines = semanticQuestion
            //        .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
            //        .ToList()
            //};

            // api/embedding/search/store


            await RefreshUIAsync();
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





        private static string TextToEmbedDefault = @"Cats are playful pets
Dogs are loyal animals
Pizza is delicious
Machine learning uses vectors";
    }
}

