using DemoAzAI.Api.Services.Interfaces;
using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain.Models;
using DemoAzAI.Infrastructure.ExternalServices.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DemoAzAI.Api.Controllers
{
    [Route("api/document")]
    [ApiController]
    public class DocumentController(
            IDocumentAnalysisRepository documentAnalysisRepository,
            IDocumentService documentAnalysisService
        ) : ControllerBase
    {

        [HttpPost("analysis")]
        public async Task<ActionResult<DocumentAnalysisResponseDto>> AnalyzeDocumentAsync(
            [FromBody] DocumentAnalysisRequestDto request)
        {
            var entity = await documentAnalysisService.AnalyzeDocumentAsync(request.ModelName, request.FileName, request.SaveToStore);

            return Ok(new DocumentAnalysisResponseDto
            {
                DocumentAnalysisEntity = entity,

            });
        }

        [HttpGet("analysis")]
        public async Task<IEnumerable<DocumentInfo>> GetDocumentAnalysisAsync()
        {
            return await documentAnalysisRepository.GetAllDocumentAnalysisAsync();
        }


        [HttpGet("analysis/id/{id}")]
        public async Task<DocumentAnalysisResponseDto?> GetDocumentAnalysisByIdAsync(string id)
        {
            var entity = await documentAnalysisRepository.GetDocumentAnalysisByIdAsync(id);

            return new DocumentAnalysisResponseDto
            {
                DocumentAnalysisEntity = entity,
            };
        }

        [HttpGet("analysis/filename/{fileName}")]
        public async Task<DocumentAnalysisResponseDto?> GetDocumentAnalysisByNameAsync(string fileName)
        {
            var entity = await documentAnalysisRepository.GetDocumentAnalysisByNameAsync(fileName);

            return new DocumentAnalysisResponseDto
            {
                DocumentAnalysisEntity = entity,
            };
        }


    }
}
