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

        [HttpPost("embedtext")]
        public async Task<ActionResult> EmbedText([FromBody] EmbeddingRequestDto request)
        {
            var response = _azFoundryEmbeddingService.GetEmbeddingAsync(request);

            return Ok(response);
        }

        //[HttpPost("embedtext")]
        //public async Task<ActionResult<EmbeddingResponseDto>> EmbedText([FromBody] EmbeddingRequestDto request)
        //{
        //    var response = _azFoundryEmbeddingService.GetEmbeddingAsync(request);

        //    return Ok(response);
        //}

    }
}


