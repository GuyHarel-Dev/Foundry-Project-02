using DemoAzAI.Blazor.Services.Api;
using DemoAzAI.Contracts.Api.Requests;
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

        private async Task OnDocumentSelected(ChangeEventArgs e)
        {
            if (e.Value == null)
            {
                return;
            }

            var docId = e.Value.ToString();

            DocumentAnalysisText = storedDocuments.First(doc => doc.Id == docId).AnalysisResult.DocumentsFieldsContent;
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

