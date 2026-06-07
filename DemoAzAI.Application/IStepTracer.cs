using System.Text;

namespace DemoAzAI.Application
{
    public interface IStepTracer
    {
        void ClearSteps();
        void AddStep(string source, string title);
        void AddUiStep(string message);
        string GetSteps();
        string GetTotalTime();

    }

}
