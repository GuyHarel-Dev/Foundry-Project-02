using DemoAzAI.Application.AI;
using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services.Interfaces;

namespace DemoAzAI.Api.Controllers
{
    [Route("api/text-embedding")]
    [ApiController]
    public class EmbeddingController(
                IEmbeddingService embeddingService
                /*ISemanticSearchService semanticSearchService*/) 
        : ControllerBase
    {

        [HttpPost("embed-lines")]
        public async Task<ActionResult<EmbeddingResponseDto>> EmbedTextLines([FromBody] EmbeddingRequestDto request)
        {
            var response = await embeddingService.EmbedTextLinesAsync(request);

            if (response == null || response.TextEmbeddings.Any() == false)
                throw new InvalidOperationException(
                    "The embedding provider returned no embeddings.");

            return Ok(response);
        }

        [HttpPost("embed")]
        public async Task<ActionResult<EmbeddingResponseDto>> EmbedText([FromBody] EmbeddingRequestDto request)
        {
            var response = await embeddingService.EmbedTextAsync(request.Text);

            return Ok(response);
        }

        //[HttpGet("search/store")]
        //public async Task<ActionResult<List<SemanticSearchResult>>> SearchEmbeddingStoreForSimilarTextLinesAsync([FromQuery] string? question)
        //{
        //    if (string.IsNullOrEmpty(question))
        //    {
        //        return BadRequest("Search question cannot be null or empty");
        //    }
        //    //if (embeddingService.EmbeddingStoreCount == 0)
        //    //{
        //    //    return BadRequest("No embeddings stored yet. Please embed some text before searching.");
        //    //};

        //    var result = await embeddingService.SearchEmbeddingStoreForSimilarTextLinesAsync(question);

        //    return Ok(result);
        //}
    }
}


