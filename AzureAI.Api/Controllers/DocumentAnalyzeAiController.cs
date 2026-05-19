using AzureAI.Api.Services.Interfaces;
using AzureAI.Contracts.Requests;
using AzureAI.Contracts.Responses;
using Microsoft.AspNetCore.Http;
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

            var response = await _documentIntelligence.AnalyzeDocument(modelName, stream);

            return Ok(response);

        }

   
    }
}
