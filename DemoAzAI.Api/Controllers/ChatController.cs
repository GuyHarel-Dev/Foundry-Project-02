using DemoAzAI.Api.Services.Interfaces;
using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using Microsoft.AspNetCore.Mvc;


namespace DemoAzAI.Api.Controllers
{
    [Route("api/chat")]
    [ApiController]
    public class ChatController(
        IChatService azFoundryChatService
        ) : ControllerBase
    {

        [HttpPost("sendprompt")]
        public async Task<ActionResult<ChatResponseDto>> SendPrompt([FromBody] ChatRequestDto request)
        {
             if (string.IsNullOrWhiteSpace(request.Prompt))
            {
                return BadRequest("Prompt text cannot be empty.");
            }
            var response = await azFoundryChatService.GetPromptResponseAsync(request);

            return Ok(response);
        }

    }
}


