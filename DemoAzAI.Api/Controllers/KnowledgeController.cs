using DemoAzAI.Api.Services.Interfaces;
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

        [HttpPost("extract")]
        public async Task<ActionResult<DocumentKnowledgeResponseDto>> ExtractKnowledge([FromBody] DocumentKnowledgeRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.TextToChunk))
            {
                return BadRequest("TextToChunk property cannot be empty.");
            }
            //var response = await knowledgeService.GetDocumentChunkingAsync(request);
            return new DocumentKnowledgeResponseDto();

            //return Ok(response);
        }

        [HttpGet("chunks/filename/{fileName}")]
        public async Task<DocumentChunkEntity?> GetDocumentChunksByNameAsync(string fileName)
        {
            //return await documentAnalysisService.GetDocumentChunksByNameAsync(fileName);

            return new DocumentChunkEntity();

        }
    }
}
