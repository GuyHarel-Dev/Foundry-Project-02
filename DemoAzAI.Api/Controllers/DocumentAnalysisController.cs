using DemoAzAI.Api.Services.Interfaces;
using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Infrastructure.ExternalServices.Stores;
using Microsoft.AspNetCore.Mvc;

namespace DemoAzAI.Api.Controllers 
{
    [Route("api/document")]
    [ApiController]
    public class DocumentAnalysisController : ControllerBase
    {
        private readonly IDocumentIntelligenceService documentIntelligenceService;

        public DocumentAnalysisController(IDocumentIntelligenceService documentIntelligenceService)
        {
            this.documentIntelligenceService = documentIntelligenceService;
        }

        [HttpPost("analyze")]
        public async Task<ActionResult<DocumentAnalysisResponseDto>> AnalyzeDocument(
            [FromBody] DocumentAnalysisRequestDto request)
        {

            var response = await documentIntelligenceService.AnalyzeDocumentAsync(request);

            return Ok(response);
        }


    }
}
