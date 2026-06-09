using System.Text;

namespace DemoAzAI.Application
{
    public class ExecutionStepTracer : IExecutionStepTracer
    {
        private readonly List<ExecutionStep> Steps = new();

        public void ClearSteps()
        {
            Steps.Clear();
        }   
        
        public string AddApiStep(string source, string title)
        {

            Steps.Add(new ExecutionStep
            {
                Title = title,
                Source = "Api",
                DateTime = DateTime.UtcNow
            });

            return GetSteps();
        }

        public string AddUiStep(string title)
        {
            Steps.Add(new ExecutionStep
            {
                Title = title,
                Source = "UI",
                DateTime = DateTime.UtcNow
            });

            return GetSteps();
        }

        public string GetSteps()
        {
            if (Steps.Count == 0)
            {
                return string.Empty;
            }

            var builder = new StringBuilder();

            foreach (var step in Steps.OrderBy(x => x.DateTime))
            {
                builder.AppendLine(
                    $"[{step.DateTime?.ToLocalTime():yyyy-MM-dd HH:mm:ss.fff}] [{step.Source}] {step.Title}");
            }

            builder.AppendLine(GetTotalTime());

            return builder.ToString();
        }

        public  string GetTotalTime()
        {
            var orderedSteps = Steps
                .Where(x => x.DateTime.HasValue)
                .OrderBy(x => x.DateTime)
                .ToList();

            if (orderedSteps.Count < 2)
            {
                return "";
            }

            var firstStep = orderedSteps.First().DateTime!.Value;
            var lastStep = orderedSteps.Last().DateTime!.Value;

            var total = (lastStep - firstStep).TotalSeconds;
            return $"Total time: {total:0.000} seconds";
        }
    }
}
