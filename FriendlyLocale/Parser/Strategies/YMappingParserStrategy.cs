namespace FriendlyLocale.Parser.Strategies
{
    using System.Collections.Generic;
    using System.Linq;
    using FriendlyLocale.Parser.Nodes;

    internal class YMappingParserStrategy : IYParserStrategy
    {
        public YNode Parse(Tokenizer tokenizer, IYParser parser)
        {
            switch (tokenizer.Current.Kind)
            {
                case TokenKind.Indent when tokenizer.Next.Value.Kind == TokenKind.MappingKey:
                {
                    var items = new YKeyValueList();

                    tokenizer.MoveNext();

                    while (tokenizer.Current.Kind == TokenKind.MappingKey)
                    {
                        items.AddNode(ParseMappingKey(tokenizer, parser));
                    }

                    if (tokenizer.Current.Kind == TokenKind.Unindent)
                    {
                        tokenizer.MoveNext();
                    }

                    return new YMapping(YNodeStyle.Block, items.ToNodes());
                }
                case TokenKind.Indent when tokenizer.Next.Next?.Value.Kind == TokenKind.MappingValue:
                {
                    var items = new YKeyValueList();

                    tokenizer.MoveNext();

                    while (tokenizer.Current.Kind != TokenKind.Unindent && tokenizer.Current.Kind != TokenKind.Eof)
                    {
                        items.AddNode(ParseMappingKey(tokenizer, parser));
                    }

                    if (tokenizer.Current.Kind == TokenKind.Unindent)
                    {
                        tokenizer.MoveNext();
                    }

                    return new YMapping(YNodeStyle.Block, items.ToNodes());
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

                        items.AddNode(ParseMappingKey(tokenizer, parser));
                    } while (tokenizer.Current.Kind == TokenKind.ItemDelimiter && tokenizer.MoveNext());

                    if (tokenizer.Current.Kind != TokenKind.MappingEnd)
                    {
                        throw ParseException.UnexpectedToken(tokenizer, TokenKind.MappingEnd);
                    }

                    tokenizer.MoveNext();

                    return new YMapping(YNodeStyle.Flow, items.ToNodes());
                }
                default:
                    return null;
            }
        }
        
        private static YMapping.YKeyValuePair ParseMappingKey(Tokenizer tokenizer, IYParser parser)
        {
            switch (tokenizer.Current.Kind)
            {
                case TokenKind.MappingKey:
                {
                    tokenizer.MoveNext();

                    var key = parser.Parse(tokenizer);

                    if (tokenizer.Current.Kind != TokenKind.MappingValue)
                    {
                        return new YMapping.YKeyValuePair(key, new YScalar(null));
                    }

                    tokenizer.MoveNext();

                    var value = parser.Parse(tokenizer);

                    return new YMapping.YKeyValuePair(key, value);
                }
                default:
                {
                    var key = parser.Parse(tokenizer);
                    if (tokenizer.Current.Kind != TokenKind.MappingValue)
                    {
                        throw ParseException.UnexpectedToken(tokenizer, TokenKind.MappingValue);
                    }

                    tokenizer.MoveNext();
                    if (tokenizer.Current.Kind == TokenKind.Anchor)
                    {
                        return null;
                    }

                    var value = parser.Parse(tokenizer);

                    return new YMapping.YKeyValuePair(key, value);
                }
            }
        }
        
        private class YKeyValueList
        {
            private readonly List<YNode> nodes;

            public YKeyValueList()
            {
                this.nodes = new List<YNode>();
            }

            public void AddNode(YMapping.YKeyValuePair item)
            {
                if (item == null)
                {
                    return;
                }

                if (item.Value is YAlias alias)
                {
                    this.nodes.AddRange(alias.Anchor.ValueChildren.ToArray());
                }
                else
                {
                    this.nodes.Add(item);
                }
            }

            public YNode[] ToNodes()
            {
                return this.nodes.ToArray();
            }
        }
    }
}