namespace FriendlyLocale.Parser
{
    using System.Collections.Generic;
    using System.Linq;

    internal class YMapping : YCollection<YKeyValuePair>
    {
        public YMapping(YNodeStyle style = YNodeStyle.Block, params YNode[] content)
            : base(style, content)
        {
        }

        protected override YNode FirstNode => this.Children.FirstOrDefault();
        protected override YNode LastNode => this.Children.LastOrDefault();

        public YNode this[YScalar key] => this.Children.FirstOrDefault(i => i.Key.Equals(key))?.Value;

        internal new static YMapping Parse(Tokenizer tokenizer)
        {
            switch (tokenizer.Current.Kind)
            {
                case TokenKind.Indent when tokenizer.Next.Value.Kind == TokenKind.MappingKey:
                {
                    var items = new YKeyValueList();

                    tokenizer.MoveNext();

                    while (tokenizer.Current.Kind == TokenKind.MappingKey)
                    {
                        items.Add(YKeyValuePair.Parse(tokenizer));
                    }

                    if (tokenizer.Current.Kind == TokenKind.Unindent)
                    {
                        tokenizer.MoveNext();
                    }

                    return new YMapping(YNodeStyle.Block, items.ToArray());
                }
                case TokenKind.Indent when tokenizer.Next.Next?.Value.Kind == TokenKind.MappingValue:
                {
                    var items = new YKeyValueList();

                    tokenizer.MoveNext();

                    while (tokenizer.Current.Kind != TokenKind.Unindent && tokenizer.Current.Kind != TokenKind.Eof)
                    {
                        items.Add(YKeyValuePair.Parse(tokenizer));
                    }

                    if (tokenizer.Current.Kind == TokenKind.Unindent)
                    {
                        tokenizer.MoveNext();
                    }

                    return new YMapping(YNodeStyle.Block, items.ToArray());
                }
                case TokenKind.MappingBegin:
                {
                    var items = new YKeyValueList();

                    tokenizer.MoveNext();

                    do
                    {
                        if (tokenizer.Current.Kind == TokenKind.MappingEnd)
                        {
                            break;
                        }

                        items.Add(YKeyValuePair.Parse(tokenizer));
                    } while (tokenizer.Current.Kind == TokenKind.ItemDelimiter && tokenizer.MoveNext());

                    if (tokenizer.Current.Kind != TokenKind.MappingEnd)
                    {
                        throw ParseException.UnexpectedToken(tokenizer, TokenKind.MappingEnd);
                    }

                    tokenizer.MoveNext();

                    return new YMapping(YNodeStyle.Flow, items.ToArray());
                }
                default:
                    return null;
            }
        }

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

        private class YKeyValueList : List<YNode>
        {
            public void Add(YKeyValuePair item)
            {
                if (item == null)
                {
                    return;
                }

                if (item.Value is YAlias alias)
                {
                    this.AddRange(alias.Anchor.ValueChildren.ToArray());
                }
                else
                {
                    base.Add(item);
                }
            }
        }
    }
}