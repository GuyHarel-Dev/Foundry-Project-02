using Shared.DTOs;

namespace Shared
{
    public interface ITraceStepper
    {
        List<TraceStepDto> Steps { get; set; }
        TraceStepDto AddStep(string title);
        string GetTrace();
    }
}
