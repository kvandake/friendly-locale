namespace FriendlyLocale.Parser.Nodes
{
    using System.Collections.Generic;
    using System.Linq;

    internal class YMapping : YCollection<YMapping.YKeyValuePair>
    {
        public YMapping(YNodeStyle style = YNodeStyle.Block, params YNode[] content)
            : base(style, content)
        {
        }

        protected override YNode FirstNode => this.Children.FirstOrDefault();
        protected override YNode LastNode => this.Children.LastOrDefault();

        public YNode this[YScalar key] => this.Children.FirstOrDefault(i => i.Key.Equals(key))?.Value;

        public override void Add(params YNode[] content)
        {
            this.Children.AddRange(Flattern(content).OfType<YKeyValuePair>());
        }

        public override void AddFirst(params object[] content)
        {
            this.Children.InsertRange(0, Flattern(content).OfType<YKeyValuePair>());
        }

        public override void RemoveNodes()
        {
            this.Children.Clear();
        }

        protected internal override YNode GetPreviousNode(YNode node)
        {
            return node == this.FirstNode ? null :
                node is YKeyValuePair pair ? this.Children[this.Children.IndexOf(pair) - 1] : null;
        }

        protected internal override YNode GetNextNode(YNode node)
        {
            return node == this.LastNode ? null :
                node is YKeyValuePair pair ? this.Children[this.Children.IndexOf(pair) + 1] : null;
        }

        protected internal override void RemoveChild(YNode node)
        {
            if (node is YKeyValuePair pair)
            {
                this.Children.Remove(pair);
            }
        }

        public override IEnumerator<YNode> GetEnumerator()
        {
            return this.Children.GetEnumerator();
        }

        public override string ToString(YNodeStyle style)
        {
            return style == YNodeStyle.Block
                ? $"!!map {{\n{string.Join("\n", this.Select(i => AddIndent(i.ToString() + ",")))}\n}}"
                : $"!!map {{ {string.Join(" ", this.Select(i => i.ToString(style) + ","))} }}";
        }

        public override string ToYamlString(YNodeStyle style)
        {
            return style == YNodeStyle.Block
                ? string.Join("\n", this.Select(i => i.ToYamlString(style)))
                : this.Any()
                    ? $"{{ {string.Join(", ", this.Select(i => i.ToYamlString(style)))} }}"
                    : "{}";
        }

        internal class YKeyValuePair : YNode
        {
            public YKeyValuePair(YNode key, YNode value)
                : base(YNodeStyle.Block)
            {
                this.Key = key;
                this.Value = value;
            }

            public YNode Key { get; }
            public YNode Value { get; }

            public override string ToString(YNodeStyle style)
            {
                return style == YNodeStyle.Block
                    ? $"? {this.Key}\n: {this.Value}"
                    : $"? {this.Key.ToString(style)} : {this.Value.ToString(style)}";
            }

            public override string ToYamlString(YNodeStyle style)
            {
                return style == YNodeStyle.Block
                    ? this.Key.ToYamlString(YNodeStyle.Flow) + ": " +
                      (this.Value.Style == YNodeStyle.Block && this.Value is YCollection<YKeyValuePair>
                          ? "\n" + AddIndent(this.Value.ToYamlString())
                          : AddIndent(this.Value.ToYamlString()).Substring(2))
                    : this.Key.ToYamlString(YNodeStyle.Flow) + ": " + this.Value.ToYamlString(YNodeStyle.Flow);
            }
        }
    }
}