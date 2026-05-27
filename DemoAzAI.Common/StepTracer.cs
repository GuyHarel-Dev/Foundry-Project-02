using DemoAzAI.Contracts.Dtos;
using System.Text;

namespace DemoAzAI.Common
{
    public static class StepTracer
    {
        private static readonly List<StepTracerStep> Steps = new();

        public static void ClearSteps()
        {
            Steps.Clear();
        }   

        public static void AddStep(string source, string title, bool clearSteps = false)
        {
            if (clearSteps)
            {
                ClearSteps();
            }

            Steps.Add(new StepTracerStep
            {
                StepTitle = title,
                StepSource = source,
                StepDateTime = DateTime.UtcNow
            });

        }
        public static string GetSteps()
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

            builder.AppendLine(GetTotalTime());

            return builder.ToString();
        }

        public static string GetTotalTime()
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
