namespace FriendlyLocale.Parser.Core
{
    internal class Token
    {
        private readonly Scanner scanner;

        public Token(Scanner scanner, TokenKind kind, int indentLevel, int? index = null, int? length = null)
            : this(scanner, kind, null, indentLevel, index, length)
        {
        }

        public Token(Scanner scanner, TokenKind kind, string value, int indentLevel, int? index = null, int? length = null)
        {
            this.scanner = scanner;
            this.Kind = kind;
            this.Value = value;
            this.IndentLevel = indentLevel;
            this.Index = index ?? scanner.Index;
            this.Length = length ?? value?.Length ?? 1;
            this.Line = scanner.GetLine(this.Index);
        }

        /// <summary>
        /// Уровень отступа.
        /// </summary>
        public int IndentLevel { get; }
        
        /// <summary>
        ///     Текущая линия.
        /// </summary>
        public int Line { get; }
        
        /// <summary>
        ///     Тип токена.
        /// </summary>
        public TokenKind Kind { get; }
        
        /// <summary>
        ///     Текущий индекс контента.
        /// </summary>
        public int Index { get; }
        
        /// <summary>
        ///     Длина контента.
        /// </summary>
        public int Length { get; }
        
        /// <summary>
        ///     Значение токена. Может быть нулем.
        /// </summary>
        public string Value { get; }

        public override string ToString()
        {
            switch (this.Kind)
            {
                case TokenKind.Indent:
                    return "<--->";
                default:
                    return $"({this.Kind}) {this.scanner.Content.Substring(this.Index, this.Length)}";
            }
        }
    }
}