
using AzureAI.Contracts.Dtos;

namespace AzureAI.Common
{
    public interface IApiStepTracer
    {
        List<ApiTraceStep> Steps { get; set; }
        void AddStep(ApiStepTypeEnum source, string title);
        void AddRange(List<ApiTraceStep> steps);

        string GetTrace();
        string GetTotalTime();
    }
}
