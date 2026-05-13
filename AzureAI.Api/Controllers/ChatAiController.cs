using AzureAI.Api.Services.Interfaces;
using AzureAI.Contracts;
using AzureAI.Contracts.Requests;
using AzureAI.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;


namespace AzureAI.Api.Controllers
{
    [Route("api/ai/chat")]
    [ApiController]
    public class ChatAiController : ControllerBase
    {
        private readonly IConfiguration _config; 
        private readonly IAzFoundryChatService _azFoundryChatService;
        private readonly ITraceStepper _traceStepper;

        public ChatAiController(
                IConfiguration config, 
                IAzFoundryChatService azFoundryService,
                ITraceStepper traceStepper)
        {
            _config = config;
            _azFoundryChatService = azFoundryService;
            _traceStepper = traceStepper;
        }

        [HttpPost("prompt")]
        public async Task<ActionResult<ChatResponseDto>> Prompt([FromBody] ChatRequestDto request)
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


