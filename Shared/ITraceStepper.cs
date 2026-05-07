using Shared.DTOs;

namespace Shared
{
    public interface ITraceStepper
    {
        List<TraceStepDto> Steps { get; set; }
        void AddStep(TraceStepperSourceEnum source, string title);
        void AddRange(List<TraceStepDto> steps);

        string GetTrace();
        string GetTotalTime();
    }
}
