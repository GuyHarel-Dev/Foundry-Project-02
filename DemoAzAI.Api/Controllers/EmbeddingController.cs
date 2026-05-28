using DemoAzAI.Contracts.Requests;
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

        [HttpPost("embedlines")]
        public async Task<ActionResult> EmbedLines([FromBody] EmbeddingTextRequestDto request)
        {
            if (request.TextLines == null || !request.TextLines.Any())
            {
                return BadRequest("TextLines cannot be null or empty.");
            }
            var response = await _azFoundryEmbeddingService.EmbeddTextLinesAsync(request);

            return Ok(response);
        }


    }
}


