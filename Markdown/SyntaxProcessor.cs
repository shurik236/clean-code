using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    internal class SyntaxProcessor
    {
        public List<Token> FixSyntaxErrors(List<Token> tokens)
        {
            return UnescapeSpecialSymbols(
                ResolveNonMatchingTags(
                    FixInlineOrInspaceUndescore(tokens)));
        }
        private List<Token> ResolveNonMatchingTags(List<Token> tokens)
        {
            var tagPositionStack = new Stack<int>();
            var openingTagsToFix = new HashSet<int>();
            var closingTagsToFix = new HashSet<int>();

            for (var i = 0; i < tokens.Count; i++)
            {
                if (!tokens[i].IsTag()) continue;

                var openingTag = i != tokens.Count - 1 &&
                                 tokens[i + 1].Type != TokenType.Whitespace &&
                                 (i == 0 || tokens[i - 1].Type == TokenType.Whitespace);

                var closingTag = i != 0 &&
                                 tokens[i - 1].Type != TokenType.Whitespace &&
                                 (i == tokens.Count - 1 || tokens[i + 1].Type == TokenType.Whitespace);

                if (openingTag) tagPositionStack.Push(i);
                if (!closingTag) continue;
                if (!tagPositionStack.Any())
                    tokens[i].Type = TokenType.Text;
                else
                {
                    var lastOpenTagIndex = tagPositionStack.Pop();
                    if (tokens[lastOpenTagIndex].Type == TokenType.Emphasis && tokens[i].Type == TokenType.Strong)
                    {
                        tokens[i].Type = TokenType.Emphasis;
                        closingTagsToFix.Add(i);
                    }
                    if (tokens[lastOpenTagIndex].Type == TokenType.Strong && tokens[i].Type == TokenType.Emphasis)
                    {
                        tokens[lastOpenTagIndex].Type = TokenType.Emphasis;
                        openingTagsToFix.Add(lastOpenTagIndex);
                    }
                }
            }

            while (tagPositionStack.Any())
                tokens[tagPositionStack.Pop()].Type = TokenType.Text;
            return FixInvalidTags(openingTagsToFix, closingTagsToFix, tokens);
        }

        private List<Token> FixInvalidTags(HashSet<int> badOpeningPositions, HashSet<int> badClosingPositions, List<Token> tokens)
        {
            var fixedTokens = new List<Token>();
            for (var i = 0; i < tokens.Count; i++)
            {
                if (badOpeningPositions.Contains(i))
                {
                    fixedTokens.Add(tokens[i]);
                    fixedTokens.Add(new Token("_", TokenType.Text));
                }
                else if (badClosingPositions.Contains(i))
                {
                    fixedTokens.Add(new Token("_", TokenType.Text));
                    fixedTokens.Add(tokens[i]);
                }
                else
                    fixedTokens.Add(tokens[i]);
            }
            return fixedTokens;
        }

        private List<Token> FixInlineOrInspaceUndescore(List<Token> tokens)
        {
            if (tokens.Count < 3) return tokens;

            if (tokens.First().IsTag() && tokens[1].Type == TokenType.Whitespace)
                tokens.First().Type = TokenType.Text;
            if (tokens.Last().IsTag() && tokens[tokens.Count-2].Type == TokenType.Whitespace)
                tokens.Last().Type = TokenType.Text;

            for (var i = 1; i < tokens.Count-1; i++)
            {
                var betweenText = tokens[i - 1].Type == TokenType.Text &&
                    tokens[i + 1].Type == TokenType.Text;
                var betweenSpaces = tokens[i - 1].Type == TokenType.Whitespace && 
                    tokens[i + 1].Type == TokenType.Whitespace;

                if (!tokens[i].IsTag()) continue;
                if (betweenText || betweenSpaces)
                    tokens[i].Type = TokenType.Text;
            }

            return tokens;
        }

        private List<Token> UnescapeSpecialSymbols(List<Token> tokens)
        {
            foreach (var t in tokens)
            {
                if (t.Type != TokenType.EscapedText) continue;
                t.Type = TokenType.Text;
                t.Value = t.Value.TrimStart('\\');
            }

            return tokens;
        }
    }
}
