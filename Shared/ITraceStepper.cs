using Shared.DTOs;

namespace Shared
{
    public interface ITraceStepper
    {
        List<TraceStepDto> Steps { get; set; }
        void AddStep(string title);
        string GetTrace();
        double GetTotalTime();
    }
}
