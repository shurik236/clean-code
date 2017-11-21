using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    internal class Tokenizer
    {
        private readonly List<IParser> parsers;
        public Tokenizer(params IParser[] parsers)
        {
            this.parsers = new List<IParser>(parsers);
        }

        public IEnumerable<Token> Tokenize(string markdownString)
        {
            var k = 0;
            while (k < markdownString.Length)
            {
                var nextToken = ReadNextToken(markdownString.Substring(k));
                yield return nextToken;
                k += nextToken.Value.Length;
            }
        }

        private Token ReadNextToken(string input)
        {
            return parsers.Select(p => p.GetNextToken(input)).FirstOrDefault(token => token != null) ??
                new Token(input.Substring(0, 1), TokenType.Text);
        }
    }
}
