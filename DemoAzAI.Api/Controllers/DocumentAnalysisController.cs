using DemoAzAI.Api.Services.Interfaces;
using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Infrastructure.ExternalServices.Repositories;
using DemoAzAI.Infrastructure.ExternalServices.Stores;
using Microsoft.AspNetCore.Mvc;

namespace DemoAzAI.Api.Controllers 
{
    [Route("api/document")]
    [ApiController]
    public class DocumentAnalysisController (
            IDocumentAnalysisService documentAnalysisService,
            IDocumentAnalysisRepository documentAnalysisRepository
        ) : ControllerBase
    {

        [HttpPost("analyze")]
        public async Task<ActionResult<DocumentAnalysisResponseDto>> AnalyzeDocument(
            [FromBody] DocumentAnalysisRequestDto request)
        {

            var response = await documentAnalysisService.AnalyzeDocumentAsync(request);

            if (response is not null && request.SaveToStore)
            {
                await documentAnalysisRepository.SaveAsync(request.FileName, response.DocumentAnalysisResult, response.BytesHash);
            }

            return Ok(response);
        }


    }
}
