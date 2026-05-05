using Shared.DTOs;

namespace Shared
{
    public interface ITraceStepper
    {
        public TraceStepDto AddStep(string title);
        public string GetTrace();
    }
}
