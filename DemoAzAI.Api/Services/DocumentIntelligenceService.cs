using Azure;
using Azure.AI.DocumentIntelligence;
using DemoAzAI.Api.Contracts.Requests;
using DemoAzAI.Api.Contracts.Responses;
using DemoAzAI.Api.Services.Interfaces;
using DemoAzAI.Domain;

namespace DemoAzAI.Api.Services
{
    public class DocumentIntelligenceService : IDocumentIntelligenceService
    {
        private readonly IConfiguration _config;
        private readonly DocumentIntelligenceClient _client;
        private readonly ILogger<DocumentIntelligenceService> _logger;

        public DocumentIntelligenceService(
            DocumentIntelligenceClient client,
            IConfiguration config,
            ILogger<DocumentIntelligenceService> logger)
        {
            _client = client;
            _config = config;
            _logger = logger;
        }

        public async Task<AnalyzeDocumentResponseDto> AnalyzeDocument(AnalyzeDocumentRequestDto request, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Sending document to Azure AI Foundry");

            var response = new AnalyzeDocumentResponseDto();

            var options = CreateDocumentOptions(request);

            var operation =
                await _client.AnalyzeDocumentAsync(
                    WaitUntil.Completed,
                    options,
                    cancellationToken);

            AnalyzeResult result = operation.Value;

            ExtractBaseInformation(operation.Value, response);
            ExtractDocumentsFields(result, response);


            return response;
        }

        private AnalyzeDocumentOptions CreateDocumentOptions(AnalyzeDocumentRequestDto doc)
        {
            var modelName = doc.ModelName;
            var stream = new MemoryStream(doc.Bytes);
            var bytes = BinaryData.FromStream(stream);

            return new AnalyzeDocumentOptions(
                modelName,
                bytes);
        }

        private void ExtractBaseInformation(AnalyzeResult result, AnalyzeDocumentResponseDto response)
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

        private static void ExtractDocumentsFields(AnalyzeResult result, AnalyzeDocumentResponseDto response)
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
