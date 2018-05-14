namespace FriendlyLocale.Parser.Translators
{
    using System.Collections.Generic;
    using FriendlyLocale.Parser.Core;
    using FriendlyLocale.Parser.Nodes;

    internal partial class YNodeTranslator
    {
        private YDocument GetDocumentValueDependent(ITokenizer tokenizer)
        {
            if (tokenizer.Current.Value.Kind != TokenKind.Document)
            {
                return null;
            }

            tokenizer.MoveNext();
            var items = new List<YNode>();
            while (tokenizer.Current.Value.Kind != TokenKind.Document && tokenizer.Current.Value.Kind != TokenKind.Eof)
            {
                items.Add(this.GetNodeValue(tokenizer));
            }

            return new YDocument(YNodeStyle.Block, items.ToArray());
        }
    }
}