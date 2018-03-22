namespace FriendlyLocale.Parser
{
    using System.Collections.Generic;

    internal abstract class YNode
    {
        public YNodeStyle Style { get; }
        

        protected YNode(YNodeStyle style)
        {
            this.Style = style;
        }

        public static IEnumerable<YNode> Parse(string content)
        {
            var tokenizer = new Tokenizer(new Scanner(content));

            while (tokenizer.Current.Kind != TokenKind.Eof && Parse(tokenizer) is YNode node)
            {
                yield return node;
            }

            if (tokenizer.Current.Kind != TokenKind.Eof)
            {
                throw ParseException.UnexpectedToken(tokenizer, TokenKind.Eof);
            }
        }

        internal static YNode Parse(Tokenizer tokenizer)
        {
            return YDocument.Parse(tokenizer) ??
                   YMapping.Parse(tokenizer) ??
                   YAlias.Parse(tokenizer) ??
                   YAnchor.Parse(tokenizer) ??
                   YSequence.Parse(tokenizer) ??
                   YScalar.Parse(tokenizer);
        }

        public override string ToString()
        {
            return this.ToString(this.Style);
        } 

        public abstract string ToString(YNodeStyle style);

        public string ToYamlString()
        {
            return this.ToYamlString(this.Style);
        }

        public abstract string ToYamlString(YNodeStyle style);

        protected static YNode ToNode(object content)
        {
            switch (content)
            {
                case YNode node:
                    return node;
                case string contentString:
                    return new YScalar(contentString);
                default:
                    return null;
            }
        }

        protected static string AddIndent(string str)
        {
            return "  " + str.Replace("\n", "\n  ");
        }

        public static explicit operator string(YNode node)
        {
            return ((YScalar) node).Value;
        }
    }
}