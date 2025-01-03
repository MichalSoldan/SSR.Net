using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SSR.Net.DotNet8.Extensions
{
    public static class TestExtension
    {
        public static IHtmlContent TestReact(this IHtmlHelper htmlHelper)
        {

            return htmlHelper.Raw("<div id=\"test\">Test</div>");
        }
    }
}
