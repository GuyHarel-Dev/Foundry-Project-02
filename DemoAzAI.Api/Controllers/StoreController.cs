using DemoAzAI.Abstractions;
using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain.Models.DocumentAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/store")]
    [ApiController]
    public class StoreController(
        IDocumentAnalysisStoreService storeService
        ) : ControllerBase
    {

        [HttpPost("document/analysis")]
        public async Task<ActionResult<StoredDocumentAnalysisResponseDto>> StoreDocumentAnalysis([FromBody] StoredDocumentAnalysisRequestDto request)
        {
            if (request is null || string.IsNullOrEmpty(request.FileName) || string.IsNullOrEmpty(request.Hash) || request.DocumentAnalysisResult is null)
            {
                return BadRequest();
            }

            var success = await storeService.SaveAsync(request.FileName, request.Hash, request.DocumentAnalysisResult);

            var response = new StoredDocumentAnalysisResponseDto
            {
                FileName = request.FileName,
                Hash = request.Hash,
                Status = success ? "Success" : "Failed"
            };

            return Ok(response);
        }

        [HttpGet("document/analysis/results")]
        public async Task<ActionResult<IEnumerable<DocumentAnalysisStoredResult>>> GetAllDocumentAnalysisResults()
        {
            var history = await storeService.GetAllAsync();

            if (history is null || history.Any() is false)
            {
                return NotFound();
            }

            return Ok(history);
        }


        [HttpGet("document/analysis/results/{id}")]
        public async Task<ActionResult<DocumentAnalysisStoredResult>> GetDocumentAnalysisResultsById(string id)
        {
            var doc = await storeService.GetByIdAsync(id);

            if (doc is null)
            {
                return NotFound();
            }
            return Ok(doc);
        }


    }
}
