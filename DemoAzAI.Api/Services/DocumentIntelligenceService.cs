using Azure;
using Azure.AI.DocumentIntelligence;
using DemoAzAI.Abstractions;
using DemoAzAI.Api.Services.Interfaces;
using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain.Models.DocumentAnalysis;
using WebApi.Extensions;

namespace DemoAzAI.Api.Services
{
    public class DocumentIntelligenceService(
            DocumentIntelligenceClient client,
            IDocumentAnalysisStoreService documentAnalysisStoreService,
            ILogger<DocumentIntelligenceService> logger)
        : IDocumentIntelligenceService
    {
        public async Task<DocumentAnalysisResponseDto> AnalyzeDocumentAsync(DocumentAnalysisRequestDto request, CancellationToken cancellationToken = default)
        {
            var documentHash = request.BytesHash;

            if (await DocumentExistsInStore(documentHash))
                return DocumentExistsResponse();

            logger.LogInformation("Sending document to analysis model");

            var result = await SendAnalyseDocumentRequestAsync(request, cancellationToken);

            if (request.AddDocumentToStore)
                return await StoreDocumentAsync(documentHash, result);

            logger.LogInformation("Returning response");

            return result;
        }

        private AnalyzeDocumentOptions CreateAnalyzeDocumentOptions(DocumentAnalysisRequestDto doc)
        {
            var modelName = doc.ModelName;
            var stream = new MemoryStream(doc.Bytes);
            var bytes = BinaryData.FromStream(stream);

            return new AnalyzeDocumentOptions(
                modelName,
                bytes);
        }

        private async Task<bool> DocumentExistsInStore(string documentHash)
        {
            return documentAnalysisStoreService.ExistsAsync(documentHash).GetAwaiter().GetResult();
        }

        private DocumentAnalysisResponseDto DocumentExistsResponse()
        {
            var response = new DocumentAnalysisResponseDto
            {
                Status = "Document already exists in store"
            };
            logger.LogWarning(response.Status);
            return response;
        }

        private async Task<DocumentAnalysisResponseDto> SendAnalyseDocumentRequestAsync(DocumentAnalysisRequestDto request, CancellationToken cancellationToken)
        {
            var options = CreateAnalyzeDocumentOptions(request);

            Operation<AnalyzeResult> operation =
                await client.AnalyzeDocumentAsync(
                    WaitUntil.Completed,
                    options,
                    cancellationToken);

            var docAnalysisResult = Map(request.ModelName, operation.Value);

            var response = new DocumentAnalysisResponseDto
            {
                DocumentAnalysisResult = docAnalysisResult
            };

            response.Status = "Document analyzed";
            logger.LogInformation(response.Status);

            return response;
        }

        private async Task<DocumentAnalysisResponseDto> StoreDocumentAsync(string documentHash, DocumentAnalysisResponseDto response)
        {
            if (response.DocumentAnalysisResult == null)
            {
                response.Status = "No document analysis result to store";
                logger.LogWarning($"{response.Status}");
                return response;
            }

            var status = await documentAnalysisStoreService.SaveAsync(documentHash, response.DocumentAnalysisResult);

            response.Status = "Document added to store";
            logger.LogWarning($"{response.Status}");

            return response;
        }

        private DocumentAnalysisResult Map(string modelName, AnalyzeResult azureAnalysisResult)
        {
            DocumentAnalysisResult result = new();

            result.ModelName = modelName;
            result.ApiVersion = azureAnalysisResult.ApiVersion;

            result.DocContent = azureAnalysisResult.Content;
            result.DocFormat = azureAnalysisResult.ContentFormat.ToString() ?? "unknown";

            result.PageCount = azureAnalysisResult.Pages.Count;
            result.ParagraphCount = azureAnalysisResult.Paragraphs?.Count ?? 0;
            result.SectionCount = azureAnalysisResult.Sections?.Count ?? 0;
            result.TableCount = azureAnalysisResult.Tables?.Count ?? 0;
            result.FigureCount = azureAnalysisResult.Figures?.Count ?? 0;
            result.LanguagesCount = azureAnalysisResult.Languages?.Count ?? 0;


            if (azureAnalysisResult.Documents != null)
            {
                foreach (var document in azureAnalysisResult.Documents)
                {
                    DocumentAnalyzed doc = new()
                    {
                        DocumentType = document.DocumentType,
                        DocumentTypeConfidence = document.Confidence

                    };

                    foreach (var fields in document.Fields)
                    {
                        doc.DocumentFields.Add(new DocumentAnalyzedFields
                        {
                            Name = fields.Key,
                            Value = fields.Value.Content,
                            FieldType = fields.Value.FieldType.ToString(),
                            FieldTypeConfidence = fields.Value.Confidence

                        });
                    }

                    result.Documents.Add(doc);
                }

            }
            return result;
        }

    }
}
