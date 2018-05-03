namespace FriendlyLocale.Parser.Nodes
{
    internal class YAlias : YNode
    {
        internal YAlias(string name, YAnchor anchor)
            : base(YNodeStyle.Block)
        {
            this.Name = name;
            this.Anchor = anchor;
        }

        public string Name { get; }

        public YAnchor Anchor { get; }

        public override string ToString(YNodeStyle style)
        {
            return this.Name;
        }

        public override string ToYamlString(YNodeStyle style)
        {
            return this.Anchor.ToYamlString(style);
        }
    }
}