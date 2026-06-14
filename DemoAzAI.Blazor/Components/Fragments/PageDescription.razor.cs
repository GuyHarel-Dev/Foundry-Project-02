namespace DemoAzAI.Blazor.Components.Fragments
{
    public partial class PageDescription
    {
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
