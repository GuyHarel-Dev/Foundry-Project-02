using Azure;
using Azure.AI.DocumentIntelligence;
using Azure.Core;
using DemoAzAI.Api.Services.Interfaces;
using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain.Models.DocumentAnalysis;
using DemoAzAI.Infrastructure.ExternalServices.Repositories;
using DemoAzAI.Infrastructure.ExternalServices.Stores;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DemoAzAI.Api.Services
{
    public class DocumentAnalysisService(
            DocumentIntelligenceClient client,
            IDocumentStoreService fileStoreService,
            IDocumentAnalysisRepository documentAnalysisRepository,
            ILogger<DocumentAnalysisService> logger)
        : IDocumentAnalysisService
    {
        public async Task<DocumentAnalysisResponseDto> AnalyzeDocumentAsync(DocumentAnalysisRequestDto request, CancellationToken cancellationToken = default)
        {

            logger.LogInformation($"Sending document {request.FileName} to analysis model");

            var options = await CreateAnalyzeDocumentOptions(request);
            var docHash = Convert.ToHexString(options.BytesSource);

            if (request.SaveToStore && await documentAnalysisRepository.ExistsAsync(docHash))
            {
                return new DocumentAnalysisResponseDto { DocumentAnalysisStatus = "Document was not store: already exists" };
            }

            Operation<AnalyzeResult> operation =
                await client.AnalyzeDocumentAsync(
                    WaitUntil.Completed,
                    options,
                    cancellationToken);

            var docAnalysisResult = Map(request.ModelName, operation.Value);

            var response = new DocumentAnalysisResponseDto
            {
                FileName = request.FileName,
                DocumentAnalysisResult = docAnalysisResult,
                BytesHash = docHash
            };


            logger.LogInformation($"Response received response");

            if (request.SaveToStore)
            {
                await documentAnalysisRepository.SaveAsync(request.FileName, response.DocumentAnalysisResult, response.BytesHash);
            }

            return response;

        }

        private async Task<AnalyzeDocumentOptions> CreateAnalyzeDocumentOptions(DocumentAnalysisRequestDto request)
        {
            var modelName = request.ModelName;
            var fileStream = await fileStoreService.GetDocumentFileStreamAsync(request.FileName);
            var bytes = BinaryData.FromStream(fileStream);

            return new AnalyzeDocumentOptions(
                modelName,
                bytes);
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


