namespace FriendlyLocale.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface ITranslateContentClient
    {
        Task<string> GetContent(ILocale locale, IProgress<float> progress = null, CancellationToken ct = default(CancellationToken));

        IList<ILocale> GetLocales();
    }
}