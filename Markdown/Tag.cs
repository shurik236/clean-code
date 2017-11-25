using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Markdown
{
    public enum TagType
    {
        Unresolved,
        Text,
        Italic,
        Bold,
        Paragraph,
        TextContainer
    }

    class Tag : IHtmlConvertible
    {
        private static Dictionary<TagType, string> TagNames = new Dictionary<TagType, string>
        {
            {TagType.Paragraph, "p" },
            {TagType.Unresolved, "" },
            {TagType.Italic, "em"},
            {TagType.Bold, "strong"}
        };

        public TagType Type { get; set; }
        public string Value { get; set; }
        public Tag Parent { get; private set; }
        public List<Tag> Children { get; }

        public Tag(TagType type)
        {
            Type = type;
            Value = "";
            Children = new List<Tag>();
        }

        public Tag(TagType type, string str)
        {
            Type = type;
            Value = str;
            Children = new List<Tag>();
        }

        public void AddChild(Tag child)
        {
            child.Parent = this;
            Children.Add(child);
        }

        public string GetHtmlString()
        {
            if (Type == TagType.Text)
                return HttpUtility.HtmlEncode(Value);

            StringBuilder sb = new StringBuilder();
            Children.ForEach(x => sb.Append(x.GetHtmlString()));

            return Type == TagType.TextContainer ? sb.ToString() : $"<{TagNames[Type]}>{sb}</{TagNames[Type]}>";
        }
    }
}
