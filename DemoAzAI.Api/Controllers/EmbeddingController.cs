using DemoAzAI.Api.Services.Interfaces;
using DemoAzAI.Contracts.Requests;
using DemoAzAI.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services.Interfaces;


namespace DemoAzAI.Api.Controllers
{
    [Route("api/embedding")]
    [ApiController]
    public class EmbeddingController : ControllerBase
    {
        private readonly IConfiguration _config; 
        private readonly IAzFoundryEmbeddingService _azFoundryEmbeddingService;

        public EmbeddingController(
                IConfiguration config,
                IAzFoundryEmbeddingService azFoundryEmbeddingService)
        {
            _config = config;
            _azFoundryEmbeddingService = azFoundryEmbeddingService;
        }

        [HttpPost("embed/text")]
        public async Task<ActionResult> EmbedText([FromBody] EmbeddingTextRequestDto request)
        {
            var response = await _azFoundryEmbeddingService.EmbeddTextAsync(request);

            return Ok(response);
        }

        [HttpPost("embed/lines")]
        public async Task<ActionResult> EmbedLines([FromBody] EmbeddingTextLinesRequestDto request)
        {
            var response = await _azFoundryEmbeddingService.EmbeddTextLinesAsync(request);

            return Ok(response);
        }

    }
}


