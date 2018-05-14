namespace FriendlyLocale.Parser.Core
{
    using System;
    using System.Collections.Generic;
    using FriendlyLocale.Parser.Nodes;

    internal class SubTokinizer : ITokenizer, IDisposable
    {
        private IDictionary<string, YAnchor> anchors;

        public SubTokinizer(Scanner scanner, IEnumerable<Token> tokens)
        {
            this.Scanner = scanner;
            this.Tokens = new LinkedList<Token>(tokens);
            this.Current = this.Tokens.First;
        }

        public IDictionary<string, YAnchor> Anchors => this.anchors ?? (this.anchors = new Dictionary<string, YAnchor>());

        public Scanner Scanner { get; }
        public LinkedList<Token> Tokens { get; private set; }
        public LinkedListNode<Token> Current { get; private set; }

        public bool MoveNext()
        {
            return (this.Current = this.Current.Next) != null;
        }

        public void Dispose()
        {
            this.Tokens = null;
            this.Current = null;
            this.anchors = null;
        }
    }
}