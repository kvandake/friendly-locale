namespace FriendlyLocale
{
    using FriendlyLocale.Impl;

    public static class TranslateExtensions
    {
        public static string TranslateWithCapitalizeFirst(this I18NProvider i18NProvider, string key, string fallback)
        {
            return CapitalizeFirstCharacter(i18NProvider.Translate(key, fallback));
        }

        private static string CapitalizeFirstCharacter(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            if (s.Length == 1)
            {
                return s.ToUpper();
            }

            return s.Remove(1).ToUpper() + s.Substring(1);
        }
    }
}