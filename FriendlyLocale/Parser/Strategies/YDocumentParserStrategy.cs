namespace FriendlyLocale.Parser.Strategies
{
    using System.Collections.Generic;
    using FriendlyLocale.Parser.Nodes;

    /// <summary>
    ///     Парсер документа
    /// </summary>
    internal class YDocumentParserStrategy : IYParserStrategy
    {
        public YNode Parse(Tokenizer tokenizer, IYParser parser)
        {
            if (tokenizer.Current.Kind != TokenKind.Document)
            {
                return null;
            }

            tokenizer.MoveNext();

            var items = new List<YNode>();

            while (tokenizer.Current.Kind != TokenKind.Document && tokenizer.Current.Kind != TokenKind.Eof)
            {
                items.Add(parser.Parse(tokenizer));
            }

            return new YDocument(YNodeStyle.Block, items.ToArray());
        }
    }
}