using DemoAzAI.Application.AI;
using DemoAzAI.Contracts.Api.Requests;
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

        [HttpPost("embed")]
        public async Task<ActionResult> EmbedText([FromBody] EmbeddingRequestDto request)
        {
            if (request.Chunks == null || !request.Chunks.Any())
            {
                return BadRequest("TextLines cannot be null or empty.");
            }
            var response = await embeddingService.EmbeddTextAsync(request);

            return Ok(response);
        }

        [HttpGet("search")]
        public async Task<ActionResult> Search([FromQuery] string? question)
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


