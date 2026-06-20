using Azure.Core;
using DemoAzAI.AI;
using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain.Entities;
using DemoAzAI.Domain.Models;
using Microsoft.AspNetCore.Components;
using System.Text.Json;

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
                await GetDocumentEmbeddingsAsync();
                StateHasChanged();
            }

        }

        private async Task SearchEmbeddings()
        {

            try
            {
                semanticSearchIsBusy = true;

                if (string.IsNullOrEmpty(documentKnowledge))
                {
                    AddBasePageExecutionError($"Select a document knowledge entity, first", logger);
                    return;
                }

                if (string.IsNullOrEmpty(inputSearchPhrase))
                {
                    AddBasePageExecutionError($"Enter a search phrase", logger);
                    return;
                }

                var phraseEmbedding = await EmbedSearchPhraseAsync(inputSearchPhrase);

                if (phraseEmbedding == null)
                {
                    AddBasePageExecutionError($"Error: No phrase embeddings in context", logger);
                    return;
                }

                if (selectedDocumentKnowledgeEntity == null)
                {
                    AddBasePageExecutionError($"Error: No document embeddings  in context", logger);
                    return;

                }

                if (phraseEmbedding != null && selectedDocumentKnowledgeEntity != null)
                {
                    var test = await ComparePhraseAndDocumentEmbeddings(phraseEmbedding, selectedDocumentKnowledgeEntity);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while analyzing the document.");
            }
            finally
            {
                semanticSearchIsBusy = false;
            }
        }

        private async Task<DocumentKnowledgeEntity> ComparePhraseAndDocumentEmbeddings(
            EmbeddingTextResponseDto phraseEmbedding,
            DocumentKnowledgeEntity documentKnowledge)
        {

            var phraseEmbeddingVector = phraseEmbedding.EmbeddedEntity?.EmbeddingVector;
            var documentEmbeddingsVector = documentKnowledge.Embeddings;

            if (phraseEmbeddingVector != null && documentEmbeddingsVector != null)
            {
                foreach (var embedding in documentEmbeddingsVector)
                {
                    if (embedding.EmbeddingVectorJson != null)
                    {
                        var documentFloats = JsonSerializer.Deserialize<float[]>(embedding.EmbeddingVectorJson);

                        if (documentFloats != null)
                        {
                            embedding.CosineSimilarity = VectorMath.CosineSimilarity(phraseEmbeddingVector, documentFloats);
                        }

                    }

                }
            }

            return documentKnowledge;
        }

        private async Task<EmbeddingTextResponseDto?> EmbedSearchPhraseAsync(string searchPhrase)
        {
            var request = new EmbeddingTextRequestDto
            {
                Text = searchPhrase
            };

            var response = await apiClient.PostAsync<EmbeddingTextRequestDto, EmbeddingTextResponseDto>
                 ("api/embedding/text", request, logger, BasePageLog);

            return response?.Value;
        }


        private async Task
            GetDocumentEmbeddingsAsync()
        {
            try
            {

                var result = await apiClient.GetAsync<IEnumerable<DocumentInfo>>("api/knowledge/embeddings", logger, BasePageLog);

                if (result is not null)
                {
                    storedEmbeddings = result.Value!;
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while getting document embeddings from store.");
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
                selectedDocumentKnowledgeEntity = await GetDocumentEmbeddingByNameAsync(docId);
                documentKnowledge = selectedDocumentKnowledgeEntity?.ToString();
            }

        }

        private async Task<DocumentKnowledgeEntity?> GetDocumentEmbeddingByNameAsync(string docId)
        {
            try
            {
                var result = await apiClient.GetAsync<DocumentKnowledgeEntity>($"api/knowledge/embeddings/{docId}", logger, BasePageLog);

                return result?.Value;
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



    }
}

