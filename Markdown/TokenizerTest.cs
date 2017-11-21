using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    internal class TokenizerShould
    {
        private static Tokenizer _tokenizer;
        [SetUp]
        public void SetUp()
        {
            _tokenizer = new Tokenizer(
                new WhitespaceParser(),
                new EscapedSymbolsParser(@"\_"),
                new UnderlineParser()
                );
        }
        [Test]
        public void ReturnEmptyList_WhenGivenEmptyString()
        {
            _tokenizer.Tokenize("").Should().BeEmpty();
        }

        [Test]
        public void ReturnTextByLetters_WhenNoSpecialSequence()
        {
            var expectedList = new List<Token>
            {
                new Token("a", TokenType.Text),
                new Token("b", TokenType.Text),
                new Token("1", TokenType.Text),
                new Token("2", TokenType.Text),
                new Token("3", TokenType.Text)
            };
            _tokenizer.Tokenize("ab123").ShouldBeEquivalentTo(expectedList);
        }

        [Test]
        public void ReturnTokenizedWhitespace_WhenGivenTextWithSpaces()
        {
            var expectedList = new List<Token>
            {
                new Token("a", TokenType.Text),
                new Token("b", TokenType.Text),
                new Token(" ", TokenType.Whitespace),
                new Token("2", TokenType.Text),
                new Token("3", TokenType.Text)
            };
            _tokenizer.Tokenize("ab 23").ShouldBeEquivalentTo(expectedList);
        }

        [Test]
        public void ReturnTokenizedEmphasis_WhenEmphasisEncountered()
        {
            var expectedList = new List<Token>
            {
                new Token("_", TokenType.Underline),
                new Token("a", TokenType.Text),
                new Token("b", TokenType.Text),
                new Token("c", TokenType.Text),
                new Token("_", TokenType.Underline)
            };
            _tokenizer.Tokenize("_abc_").ShouldBeEquivalentTo(expectedList);
        }

        [Test]
        public void ReturnTokenizedStrong_WhenStrongEncountered()
        {
            var expectedList = new List<Token>
            {
                new Token("__", TokenType.Underline),
                new Token("a", TokenType.Text),
                new Token("b", TokenType.Text),
                new Token("c", TokenType.Text),
                new Token("__", TokenType.Underline)
            };
            _tokenizer.Tokenize("__abc__").ShouldBeEquivalentTo(expectedList);
        }

        [Test]
        public void ReturnEscapedUnderscores_WhenGivenEscapedUnderscores()
        {
            var expectedList = new List<Token>
            {
                new Token("\\_", TokenType.EscapedText),
                new Token("a", TokenType.Text)
            };
            _tokenizer.Tokenize("\\_a").ShouldBeEquivalentTo(expectedList);
        }
    }
}
