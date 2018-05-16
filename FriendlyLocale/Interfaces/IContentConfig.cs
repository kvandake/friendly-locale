namespace FriendlyLocale.Interfaces
{
    using System;

    public interface IContentConfig
    {
        bool ThrowWhenKeyNotFound { get; set; }
        
        Action<string> Logger { get; set; }
    }
}