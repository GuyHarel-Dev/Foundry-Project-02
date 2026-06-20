using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain.Entities;
using DemoAzAI.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace DemoAzAI.Blazor.Components.Pages
{
    public partial class Semantic
    {


        [Inject]
        private ILogger<Semantic> logger { get; set; } = default!;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await GetDocumentAnalysisAsync();
                StateHasChanged();
            }

        }

        private async Task GetDocumentAnalysisAsync()
        {
            try
            {

                var result = await apiClient.GetAsync<IEnumerable<DocumentInfo>>("api/document/analysis", logger, BasePageLog);

                if (result is not null)
                {
                    //storedDocuments = result.Value!.ToList();
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while getting document results from store.");
            }
            finally
            {

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
                //selectedDocumentAnalysisEntity = await GetDocumentAnalysisByIdAsync(docId);
            }

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

