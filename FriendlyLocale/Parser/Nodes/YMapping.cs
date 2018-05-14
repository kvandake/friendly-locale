namespace FriendlyLocale.Parser.Nodes
{
    using System.Collections.Generic;
    using System.Linq;

    internal class YMapping : YCollection<YKeyValuePair>
    {
        public YMapping(int indentLevel, YNodeStyle style = YNodeStyle.Block, params YNode[] content)
            : base(style, content)
        {
            this.IndentLevel = indentLevel;
        }
        
        public int IndentLevel { get; }

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

        
    }
}