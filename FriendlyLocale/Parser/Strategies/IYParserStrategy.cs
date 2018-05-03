namespace FriendlyLocale.Parser.Strategies
{
    using FriendlyLocale.Parser.Nodes;

    internal interface IYParserStrategy
    {
        YNode Parse(Tokenizer tokenizer, IYParser parser);
    }
}