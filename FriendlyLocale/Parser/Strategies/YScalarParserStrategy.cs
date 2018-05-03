namespace FriendlyLocale.Parser.Strategies
{
    using System;
    using FriendlyLocale.Parser.Nodes;

    /// <summary>
    ///     Парсинг значений.
    /// </summary>
    internal class YScalarParserStrategy : IYParserStrategy
    {
        public YNode Parse(Tokenizer tokenizer, IYParser parser)
        {
            switch (tokenizer.Current.Kind)
            {
                case TokenKind.StringDouble:
                case TokenKind.StringSingle:
                case TokenKind.StringFolding:
                case TokenKind.StringLiteral:
                {
                    var kind = tokenizer.Current.Kind;
                    var value = tokenizer.Current.Value;

                    if (tokenizer.Current.Kind == TokenKind.StringDouble)
                    {
                        value = YScalar.UnescapeString(value);
                    }

                    tokenizer.MoveNext();

                    var style = kind == TokenKind.StringFolding || kind == TokenKind.StringLiteral ? YNodeStyle.Block : YNodeStyle.Flow;
                    return new YScalar(value, style);
                }
                case TokenKind.StringPlain:
                {
                    var value = tokenizer.Current.Value;

                    tokenizer.MoveNext();

                    if (string.IsNullOrEmpty(value))
                    {
                        return new YScalar(null);
                    }

                    return value.Equals("null", StringComparison.OrdinalIgnoreCase) ? new YScalar(null) : new YScalar(value);
                }
                default:
                    return null;
            }
        }
    }
}