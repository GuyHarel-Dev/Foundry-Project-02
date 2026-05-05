using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.DTOs;
using WebApi.Services;


namespace WebApi.Controllers
{
    [Route("api/ai/chat")]
    [ApiController]
    public class ChatAiController : ControllerBase
    {
        private readonly IConfiguration _config; 
        private readonly IAzFoundryChatService _azFoundryChatService;
        private readonly ITraceStepper _executionTraceStep;

        public ChatAiController(
                IConfiguration config, 
                IAzFoundryChatService azFoundryService,
                ITraceStepper executionTraceStep)
        {
            _config = config;
            _azFoundryChatService = azFoundryService;
            _executionTraceStep = executionTraceStep;
        }

        [HttpPost("prompt")]
        public async Task<IActionResult> Prompt([FromBody] ChatRequestDto request)
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


