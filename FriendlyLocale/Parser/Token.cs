namespace FriendlyLocale.Parser
{
    internal class Token
    {
        public Token(Scanner scanner, TokenKind kind, int? index = null, int? length = null)
            : this(scanner, kind, null, index, length)
        {
        }

        public Token(Scanner scanner, TokenKind kind, string value, int? index = null, int? length = null)
        {
            this.Content = scanner.Content;
            this.Kind = kind;
            this.Value = value;
            this.Index = index ?? scanner.Index;
            this.Length = length ?? value?.Length ?? 1;
        }

        public string Content { get; }
        public TokenKind Kind { get; }
        public int Index { get; }
        public int Length { get; }
        public string Value { get; set; }

        public override string ToString()
        {
            return $"({this.Kind}) {this.Content.Substring(this.Index, this.Length)}";
        }
    }
}