namespace FriendlyLocale.Parser
{
    using System.Diagnostics;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using FriendlyLocale.Parser.Core;
    using FriendlyLocale.Parser.Exceptions;
    using FriendlyLocale.Parser.Nodes;
    using YNodeTranslator = FriendlyLocale.Parser.Translators.YNodeTranslator;

    internal class YParser
    {
        private const string Separator = ".";

        public YParser(params string[] contents)
        {
            this.map = this.ParseContent(contents);
        }
        
        public bool ThrowWhenKeyNotFound { get; set; }

        internal IDictionary<string, string> map { get; }

        public string FindValue(params string[] innerKeys)
        {
            if (innerKeys.Length == 0)
            {
                return string.Empty;
            }

            var key = string.Join(Separator, innerKeys);
            return this.FindValue(key);
        }

        public string FindValue(string key)
        {
            if (this.map.ContainsKey(key))
            {
                return this.map[key];
            }

            return this.ThrowWhenKeyNotFound
                ? throw new KeyNotFoundException($"Key <{key}> not found in the current locale")
                : string.Empty;
        }

        private IDictionary<string, string> ParseContent(string content)
        {
            var dict = new Dictionary<string, string>();
            var nodes = this.GetNodes(content);
            foreach (var node in nodes)
            {
                this.TryParseContentItems(node, ref dict);
            }

            return new ReadOnlyDictionary<string, string>(dict);
        }

        private IDictionary<string, string> ParseContent(string[] contents)
        {
            var dict = new Dictionary<string, string>();
            foreach (var content in contents)
            {
                foreach (var dictItem in this.ParseContent(content))
                {
                    dict[dictItem.Key] = dictItem.Value;
                }
            }

            return new ReadOnlyDictionary<string, string>(dict);
        }

        private void TryParseContentItems(YNode node, ref Dictionary<string, string> dict, string prefix = null)
        {
            switch (node)
            {
                case YKeyValuePair keyValuePair:
                    string key = null;
                    try
                    {
                        key = (string) keyValuePair.Key;
                    }
                    catch (System.Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }

                    key = string.IsNullOrEmpty(prefix) ? key : string.Concat(prefix, Separator, key);
                    if (keyValuePair.Value is YScalar valueScalar)
                    {
                        dict[key] = valueScalar.Value;
                    }
                    else
                    {
                        this.TryParseContentItems(keyValuePair.Value, ref dict, key);
                    }

                    break;
                case YSequence sequence:
                    // TODO: need suppoer Enum fields
                    foreach (var sequenceChild in sequence.Children)
                    {
                        if (sequenceChild is YScalar yScalar)
                        {
                            dict[string.Concat(prefix, Separator, yScalar.Value)] = yScalar.Value;
                        }
                    }

                    break;
                case YMapping mapping:
                    foreach (var mapItem in mapping)
                    {
                        this.TryParseContentItems(mapItem, ref dict, prefix);
                    }

                    break;

                case IEnumerable<YNode> collection:
                    foreach (var colItem in collection)
                    {
                        this.TryParseContentItems(colItem, ref dict, prefix);
                    }

                    break;
            }
        }

        private IEnumerable<YNode> GetNodes(string content)
        {
            using (var scanner = new Scanner(content))
            {
                using (var tokenizer = new Tokenizer(scanner))
                {
                    var nodeTranslator = new YNodeTranslator();
                    while (tokenizer.Current.Value.Kind != TokenKind.Eof)
                    {
                        var node = nodeTranslator.Translate(tokenizer);
                        if (node is YAnchor)
                        {
                            continue;
                        }

                        yield return node;
                    }

                    while (tokenizer.Current.Value.Kind == TokenKind.Unindent)
                    {
                        tokenizer.MoveNext();
                    }

                    if (tokenizer.Current.Value.Kind != TokenKind.Eof)
                    {
                        throw ParseException.UnexpectedToken(tokenizer, TokenKind.Eof);
                    }
                }
            }
        }
    }
}