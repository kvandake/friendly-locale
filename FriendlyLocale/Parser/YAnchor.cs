namespace FriendlyLocale.Parser
{
    using System.Collections.Generic;

    internal class YAnchor : YNode
    {
        // https://gist.github.com/bowsersenior/979804
        private YAnchor(YNode value) 
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

        internal new static YNode Parse(Tokenizer tokenizer)
        {
            switch (tokenizer.Current.Kind)
            {
                case TokenKind.Anchor:
                    var name = tokenizer.Current.Value;
                    tokenizer.MoveNext();
                    var anchor = new YAnchor(YMapping.Parse(tokenizer));
                    tokenizer.Anchors[name] = anchor;
                    break;
            }

            return null;
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