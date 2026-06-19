using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain.Entities;
using Microsoft.AspNetCore.Components;

namespace DemoAzAI.Blazor.Components.Pages
{
    public partial class Semantic
    {


        [Inject]
        private ILogger<Semantic> logger { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            await GetDocumentResultsFromStore();
        }

        private async Task SendTextEmbeddingRequest()
        {
            try
            {

                sendTextEmbeddingRequestBusy = true;

                //var request = new EmbeddingRequestDto()
                //{
                //    TextLines = new List<string>()
                //    {
                //        "The cat is sleeping on the sofa",
                //        "A kitten is resting on a couch",
                //        "The dog is playing in the garden",
                //        "The stock market closed higher today",
                //        "Interest rates were increased by the central bank",
                //        "A recipe for chocolate cake requires eggs and flour"
                //    }
                //};

                var request = new EmbeddingTextLinesRequestDto()
                {
                    TextLines = documentAnalysisTextChunk.Split("\n", StringSplitOptions.RemoveEmptyEntries).ToList()
                };


                var result = await apiClient.PostAsync<EmbeddingTextLinesRequestDto, EmbeddingTextResponseDto>("api/text-embedding/embed-lines", request, logger, BasePageLog);

                if (result is not null)
                {
                    documentAnalysisTextChunk = result.Value!.GetSummary();
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

        private async Task OnDocumentSelected(ChangeEventArgs e)
        {
            if (e.Value == null)
            {
                return;
            }

            var docId = e.Value.ToString();

            //DocumentAnalysisText = storedDocuments.First(doc => doc.Id == docId).AnalysisResult.DocumentsFieldsContent;
            //DocumentAnalysisTextChunk = string.Empty;
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
                    //DocumentAnalysisText = storedDocuments.First().AnalysisResult.DocumentsFieldsContent;
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

    }
}

