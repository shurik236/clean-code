using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    internal class SyntaxProcessorShould
    {
        [Test]
        public void UnescapeUnderscore()
        {
            var tokens = new List<Token>
            {
                new Token("\\_", TokenType.EscapedText),
                new Token("\\_", TokenType.EscapedText)
            };
            var expected = new List<Token>
            {
                new Token("_", TokenType.Text),
                new Token("_", TokenType.Text)
            };
            expected.ShouldBeEquivalentTo(new SyntaxProcessor().FixSyntaxErrors(tokens));
        }

        [Test]
        public void TextifyInvalidTags_WhenInline()
        {
            var tokens = new List<Token>
            {
                new Token("a", TokenType.Text),
                new Token("_", TokenType.Underline),
                new Token("a", TokenType.Text)
            };
            var expected = new List<Token>
            {
                new Token("a", TokenType.Text),
                new Token("_", TokenType.Text),
                new Token("a", TokenType.Text)
            };
            expected.ShouldBeEquivalentTo(new SyntaxProcessor().FixSyntaxErrors(tokens));
        }

        [Test]
        public void TextifyInvalidTags_WhenBetweenSpaces()
        {
            var tokens = new List<Token>
            {
                new Token(" ", TokenType.Whitespace),
                new Token("_", TokenType.Underline),
                new Token(" ", TokenType.Whitespace)
            };
            var expected = new List<Token>
            {
                new Token(" ", TokenType.Whitespace),
                new Token("_", TokenType.Text),
                new Token(" ", TokenType.Whitespace)
            };
            expected.ShouldBeEquivalentTo(new SyntaxProcessor().FixSyntaxErrors(tokens));
        }
    }
}
