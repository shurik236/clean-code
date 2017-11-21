using System.Linq;

namespace Markdown
{
    public class UnderlineParser : IParser
    {
        public Token GetNextToken(string str)
        {
            var underlineSubstr = string.Concat(str.TakeWhile(x => x == '_'));
            return underlineSubstr == "" ? null : new Token(underlineSubstr, TokenType.Underline);
        }
    }
}