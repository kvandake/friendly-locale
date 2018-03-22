namespace FriendlyLocale.Parser
{
    internal class YAlias : YNode
    {
        private YAlias(string name, YAnchor anchor)
            : base(YNodeStyle.Block)
        {
            this.Name = name;
            this.Anchor = anchor;
        }

        public string Name { get; }

        public YAnchor Anchor { get; }
       

        internal new static YNode Parse(Tokenizer tokenizer)
        {
            switch (tokenizer.Current.Kind)
            {
                case TokenKind.Alias:
                    var anchorName = tokenizer.Current.Value;
                    var anchorValue = tokenizer.Anchors[anchorName];
                    tokenizer.MoveNext();
                    return new YAlias(anchorName, anchorValue);
            }

            return null;
        }

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