using System.Collections.Generic;
using System.Web;

namespace Markdown
{
    public enum TagType
    {
        Unresolved,
        Text,
        Italic,
        Bold
    }

    public enum TagState{
        Irrelevant,
        Opening,
        Closing
    }

    class Tag : IHtmlConvertible
    {
        private static Dictionary<TagType, string> TagNames = new Dictionary<TagType, string>
        {
            {TagType.Unresolved, "" },
            {TagType.Italic, "em"},
            {TagType.Bold, "strong"}
        };

        public TagType Type { get; set; }
        public string Value { get; set; }
        public TagState State { get; set; }


        public Tag(TagType type)
        {
            Type = type;
            Value = "";
        }

        public Tag(TagType type, TagState state, string str)
        {
            Type = type;
            State = state;
            Value = str;
        }

        public void Clear()
        {
            State = TagState.Irrelevant;
            Type = TagType.Text;
            Value = "";
        }

        public string GetHtmlString()
        {
            if (Type == TagType.Text)
                return HttpUtility.HtmlEncode(Value);
            var closedTagMark = State == TagState.Closing ? "/" : "";

            return $"<{closedTagMark}{TagNames[Type]}>";

        }

    }
}
