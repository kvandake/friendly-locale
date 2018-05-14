namespace FriendlyLocale.Parser.Translators
{
    using FriendlyLocale.Parser.Core;
    using FriendlyLocale.Parser.Nodes;

    internal partial class YNodeTranslator
    {
        public YNode Translate(ITokenizer tokenizer)
        {
            return tokenizer.Current.Value.Kind != TokenKind.Eof ? this.GetRootNode(tokenizer) : null;
        }

        private YNode GetRootNode(ITokenizer tokenizer)
        {
            return this.GetAnchorValueDependent(tokenizer) ??
                   this.GetDocumentValueDependent(tokenizer) ??
                   this.GetMappingValueDependent(tokenizer) as YNode;
        }

        private YNode GetNodeKey(ITokenizer tokenizer)
        {
            return this.GetScalarValueDependent(tokenizer);
        }

        private YNode GetNodeValue(ITokenizer tokenizer)
        {
            return this.GetAliasValueDependent(tokenizer) ??
                   this.GetMappingValueDependent(tokenizer) ??
                   this.GetScalarValueDependent(tokenizer) ??
                   this.GetSequenceValueDependent(tokenizer) as YNode;
        }
    }
}