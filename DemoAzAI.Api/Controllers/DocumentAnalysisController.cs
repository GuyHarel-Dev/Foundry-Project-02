using DemoAzAI.Api.Services.Interfaces;
using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using Microsoft.AspNetCore.Mvc;

namespace DemoAzAI.Api.Controllers 
{
    [Route("api/document")]
    [ApiController]
    public class DocumentAnalysisController : ControllerBase
    {
        private readonly IDocumentIntelligenceService _documentIntelligence;

        public DocumentAnalysisController(IDocumentIntelligenceService documentIntelligence)
        {
            _documentIntelligence = documentIntelligence;
        }

        [HttpPost("analyse")]
        public async Task<ActionResult<DocumentAnalysisResponseDto>> AnalyzeDocument(
            [FromBody] DocumenteAnalysisRequestDto request)
        {

            var response = await _documentIntelligence.AnalyzeDocument(request);

            return Ok(response);

        }

   
    }
}
