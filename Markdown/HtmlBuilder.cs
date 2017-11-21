using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    internal class HtmlBuilder
    {
        public string HtmlFromTags<T>(List<T> tags) where T : IHtmlConvertible
        {
            var strBuilder = new StringBuilder();
            foreach (var token in tags)
            {
                strBuilder.Append(token.GetHtmlString());
            }

            return strBuilder.ToString();
        }
    }
}
