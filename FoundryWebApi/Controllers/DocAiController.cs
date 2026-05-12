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
        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            _documentIntelligence.test();
            return Ok();

        }
    }
}
