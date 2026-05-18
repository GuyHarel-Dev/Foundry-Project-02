using Azure;
using Azure.AI.DocumentIntelligence;
using AzureAI.Api.Services.Interfaces;
using AzureAI.Common;
using AzureAI.Contracts.Dtos;
using AzureAI.Contracts.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AzureAI.Api.Services
{
    public class AzAiDocIntelligenceService : IAzAiDocIntelligenceService
    {
        private readonly IConfiguration _config;
        private readonly DocumentIntelligenceClient _client;
        private readonly ILogger<AzAiDocIntelligenceService> _logger;
        private readonly IApiStepTracer _traceStepper;

        public AzAiDocIntelligenceService(
            DocumentIntelligenceClient client,
            IConfiguration config,
            ILogger<AzAiDocIntelligenceService> logger,
            IApiStepTracer traceStepper)
        {
            _client = client;
            _config = config;
            _logger = logger;
            _traceStepper = traceStepper;
        }

        public async Task<AnalyzeDocumentResponseDto> PrebuildModelTest(IFormFile formFile)
        {

            return new AnalyzeDocumentResponseDto();
        }

        public async Task<AnalyzeDocumentResponseDto> AnalyzeDocument(string model, Stream stream, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Sending document to Azure AI Foundry");

            var bytes = BinaryData.FromStream(stream);

            var options = new AnalyzeDocumentOptions(
                model,
                bytes);

            _traceStepper.AddStep(ApiStepTypeEnum.WebApi, "Sending document for analysis");

            var operation =
                await _client.AnalyzeDocumentAsync(
                    WaitUntil.Completed,
                    options,
                    cancellationToken);

            var response = new AnalyzeDocumentResponseDto();

            AnalyzeResult result = operation.Value;

            ExtractBaseInformation(operation.Value, response);
            ExtractFields(result, response);

            _traceStepper.AddStep(ApiStepTypeEnum.WebApi, "Returnining response");

            return response;
        }

        //private void ExtractStatistics(AnalyzeResult result, AnalyzeDocumentResponseDto response)
        //{
        //    return new ModelStatisticsDto
        //    {
        //        //ResponseId = result.Id,
        //        //Model = result.Value.Model,
        //        //InputTokens = result.Value.Usage?.InputTokenCount,
        //        //OutputTokens = result.Value.Usage?.OutputTokenCount,
        //        //TotalTokens = result.Value.Usage?.TotalTokenCount
        //    };

        //}

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

        private static void ExtractFields(AnalyzeResult result, AnalyzeDocumentResponseDto response)
        {
            if (result.Documents != null)
            {
                foreach (var document in result.Documents)
                {
                    foreach (var field in document.Fields)
                    {
                        response.Fields.Add(new DocumentFieldDto
                        {
                            Name = field.Key,

                            Value = field.Value.Content,

                            FieldType = field.Value.FieldType.ToString(),

                            Confidence = field.Value.Confidence
                        });
                    }
                }

            }
        }
    }
}
