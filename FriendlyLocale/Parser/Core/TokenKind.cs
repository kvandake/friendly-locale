namespace FriendlyLocale.Parser.Core
{
    internal enum TokenKind
    {
        Eof,
        NewLine,
        Indent,
        Unindent,
        Document,
        Directive,
        Tag,
        Alias,
        Anchor,
        StringPlain,
        StringSingle,
        StringDouble,
        StringLiteral,
        StringFolding,
        SequenceBegin,
        SequenceEnd,
        SequenceValue,
        MappingBegin,
        MappingEnd,
        MappingKey,
        MappingValue,
        ItemDelimiter
    }
}