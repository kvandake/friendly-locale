namespace FriendlyLocale.Parser.Nodes
{
    internal class YKeyValuePair : YNode
    {
        public YKeyValuePair(YNode key, YNode value = null)
            : base(YNodeStyle.Block)
        {
            this.Key = key;
            this.Value = value;
        }
        
        public YNode Key { get; }
        public YNode Value { get; set; }

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