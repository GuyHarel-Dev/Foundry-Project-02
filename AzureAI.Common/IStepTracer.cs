
using AzureAI.Contracts.Dtos;

namespace AzureAI.Common
{
    public interface IStepTracer
    {
        List<StepTracerStep> Steps { get; set; }
        void AddStep(StepTracerTraceTypeEnum source, string title);
        void Clear();
        void AddRange(List<StepTracerStep> steps);

        string GetTrace();
        string GetTotalTime();
    }
}
