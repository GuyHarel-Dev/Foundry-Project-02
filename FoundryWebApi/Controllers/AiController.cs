using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Services;


namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AiController : ControllerBase
    {
        private readonly IConfiguration _config; 
        private readonly IAzFoundryChatService _azFoundryChatService;

        public AiController(IConfiguration config, IAzFoundryChatService azFoundryService)
        {
            _config = config;
            _azFoundryChatService = azFoundryService;
        }

        [HttpPost("prompt")]
        public async Task<IActionResult> Prompt([FromBody] PromptRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Request))
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


