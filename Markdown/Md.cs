using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
	public class Md
	{
        private readonly Tokenizer tokenizer = new Tokenizer(
            new WhitespaceParser(),
            new EscapedSymbolsParser(@"\_"),
            new UnderlineParser()
            );

        private readonly SyntaxProcessor syntaxProcessor = new SyntaxProcessor();
        private readonly HtmlBuilder htmlBuilder = new HtmlBuilder();
        private readonly TagConverter tagConverter = new TagConverter();
		public string RenderToHtml(string markdown)
		{
		    var tokens = tokenizer.Tokenize(markdown);
		    var fixedTokens = syntaxProcessor.FixSyntaxErrors(tokens.ToList());
		    var paragraph = tagConverter.GenerateTags(fixedTokens);
		    return htmlBuilder.GenerateHtmlCode(paragraph);
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

	    [TestCase("", "<p></p>", 
            TestName = "Empty string -> empty paragraph")]
        [TestCase("abcdef123", "<p>abcdef123</p>", 
            TestName = "abcdef123 -> <p>abcdef123</p>")]
        [TestCase("_abcdef123_", "<p><em>abcdef123</em></p>", 
            TestName = "_abcdef123_ -> <p><em>abcdef123</em></p>")]
	    [TestCase("__abcdef123__", "<p><strong>abcdef123</strong></p>", 
            TestName = "__abcdef123__ -> <p><strong>abcdef123</strong></p>")]
        [TestCase(@"\_abc123\_", "<p>_abc123_</p>",
            TestName = "\\_abc123\\_ -> <p>_abc123_</p>")]
        [TestCase("I __have _been_ asleep__", "<p>I <strong>have <em>been</em> asleep</strong></p>", 
            TestName = "em inside strong")]
        [TestCase("I _have __been__ asleep_", "<p>I <em>have <strong>been</strong> asleep</em></p>", 
            TestName = "strong inside em")]
        [TestCase("numbers_12_3", "<p>numbers_12_3</p>", 
            TestName = "numbers_12_3 -> <p>numbers_12_3</p>")]
        [TestCase("_numbers_12_3_", "<p><em>numbers_12_3</em></p>", 
            TestName = "_numbers_12_3_ -> <p><em>numbers_12_3</em></p>")]
        [TestCase("_abcdef123", "<p>_abcdef123</p>", 
            TestName = "_abcdef123 -> <p>_abcdef123</p>")]
        [TestCase("__without _pair", "<p>__without _pair</p>",
            TestName = "__without _pair -> <p>__without _pair</p>")]
	    [TestCase("__abcdef123_", "<p>_<em>abcdef123</em></p>",
	        TestName = "__abcdef123_ -> <p>_<em>abcdef123</em></p>")]
        [TestCase("left __has_ more", "<p>left _<em>has</em> more</p>",
            TestName = "left __has_ more -> <p>left _<em>has</em> more</p>")]
        [TestCase("right _has__ more", "<p>right <em>has</em>_ more</p>",
            TestName = "right _has__ more -> <p>right <em>has</em>_ more</p>")]
        [TestCase("_bold__italic_", "<p><em>bold__italic</em></p>",
            TestName = "_bold__italic_ -> <p><em>bold__italic</em></p>")]
        [TestCase("___italic_bold", "<p>___italic_bold</p>",
            TestName = "___italic_bold -> <p>___italic_bold</p>")]
        [TestCase("___bold italic___", "<p><em><strong>bold italic</strong></em></p>",
            TestName = "___bold italic___ -> <p><em><strong>bold italic</strong></em></p>")]
        [TestCase("_kek___kek_", "<p><em>kek___kek</em></p>",
            TestName = "_kek___kek_ -> <p><em>kek___kek</em></p>")]
        [TestCase("___kek__ kek_", "<p><em><strong>kek</strong> kek</em></p>",
            TestName = "___kek__ kek_ -> <p><em><strong>kek</strong> kek</em></p>")]
        public void RenderString(string markdownString, string expectedHtmlString)
	    {
	        _mdRenderer.RenderToHtml(markdownString).Should().BeEquivalentTo(expectedHtmlString);
	    }
	    
	}
}
