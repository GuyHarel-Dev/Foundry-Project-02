using DemoAzAI.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/store")]
    [ApiController]
    public class StoreController : ControllerBase
    {

        [HttpPost("document/analysis")]
        public IActionResult StoreDocument([FromBody] DocumentAnalysisResult document)
        {
            // Simulate a create operation
            return Ok();
        }

        [HttpPost("embeddings")]
        public IActionResult StoreChunks([FromBody] List<EmbeddedChunk> chunks)
        {
            // Simulate a create operation
            return Ok();
        }

    }
}
