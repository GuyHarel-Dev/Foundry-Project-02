using Azure;
using Azure.AI.DocumentIntelligence;
using DemoAzAI.Abstractions;
using DemoAzAI.Api.Services.Interfaces;
using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain;
using WebApi.Extensions;

namespace DemoAzAI.Api.Services
{
    public class DocumentIntelligenceService(
            DocumentIntelligenceClient client,
            IDocumentAnalysisStoreService documentAnalysisStoreService,
            ILogger<DocumentIntelligenceService> logger) 
        : IDocumentIntelligenceService
    {

        public async Task<DocumentAnalysisResponseDto> AnalyzeDocument(DocumentAnalysisRequestDto request, CancellationToken cancellationToken = default)
        {
            logger.LogInformationWithUtc("Sending document to Azure AI Foundry");

            var options = CreateDocumentAnalysisOptions(request);

            Operation<AnalyzeResult> operation =
                await client.AnalyzeDocumentAsync(
                    WaitUntil.Completed,
                    options,
                    cancellationToken);

            var docAnalysisResult = Map(operation.Value);

            //if (request.AddDocumentToStore)
            //    AddDocumentToStore(request, response);

            logger.LogInformationWithUtc("Returning response");

            return new DocumentAnalysisResponseDto { DocumentAnalysisResult = docAnalysisResult };
        }

        //private string AddDocumentToStore(DocumenteAnalysisResult result)
        //{
        //    return "abc"; // documentAnalysisStoreService.SaveAsync(request.Bytes, doc);
        //}

        private AnalyzeDocumentOptions CreateDocumentAnalysisOptions(DocumentAnalysisRequestDto doc)
        {
            var modelName = doc.ModelName;
            var stream = new MemoryStream(doc.Bytes);
            var bytes = BinaryData.FromStream(stream);

            return new AnalyzeDocumentOptions(
                modelName,
                bytes);
        }

        private DocumentAnalysisResult Map(AnalyzeResult azureAnalysisResult)
        {
            DocumentAnalysisResult result = new();

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
                foreach(var document in azureAnalysisResult.Documents)
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
