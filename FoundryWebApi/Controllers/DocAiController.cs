using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/ai/doc")]
    [ApiController]
    public class DocAiController : ControllerBase
    {
        private readonly AzAiDocIntelligenceService _documentIntelligence;

        public DocAiController(AzAiDocIntelligenceService documentIntelligence)
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
