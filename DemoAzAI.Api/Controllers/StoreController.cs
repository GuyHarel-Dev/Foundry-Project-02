using DemoAzAI.Contracts.Dtos;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services.Store;

namespace WebApi.Controllers
{
    [Route("api/store")]
    [ApiController]
    public class StoreController (IEmbeddingStoreService embeddingStoreService) : ControllerBase
    {

        [HttpGet]
        [Route("embeddingvalues/{guid:guid}")]
        public ActionResult<TextEmbeddingDto> Get(Guid guid)
        {
            var embedding = embeddingStoreService.Embeddings
                .FirstOrDefault(e => e.Guid == guid);

            if (embedding is null)
            {
                return NotFound($"Guid not found in in-memory embedding store: {guid}");
            }

            return Ok(embedding);
        }
    }
}
