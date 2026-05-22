using DemoAzAI.Api.Services.Interfaces;
using DemoAzAI.Contracts.Requests;
using DemoAzAI.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;


namespace DemoAzAI.Api.Controllers
{
    [Route("api/chat")]
    [ApiController]
    public class ChatAiController : ControllerBase
    {
        private readonly IConfiguration _config; 
        private readonly IAzFoundryChatService _azFoundryChatService;

        public ChatAiController(
                IConfiguration config, 
                IAzFoundryChatService azFoundryService)
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


