using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    internal class SyntaxProcessor
    {
        private const int MaxTagCapacity = 3;

        public List<Token> FixSyntaxErrors(List<Token> tokens)
        {
            return ResolveNonMatchingTags(
                ResolveOpeningClosingSequences(
                TextifyInlineOrInspaceUndescore(
                UnescapeSpecialSymbols(tokens))));
        }

        private List<Token> ResolveNonMatchingTags(List<Token> tokens)
        {
            var openTagsStack = new Stack<int>();

            for (var i = 0; i < tokens.Count; i++)
            {
                switch (tokens[i].Type)
                {
                    case TokenType.Opening:
                        openTagsStack.Push(i);
                        continue;
                    case TokenType.Closing:
                        if (!openTagsStack.Any())
                            tokens[i].Type = TokenType.Text;
                        else
                            openTagsStack.Pop();
                        break;
                }
            }

            while (openTagsStack.Any())
            {
                tokens[openTagsStack.Pop()].Type = TokenType.Text;
            }

            return tokens;
        }
        
        private List<Token> UnpackOpeningSequence(Token openingSequence, int tagCapacity)
        {
            var result = new List<Token>();
            for (var i = 0; i<openingSequence.Value.Length; i++)
            {
                if (i - tagCapacity < 0)
                    result.Add(new Token("_", TokenType.Opening));
                else
                    result.Add(new Token("_", TokenType.Text));
            }
            return result;
        }

        private List<Token> UnpackClosingSequence(Token closingSequence, int tagCapacity)
        {
            var result = new List<Token>();
            for (var i = 0; i < closingSequence.Value.Length; i++)
            {
                if (i + tagCapacity - closingSequence.Value.Length < 0)
                    result.Add(new Token("_", TokenType.Text));
                else
                    result.Add(new Token("_", TokenType.Closing));
            }
            return result;
        }

        private List<Token> ResolveOpeningClosingSequences(List<Token> tokens)
        {
            var resolved = new List<Token>();
            for (var i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].Type != TokenType.Underline)
                {
                    resolved.Add(tokens[i]);
                    continue;
                }
                
                var openingSequence = i != tokens.Count-1 &&
                                        tokens[i+1].Type != TokenType.Whitespace &&
                                        (i == 0 || tokens[i - 1].Type != TokenType.Text);

                var closingSequence = i != 0 &&
                                        tokens[i - 1].Type != TokenType.Whitespace &&
                                        (i == tokens.Count - 1 || tokens[i + 1].Type != TokenType.Text);

                if (openingSequence)
                    resolved.AddRange(UnpackOpeningSequence(tokens[i], MaxTagCapacity));
                else if (closingSequence)
                    resolved.AddRange(UnpackClosingSequence(tokens[i], MaxTagCapacity));                
            }

            return resolved;
        }

        private List<Token> TextifyInlineOrInspaceUndescore(List<Token> tokens)
        {
            if (tokens.Count < 3) return tokens;

            if (tokens.First().Type == TokenType.Underline && tokens[1].Type == TokenType.Whitespace)
                tokens.First().Type = TokenType.Text;
            if (tokens.Last().Type == TokenType.Underline && tokens[tokens.Count-2].Type == TokenType.Whitespace)
                tokens.Last().Type = TokenType.Text;

            for (var i = 1; i < tokens.Count-1; i++)
            {
                var betweenText = tokens[i - 1].Type == TokenType.Text &&
                    tokens[i + 1].Type == TokenType.Text;
                var betweenSpaces = tokens[i - 1].Type == TokenType.Whitespace && 
                    tokens[i + 1].Type == TokenType.Whitespace;

                if (tokens[i].Type != TokenType.Underline) continue;
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
