using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    internal class HtmlBuilder
    {
        public string GenerateHtmlCode(IHtmlConvertible htmlConvertible)
        {
            return htmlConvertible.GetHtmlString();
        }
    }
}
