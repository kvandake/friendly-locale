namespace FriendlyLocale.Parser
{
    using System.Collections.Generic;
    using System.Linq;

    internal class YDocument : YCollection<YNode>
    {
        public YDocument(YNodeStyle style, params YNode[] content)
            : base(style, content)
        {
        }

        protected override YNode FirstNode => this.Children.FirstOrDefault();
        protected override YNode LastNode => this.Children.LastOrDefault();

        internal new static YDocument Parse(Tokenizer tokenizer)
        {
            if (tokenizer.Current.Kind != TokenKind.Document)
            {
                return null;
            }

            tokenizer.MoveNext();

            var items = new List<YNode>();

            while (tokenizer.Current.Kind != TokenKind.Document && tokenizer.Current.Kind != TokenKind.Eof)
            {
                items.Add(YNode.Parse(tokenizer));
            }

            return new YDocument(YNodeStyle.Block, items.ToArray());
        }

        public override void Add(params YNode[] content)
        {
            this.Children.AddRange(Flattern(content).Select(ToNode).Where(i => i != null));
        }

        public override void AddFirst(params object[] content)
        {
            this.Children.InsertRange(0, Flattern(content).Select(ToNode).Where(i => i != null));
        }

        protected internal override YNode GetPreviousNode(YNode node)
        {
            return node == this.FirstNode ? null : this.Children[this.Children.IndexOf(node) - 1];
        }

        protected internal override YNode GetNextNode(YNode node)
        {
            return node == this.LastNode ? null : this.Children[this.Children.IndexOf(node) + 1];
        }

        protected internal override void RemoveChild(YNode node)
        {
            this.Children.Remove(node);
        }

        public override void RemoveNodes()
        {
            this.Children.Clear();
        }

        public override IEnumerator<YNode> GetEnumerator()
        {
            return this.Children.GetEnumerator();
        }

        public override string ToString(YNodeStyle style)
        {
            return "---\n" + string.Join("\n", this.Select(i => i.ToString()));
        }

        public override string ToYamlString(YNodeStyle style)
        {
            return "---\n" + string.Join("\n", this.Select(i => i.ToYamlString()));
        }
    }
}