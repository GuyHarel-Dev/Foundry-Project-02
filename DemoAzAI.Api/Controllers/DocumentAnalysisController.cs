using DemoAzAI.Api.Services.Interfaces;
using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain.Entities;
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

            var response = await documentAnalysisService.AnalyzeDocumentAsync(request);

            return Ok(response);
        }

        [HttpGet("all-analysis")]
        public async Task<IEnumerable<DocumentInfo>> GetDocumentAnalysisAsync()
        {
            return await documentAnalysisService.GetAllDocumentAnalysisAsync();
        }

        [HttpGet("analysis/{fileName}")]
        public async Task<DocumentAnalysisEntity?> GetDocumentAnalysisByIdAsync(string fileName)
        {
            return await documentAnalysisService.GetDocumentAnalysisByNameAsync(fileName);
        }

    }
}
