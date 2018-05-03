namespace FriendlyLocale.Parser.Strategies
{
    using FriendlyLocale.Parser.Nodes;

    internal class YAnchorParserStrategy : IYParserStrategy
    {
        public YNode Parse(Tokenizer tokenizer, IYParser parser)
        {
            switch (tokenizer.Current.Kind)
            {
                case TokenKind.Anchor:
                    var name = tokenizer.Current.Value;
                    tokenizer.MoveNext();
                    var anchor = new YAnchor(parser.Parse(tokenizer));
                    tokenizer.Anchors[name] = anchor;
                    break;
            }

            return null;
        }
    }
}