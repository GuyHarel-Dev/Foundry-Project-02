using DemoAzAI.Api.Services.Interfaces;
using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain.Entities;
using DemoAzAI.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace DemoAzAI.Api.Controllers 
{
    [Route("api/document")]
    [ApiController]
    public class DocumentAnalysisController (
            IDocumentAnalysisService documentAnalysisService
        ) : ControllerBase
    {

        [HttpPost("analyze")]
        public async Task<ActionResult<DocumentAnalysisResponseDto>> AnalyzeDocumentAsync(
            [FromBody] DocumentAnalysisRequestDto request)
        {

            var response = await documentAnalysisService.AnalyzeDocumentAsync(request);

            return Ok(response);
        }

        [HttpGet("analysis")]
        public async Task<IEnumerable<DocumentAnalysisInfo>> GetDocumentAnalysisAsync()
        {
            return await documentAnalysisService.GetDocumentAnalysisAsync();
        }

        [HttpGet("analysis/{id}")]
        public async Task<DocumentAnalysisEntity?> GetDocumentAnalysisByIdAsync(string id)
        {
            return await documentAnalysisService.GetDocumentAnalysisByIdAsync(id);
        }

    }
}
