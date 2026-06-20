using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain.Entities;
using DemoAzAI.Domain.Models;
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

            return response;
        }

        [HttpGet("embeddings")]
        public async Task<IEnumerable<DocumentInfo>?> GetDocumentKnowledgeNamesAsync()
        {
            return await knowledgeService.GetDocumentKnowledgeEmbeddingsAsync();

        }

        [HttpGet("embeddings/{fileName}")]
        public async Task<DocumentKnowledgeEntity?> GetDocumentKnowledgeByFileNameAsync(string fileName)
        {
            return await knowledgeService.GetDocumentKnowledgeByFileNameAsync(fileName);

        }
    }
}
