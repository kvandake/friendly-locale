namespace FriendlyLocale.Parser.Nodes
{
    internal abstract class YNode
    {
        protected YNode(YNodeStyle style)
        {
            this.Style = style;
        }

        public YNodeStyle Style { get; }

        public override string ToString()
        {
            return this.ToString(this.Style);
        }

        public abstract string ToString(YNodeStyle style);

        public string ToYamlString()
        {
            return this.ToYamlString(this.Style);
        }

        public abstract string ToYamlString(YNodeStyle style);

        protected static YNode ToNode(object content)
        {
            switch (content)
            {
                case YNode node:
                    return node;
                case string contentString:
                    return new YScalar(contentString);
                default:
                    return null;
            }
        }

        protected static string AddIndent(string str)
        {
            return "  " + str.Replace("\n", "\n  ");
        }

        public static explicit operator string(YNode node)
        {
            return ((YScalar) node).Value;
        }
    }
}