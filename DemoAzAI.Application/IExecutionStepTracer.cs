using System.Text;

namespace DemoAzAI.Application
{
    public interface IExecutionStepTracer
    {
        void ClearSteps();
        string AddApiStep(string source, string title);
        string AddUiStep(string message);
        string GetSteps();
        string GetTotalTime();

    }

}
