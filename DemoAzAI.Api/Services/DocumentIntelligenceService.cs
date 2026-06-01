using Azure;
using Azure.AI.DocumentIntelligence;
using DemoAzAI.Api.Services.Interfaces;
using DemoAzAI.Application.Extensions;
using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain;
using WebApi.Extensions;

namespace DemoAzAI.Api.Services
{
    public class DocumentIntelligenceService(
            DocumentIntelligenceClient client,
            ILogger<DocumentIntelligenceService> logger) 
        : IDocumentIntelligenceService
    {

        public async Task<DocumentAnalysisResponseDto> AnalyzeDocument(DocumenteAnalysisRequestDto request, CancellationToken cancellationToken = default)
        {
            logger.LogInformationWithUtc("Sending document to Azure AI Foundry");

            var options = CreateDocumentOptions(request);

            Operation<AnalyzeResult> operation =
                await client.AnalyzeDocumentAsync(
                    WaitUntil.Completed,
                    options,
                    cancellationToken);

            DocumentAnalysisResponseDto response = new();

            ExtractBaseInformation(operation.Value, response);
            ExtractDocumentsFields(operation.Value, response);

            if (request.AddDocumentToStore)
                AddDocumentToStore(request, response);

            logger.LogInformationWithUtc("Returning response");

            return response;
        }

        private string AddDocumentToStore(DocumenteAnalysisRequestDto request, DocumentAnalysisResponseDto response)
        {
            var hash = request.Bytes.ComputeHash();
            // store document

            response.DocumentHash = hash;

            return hash;
        }

        private AnalyzeDocumentOptions CreateDocumentOptions(DocumenteAnalysisRequestDto doc)
        {
            var modelName = doc.ModelName;
            var stream = new MemoryStream(doc.Bytes);
            var bytes = BinaryData.FromStream(stream);

            return new AnalyzeDocumentOptions(
                modelName,
                bytes);
        }

        private void ExtractBaseInformation(AnalyzeResult result, DocumentAnalysisResponseDto response)
        {
            response.DocContent = result.Content;
            response.DocFormat = result.ContentFormat;
            response.PageCount = result.Pages.Count;
            response.ParagraphCount = result.Paragraphs?.Count ?? 0;
            response.SectionCount = result.Sections?.Count ?? 0;
            response.TableCount = result.Tables?.Count ?? 0;
            response.FigureCount = result.Figures?.Count ?? 0;
            response.LanguagesCount = result.Languages?.Count ?? 0;
        }

        private static void ExtractDocumentsFields(AnalyzeResult result, DocumentAnalysisResponseDto response)
        {
            if (result.Documents != null)
            {
                foreach (var document in result.Documents)
                {
                    var docDto = new DocumentAnalysisResult
					{
                        DocumentType = document.DocumentType,
                        Confidence = document.Confidence
                    };

                    foreach (var field in document.Fields)
                    {
                        docDto.Fields.Add(new DocumentAnalysisResultField
                        (
                            field.Key,
                            field.Value.Content,
							 field.Value.FieldType.ToString(),
							field.Value.Confidence ?? 0
                        ));

					}

                    response.Documents.Add(docDto);
                }

            }
        }
    }
}
