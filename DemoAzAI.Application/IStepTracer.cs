using System.Text;

namespace DemoAzAI.Application
{
    public interface IStepTracer
    {
        void ClearSteps();
        void AddStep(string source, string title, bool clearSteps = false);
        string GetSteps();
        string GetTotalTime();

    }

}
