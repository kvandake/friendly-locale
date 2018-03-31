﻿namespace FriendlyLocale.Parser
{
    using System.Collections.Generic;
    using System.Linq;

    internal class YSequence : YCollection<YNode>
    {
        private YSequence(YNodeStyle style, params YNode[] content)
            : base(style, content)
        {
        }

        protected override YNode FirstNode => this.Children.FirstOrDefault();
        protected override YNode LastNode => this.Children.LastOrDefault();

        internal new static YSequence Parse(Tokenizer tokenizer)
        {
            switch (tokenizer.Current.Kind)
            {
                case TokenKind.Indent when tokenizer.Next.Value.Kind == TokenKind.SequenceValue:
                {
                    var items = new List<YNode>();

                    tokenizer.MoveNext();

                    while (tokenizer.Current.Kind != TokenKind.Unindent && tokenizer.Current.Kind != TokenKind.Eof)
                    {
                        if (tokenizer.Current.Kind != TokenKind.SequenceValue)
                        {
                            throw ParseException.UnexpectedToken(tokenizer, TokenKind.SequenceValue);
                        }

                        tokenizer.MoveNext();
                        items.Add(YNode.Parse(tokenizer));
                    }

                    if (tokenizer.Current.Kind == TokenKind.Unindent)
                    {
                        tokenizer.MoveNext();
                    }

                    return new YSequence(YNodeStyle.Block, items.ToArray());
                }
                case TokenKind.SequenceBegin:
                {
                    var items = new List<YNode>();

                    tokenizer.MoveNext();

                    do
                    {
                        if (tokenizer.Current.Kind == TokenKind.SequenceEnd)
                        {
                            break;
                        }

                        items.Add(YNode.Parse(tokenizer));
                    } while (tokenizer.Current.Kind == TokenKind.ItemDelimiter && tokenizer.MoveNext());

                    if (tokenizer.Current.Kind != TokenKind.SequenceEnd)
                    {
                        throw ParseException.UnexpectedToken(tokenizer, TokenKind.SequenceEnd);
                    }

                    tokenizer.MoveNext();

                    return new YSequence(YNodeStyle.Flow, items.ToArray());
                }
                default:
                    return null;
            }
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
            return style == YNodeStyle.Block
                ? $"!!seq [\n{string.Join("\n", this.Select(i => AddIndent(i.ToString() + ",")))}\n]"
                : $"!!seq [ {string.Join(" ", this.Select(i => i.ToString(style) + ","))} ]";
        }

        public override string ToYamlString(YNodeStyle style)
        {
            return style == YNodeStyle.Block
                ? string.Join("\n", this.Select(i =>
                {
                    var rt = i.ToYamlString();

                    if (i.Style == YNodeStyle.Block && i is YSequence)
                    {
                        rt = "\n" + AddIndent(rt);
                    }
                    else
                    {
                        rt = AddIndent(rt).Substring(2);
                    }

                    return "- " + rt;
                }))
                : this.Any()
                    ? $"[ {string.Join(", ", this.Select(i => i.ToYamlString(style)))} ]"
                    : "[]";
        }
    }
}