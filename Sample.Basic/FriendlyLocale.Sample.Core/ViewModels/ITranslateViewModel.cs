namespace FriendlyLocale.Sample
{
    using System.Collections.Generic;
    using FriendlyLocale.Sample.Models;

    public interface ITranslateViewModel
    {
        string Title { get; }

        IList<LocaleCellElement> Items { get; }

        void Reload();
    }
}