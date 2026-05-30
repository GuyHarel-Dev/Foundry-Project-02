using DemoAzAI.Application.AI;
using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services.Interfaces;

namespace DemoAzAI.Api.Controllers
{
    [Route("api/embedding")]
    [ApiController]
    public class EmbeddingController(
                IEmbeddingService embeddingService,
                ISemanticSearchService semanticSearchService) 
        : ControllerBase
    {

        [HttpPost("embed/chunks")]
        public async Task<ActionResult<EmbeddingResponseDto>> EmbedChunks([FromBody] EmbeddingRequestDto request)
        {
            if (request.Chunks == null || !request.Chunks.Any() || request.Chunks.Any(string.IsNullOrWhiteSpace))
            {
                return BadRequest("Chunks cannot be null or empty.");
            }
            var response = await embeddingService.EmbeddTextAsync(request);

            if (response == null || response.Any() == false)
                throw new InvalidOperationException(
                    "The embedding provider returned no embeddings.");

            var test = new EmbeddingResponseDto();

            return Ok(test);
        }

        [HttpGet("search/store")]
        public async Task<ActionResult> SearchEmbeddings([FromQuery] string? question)
        {
            if (string.IsNullOrEmpty(question))
            {
                return BadRequest("Search question cannot be null or empty");
            }
            if (embeddingService.EmbeddingStoreCount == 0)
            {
                return BadRequest("No embeddings stored yet. Please embed some text before searching.");
            };

            await embeddingService.SearchEmbedding(question);

            // Implement search logic here, e.g., using the stored embeddings to find relevant chunks based on the query.
            return Ok("Search functionality not implemented yet.");
        }
    }
}


