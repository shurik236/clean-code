using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    class TagConverter
    {
        public List<Tag> GenerateTags(List<Token> tokens)
        {
            var openTagsStack = new Stack<int>();
            var tags = new List<Tag>();
            for (var i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].Type == TokenType.Opening)
                {
                    var openings = AcquireOpeningSequence(i, tokens);
                    {
                        for (var j = 0; j < openings.Count; j++)
                        {
                            openTagsStack.Push(i);
                            tags.Add(new Tag(TagType.Unresolved, TagState.Opening, ""));
                            i++;
                        }
                        i--;
                        continue;
                    }
                }

                var closings = AcquireClosingSequence(i, tokens);
                if (!closings.Any()) tags.Add(new Tag(TagType.Text, TagState.Irrelevant, tokens[i].Value));

                if (closings.Count == 1)
                {
                    var openingTagPosition = openTagsStack.Pop();
                    tags[openingTagPosition].Type = TagType.Emphasis;
                    tags.Add(new Tag(TagType.Emphasis, TagState.Closing, tokens[i].Value));
                }
                else if (closings.Count == 2)
                {
                    var openingTagPosition = openTagsStack.Pop();
                    var prevOpeningTagPosition = openTagsStack.Peek();
                    if (prevOpeningTagPosition + 1 == openingTagPosition)
                    {
                        openTagsStack.Pop();
                        tags[openingTagPosition].Clear();
                        tags[prevOpeningTagPosition].Type = TagType.Strong;
                        tags.Add(new Tag(TagType.Text, TagState.Irrelevant, ""));
                        tags.Add(new Tag(TagType.Strong, TagState.Closing, ""));
                        i++;
                    }
                    else
                    {
                        tags[openingTagPosition].Type = TagType.Emphasis;
                        tags.Add(new Tag(TagType.Emphasis, TagState.Closing, ""));
                    }
                }
            }
            return tags;
        }

        private List<Token> AcquireOpeningSequence(int start, List<Token> tokens)
        {
            var result = new List<Token>();
            for (var i = start; i < tokens.Count; i++)
            {
                if (tokens[i].Type != TokenType.Opening)
                    break;
                result.Add(tokens[i]);
            }

            return result;
        }

        private List<Token> AcquireClosingSequence(int start, List<Token> tokens)
        {
            var result = new List<Token>();
            for (var i = start; i < tokens.Count; i++)
            {
                if (tokens[i].Type != TokenType.Closing || result.Count == 2)
                    break;
                result.Add(tokens[i]);
            }

            return result;
        }
    }
}
