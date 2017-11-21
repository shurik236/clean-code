using System.Collections.Generic;

namespace Markdown
{
    public class EscapedSymbolsParser : IParser
    {
        private readonly HashSet<string> escapedSequences;

        public EscapedSymbolsParser(params string[] escapedSequences)
        {
            this.escapedSequences = new HashSet<string>(escapedSequences);
        }

        public Token GetNextToken(string str)
        {
            foreach (var sequence in escapedSequences)
            {
                if (!str.StartsWith(sequence)) continue;
                return new Token(sequence, TokenType.EscapedText);
            }
            return null;
        }
    }
}