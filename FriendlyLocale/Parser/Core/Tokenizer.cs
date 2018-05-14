namespace FriendlyLocale.Parser.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using FriendlyLocale.Parser.Exceptions;

    internal class Tokenizer : SubTokinizer, IEnumerable<Token>
    {
        public Tokenizer(Scanner scanner)
            : base(scanner, Tokenize(scanner))
        {
        }

        public LinkedListNode<Token> Previous => this.Current.Previous;
        public LinkedListNode<Token> Next => this.Current.Next;

        public IEnumerator<Token> GetEnumerator()
        {
            do
            {
                yield return this.Current.Value;
            } while (this.MoveNext());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private static IReadOnlyList<Token> Tokenize(Scanner scanner)
        {
            var tokens = new LinkedList<Token>();
            while (!scanner.IsEnd)
            {
                Read(scanner, ref tokens);

                if (tokens.LastOrDefault()?.Kind == TokenKind.Eof)
                {
                    break;
                }
            }

            if (tokens.Last?.Value.Kind == TokenKind.Eof)
            {
                tokens.RemoveLast();
            }

            tokens.AddLast(new Token(
                scanner: scanner,
                kind: TokenKind.Eof,
                indentLevel: scanner.CurrentIndent,
                length: 0));

            return tokens.ToList();
        }

        private static void Read(Scanner scanner, ref LinkedList<Token> tokens)
        {
            scanner.SkipEmptyLines();
            scanner.SkipWhiteSpace();

            var indent = scanner.Column;

            if (SkipComment(scanner) && scanner.ReadLineBreak() != null)
            {
                scanner.SkipEmptyLines();
                indent = scanner.SkipWhiteSpace();
            }

            if (!scanner.IsFlowContent)
            {
                if (scanner.ReadLineBreak() != null)
                {
                    scanner.MaybeSimpleKey = true;
                    scanner.SkipEmptyLines();
                    indent = scanner.SkipWhiteSpace();
                }

                if (indent == 0)
                {
                    switch (scanner.Current)
                    {
                        case '%':
                            var begin = scanner.Index;
                            scanner.MaybeSimpleKey = false;
                            tokens.AddLast(new Token(
                                scanner: scanner,
                                kind: TokenKind.Directive,
                                value: scanner.ReadUntilLineBreakOrEof().Substring(1),
                                indentLevel: scanner.CurrentIndent,
                                index: begin));

                            return;
                        default:
                            if (!scanner.IsCurrent("---", "...") || !scanner.IsWhiteSpaceOrLineBreakOrEof(3))
                            {
                                break;
                            }

                            scanner.MaybeSimpleKey = false;
                            tokens.AddLast(NewToken(scanner.Current == '-' ? TokenKind.Document : TokenKind.Eof, 3));

                            return;
                    }
                }
            }

            switch (scanner.Current)
            {
                case '\0':
                    tokens.AddLast(new Token(
                        scanner: scanner,
                        kind: TokenKind.Eof,
                        indentLevel: scanner.CurrentIndent,
                        length: 0));

                    return;
                case '!':
                {
                    var begin = scanner.Index++;
                    var value = scanner.ReadUntilWhiteSpaceOrEof();

                    scanner.MaybeSimpleKey = false;
                    tokens.AddLast(new Token(
                        scanner: scanner,
                        kind: TokenKind.Tag,
                        value: value,
                        indentLevel: scanner.CurrentIndent,
                        index: begin,
                        length: scanner.Index - begin));

                    return;
                }
                case '[':
                case '{':
                {
                    var kind = scanner.Current;

                    scanner.FlowLevel++;
                    scanner.MaybeSimpleKey = true;
                    tokens.AddLast(NewToken(kind == '[' ? TokenKind.SequenceBegin : TokenKind.MappingBegin));

                    return;
                }
                case ']':
                case '}':
                {
                    var kind = scanner.Current;

                    scanner.FlowLevel--;
                    scanner.MaybeSimpleKey = false;
                    tokens.AddLast(NewToken(kind == ']' ? TokenKind.SequenceEnd : TokenKind.MappingEnd));

                    return;
                }
                case ',':
                    scanner.MaybeSimpleKey = true;
                    tokens.AddLast(NewToken(TokenKind.ItemDelimiter));

                    return;
                case '-' when scanner.IsWhiteSpaceOrLineBreakOrEof(1):
                {
                    if (scanner.IsFlowContent)
                    {
                        throw ParseException.TokenNotAllowed(scanner);
                    }

                    if (!scanner.MaybeSimpleKey)
                    {
                        throw ParseException.TokenNotAllowed(scanner);
                    }

                    var column = scanner.Column;

                    if (column > scanner.CurrentIndent)
                    {
                        scanner.PushIndent(column);
                        tokens.AddLast(new Token(
                            scanner: scanner,
                            kind: TokenKind.Indent,
                            indentLevel: scanner.CurrentIndent,
                            index: scanner.Index - column,
                            length: column));
                    }

                    var begin = scanner.Index++;

                    scanner.SkipWhiteSpace();
                    scanner.MaybeSimpleKey = true;
                    tokens.AddLast(new Token(
                        scanner: scanner,
                        kind: TokenKind.SequenceValue,
                        indentLevel: scanner.CurrentIndent,
                        index: begin));

                    return;
                }
                case '?' when scanner.IsFlowContent || scanner.IsWhiteSpaceOrLineBreakOrEof(1):
                    if (!scanner.IsFlowContent)
                    {
                        if (!scanner.MaybeSimpleKey)
                        {
                            throw ParseException.TokenNotAllowed(scanner);
                        }

                        if (indent > scanner.CurrentIndent)
                        {
                            scanner.PushIndent(indent);
                            tokens.AddLast(new Token(
                                scanner: scanner,
                                kind: TokenKind.Indent,
                                indentLevel: scanner.CurrentIndent,
                                index: scanner.Index - indent,
                                length: indent));
                        }
                    }

                    scanner.MaybeSimpleKey = !scanner.IsFlowContent;
                    tokens.AddLast(NewToken(TokenKind.MappingKey));

                    return;
                case ':' when scanner.IsFlowContent || scanner.IsWhiteSpaceOrLineBreakOrEof(1):
                    if (!scanner.IsFlowContent)
                    {
                        if (!scanner.MaybeSimpleKey)
                        {
                            throw ParseException.TokenNotAllowed(scanner);
                        }
                    }

                    scanner.MaybeSimpleKey = !scanner.IsFlowContent;
                    tokens.AddLast(NewToken(TokenKind.MappingValue));

                    return;
                case '&':
                case '*':
                {
                    var kind = scanner.Current;
                    var begin = scanner.Index++;
                    var value = scanner.ReadWhile(i => char.IsLetterOrDigit(i) || i == '-' || i == '_');

                    if (string.IsNullOrEmpty(value))
                    {
                        throw ParseException.UnexpectedToken(scanner, "identifier");
                    }

                    scanner.MaybeSimpleKey = kind == '*';
                    tokens.AddLast(
                        new Token(
                            scanner: scanner,
                            kind: kind == '*' ? TokenKind.Alias : TokenKind.Anchor,
                            value: value,
                            indentLevel: scanner.CurrentIndent,
                            index: begin,
                            length: scanner.Index - begin));

                    return;
                }
                case '|' when !scanner.IsFlowContent:
                case '>' when !scanner.IsFlowContent:
                {
                    var kind = scanner.Current;
                    var begin = scanner.Index++;
                    var currentIndent = 0;
                    var indentIndicator = 0;

                    scanner.SkipWhiteSpace();
                    SkipComment(scanner);

                    if (!scanner.IsLineBreakOrEof())
                    {
                        throw ParseException.UnexpectedToken(scanner, '\n');
                    }

                    scanner.ReadLineBreak();
                    scanner.SkipEmptyLines();

                    if (indentIndicator > 0)
                    {
                        currentIndent = scanner.CurrentIndent + indentIndicator;
                    }

                    var sb = new StringBuilder();
                    var wasLineBreakLine = false;

                    var appendNewLine = false;
                    while (!scanner.IsEnd)
                    {
                        var lineIndent = scanner.PeekWhiteSpace();

                        if (currentIndent > 0 && lineIndent.Length < currentIndent)
                        {
                            break;
                        }

                        if (appendNewLine)
                        {
                            sb.Append(Environment.NewLine);
                            appendNewLine = false;
                        }

                        if (indentIndicator == 0)
                        {
                            indentIndicator = lineIndent.Length - currentIndent;
                            currentIndent += indentIndicator;
                        }

                        if (scanner.IsLineBreak(currentIndent))
                        {
                            scanner.Index += currentIndent;

                            if (scanner.IsLineBreak())
                            {
                                if (kind != '>' || wasLineBreakLine)
                                {
                                    if (sb.Length > 0 && sb[sb.Length - 1] == ' ')
                                    {
                                        sb.Remove(sb.Length - 1, 1);
                                    }

                                    appendNewLine = true;
                                }

                                scanner.SkipEmptyLines();
                            }

                            wasLineBreakLine = true;
                        }
                        else
                        {
                            sb.Append(scanner.ReadUntilLineBreakOrEof().Substring(currentIndent));
                            wasLineBreakLine = false;

                            if (scanner.IsLineBreak())
                            {
                                if (kind != '>' || lineIndent.Length > currentIndent)
                                {
                                    appendNewLine = true;
                                }
                                else if (sb.Length > 0 && sb[sb.Length - 1] != ' ')
                                {
                                    sb.Append(" ");
                                }
                            }
                        }

                        scanner.SkipEmptyLines();
                    }

                    var value = sb.ToString();
                    scanner.MaybeSimpleKey = false;
                    tokens.AddLast(new Token(
                        scanner: scanner,
                        kind: kind == '>' ? TokenKind.StringFolding : TokenKind.StringLiteral,
                        value: value,
                        indentLevel: scanner.CurrentIndent,
                        index: begin,
                        length: scanner.Index - begin));

                    return;
                }
                case '"':
                case '\'':
                {
                    var kind = scanner.Current;
                    var begin = scanner.Index++;
                    var sb = new StringBuilder();

                    while (!scanner.IsEnd)
                    {
                        if (scanner.IsLineBreak())
                        {
                            scanner.ReadLineBreak();
                            scanner.SkipWhiteSpace();

                            if (scanner.IsLineBreak())
                            {
                                do
                                {
                                    sb.Append(scanner.ReadLineBreak());
                                    scanner.SkipWhiteSpace();
                                } while (scanner.IsLineBreak());
                            }
                            else if (sb.Length > 0 && sb[sb.Length - 1] != ' ')
                            {
                                sb.Append(" ");
                            }
                        }

                        if (kind == '\'' && scanner.Current == kind && scanner.PeekChar(1) == kind)
                        {
                            scanner.Index++;
                        }
                        else if (scanner.Current == '\\')
                        {
                            sb.Append(scanner.Current);
                            scanner.Index++;
                        }
                        else if (scanner.Current == kind)
                        {
                            scanner.Index++;

                            break;
                        }

                        sb.Append(scanner.Current);
                        scanner.Index++;
                    }

                    scanner.MaybeSimpleKey = true;
                    tokens.AddLast(new Token(
                        scanner: scanner,
                        kind: kind == '"' ? TokenKind.StringDouble : TokenKind.StringSingle,
                        value: sb.ToString(),
                        indentLevel: scanner.CurrentIndent,
                        index: begin,
                        length: scanner.Index - begin));

                    return;
                }
                default:
                {
                    if (scanner.IsCurrent('-', '?', ':', '|', '>', '%', '@', '`'))
                    {
                        throw ParseException.UnexpectedToken(scanner, "Identifier");
                    }

                    var begin = scanner.Index;
                    var trailingSpaces = 0;
                    var sb = new StringBuilder();

                    while (!scanner.IsEnd)
                    {
                        if (scanner.Current == ':' && scanner.IsWhiteSpaceOrLineBreakOrEof(1) ||
                            scanner.IsFlowContent && scanner.IsCurrent('?', ':', ',', '[', ']', '{', '}') ||
                            scanner.Current == '#' ||
                            scanner.IsCurrent("---", "...") && scanner.IsLineBreakOrEof(-1))
                        {
                            break;
                        }

                        if (scanner.IsLineBreak())
                        {
                            if (trailingSpaces > 0)
                            {
                                sb.Remove(sb.Length - trailingSpaces, trailingSpaces);
                            }

                            trailingSpaces = 0;
                            scanner.MaybeSimpleKey = false;
                            scanner.ReadLineBreak();

                            var newIndent = scanner.SkipWhiteSpace();

                            if (scanner.IsLineBreak())
                            {
                                sb.Append(scanner.SkipEmptyLines());
                                newIndent = scanner.SkipWhiteSpace();
                            }

                            if (newIndent <= scanner.CurrentIndent)
                            {
                                break;
                            }

                            if (sb.Length > 0 && sb[sb.Length - 1] != ' ')
                            {
                                sb.Append(" ");
                            }

                            if (!scanner.IsFlowContent && scanner.CurrentIndent > 0 && newIndent < scanner.CurrentIndent)
                            {
                                break;
                            }

                            continue;
                        }

                        if (scanner.IsWhiteSpace())
                        {
                            trailingSpaces++;
                        }
                        else
                        {
                            trailingSpaces = 0;
                        }

                        sb.Append(scanner.Current);
                        scanner.Index++;
                    }

                    scanner.MaybeSimpleKey = true;
                    // maybe an indent?
                    var tokenLine = scanner.GetLine(begin);
                    if (tokens.Last?.Value.Line != tokenLine)
                    {
                        indent = scanner.GetColumn(begin);
                        if (indent != scanner.CurrentIndent)
                        {
                            scanner.PushIndent(indent);
                            var indentToken = new Token(
                                scanner: scanner,
                                kind: TokenKind.Indent,
                                indentLevel: scanner.CurrentIndent,
                                index: begin - indent,
                                length: indent);
                            tokens.AddLast(indentToken);
                        }
                    }

                    var token = new Token(
                        scanner: scanner,
                        kind: TokenKind.StringPlain,
                        value: sb.ToString().TrimEnd(),
                        indentLevel: scanner.CurrentIndent,
                        index: begin,
                        length: scanner.Index - begin);

                    tokens.AddLast(token);

                    return;
                }
            }

            Token NewToken(TokenKind kind, int? length = null)
            {
                var rt = new Token(
                    scanner: scanner,
                    kind: kind,
                    indentLevel: scanner.CurrentIndent,
                    length: length);

                scanner.Index += rt.Length;

                return rt;
            }
        }

        private static bool SkipComment(Scanner scanner)
        {
            if (scanner.Current != '#')
            {
                return false;
            }

            scanner.ReadUntilLineBreakOrEof();

            return true;
        }
    }
}