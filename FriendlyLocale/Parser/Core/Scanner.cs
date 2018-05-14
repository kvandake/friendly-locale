namespace FriendlyLocale.Parser.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal class Scanner : IDisposable
    {
        private readonly Stack<int> indents = new Stack<int>();

        public Scanner(string content)
        {
            this.Content = content.Replace("\r\n", "\n");
        }

        public string Content { get; private set; }
        public int Index { get; set; }
        public int Column => this.GetColumn(this.Index);
        public int Line => this.GetLine(this.Index);
        public char Current => this.Index < this.Content.Length ? this.Content[this.Index] : '\0';
        public bool IsEnd => this.Current == '\0';

        public bool IsFlowContent => this.FlowLevel > 0;
        public int FlowLevel { get; set; }
        public int CurrentIndent => this.indents.Any() ? this.indents.Peek() : -1;
        public bool MaybeSimpleKey { get; set; } = true;

        public void Dispose()
        {
            this.Content = null;
        }

        public int GetColumn(int index)
        {
            var lineStart = index > 0 ? this.Content.LastIndexOfAny(new[] {'\r', '\n'}, index - 1) : -1;

            return lineStart == -1 ? index : index - lineStart - 1;
        }

        public int GetLine(int index)
        {
            return this.Content.Substring(0, index).Length - this.Content.Substring(0, index).Replace("\n", null).Length;
        }

        public void PushIndent(int spaces)
        {
            this.indents.Push(spaces);
        }

        public bool IsCurrent(params char[] any)
        {
            return any.Any(i => this.Current == i);
        }

        public bool IsCurrent(params string[] any)
        {
            return any.Any(i => this.Peek(i.Length) == i);
        }

        private bool IsWhiteSpace(char c)
        {
            return c == ' ' || (this.IsFlowContent || !this.MaybeSimpleKey) && c == '\t';
        }

        public bool IsWhiteSpace(int offset = 0)
        {
            return this.IsWhiteSpace(this.PeekChar(offset));
        }

        private bool IsWhiteSpaceOrEof(char c)
        {
            return this.IsWhiteSpace(c) || c == '\0';
        }

        public bool IsWhiteSpaceOrEof(int offset = 0)
        {
            return this.IsWhiteSpaceOrEof(this.PeekChar(offset));
        }

        private bool IsLineBreak(char c)
        {
            return c == '\r' || c == '\n';
        }

        public bool IsLineBreak(int offset = 0)
        {
            return this.IsLineBreak(this.PeekChar(offset));
        }

        private bool IsLineBreakOrEof(char c)
        {
            return this.IsLineBreak(c) || c == '\0';
        }

        public bool IsLineBreakOrEof(int offset = 0)
        {
            return this.IsLineBreakOrEof(this.PeekChar(offset));
        }

        private bool IsWhiteSpaceOrLineBreakOrEof(char c)
        {
            return this.IsWhiteSpace(c) || this.IsLineBreak(c) || c == '\0';
        }

        public bool IsWhiteSpaceOrLineBreakOrEof(int offset = 0)
        {
            return this.IsWhiteSpaceOrLineBreakOrEof(this.PeekChar(offset));
        }

        public char PeekChar(int offset)
        {
            return 0 <= this.Index + offset && this.Index + offset < this.Content.Length ? this.Content[this.Index + offset] : '\0';
        }

        public string Peek(int length)
        {
            return new string(Enumerable.Range(this.Index, length)
                .TakeWhile(i => i < this.Content.Length)
                .Select(i => this.Content[i])
                .ToArray());
        }

        public string PeekWhile(Func<char, bool> predicate)
        {
            return new string(Enumerable.Range(this.Index, this.Content.Length - this.Index)
                .Select(i => this.Content[i])
                .TakeWhile(predicate)
                .ToArray());
        }

        public string PeekWhiteSpace()
        {
            return this.PeekWhile(this.IsWhiteSpace);
        }

        public string PeekUntilWhiteSpace()
        {
            return this.PeekWhile(i => !this.IsWhiteSpace(i));
        }

        private string Read(string peek)
        {
            this.Index += peek.Length;

            return peek;
        }

        public string ReadWhile(Func<char, bool> predicate)
        {
            return this.Read(this.PeekWhile(predicate));
        }

        public string ReadUntilWhiteSpaceOrEof()
        {
            var begin = this.Index;

            while (!this.IsWhiteSpaceOrEof())
            {
                this.Index++;
            }

            return this.Content.Substring(begin, this.Index - begin);
        }

        public string ReadUntilLineBreakOrEof()
        {
            var begin = this.Index;

            while (!this.IsLineBreakOrEof())
            {
                this.Index++;
            }

            return this.Content.Substring(begin, this.Index - begin);
        }

        public int SkipWhiteSpace()
        {
            var begin = this.Index;

            while (this.IsWhiteSpace())
            {
                this.Index++;
            }

            return this.Index - begin;
        }

        public string SkipEmptyLines()
        {
            var sb = new StringBuilder();

            while (this.Index < this.Content.Length)
            {
                var ws = this.PeekWhiteSpace();
                if (this.IsLineBreakOrEof(ws.Length))
                {
                    this.Index += ws.Length;
                    sb.Append(this.ReadLineBreak());
                }
                else
                {
                    break;
                }
            }

            return sb.ToString();
        }

        public string ReadLineBreak()
        {
            if (this.IsCurrent("\r\n"))
            {
                this.Index += 2;

                return "\r\n";
            }

            if (this.IsLineBreak())
            {
                var c = this.Current;

                this.Index++;

                return c.ToString();
            }

            return null;
        }

        public override string ToString()
        {
            var lineStart = this.Index > 0 ? this.Content.LastIndexOfAny(new[] {'\r', '\n'}, this.Index - 1) : -1;
            var lineEnd = this.Content.IndexOfAny(new[] {'\r', '\n'}, this.Index);

            if (lineStart == -1)
            {
                lineStart = 0;
            }

            if (lineEnd == -1)
            {
                lineEnd = this.Content.Length;
            }

            var currentLine = this.Content.Substring(lineStart, lineEnd - lineStart);

            return $"[{this.Line + 1:00}, {this.Column + 1:00}] {currentLine}";
        }
    }
}