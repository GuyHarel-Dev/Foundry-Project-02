using Microsoft.AspNetCore.Mvc;
using WebApi.Services;
using SharedDTO;

namespace WebApi.Controllers
{
    [Route("api/ai/chat")]
    [ApiController]
    public class ChatAiController : ControllerBase
    {
        private readonly IConfiguration _config; 
        private readonly IAzFoundryChatService _azFoundryChatService;

        public ChatAiController(IConfiguration config, IAzFoundryChatService azFoundryService)
        {
            _config = config;
            _azFoundryChatService = azFoundryService;
        }

        [HttpPost("prompt")]
        public async Task<IActionResult> Prompt([FromBody] ChatRequest request)
        {
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
    }
}


