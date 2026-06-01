using Azure.Core;
using DemoAzAI.Abstractions;
using DemoAzAI.Application.Extensions;
using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain;
using WebApi.Services.Interfaces;

namespace WebApi.Services
{
    public class StoreService (
        IDocumentAnalysisStoreService documentAnalysisStoreService
        )  : IStoreService
    {
        public void StoreDocumentAnalysisResult(Byte[] bytes, DocumentAnalysisResponseDto doc)
        {
            //doc.DocumentHash = bytes.ComputeHash();
            //documentAnalysisStoreService.SaveAsync("abc", doc);
        }
    }
}
