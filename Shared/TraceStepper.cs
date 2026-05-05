using Shared.DTOs;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Shared
{
    public class TraceStepper : ITraceStepper
    {
        public List<TraceStepDto> Steps { get; set; } = new();

        public TraceStepper()
        {
                
        }
        public TraceStepper(List<TraceStepDto> steps)
        {
            Steps.AddRange(steps);
        }

        public void AddStep(string title)
        {
            var step = new TraceStepDto
            {
                StepTitle = title,
                StepDateTime = DateTime.UtcNow
            };
            Steps.Add(step);
        }

        public string GetTrace()
        {
            if (Steps.Count == 0)
            {
                return string.Empty;
            }

            var builder = new StringBuilder();

            foreach (var step in Steps.OrderBy(x => x.StepDateTime))
            {
                builder.AppendLine(
                    $"[{step.StepDateTime:yyyy-MM-dd HH:mm:ss.fff} UTC] {step.StepTitle}");
            }

            return builder.ToString();
        }

        public string GetTotalTime()
        {
            var orderedSteps = Steps
                .Where(x => x.StepDateTime.HasValue)
                .OrderBy(x => x.StepDateTime)
                .ToList();

            if (orderedSteps.Count < 2)
            {
                return "";
            }

            var firstStep = orderedSteps.First().StepDateTime!.Value;
            var lastStep = orderedSteps.Last().StepDateTime!.Value;

            var total =  (lastStep - firstStep).TotalSeconds;
            return $"Total time: {total:0.000} seconds";
        }
    }
}
