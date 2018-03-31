namespace FriendlyLocale.Exceptions
{
    using System;

    public class FriendlyTranslateException : Exception
    {
        public FriendlyTranslateException()
        {
        }

        public FriendlyTranslateException(string message) : base(message)
        {
        }

        public FriendlyTranslateException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public static FriendlyTranslateException BuilderException(string configName)
        {
            return new FriendlyTranslateException($"{configName} is required");
        }
    }
}