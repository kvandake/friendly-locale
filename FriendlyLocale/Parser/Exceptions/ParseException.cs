namespace FriendlyLocale.Parser.Exceptions
{
    using System;
    using System.Linq;
    using FriendlyLocale.Parser.Core;

    public class ParseException : Exception
    {
        public ParseException(string content, int index, string message)
            : this(content, index, GetLine(content, index), GetColumn(content, index), message)
        {
        }

        public ParseException(string content, int index, int line, int column, string message)
            : base($"{message} at line {line + 1} column {column + 1}")
        {
            this.Content = this.Content;
            this.Index = index;
            this.Line = this.Line;
            this.Column = this.Column;
        }

        public string Content { get; }
        public int Index { get; }
        public int Line { get; }
        public int Column { get; }

        private static int GetColumn(string content, int index)
        {
            var lineStart = index > 0 ? content.LastIndexOfAny(new[] {'\r', '\n'}, index - 1) : -1;

            return lineStart == -1 ? index : index - lineStart - 1;
        }

        private static int GetLine(string content, int index)
        {
            return content.Substring(0, index).Length - content.Substring(0, index).Replace("\n", null).Length;
        }

        internal static ParseException TokenNotAllowed(Scanner scanner)
        {
            return new ParseException(scanner.Content, scanner.Index, $"{GetTokenName(scanner.Current)} not allowed in this context");
        }

        internal static ParseException UnexpectedToken(Scanner scanner, params char[] expected)
        {
            return UnexpectedToken(scanner, GetTokenNames(expected));
        }

        internal static ParseException UnexpectedToken(Scanner scanner, string expected)
        {
            return new ParseException(scanner.Content, scanner.Index, $"unexpected {GetTokenName(scanner.Current)}, {expected} expected");
        }
        
        internal static ParseException Tokenizer(ITokenizer tokenizer, string message)
        {
            return new ParseException(tokenizer.Scanner.Content, tokenizer.Scanner.Index, message);
        }

        internal static ParseException UnexpectedToken(ITokenizer tokenizer, TokenKind expected)
        {
            return new ParseException(tokenizer.Scanner.Content, tokenizer.Current.Value.Index,
                $"unexpected {tokenizer.Current.Value.Kind}, {expected} expected");
        }

        private static string GetTokenNames(params char[] current)
        {
            return string.Join(" or ", current.Select(GetTokenName));
        }

        private static string GetTokenName(char current)
        {
            switch (current)
            {
                case '\0': return "EOF";
                case '\n': return "NEWLINE";
                case ' ': return "SPACE";
                default: return current.ToString();
            }
        }
    }
}