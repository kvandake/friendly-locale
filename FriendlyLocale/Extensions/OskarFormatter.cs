namespace FriendlyLocale.Extensions
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;
    using System.Text.RegularExpressions;

    /// <summary>
    ///     https://haacked.com/archive/2009/01/04/fun-with-named-formats-string-parsing-and-edge-cases.aspx/
    ///     Copied from http://mo.notono.us/2008/07/c-stringinject-format-strings-by-key.html
    /// </summary>
    internal static class OskarFormatter
    {
        /// <summary>
        ///     Extension method that replaces keys in a string with the values of matching object properties.
        ///     <remarks>Uses <see cref="string.Format()" /> internally; custom formats should match those used for that method.</remarks>
        /// </summary>
        /// <param name="formatString">The format string, containing keys like {foo} and {foo:SomeFormat}.</param>
        /// <param name="injectionObject">The object whose properties should be injected in the string</param>
        /// <returns>A version of the formatString string with keys replaced by (formatted) key values.</returns>
        public static string InjectNamedFormats(this string formatString, object injectionObject)
        {
            return formatString.InjectNamedFormats(GetPropertyHash(injectionObject));
        }

        private static string InjectNamedFormats(this string format, IDictionary<string, object> attributes)
        {
            var result = format;
            if (attributes == null || format == null)
            {
                return result;
            }

            foreach (var attributeKey in attributes.Keys)
            {
                result = result.InjectSingleValue(attributeKey, attributes[attributeKey]);
            }

            return result;
        }

        private static string InjectSingleValue(this string format, string key, object replacementValue)
        {
            var result = format;
            //regex replacement of key with value, where the generic key format is:
            //Regex foo = new Regex("{(foo)(?:}|(?::(.[^}]*)}))");
            var attributeRegex = new Regex("{(" + key + ")(?:}|(?::(.[^}]*)}))"); //for key = foo, matches {foo} and {foo:SomeFormat}

            //loop through matches, since each key may be used more than once (and with a different format string)
            foreach (Match m in attributeRegex.Matches(format))
            {
                string replacement;
                if (m.Groups[2].Length > 0) //matched {foo:SomeFormat}
                {
                    //do a double string.Format - first to build the proper format string, and then to format the replacement value
                    var attributeFormatString = string.Format(CultureInfo.InvariantCulture, "{{0:{0}}}", m.Groups[2]);
                    replacement = string.Format(CultureInfo.CurrentCulture, attributeFormatString, replacementValue);
                }
                else //matched {foo}
                {
                    replacement = (replacementValue ?? string.Empty).ToString();
                }

                //perform replacements, one match at a time
                result = result.Replace(m.ToString(), replacement); //attributeRegex.Replace(result, replacement, 1);
            }

            return result;
        }

        /// <summary>
        ///     Creates a HashTable based on current object state.
        ///     <remarks>Copied from the MVCToolkit HtmlExtensionUtility class</remarks>
        /// </summary>
        private static IDictionary<string, object> GetPropertyHash(object properties)
        {
            var values = new Dictionary<string, object>();
            if (properties == null)
            {
                return values;
            }

            var typeInfo = properties.GetType().GetTypeInfo();
            foreach (var propInfo in typeInfo.DeclaredProperties)
            {
                values.Add(propInfo.Name, propInfo.GetValue(properties));
            }

            return values;
        }
    }
}