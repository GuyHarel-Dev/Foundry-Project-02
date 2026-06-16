using DemoAzAI.Api.Services.Interfaces;
using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace DemoAzAI.Api.Controllers 
{
    [Route("api/document-analysis")]
    [ApiController]
    public class DocumentAnalysisController (
            IDocumentAnalysisService documentAnalysisService
        ) : ControllerBase
    {

        [HttpPost()]
        public async Task<ActionResult<DocumentAnalysisResponseDto>> AnalyzeDocumentAsync(
            [FromBody] DocumentAnalysisRequestDto request)
        {
            var entity = await documentAnalysisService.AnalyzeDocumentAsync(request.ModelName, request.FileName, request.SaveToStore);

            return Ok(new DocumentAnalysisResponseDto
            {
                DocumentAnalysisEntity = entity,

            });
        }

        [HttpGet()]
        public async Task<IEnumerable<DocumentInfo>> GetDocumentAnalysisAsync()
        {
            return await documentAnalysisService.GetAllDocumentAnalysisAsync();
        }


        [HttpGet("id/{id}")]
        public async Task<DocumentAnalysisResponseDto?> GetDocumentAnalysisByIdAsync(string id)
        {
            var entity = await documentAnalysisService.GetDocumentAnalysisByIdAsync(id);

            return new DocumentAnalysisResponseDto
            {
                DocumentAnalysisEntity = entity,
            };
        }

        [HttpGet("filename/{fileName}")]
        public async Task<DocumentAnalysisResponseDto?> GetDocumentAnalysisByNameAsync(string fileName)
        {
            var entity = await documentAnalysisService.GetDocumentAnalysisByNameAsync(fileName);

            return new DocumentAnalysisResponseDto
            {
                DocumentAnalysisEntity = entity,
            };
        }

    }
}
