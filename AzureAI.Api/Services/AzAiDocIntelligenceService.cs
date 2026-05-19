using Azure;
using Azure.AI.DocumentIntelligence;
using Azure.Core;
using AzureAI.Api.Services.Interfaces;
using AzureAI.Common;
using AzureAI.Contracts.Dtos;
using AzureAI.Contracts.Requests;
using AzureAI.Contracts.Responses;

namespace AzureAI.Api.Services
{
    public class AzAiDocIntelligenceService : IAzAiDocIntelligenceService
    {
        private readonly IConfiguration _config;
        private readonly DocumentIntelligenceClient _client;
        private readonly ILogger<AzAiDocIntelligenceService> _logger;

        public AzAiDocIntelligenceService(
            DocumentIntelligenceClient client,
            IConfiguration config,
            ILogger<AzAiDocIntelligenceService> logger)
        {
            _client = client;
            _config = config;
            _logger = logger;
        }

        public async Task<AnalyzeDocumentResponseDto> AnalyzeDocument(AnalyzeDocumentRequestDto request, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Sending document to Azure AI Foundry");

            var response = new AnalyzeDocumentResponseDto();

            response.Steps.AddRange(request.Steps);
            response.Steps.Add(new StepTracerStepDto
            {
                StepTitle = "step 2",
                StepDateTime = DateTime.UtcNow
            });

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
                    var docDto = new DocumentDto
                    {
                        DocumentType = document.DocumentType,
                        Confidence = document.Confidence
                    };

                    foreach (var field in document.Fields)
                    {
                        docDto.Fields.Add(new DocumentFieldDto
                        {
                            Name = field.Key,

                            Value = field.Value.Content,

                            FieldType = field.Value.FieldType.ToString(),

                            Confidence = field.Value.Confidence
                        });
                    }

                    response.Documents.Add(docDto);
                }

            }
        }
    }
}
