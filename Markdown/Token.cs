using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public enum TokenType
    {
        Text,
        Whitespace,
        Underline,
        EscapedText,
        Opening,
        Closing
    }
    public class Token
    {
        public string Value { get; set; }
        public TokenType Type { get; set; }

        public Token(string value, TokenType type)
        {
            Value = value;
            Type = type;
        }

        public void Clear()
        {
            Type = TokenType.Text;
            Value = "";
        }
    }
}
