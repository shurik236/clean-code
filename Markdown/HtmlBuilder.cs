using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Markdown
{
    internal class HtmlBuilder
    {
        private readonly Dictionary<TokenType, string> tagNames;

        public HtmlBuilder()
        {
            tagNames = new Dictionary<TokenType, string>
            {
                {TokenType.Emphasis, "em"},
                {TokenType.Strong, "strong" }
            };
        }

        public string HtmlFromTokens(List<Token> tokens)
        {
            var tagStack = new Stack<Token>();
            var strBuilder = new StringBuilder();
            foreach (var token in tokens)
            {
                if (!token.IsTag())
                {
                    strBuilder.Append(WebUtility.HtmlEncode(token.Value));
                    continue;
                }

                if (!tagStack.Any() || tagStack.Peek().Type != token.Type)
                {
                    tagStack.Push(token);
                    strBuilder.Append($"<{tagNames[token.Type]}>");
                }
                else
                {
                    tagStack.Pop();
                    strBuilder.Append($"</{tagNames[token.Type]}>");
                }    
            }

            return strBuilder.ToString();
        }
    }
}
