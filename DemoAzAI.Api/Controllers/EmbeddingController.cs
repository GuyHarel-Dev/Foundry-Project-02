using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services.Interfaces;

namespace DemoAzAI.Api.Controllers
{
    [Route("api/text-embedding")]
    [ApiController]
    public class EmbeddingController(
                IEmbeddingService embeddingService
        ) 
        : ControllerBase
    {

        [HttpPost("embed-lines")]
        public async Task<ActionResult<EmbeddingResponseDto>> EmbedTextLines([FromBody] EmbeddingTextLinesRequestDto request)
        {
            var response = await embeddingService.EmbedTextLinesAsync(request);

            if (response == null || response.DocumentEmbeddingEntity.EmbeddedChunks.Any() == false)
                throw new InvalidOperationException(
                    "The embedding provider returned no embeddings.");

            return Ok(response);
        }

        [HttpPost("embed")]
        public async Task<ActionResult<EmbeddingResponseDto>> EmbedText([FromBody] EmbeddingTextRequestDto request)
        {
            var response = await embeddingService.EmbedTextAsync(request.Text);

            return Ok(response);
        }

    }
}


