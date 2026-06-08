using DemoAzAI.Blazor.Services.Api;
using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain.Models.DocumentAnalysis;
using Microsoft.AspNetCore.Components;

namespace DemoAzAI.Blazor.Components.Pages
{
    public partial class SemanticAnalysis
    {
        [Inject]
        private ApiClient apiClient { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            await GetDocumentResultsFromStore();
        }

        private async Task SendTextEmbeddingRequest()
        {
            var textLines = DocumentAnalysisTextChunk;

            //var textLinesSeperated = DocumentAnalysisTextChunk.Split("\n");

            var textLinesSeperated = new string[] { "aa", "bb" };

            try
            {

                sendTextPreparationRequestBusy = true;

                var request = new EmbeddingRequestDto()
                {
                    TextLines = textLinesSeperated.ToList()
                };

                var result = await apiClient.PostAsync<EmbeddingRequestDto, string>("api/embedding/embed/textlines", request);
                if (result == null || result.Value is null)
                {
                    Logger.LogWarning($"Document chunking request returned no value");
                    return;
                }

                if (!result.IsSuccess)
                {
                    executionSteps = result.ErrorMessage ?? "Unknown API error.";
                    return;
                }

                var response = result.Value;



            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error while getting document results from store.");
            }
            finally
            {
                sendTextPreparationRequestBusy = false;
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

                var result = await apiClient.PostAsync<ChatRequestDto, ChatResponseDto>("api/chat/sendprompt", request);
                if (result == null || result.Value is null)
                {
                    Logger.LogWarning($"Document chunking request returned no value");
                    return;
                }

                if (!result.IsSuccess)
                {
                    executionSteps = result.ErrorMessage ?? "Unknown API error.";
                    return;
                }

                var response = result.Value;

                DocumentAnalysisTextChunk = response.ChatResponse;

                if (response.ModelStatistics != null)
                {
                    DocumentAnalysisTextChunkStatistics = response.ModelStatistics.ToString(); 
                }

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error while getting document results from store.");
            }
            finally
            {
                sendTextPreparationRequestBusy = false;
            }

        }

        private async Task OnDocumentSelected(ChangeEventArgs e)
        {
            if (e.Value == null)
            {
                return;
            }

            var docId = e.Value.ToString();

            DocumentAnalysisText = storedDocuments.First(doc => docId == docId).AnalysisResult.DocumentsFieldsContent;
            DocumentAnalysisTextChunk = string.Empty;
        }

        private async Task GetDocumentResultsFromStore()
        {
            try
            {

                getDocumentResultsFromStoreIsBusy = true;

                var result = await apiClient.GetAsync<IEnumerable<DocumentAnalysisStoredResult>>("api/store/document/analysis/results");

                if (result == null || result.Value is null)
                {
                    Logger.LogWarning($"No document retreived from store");
                    return;
                }

                storedDocuments = result.Value.ToList();
                DocumentAnalysisText = storedDocuments.First().AnalysisResult.DocumentsFieldsContent;


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
            if (string.IsNullOrWhiteSpace(semanticQuestion))
            {
                semanticQuestion = "Please enter a semantic question...";
                return;
            }

            searchSteps = AddUiStepperStep(StepTracer, "BlazorUi", $"{nameof(SemanticSearch)}: Searching embedding store...");

            var request = new EmbeddingRequestDto
            {
                AddEmbeddingsToStore = false,
                TextLines = semanticQuestion
                    .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList()
            };

            // api/embedding/search/store


            await RefreshUI();
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

