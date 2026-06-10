using DemoAzAI.Abstractions.Entities;
using DemoAzAI.Abstractions.Repositories;
using DemoAzAI.Contracts.Api.Requests;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/store")]
    [ApiController]
    public class RepositoryController(
        IDocumentAnalysisRepository documentAnalysysStoreService
        ) : ControllerBase
    {

        //[HttpPost("document")]
        //public async Task<ActionResult<DocumentAnalysisUploadResponseDto>> UploadDocument([FromBody] DocumentRequestDto request)
        //{
        //    if (request is null || string.IsNullOrEmpty(request.FileName) || string.IsNullOrEmpty(request.BytesHash) || request.DocumentAnalysisResult is null)
        //    {
        //        return BadRequest();
        //    }

        //    if (await documentAnalysysStoreService.ExistsAsync(request.BytesHash))
        //    {
        //        return Conflict($"Document {request.FileName} hash: {request.BytesHash} already exists.");
        //    }

        //    var success = await documentAnalysysStoreService.SaveAsync(request.FileName, request.BytesHash, request.DocumentAnalysisResult);

        //    var response = new DocumentAnalysisUploadResponseDto
        //    {
        //        FileName = request.FileName,
        //        Hash = request.BytesHash,
        //        Status = success ? "Success" : "Failed"
        //    };

        //    return Ok(response);
        //}

        //[HttpGet("documents")]
        //public async Task<ActionResult<IEnumerable<DocumentEntity>>> GetAvailableDocuments()
        //{

        //    return Ok();
        //}
        
        //[HttpPost("document/analysis")]
        //public async Task<ActionResult<DocumentAnalysisUploadResponseDto>> StoreDocumentAnalysis([FromBody] DocumentRequestDto request)
        //{
        //    if (request is null || string.IsNullOrEmpty(request.FileName) || string.IsNullOrEmpty(request.BytesHash) || request.DocumentAnalysisResult is null)
        //    {
        //        return BadRequest();
        //    }

        //    if (await documentAnalysysStoreService.ExistsAsync(request.BytesHash))
        //    {
        //        return Conflict($"Document {request.FileName} hash: {request.BytesHash} already exists.");
        //    }

        //    var success = await documentAnalysysStoreService.SaveAsync(request.FileName, request.BytesHash, request.DocumentAnalysisResult);

        //    var response = new DocumentAnalysisUploadResponseDto
        //    {
        //        FileName = request.FileName,
        //        Hash = request.BytesHash,
        //        Status = success ? "Success" : "Failed"
        //    };

        //    return Ok(response);
        //}

        [HttpGet("document/analysis/results")]
        public async Task<ActionResult<IEnumerable<DocumentAnalysisEntity>>> GetAllDocumentAnalysisResults()
        {
            var history = await documentAnalysysStoreService.GetAllAsync();

            if (history is null || history.Any() is false)
            {
                return NotFound();
            }

            return Ok(history);
        }


        [HttpGet("document/analysis/results/{id}")]
        public async Task<ActionResult<DocumentAnalysisEntity>> GetDocumentAnalysisResultsById(string id)
        {
            var doc = await documentAnalysysStoreService.GetByIdAsync(id);

            if (doc is null)
            {
                return NotFound();
            }
            return Ok(doc);
        }


    }
}
