using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    class TagConverter
    {
        private const int TagLengthLimit = 2;
        public List<Tag> GenerateTags(List<Token> tokens)
        {
            var openTagsStack = new Stack<int>();
            var tags = new List<Tag>();
            for (var i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].Type == TokenType.Opening)
                {
                    var openings = AcquireOpeningSequence(i, tokens).ToArray();
                    for (var j = 0; j < openings.Length; j++)
                    {
                        openTagsStack.Push(i);
                        tags.Add(new Tag(TagType.Unresolved, TagState.Opening, ""));
                        i++;
                    }
                    i--;
                    continue;
                }

                var closings = AcquireClosingSequence(i, tokens).ToArray();
                if (!closings.Any()) tags.Add(new Tag(TagType.Text, TagState.Irrelevant, tokens[i].Value));

                if (closings.Length == 1)
                {
                    var openingTagPosition = openTagsStack.Pop();
                    tags[openingTagPosition].Type = TagType.Italic;
                    tags.Add(new Tag(TagType.Italic, TagState.Closing, tokens[i].Value));
                }
                else if (closings.Length == 2)
                {
                    var openingTagPosition = openTagsStack.Pop();
                    var prevOpeningTagPosition = openTagsStack.Peek();
                    if (prevOpeningTagPosition + 1 == openingTagPosition)
                    {
                        openTagsStack.Pop();
                        tags[openingTagPosition].Clear();
                        tags[prevOpeningTagPosition].Type = TagType.Bold;
                        tags.Add(new Tag(TagType.Text, TagState.Irrelevant, ""));
                        tags.Add(new Tag(TagType.Bold, TagState.Closing, ""));
                        i++;
                    }
                    else
                    {
                        tags[openingTagPosition].Type = TagType.Italic;
                        tags.Add(new Tag(TagType.Italic, TagState.Closing, ""));
                    }
                }
            }
            return tags;
        }

        private IEnumerable<Token> AcquireOpeningSequence(int start, List<Token> tokens)
        {
            for (var i = start; i < tokens.Count; i++)
            {
                if (tokens[i].Type != TokenType.Opening)
                    break;
                yield return tokens[i];
            }

        }

        private IEnumerable<Token> AcquireClosingSequence(int start, List<Token> tokens)
        {
            var sequenceLength = 0;
            for (var i = start; i < tokens.Count; i++)
            {
                if (tokens[i].Type != TokenType.Closing || sequenceLength == TagLengthLimit)
                    break;
                yield return tokens[i];
                sequenceLength++;
            }
        }
    }
}
