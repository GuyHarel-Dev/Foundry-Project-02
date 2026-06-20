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
        public async Task<ActionResult<IEnumerable<DocumentInfo>>?> GetDocumentKnowledgeNamesAsync()
        {
            var entities = await knowledgeService.GetDocumentKnowledgeEmbeddingsAsync();
            return Ok(entities);

        }

        [HttpGet("embeddings/{fileName}")]
        public async Task<ActionResult<DocumentKnowledgeEntity?>> GetDocumentKnowledgeByFileNameAsync(string fileName)
        {
            var entity = await knowledgeService.GetDocumentKnowledgeByFileNameAsync(fileName);

            if (entity == null) 
            {
                return NotFound($"Document embeddings not found for document name: {fileName}");
            }

            return entity;

        }
    }
}
