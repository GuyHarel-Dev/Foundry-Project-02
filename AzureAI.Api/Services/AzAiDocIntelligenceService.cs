using Azure;
using Azure.AI.DocumentIntelligence;
using AzureAI.Api.Services.Interfaces;
using AzureAI.Common;
using AzureAI.Contracts.Dtos;
using AzureAI.Contracts.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AzureAI.Api.Services
{
    public class AzAiDocIntelligenceService : IAzAiDocIntelligenceService
    {
        private readonly IConfiguration _config;
        private readonly DocumentIntelligenceClient _documentIntelligenceClient;
        private readonly ILogger<AzAiDocIntelligenceService> _logger;
        private readonly IApiStepTracer _traceStepper;

        public AzAiDocIntelligenceService(
            DocumentIntelligenceClient documentIntelligenceClient,
            IConfiguration config,
            ILogger<AzAiDocIntelligenceService> logger,
            IApiStepTracer traceStepper)
        {
            _documentIntelligenceClient = documentIntelligenceClient;
            _config = config;
            _logger = logger;
            _traceStepper = traceStepper;
        }

        public async Task<AnalyzeDocumentResponseDto> PrebuildModelTest(IFormFile formFile)
        {
            using var stream = formFile.OpenReadStream();

            var bytes = BinaryData.FromStream(stream);

            var options = new AnalyzeDocumentOptions(
                "prebuilt-layout",
                bytes);

            Operation<AnalyzeResult> operation =
                await _documentIntelligenceClient.AnalyzeDocumentAsync(
                    WaitUntil.Completed,
                    options);

            AnalyzeResult result = operation.Value;




            return new AnalyzeDocumentResponseDto
            {
                DocContent = result.Content,
                DocFormat = result.ContentFormat,
                PageCount = result.Pages.Count,
                ParagraphCount = result.Paragraphs?.Count ?? 0,
                SectionCount = result.Sections?.Count ?? 0,
                TableCount = result.Tables?.Count ?? 0
            };
        }

        public async Task<AnalyzeDocumentResponseDto> AnalyzeDocument(string model, IFormFile formFile)
        {
            using var stream = formFile.OpenReadStream();

            var bytes = BinaryData.FromStream(stream);

            var options = new AnalyzeDocumentOptions(
                model,
                bytes);

            Operation<AnalyzeResult> operation =
                await _documentIntelligenceClient.AnalyzeDocumentAsync(
                    WaitUntil.Completed,
                    options);

            var response = new AnalyzeDocumentResponseDto();

            AnalyzeResult result = operation.Value;

            ExtractBaseInformation(operation.Value, response);
            ExtractFields(result, response);

            return response;
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

        private static JsonElement SerializeToElement(AnalyzeResult result)
        {
            return JsonSerializer.SerializeToElement(
                result,
                new JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });
        }
    }
}


/*
    Pages
        Number
           words 
                content
           SelectionMarks
           Lines
            Content

            Paragraphs
                content

        Tables
            RowCount
            ColumnCount 

        Figures
        Sections
              Elements

        Languages

 */