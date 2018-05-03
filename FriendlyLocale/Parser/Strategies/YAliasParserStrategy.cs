namespace FriendlyLocale.Parser.Strategies
{
    using FriendlyLocale.Parser.Nodes;

    internal class YAliasParserStrategy : IYParserStrategy
    {
        public YNode Parse(Tokenizer tokenizer, IYParser parser)
        {
            switch (tokenizer.Current.Kind)
            {
                case TokenKind.Alias:
                    var anchorName = tokenizer.Current.Value;
                    var anchorValue = tokenizer.Anchors[anchorName];
                    tokenizer.MoveNext();
                    return new YAlias(anchorName, anchorValue);
            }

            return null;
        }
    }
}