using System.Collections.Generic;
using System.Linq;

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
            return 
                (from sequence in escapedSequences
                 where str.StartsWith(sequence)
                 select new Token(sequence, TokenType.EscapedText)).FirstOrDefault();
        }
    }
}
