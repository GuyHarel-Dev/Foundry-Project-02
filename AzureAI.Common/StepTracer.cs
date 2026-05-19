using AzureAI.Contracts.Dtos;
using System.Text;

namespace AzureAI.Common
{
    public static class StepTracer
    {
        public static void AddStep(List<StepTracerStepDto> Steps, string source, string title)
        {
            Steps.Add(new StepTracerStepDto
            {
                StepTitle = title,
                StepSource = source,
                StepDateTime = DateTime.UtcNow
            });
        }
        public static string GetTrace(List<StepTracerStepDto> Steps)
        {
            if (Steps.Count == 0)
            {
                return string.Empty;
            }

            var builder = new StringBuilder();

            foreach (var step in Steps.OrderBy(x => x.StepDateTime))
            {
                builder.AppendLine(
                    $"[{step.StepDateTime?.ToLocalTime():yyyy-MM-dd HH:mm:ss.fff}] [{step.StepSource}] {step.StepTitle}");
            }

            builder.AppendLine(GetTotalTime(Steps));

            return builder.ToString();
        }

        public static string GetTotalTime(List<StepTracerStepDto> Steps)
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

            var total = (lastStep - firstStep).TotalSeconds;
            return $"Total time: {total:0.000} seconds";
        }
    }
}
