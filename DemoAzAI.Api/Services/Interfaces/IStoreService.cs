using DemoAzAI.Contracts.Api.Responses;
using DemoAzAI.Domain;

namespace WebApi.Services.Interfaces
{
    public interface IStoreService
    {
        void StoreDocumentAnalysisResult(Byte[] bytes, DocumentAnalysisResponseDto doc);
        
    }
}
