namespace FriendlyLocale.Parser.Translators
{
    using System.Collections.Generic;
    using FriendlyLocale.Parser.Core;
    using FriendlyLocale.Parser.Exceptions;
    using FriendlyLocale.Parser.Nodes;

    internal partial class YNodeTranslator
    {
        private YSequence GetSequenceValueDependent(ITokenizer tokenizer)
        {
            switch (tokenizer.Current.Value.Kind)
            {
                case TokenKind.Indent when tokenizer.Current.Next?.Value.Kind == TokenKind.SequenceValue:
                {
                    var sequenceNode = new YSequence(YNodeStyle.Block);
                    var items = new List<YNode>();
                    tokenizer.MoveNext();

                    while (tokenizer.Current.Value.Kind != TokenKind.Unindent && tokenizer.Current.Value.Kind != TokenKind.Eof)
                    {
                        if (tokenizer.Current.Value.Kind != TokenKind.SequenceValue)
                        {
                            throw ParseException.UnexpectedToken(tokenizer, TokenKind.SequenceValue);
                        }

                        tokenizer.MoveNext();
                        items.Add(this.GetNodeValue(tokenizer));
                    }

                    if (tokenizer.Current.Value.Kind == TokenKind.Unindent)
                    {
                        tokenizer.MoveNext();
                    }
                    
                    sequenceNode.Add(items.ToArray());

                    return sequenceNode;
                }
                case TokenKind.SequenceBegin:
                {
                    var sequenceNode = new YSequence(YNodeStyle.Flow);
                    var items = new List<YNode>();
                    tokenizer.MoveNext();

                    do
                    {
                        if (tokenizer.Current.Value.Kind == TokenKind.SequenceEnd)
                        {
                            break;
                        }

                        items.Add(this.GetNodeValue(tokenizer));
                    } while (tokenizer.Current.Value.Kind == TokenKind.ItemDelimiter && tokenizer.MoveNext());

                    if (tokenizer.Current.Value.Kind != TokenKind.SequenceEnd)
                    {
                        throw ParseException.UnexpectedToken(tokenizer, TokenKind.SequenceEnd);
                    }

                    tokenizer.MoveNext();
                    sequenceNode.Add(items.ToArray());

                    return sequenceNode;
                }
                default:
                    return null;
            }
        }
    }
}