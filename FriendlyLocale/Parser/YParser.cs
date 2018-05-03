namespace FriendlyLocale.Parser
{
    using System.Diagnostics;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using FriendlyLocale.Parser.Nodes;
    using FriendlyLocale.Parser.Strategies;

    internal class YParser : IYParser
    {
        private IList<IYParserStrategy> strategies;

        private IEnumerable<IYParserStrategy> Strategies => this.strategies ?? (this.strategies = this.GetStrategies());
        
        public YParser(YParserConfig config, params string[] contents)
        {
            this.Config = config ?? new YParserConfig();
            this.map = this.ParseContent(contents);
        }

        private IDictionary<string, string> map { get; }

        public YParserConfig Config { get; }
        
        public string FindValue(params string[] innerKeys)
        {
            if (innerKeys.Length == 0)
            {
                return string.Empty;
            }

            var key = string.Join(this.Config.Separator, innerKeys);
            return this.FindValue(key);
        }

        public string FindValue(string key)
        {
            return this.map.ContainsKey(key) ? this.map[key] : string.Empty;
        }

        public YNode Parse(Tokenizer tokenizer)
        {
            foreach (var strategy in this.Strategies)
            {
                var node = strategy.Parse(tokenizer, this);
                if (node != null)
                {
                    return node;
                }
            }

            return null;
        }

        protected virtual IList<IYParserStrategy> GetStrategies()
        {
            return new List<IYParserStrategy>
            {
                new YAliasParserStrategy(),
                new YAnchorParserStrategy(),
                new YMappingParserStrategy(),
                new YScalarParserStrategy(),
                new YSequenceParserStrategy(),
                new YDocumentParserStrategy()
            };
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
                case YMapping.YKeyValuePair keyValuePair:
                    string key = null;
                    try
                    {
                        key = (string) keyValuePair.Key;
                    }
                    catch (System.Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }

                    key = string.IsNullOrEmpty(prefix) ? key : string.Concat(prefix, this.Config.Separator, key);
                    if (keyValuePair.Value is YScalar valueScalar)
                    {
                        dict[key] = valueScalar.Value;
                    }
                    else
                    {
                        this.TryParseContentItems(keyValuePair.Value, ref dict, key);
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
                    while (tokenizer.Current.Kind != TokenKind.Eof && this.Parse(tokenizer) is YNode node)
                    {
                        yield return node;
                    }

                    if (tokenizer.Current.Kind != TokenKind.Eof)
                    {
                        throw ParseException.UnexpectedToken(tokenizer, TokenKind.Eof);
                    }
                }
            }
        }

        public class YParserConfig
        {
            public string Separator { get; set; } = ".";

            public static YParserConfig Default => new YParserConfig();
        }
    }
}