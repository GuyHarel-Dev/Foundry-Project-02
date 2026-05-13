using Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;
using WebApi.Services.Interfaces;

namespace WebApi.Controllers 
{
    [Route("api/ai/doc")]
    [ApiController]
    public class DocAiController : ControllerBase
    {
        private readonly IAzAiDocIntelligenceService _documentIntelligence;

        public DocAiController(IAzAiDocIntelligenceService documentIntelligence)
        {
            _documentIntelligence = documentIntelligence;
        }

        [HttpGet("AnalyzeDocument")]
        public async Task<ActionResult<AnalyzeDocumentResponseDto>> AnalyzeDocument()
        {
            var response = await _documentIntelligence.AnalyzeDocument();

            return Ok(response);

        }
    }
}
