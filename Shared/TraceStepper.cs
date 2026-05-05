using Shared.DTOs;
using System.Reflection.Metadata.Ecma335;

namespace Shared
{
    public class TraceStepper : ITraceStepper
    {
        public List<TraceStepDto> Steps { get; set; } =  new List<TraceStepDto>();

        public TraceStepDto AddStep(string title)
        {
            var step = new TraceStepDto
            {
                Title = title,
                BeginDateTime = DateTime.UtcNow
            };
            Steps.Add(step);
            return step;
        }

        public string GetTrace()
        {
            return string.Join(Environment.NewLine, Steps.Select(s =>
                $"{s.Title}: {(s.BeginDateTime.HasValue ? s.BeginDateTime.Value.ToString("o") : "N/A")} - {(s.EndDateTime.HasValue ? s.EndDateTime.Value.ToString("o") : "N/A")}"));
        }
    }
}
