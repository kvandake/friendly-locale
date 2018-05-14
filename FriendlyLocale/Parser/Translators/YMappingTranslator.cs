namespace FriendlyLocale.Parser.Translators
{
    using System.Collections.Generic;
    using System.Linq;
    using FriendlyLocale.Parser.Core;
    using FriendlyLocale.Parser.Exceptions;
    using FriendlyLocale.Parser.Nodes;

    internal partial class YNodeTranslator
    {
        private YMapping GetMappingValueDependent(ITokenizer tokenizer)
        {
            switch (tokenizer.Current.Value.Kind)
            {
                case TokenKind.Indent when tokenizer.Current.Next?.Value.Kind == TokenKind.MappingKey:
                {
                    var mappingNode = new YMapping(tokenizer.Current.Value.IndentLevel);
                    var items = new YKeyValueList();
                    tokenizer.MoveNext();
                    while (tokenizer.Current.Value.Kind == TokenKind.MappingKey)
                    {
                        var keyValueNode = this.ParseMappingKey(tokenizer);
                        items.AddNode(keyValueNode);
                    }

                    while (tokenizer.Current.Value.Kind == TokenKind.Unindent)
                    {
                        tokenizer.MoveNext(); 
                    }

                    mappingNode.Add(items.ToNodes());

                    return mappingNode;
                }
                case TokenKind.MappingValue:
                {
                    var mappingNode = new YMapping(tokenizer.Current.Value.IndentLevel);
                    tokenizer.MoveNext();
                    var value = this.GetNodeValue(tokenizer);
                    mappingNode.Add(value);
                    
                    return mappingNode;
                }
                case TokenKind.Indent when tokenizer.Current.Next?.Next?.Value.Kind == TokenKind.MappingValue:
                {
                    var mappingNode = new YMapping(tokenizer.Current.Value.IndentLevel);
                    var items = new YKeyValueList();
                    tokenizer.MoveNext();
                    
                    // Добавлеяем элементы в список
                    do
                    {
                        var keyValueNode = this.ParseMappingKey(tokenizer);
                        items.AddNode(keyValueNode);
                    } while (tokenizer.Current.Value.Kind != TokenKind.Unindent &&
                             tokenizer.Current.Value.Kind != TokenKind.Eof &&
                             tokenizer.Current.Value.Kind != TokenKind.Indent &&
                             tokenizer.Current.Value.IndentLevel >= mappingNode.IndentLevel);

                    // Удаляем ненужные отступы
                    while (tokenizer.Current.Value.Kind == TokenKind.Unindent)
                    {
                        tokenizer.MoveNext(); 
                    }

                    // Проверяем уровень вложенности
                    if (tokenizer.Current.Value.IndentLevel != 0 && tokenizer.Current.Value.Kind == TokenKind.Indent)
                    {
                        while (tokenizer.Current.Value.IndentLevel == mappingNode.IndentLevel 
                               && tokenizer.Current.Value.Kind != TokenKind.Eof)
                        {
                            if (tokenizer.Current.Value.Kind == TokenKind.Indent)
                            {
                                tokenizer.MoveNext();
                            }
                        
                            var keyValueNode = this.ParseMappingKey(tokenizer);
                            items.AddNode(keyValueNode);
                        } 
                    }
                    
                    mappingNode.Add(items.ToNodes());

                    return mappingNode;
                }
                case TokenKind.MappingBegin:
                {
                    var mappingNode = new YMapping(tokenizer.Current.Value.IndentLevel);
                    var items = new YKeyValueList();
                    tokenizer.MoveNext();
                    do
                    {
                        if (tokenizer.Current.Value.Kind == TokenKind.MappingEnd)
                        {
                            break;
                        }

                        var keyValueNode = this.ParseMappingKey(tokenizer);
                        items.AddNode(keyValueNode);
                    } while (tokenizer.Current.Value.Kind == TokenKind.ItemDelimiter && tokenizer.MoveNext());

                    if (tokenizer.Current.Value.Kind != TokenKind.MappingEnd)
                    {
                        throw ParseException.UnexpectedToken(tokenizer, TokenKind.MappingEnd);
                    }

                    tokenizer.MoveNext();
                    mappingNode.Add(items.ToNodes());

                    return mappingNode;
                }
                default:
                    return null;
            }
        }

        private YKeyValuePair ParseMappingKey(ITokenizer tokenizer)
        {
            switch (tokenizer.Current.Value.Kind)
            {
                case TokenKind.MappingKey:
                {
                    tokenizer.MoveNext();

                    var key = this.GetNodeKey(tokenizer);

                    if (tokenizer.Current.Value.Kind != TokenKind.MappingValue)
                    {
                        return new YKeyValuePair(key, new YScalar(null));
                    }

                    tokenizer.MoveNext();
                    var keyValuePair = new YKeyValuePair(key);
                    var value = this.GetNodeValue(tokenizer);
                    keyValuePair.Value = value;

                    return keyValuePair;
                }
                default:
                {
                    var key = this.GetNodeKey(tokenizer);
                    tokenizer.MoveNext();
                    
                    var keyValuePair = new YKeyValuePair(key);
                    var value = this.GetNodeValue(tokenizer);
                    keyValuePair.Value = value;

                    return keyValuePair;
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

            public void AddNode(YKeyValuePair item)
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