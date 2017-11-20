using System.Collections.Generic;

namespace Markdown
{
    public enum TagType
    {
        Unresolved,
        Text,
        Emphasis,
        Strong
    }

    public enum TagState{
        Irrelevant,
        Opening,
        Closing
    }

    class Tag
    {
        private static Dictionary<TagType, string> TagNames = new Dictionary<TagType, string>
        {
            {TagType.Unresolved, "" },
            {TagType.Emphasis, "em"},
            {TagType.Strong, "strong"}
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

        public string StringValue()
        {
            if (Type == TagType.Text)
                return Value;
            var closedTagMark = State == TagState.Closing ? "/" : "";

            return "<" + closedTagMark + TagNames[Type] + ">";

        }

    }
}
