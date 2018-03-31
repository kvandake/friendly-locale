namespace FriendlyLocale.Parser
{
    internal class YKeyValuePair : YNode
    {
        public YKeyValuePair(YNode key, YNode value)
            : base(YNodeStyle.Block)
        {
            this.Key = key;
            this.Value = value;
        }

        public YNode Key { get; }
        public YNode Value { get; }

        internal new static YKeyValuePair Parse(Tokenizer tokenizer)
        {
            switch (tokenizer.Current.Kind)
            {
                case TokenKind.MappingKey:
                {
                    tokenizer.MoveNext();

                    var key = YNode.Parse(tokenizer);

                    if (tokenizer.Current.Kind != TokenKind.MappingValue)
                    {
                        return new YKeyValuePair(key, new YScalar(null));
                    }

                    tokenizer.MoveNext();

                    var value = YNode.Parse(tokenizer);

                    return new YKeyValuePair(key, value);
                }
                case TokenKind.Alias:
                    var keyAlias = YNode.Parse(tokenizer);
                    tokenizer.MoveNext();
                    var valueAlias = YNode.Parse(tokenizer);
                    return new YKeyValuePair(keyAlias, valueAlias);
                default:
                {
                    var key = YNode.Parse(tokenizer);
                    if (tokenizer.Current.Kind != TokenKind.MappingValue)
                    {
                        throw ParseException.UnexpectedToken(tokenizer, TokenKind.MappingValue);
                    }

                    tokenizer.MoveNext();
                    if (tokenizer.Current.Kind == TokenKind.Anchor)
                    {
                        return null;
                    }

                    var value = YNode.Parse(tokenizer);

                    return new YKeyValuePair(key, value);
                }
            }
        }

        public override string ToString(YNodeStyle style)
        {
            return style == YNodeStyle.Block
                ? $"? {this.Key}\n: {this.Value}"
                : $"? {this.Key.ToString(style)} : {this.Value.ToString(style)}";
        }

        public override string ToYamlString(YNodeStyle style)
        {
            return style == YNodeStyle.Block
                ? this.Key.ToYamlString(YNodeStyle.Flow) + ": " +
                  (this.Value.Style == YNodeStyle.Block && this.Value is YCollection<YKeyValuePair>
                      ? "\n" + AddIndent(this.Value.ToYamlString())
                      : AddIndent(this.Value.ToYamlString()).Substring(2))
                : this.Key.ToYamlString(YNodeStyle.Flow) + ": " + this.Value.ToYamlString(YNodeStyle.Flow);
        }
    }
}