using System.Linq;

namespace Markdown
{
    public class WhitespaceParser : IParser
    {
        public Token GetNextToken(string str)
        {
            var spaces = string.Concat(str.TakeWhile(x => x == ' '));
            return spaces == "" ? null : new Token(spaces, TokenType.Whitespace);
        }
    }
}