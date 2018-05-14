namespace FriendlyLocale.Parser.Core
{
    using System.Collections.Generic;
    using FriendlyLocale.Parser.Nodes;

    internal interface ITokenizer
    {
        IDictionary<string, YAnchor> Anchors { get; }
        
        LinkedList<Token> Tokens { get; }

        LinkedListNode<Token> Current { get; }

        bool MoveNext();
        
        Scanner Scanner { get; }
    }
}