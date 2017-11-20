using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Markdown
{
    internal class HtmlBuilder
    {
        public string HtmlFromTags(List<Tag> tags)
        {
            var strBuilder = new StringBuilder();
            foreach (var token in tags)
            {
                strBuilder.Append(token.StringValue());
            }

            return strBuilder.ToString();
        }
    }
}
