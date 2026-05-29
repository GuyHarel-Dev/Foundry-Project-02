using DemoAzAI.Api.Services.Interfaces;
using DemoAzAI.Api.Contracts.Requests;
using DemoAzAI.Api.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;


namespace DemoAzAI.Api.Controllers
{
    [Route("api/chat")]
    [ApiController]
    public class ChatAiController : ControllerBase
    {
        private readonly IConfiguration _config; 
        private readonly IChatService _azFoundryChatService;

        public ChatAiController(
                IConfiguration config, 
                IChatService azFoundryService)
        {
            _config = config;
            _azFoundryChatService = azFoundryService;
        }

        [HttpPost("sendprompt")]
        public async Task<ActionResult<ChatResponseDto>> SendPrompt([FromBody] ChatRequestDto request)
        {
             if (string.IsNullOrWhiteSpace(request.Prompt))
            {
                return BadRequest("Prompt text cannot be empty.");
            }
            var response = await _azFoundryChatService.GetPromptResponseAsync(request);

            return Ok(response);
        }

    }
}


