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

        private async Task GetDocumentResultsFromStore()
        {
            var stepSource = $"UI SemanticAnalysis:{nameof(GetDocumentResultsFromStore)}";
            try
            {
                StepTracer.ClearSteps();

                StepTracer.AddUiStep("message");


                getDocumentResultsFromStoreIsBusy = true;

                StepTracer.AddStep($"{stepSource}:", $"Getting document analysis results from store");

                var result = await apiClient.GetAsync<IEnumerable<DocumentAnalysisStoredResult>>("api/store/document/analysis/results");

                if (result == null || result.Value is null)
                {
                    Logger.LogWarning($"No document retreived from store");
                    return;
                }

                storedDocuments = result.Value.ToList();

                StepTracer.AddStep($"{stepSource}:", $"Done getting document analysis results from store");

            }
            catch (Exception ex)
            {
                StepTracer.AddStep($"{stepSource}:", ex.Message);

                Logger.LogError(ex, "Error while getting document results from store.");
            }
            finally
            {
                getDocumentResultsFromStoreIsBusy = false;
            }

            executionSteps = StepTracer.GetSteps();
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

