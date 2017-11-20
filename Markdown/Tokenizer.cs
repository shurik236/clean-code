using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    internal class Tokenizer
    {
        private readonly List<IParser> parsers;
        public Tokenizer(List<IParser> parsers)
        {
            this.parsers = parsers;
        }

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
            foreach (var parser in parsers)
            {
                var nextToken = parser.GetNextToken(input);
                if (nextToken != null)
                    return nextToken;
            }
            return new Token(input.Substring(0, 1), TokenType.Text);
        }
    }

    public interface IParser
    {
        Token GetNextToken(string str);
    }

    public class UnderlineParser : IParser
    {
        public Token GetNextToken(string str)
        {
            var underlineSubstr = string.Concat(str.TakeWhile(x => x == '_'));
            return underlineSubstr == "" ? null : new Token(underlineSubstr, TokenType.Underline);
        }
    }

    public class EscapedSymbolParser : IParser
    {
        public Token GetNextToken(string str)
        {
            var escapedSequences = new HashSet<string> { "\\_" };
            foreach (var sequence in escapedSequences)
            {
                if (!str.StartsWith(sequence)) continue;
                return new Token(sequence, TokenType.EscapedText);
            }
            return null;
        }
    }

    public class WhitespaceParser : IParser
    {
        public Token GetNextToken(string str)
        {
            var spaces = string.Concat(str.TakeWhile(x => x == ' '));
            return spaces == "" ? null : new Token(spaces, TokenType.Whitespace);
        }
    }
}
