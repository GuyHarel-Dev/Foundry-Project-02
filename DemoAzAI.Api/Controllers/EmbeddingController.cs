using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services.Interfaces;

namespace DemoAzAI.Api.Controllers
{
    [Route("api/embedding")]
    [ApiController]
    public class EmbeddingController(
                IEmbeddingService embeddingService
        ) 
        : ControllerBase
    {

        [HttpPost("text-lines")]
        public async Task<ActionResult<EmbeddingTextLinesResponseDto>> EmbedTextLines([FromBody] EmbeddingTextLinesRequestDto request)
        {
            var response = await embeddingService.EmbedTextLinesAsync(request);

            if (response == null || response.EmbeddedTexts is null || response.EmbeddedTexts.Any() == false)
                throw new InvalidOperationException(
                    "EmbedTextLines returned no embeddings.");

            return Ok(response);
        }

        [HttpPost("text")]
        public async Task<ActionResult<EmbeddingTextResponseDto>> EmbedText([FromBody] EmbeddingTextRequestDto request)
        {
            var response = await embeddingService.EmbedTextAsync(request.Text);

            return Ok(response);
        }

    }
}


