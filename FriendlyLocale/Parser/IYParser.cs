namespace FriendlyLocale.Parser
{
    using FriendlyLocale.Parser.Nodes;

    internal interface IYParser
    {
        YNode Parse(Tokenizer tokenizer);
    }
}