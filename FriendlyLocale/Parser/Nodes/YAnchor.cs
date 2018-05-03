namespace FriendlyLocale.Parser.Nodes
{
    using System.Collections.Generic;

    internal class YAnchor : YNode
    {
        // https://gist.github.com/bowsersenior/979804
        internal YAnchor(YNode value)
            : base(YNodeStyle.Block)
        {
            this.Value = value;
        }

        public YNode Value { get; }

        public IEnumerable<YNode> ValueChildren
        {
            get
            {
                switch (this.Value)
                {
                    case YMapping mapping:
                        return mapping.Children;
                    case YDocument document:
                        return document.Children;
                    case YSequence sequence:
                        return sequence.Children;
                    default:
                        return new List<YNode>();
                }
            }
        }

        public override string ToString(YNodeStyle style)
        {
            return this.Value.ToString(style);
        }

        public override string ToYamlString(YNodeStyle style)
        {
            return this.Value.ToYamlString(style);
        }
    }
}