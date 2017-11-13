namespace Markdown
{
    internal enum TokenType
    {
        Text,
        Whitespace,
        Emphasis,
        Strong,
        EscapedText
    }
    internal class Token
    {
        public string Value { get; set; }
        public TokenType Type { get; set; }

        public Token(string value, TokenType type)
        {
            Value = value;
            Type = type;
        }

        public bool IsTag()
        {
            return Type == TokenType.Emphasis || Type == TokenType.Strong;
        }
    }
}
