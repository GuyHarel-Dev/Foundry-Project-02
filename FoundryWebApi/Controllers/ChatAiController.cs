using Microsoft.AspNetCore.Mvc;
using WebApi.Services;
using SharedDTO;
using Microsoft.Extensions.Azure;

namespace WebApi.Controllers
{
    [Route("api/ai/chat")]
    [ApiController]
    public class ChatAiController : ControllerBase
    {
        private readonly IConfiguration _config; 
        private readonly IAzFoundryChatService _azFoundryChatService;
        private readonly ExecutionTraceService _traces;

        public ChatAiController(
                IConfiguration config, 
                IAzFoundryChatService azFoundryService,
                ExecutionTraceService traces)
        {
            _config = config;
            _azFoundryChatService = azFoundryService;
            _traces = traces;
        }

        [HttpPost("prompt")]
        public async Task<IActionResult> Prompt([FromBody] ChatRequest request)
        {
            _traces.AddTrace("...prompt...");

            if (string.IsNullOrWhiteSpace(request.Prompt))
            {
                return BadRequest("Prompt text cannot be empty.");
            }
            var response = await _azFoundryChatService.GetResponseAsync(request);
            return Ok(response);
        }

        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {

            return Ok();

        }

        [HttpGet("trace")]
        public async Task<IActionResult> Trace()
        {
            await Task.Delay(100);
            return Ok(_traces.ToString());

        }
    }
}


