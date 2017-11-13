using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    internal class Tokenizer
    {
        private readonly Dictionary<string, TokenType> specialSequencesTypes = new Dictionary<string, TokenType>
        {
            {"_", TokenType.Emphasis },
            {"__", TokenType.Strong },
            {"\\_", TokenType.EscapedText },
            {" ", TokenType.Whitespace }
        };

        public List<Token> Tokenize(string markdownString)
        {
            var tokenList = new List<Token>();
            var k = 0;
            while (k < markdownString.Length)
            {
                var nextToken = ReadNextToken(markdownString.Substring(k));
                tokenList.Add(nextToken);
                k += nextToken.Value.Length;
            }

            return tokenList;
        }

        private Token ReadNextToken(string input)
        {
            var prefix = ""+input[0];
            while (prefix.Length < input.Length && IsPossiblySpecialSequence(prefix + input[prefix.Length]))
            {
                prefix += input[prefix.Length];
            }

            return !specialSequencesTypes.ContainsKey(prefix) ?
                new Token(prefix, TokenType.Text) :
                new Token(prefix, specialSequencesTypes[prefix]);
        }

        private bool IsPossiblySpecialSequence(string str)
        {
            return specialSequencesTypes.Keys.Any(x => x.StartsWith(str));
        }
    }
}
