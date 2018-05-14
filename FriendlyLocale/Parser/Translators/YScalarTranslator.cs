namespace FriendlyLocale.Parser.Translators
{
    using System;
    using FriendlyLocale.Parser.Core;
    using FriendlyLocale.Parser.Nodes;

    internal partial class YNodeTranslator
    {
        private YScalar GetScalarValueDependent(ITokenizer tokenizer)
        {
            switch (tokenizer.Current.Value.Kind)
            {
                case TokenKind.StringDouble:
                case TokenKind.StringSingle:
                case TokenKind.StringFolding:
                case TokenKind.StringLiteral:
                {
                    var kind = tokenizer.Current.Value.Kind;
                    var value = tokenizer.Current.Value.Value;

                    if (tokenizer.Current.Value.Kind == TokenKind.StringDouble)
                    {
                        value = YScalar.UnescapeString(value);
                    }

                    tokenizer.MoveNext();

                    var style = kind == TokenKind.StringFolding || kind == TokenKind.StringLiteral ? YNodeStyle.Block : YNodeStyle.Flow;
                    return new YScalar(value, style);
                }
                case TokenKind.StringPlain:
                {
                    var value = tokenizer.Current.Value.Value;

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