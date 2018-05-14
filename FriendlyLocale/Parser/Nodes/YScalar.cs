namespace FriendlyLocale.Parser.Nodes
{
    using System.Globalization;
    using System.Text.RegularExpressions;

    internal class YScalar : YNode
    {
        private static readonly Regex unicodeEscape =
            new Regex(@"\\(?:x(?<num>[0-9A-Fa-f]{2})|u(?<num>[0-9A-Fa-f]{4})|U(?<num>[0-9A-Fa-f]{8}))");

        public YScalar(YNodeStyle style = YNodeStyle.Flow)
            : base(style)
        {
        }

        public YScalar(string value, YNodeStyle style = YNodeStyle.Flow)
            : this(style)
        {
            this.Value = value;
        }

        public string Value { get; }

        public static string EscapeString(string str)
        {
            return str.Replace("\\", "\\\\")
                .Replace("\x22", "\\\"")
                .Replace("\x07", "\\a")
                .Replace("\x08", "\\b")
                .Replace("\x1b", "\\e")
                .Replace("\x0c", "\\f")
                .Replace("\x0a", "\\n")
                .Replace("\x0d", "\\r")
                .Replace("\0x9", "\\t")
                .Replace("\x0b", "\\v")
                .Replace("\x00", "\\0")
                .Replace("\xA0", "\\_")
                .Replace("\x85", "\\N")
                .Replace("\u2028", "\\L")
                .Replace("\u2029", "\\P");
        }

        public static string UnescapeString(string str)
        {
            str = str.Replace("\\\"", "\x22")
                .Replace("\\a", "\x07")
                .Replace("\\b", "\x08")
                .Replace("\\e", "\x1b")
                .Replace("\\f", "\x0c")
                .Replace("\\n", "\x0a")
                .Replace("\\r", "\x0d")
                .Replace("\\t", "\0x9")
                .Replace("\\v", "\x0b")
                .Replace("\\0", "\x00")
                .Replace("\\ ", "\x20")
                .Replace("\\_", "\xA0")
                .Replace("\\N", "\x85")
                .Replace("\\L", "\u2028")
                .Replace("\\P", "\u2029");

            str = unicodeEscape.Replace(str, m => char.ConvertFromUtf32(int.Parse(m.Groups["num"].Value, NumberStyles.HexNumber)));

            return str.Replace("\\\\", "\\");
        }

        public override string ToString(YNodeStyle style)
        {
            return style == YNodeStyle.Block
                ? $"str: |-\n{AddIndent(this.Value)}"
                : $"str: \"{EscapeString(this.Value)}\"";
        }

        public override string ToYamlString(YNodeStyle style)
        {
            return style == YNodeStyle.Block
                ? $"|-\n{AddIndent(this.Value)}"
                : this.Value.IndexOfAny(new[] {'-', '{', '}', '[', ']', '|', '>', '?'}) == 0 ||
                  this.Value.IndexOfAny(new[] {'\a', '\b', '\t', '\0', '\r', ':'}) != -1
                    ? $"\"{EscapeString(this.Value)}\""
                    : this.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is YScalar scalar &&
                   (scalar.Value?.Equals(this.Value) ?? this.Value == null);
        }

        public override int GetHashCode()
        {
            return typeof(YScalar).GetHashCode() ^ (this.Value?.GetHashCode() ?? 0);
        }

        public static implicit operator YScalar(string value)
        {
            return new YScalar(value);
        }
    }
}