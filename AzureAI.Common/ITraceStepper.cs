
using AzureAI.Contracts.Dtos;

namespace AzureAI.Common
{
    public interface ITraceStepper
    {
        List<TraceStep> Steps { get; set; }
        void AddStep(TraceStepperSourceEnum source, string title);
        void AddRange(List<TraceStep> steps);

        string GetTrace();
        string GetTotalTime();
    }
}
