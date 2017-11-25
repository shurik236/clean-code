using System.Collections.Generic;

namespace Markdown
{
    class TagConverter
    {
        public Tag GenerateTags(List<Token> tokens)
        {
            var paragraph = new Tag(TagType.Paragraph);
            var currentTag = paragraph;

            foreach (Token token in tokens)
            {
                switch (token.Type)
                {
                    case TokenType.Opening:
                        var newTag = new Tag(TagType.Unresolved, "");
                        currentTag.AddChild(newTag);
                        currentTag = newTag;
                        break;

                    case TokenType.Closing:
                        if (currentTag.Children.Count == 1 && currentTag.Children[0].Type == TagType.Italic)
                        {
                            currentTag.Children[0].Type = TagType.TextContainer;
                            currentTag.Type = TagType.Bold;
                        }
                        else
                            currentTag.Type = TagType.Italic;

                        currentTag = currentTag.Parent;
                        break;

                    default:
                        currentTag.AddChild(new Tag(TagType.Text, token.Value));
                        break;
                }
            }
            return paragraph;
        }
    }
}
