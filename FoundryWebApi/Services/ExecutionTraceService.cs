namespace WebApi.Services
{
    public class ExecutionTraceService
    {
        private readonly List<string> traces = new List<string>();

        public ExecutionTraceService()
        {
            traces.Add(traces.Count + 1 + ". Execution started");
        }

        public void AddTrace(string trace)
        {
            traces.Add(trace);
        }
    }
}
