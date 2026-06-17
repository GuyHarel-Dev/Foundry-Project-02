using Azure;
using Azure.AI.DocumentIntelligence;
using DemoAzAI.Api.Services.Interfaces;
using DemoAzAI.Domain.Entities;
using DemoAzAI.Domain.Models;
using DemoAzAI.Infrastructure.ExternalServices.Repositories.Interfaces;
using DemoAzAI.Infrastructure.ExternalServices.Stores;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DemoAzAI.Api.Services
{
    public class DocumentService(
            DocumentIntelligenceClient client,
            IDocumentStoreService fileStoreService,
            IDocumentAnalysisRepository documentAnalysisRepository,
            IDocumentChunksRepository documentChunksRepository,
            ILogger<DocumentService> logger)
        : IDocumentService
    {

        public async Task<DocumentChunkEntity?> GetDocumentChunksByNameAsync(string fileName)
        {
            return await documentChunksRepository.GetDocumentChunksAsync(fileName);
        }

        public async Task<DocumentAnalysisEntity?> GetDocumentAnalysisByIdAsync(string id)
        {
            return await documentAnalysisRepository.GetDocumentAnalysisByIdAsync(id);
        }

        public async Task<DocumentAnalysisEntity?> GetDocumentAnalysisByNameAsync(string fileName)
        {
            return await documentAnalysisRepository.GetDocumentAnalysisByNameAsync(fileName);

        }

        public async Task<IEnumerable<DocumentInfo>> GetAllDocumentAnalysisAsync()
        {
            return await documentAnalysisRepository.GetAllDocumentAnalysisAsync();
        }

        public async Task<DocumentAnalysisEntity> AnalyzeDocumentAsync(string modelName, string fileName, bool saveToStore)
        {

            logger.LogInformation($"Sending document {fileName} to analysis model");

            var options = await CreateAnalyzeDocumentOptions(modelName, fileName);
            var docHash = Convert.ToHexString(options.BytesSource);

            Operation<AnalyzeResult> operation =
                await client.AnalyzeDocumentAsync(
                    WaitUntil.Completed,
                    options);

            var entity = new DocumentAnalysisEntity
            {
                FileName = fileName,
                BytesHash = docHash,
                DocumentAnalysisResult = Map(modelName, operation.Value)
            };

            logger.LogInformation($"Response received response");

            if (saveToStore)
            {
                await documentAnalysisRepository.SaveDocumentAnalysisAsync(
                    entity.FileName,
                    entity.DocumentAnalysisResult,
                    entity.BytesHash);

                logger.LogInformation($"Document analysis saved to store response");
            }

            return entity;

        }

        private async Task<AnalyzeDocumentOptions> CreateAnalyzeDocumentOptions(string modelName, string fileName)
        {
            var fileStream = fileStoreService.GetDocumentFileStream(fileName);
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


