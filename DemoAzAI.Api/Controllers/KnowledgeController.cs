using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services.Interfaces;

namespace WebApi.Controllers
{
    [Route("api/knowledge")]
    [ApiController]
    public class KnowledgeController(
            IKnowledgeService knowledgeService
        ) : ControllerBase
    {

        [HttpPost("embed")]
        public async Task<ActionResult<DocumentKnowledgeResponseDto>> EmbedDocumentKnowledgeAsync([FromBody] DocumentKnowledgeRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.TextToChunk))
            {
                return BadRequest("TextToChunk property cannot be empty.");
            }
            var response = await knowledgeService.EmbedDocumentKnowledgeAsync(request);
            return new DocumentKnowledgeResponseDto();

            //return Ok(response);
        }

        [HttpGet("chunks/filename/{fileName}")]
        public async Task<DocumentEmbeddingEntity?> GetDocumentChunksByNameAsync(string fileName)
        {
            //return await documentAnalysisService.GetDocumentChunksByNameAsync(fileName);

            return new DocumentEmbeddingEntity();

        }
    }
}
