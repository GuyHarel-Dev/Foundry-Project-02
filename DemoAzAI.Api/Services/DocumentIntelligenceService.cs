using Azure;
using Azure.AI.DocumentIntelligence;
using DemoAzAI.Api.Services.Interfaces;
using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain.Models.DocumentAnalysis;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DemoAzAI.Api.Services
{
    public class DocumentIntelligenceService(
            DocumentIntelligenceClient client,
            ILogger<DocumentIntelligenceService> logger)
        : IDocumentIntelligenceService
    {
        public async Task<DocumentAnalysisResponseDto> AnalyzeDocumentAsync(DocumentAnalysisRequestDto request, CancellationToken cancellationToken = default)
        {
            //var documentHash = request.BytesHash;

            //logger.LogInformation($"Sending document {request.FileName} to analysis model");

            var result = await SendAnalyseDocumentRequestAsync(request, cancellationToken);

            logger.LogInformation($"Returning response");

            return result;
        }

        private AnalyzeDocumentOptions CreateAnalyzeDocumentOptions(DocumentAnalysisRequestDto doc)
        {
            //var modelName = doc.ModelName;
            //var stream = new MemoryStream(doc.Bytes);
            //var bytes = BinaryData.FromStream(stream);

            //return new AnalyzeDocumentOptions(
            //    modelName,
            //    bytes);

            byte[] test = new byte[] { 113 };
            return new AnalyzeDocumentOptions("", new Uri(""));
        }

        private async Task<DocumentAnalysisResponseDto> SendAnalyseDocumentRequestAsync(DocumentAnalysisRequestDto request, CancellationToken cancellationToken)
        {
            //var options = CreateAnalyzeDocumentOptions(request);

            //Operation<AnalyzeResult> operation =
            //    await client.AnalyzeDocumentAsync(
            //        WaitUntil.Completed,
            //        options,
            //        cancellationToken);

            //var docAnalysisResult = Map(request.ModelName, operation.Value);

            //var response = new DocumentAnalysisResponseDto
            //{
            //    FileName = request.FileName,
            //    DocumentAnalysisResult = docAnalysisResult
            //};

            //return response;

            return new DocumentAnalysisResponseDto();
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
                var sb = new StringBuilder();

                foreach (var document in azureAnalysisResult.Documents)
                {
                    sb.AppendLine($"DocumentType:{document.DocumentType}");
                    sb.AppendLine($"Confidence:{document.Confidence}");
                    sb.AppendLine("");

                    foreach (var field in document.Fields)
                    {
                        var fieldName = field.ToString();
                        var fieldValues = JsonSerializer.Serialize(field, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, WriteIndented = true });

                        sb.AppendLine($"fieldName:{fieldName}");
                        sb.AppendLine($"fieldValues:{fieldValues}");

                        sb.AppendLine("");
                    }

                }

                result.DocumentsFieldsContent = sb.ToString();
            }
            return result;
        }


    }
}


