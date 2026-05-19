using AzureAI.Contracts.Dtos;
using System.Text;

namespace AzureAI.Common
{
    public class ApiStepTracer : IApiStepTracer
    {
        public List<ApiTraceStep> Steps { get; set; } = new();

        public ApiStepTracer()
        {
                
        }
        public ApiStepTracer(List<ApiTraceStep> steps)
        {
            Steps.AddRange(steps);
        }

        public void AddStep(ApiStepTypeEnum source, string title)
        {
            var step = new ApiTraceStep
            {
                Source = TraceStepperSourceEnumName.ToDisplayName(source),
                StepTitle = title,
                StepDateTime = DateTime.UtcNow
            };
            Steps.Add(step);
        }
        public void AddRange(List<ApiTraceStep> steps)
        {
            Steps.AddRange(steps);
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
                    $"[{step.StepDateTime?.ToLocalTime():yyyy-MM-dd HH:mm:ss.fff}] [{step.Source}] {step.StepTitle}");
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

        public void Clear()
        {
            Steps.Clear();
        }
    }
}
