namespace FriendlyLocale.Parser.Strategies
{
    using System.Collections.Generic;
    using FriendlyLocale.Parser.Nodes;

    /// <summary>
    ///     Парсинг последовательности.
    /// </summary>
    internal class YSequenceParserStrategy : IYParserStrategy
    {
        public YNode Parse(Tokenizer tokenizer, IYParser parser)
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
                        items.Add(parser.Parse(tokenizer));
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

                        items.Add(parser.Parse(tokenizer));
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
    }
}