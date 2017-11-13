using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
	public class Md
	{
        private readonly Tokenizer tokenizer = new Tokenizer();
        private readonly SyntaxProcessor syntaxProcessor = new SyntaxProcessor();
        private readonly HtmlBuilder htmlBuilder = new HtmlBuilder();
		public string RenderToHtml(string markdown)
		{
		    var tokens = tokenizer.Tokenize(markdown);
		    var fixedTokens = syntaxProcessor.FixSyntaxErrors(tokens);

		    return htmlBuilder.HtmlFromTokens(fixedTokens);
		}
	}

	[TestFixture]
	public class Md_ShouldRenderSpecStrings
	{
	    private static Md _mdRenderer;

	    [SetUp]
	    public void SetUp()
	    {
	        _mdRenderer = new Md();
	    }

	    [TestCase("", "", 
            TestName = "Empty string -> empty paragraph")]
        [TestCase("abcdef123", "abcdef123", 
            TestName = "abcdef123 -> abcdef123")]
        [TestCase("_abcdef123_", "<em>abcdef123</em>", 
            TestName = "_abcdef123_ -> <em>abcdef123</em>")]
	    [TestCase("__abcdef123__", "<strong>abcdef123</strong>", 
            TestName = "__abcdef123__ -> <strong>abcdef123</strong>")]
        [TestCase(@"\_abc123\_", "_abc123_",
            TestName = "\\_abc123\\_ -> _abc123_")]
        [TestCase("I __have _been_ asleep__", "I <strong>have <em>been</em> asleep</strong>", 
            TestName = "em inside strong")]
        [TestCase("I _have __been__ asleep_", "I <em>have <strong>been</strong> asleep</em>", 
            TestName = "strong inside em")]
        [TestCase("numbers_12_3", "numbers_12_3", 
            TestName = "numbers_12_3 -> numbers_12_3")]
        [TestCase("_numbers_12_3_", "<em>numbers_12_3</em>", 
            TestName = "_numbers_12_3_ -> <em>numbers_12_3</em>")]
        [TestCase("_abcdef123", "_abcdef123", 
            TestName = "_abcdef123 -> _abcdef123")]
        [TestCase("__without _pair", "__without _pair",
            TestName = "__without _pair -> __without _pair")]
	    [TestCase("__abcdef123_", "<em>_abcdef123</em>",
	        TestName = "__abcdef123_ -> <em>_abcdef123</em>")]
        [TestCase("left __has_ more", "left <em>_has</em> more",
            TestName = "left __has_ more -> left <em>_has</em> more")]
        [TestCase("right _has__ more", "right <em>has_</em> more",
            TestName = "right _has__ more -> right <em>has_</em> more")]
        public void RenderString(string markdownString, string expectedHtmlString)
	    {
	        _mdRenderer.RenderToHtml(markdownString).Should().BeEquivalentTo(expectedHtmlString);
	    }
	    
	}
}
