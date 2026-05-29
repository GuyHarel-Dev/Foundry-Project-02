using DemoAzAI.Api.Services.Interfaces;
using DemoAzAI.Api.Contracts.Requests;
using DemoAzAI.Api.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace DemoAzAI.Api.Controllers 
{
    [Route("api/document")]
    [ApiController]
    public class DocumentAnalyzeAiController : ControllerBase
    {
        private readonly IDocumentIntelligenceService _documentIntelligence;

        public DocumentAnalyzeAiController(IDocumentIntelligenceService documentIntelligence)
        {
            _documentIntelligence = documentIntelligence;
        }

        [HttpPost("analyze")]
        public async Task<ActionResult<AnalyzeDocumentResponseDto>> AnalyzeDocument(
            [FromBody] AnalyzeDocumentRequestDto request)
        {
            var modelName = request.ModelName;
            await using var stream = new MemoryStream(request.Bytes);

            var response = await _documentIntelligence.AnalyzeDocument(request);

            return Ok(response);

        }

   
    }
}
