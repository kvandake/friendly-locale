namespace FriendlyLocale.Parser
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class YParser
    {
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

        private IDictionary<string, string> ParseContent(string content)
        {
            using (var scanner = new Scanner(content))
            {
                using (var tokinizer = new Tokenizer(scanner))
                {
                    var document = YMapping.Parse(tokinizer);
                    var dict = new Dictionary<string, string>();
                    foreach (var docItem in document)
                    {
                        this.TryParseContentItems(docItem, ref dict);
                    }

                    return new ReadOnlyDictionary<string, string>(dict);
                }
            }
        }

        private void TryParseContentItems(YNode node, ref Dictionary<string, string> dict, string prefix = null)
        {
            switch (node)
            {
                case YKeyValuePair keyValuePair:
                    var key = (string) keyValuePair.Key;
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

        public class YParserConfig
        {
            public string Separator { get; set; } = ".";

            public static YParserConfig Default => new YParserConfig();
        }
    }
}