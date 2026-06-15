namespace DemoAzAI.Blazor.Components.Fragments
{
    public partial class PageDescription
    {
        private bool DisplayPageLogArea = false;

        public string GetPageLogClass()
        {
            if (!string.IsNullOrEmpty(PageLog))
                return "page-area page-log-visible";

            return "page-area page-log-invisible";
        }

        public string GetPageStateClass()
        {
            if (PageState == "error")
            {
                return "page-error";
            }

            return "page-ok";
        }
    }
}
