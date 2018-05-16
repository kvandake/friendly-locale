namespace FriendlyLocale.Configs
{
    using System;
    using FriendlyLocale.Interfaces;

    public abstract class BaseContentConfig : IContentConfig
    {
        /// <summary>
        /// Optional: Throw an exception when keys are not found (recommended only for debugging).
        /// </summary>
        public bool ThrowWhenKeyNotFound { get; set; }

        public Action<string> Logger { get; set; }
    }
}