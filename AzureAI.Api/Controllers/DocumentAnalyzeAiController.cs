using AzureAI.Api.Services.Interfaces;
using AzureAI.Common;
using AzureAI.Contracts.Dtos;
using AzureAI.Contracts.Requests;
using AzureAI.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace AzureAI.Api.Controllers 
{
    [Route("api/document")]
    [ApiController]
    public class DocumentAnalyzeAiController : ControllerBase
    {
        private readonly IAzAiDocIntelligenceService _documentIntelligence;

        public DocumentAnalyzeAiController(IAzAiDocIntelligenceService documentIntelligence)
        {
            _documentIntelligence = documentIntelligence;
        }

        [HttpPost("analyze")]
        public async Task<ActionResult<AnalyzeDocumentResponseDto>> AnalyzeDocument(
            [FromBody] AnalyzeDocumentRequestDto request)
        {
            var modelName = request.ModelName;
            await using var stream = new MemoryStream(request.Bytes);

            var response = await _documentIntelligence.AnalyzeDocument(request);

            return Ok(response);

        }

   
    }
}
