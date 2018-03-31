namespace FriendlyLocale.Sample
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using FriendlyLocale.Interfaces;
    using FriendlyLocale.Sample.Models;
    using Xamarin.Forms;

    public abstract class BaseTranslateViewModel : BaseViewModel, ITranslateViewModel
    {
        private IList<LocaleCellElement> items;

        protected II18N I18N => FriendlyLocale.I18N.Instance;

        public IList<LocaleCellElement> Items
        {
            get => this.items;
            set => this.SetProperty(ref this.items, value);
        }

        public void Reload()
        {
            this.ReloadLocale();
        }

        protected abstract void ReloadLocale();

        protected void ReloadItems()
        {
            var result = new ObservableCollection<LocaleCellElement>();
            foreach (var locale in this.I18N.GetAvailableLocales())
            {
                var localeCell = this.CreateLocaleCellElement(locale);
                localeCell.Command = new Command(() => this.ChangeLocale(localeCell));
                result.Add(localeCell);
            }

            this.Items = result;
        }

        protected virtual LocaleCellElement CreateLocaleCellElement(ILocale locale)
        {
            return new LocaleCellElement(locale);
        }

        protected async void ChangeLocale(LocaleCellElement cell)
        {
            var selectedCell = this.Items.FirstOrDefault(x => x.IsChecked);
            if (selectedCell != null)
            {
                selectedCell.IsChecked = false;
            }

            if (cell is RemoteLocaleCellElement remoteCell && !remoteCell.Locale.Downloaded)
            {
                remoteCell.IsDownload = true;
                var progress = new Progress<float>();
                progress.ProgressChanged += (s, e) =>
                {
                    Console.WriteLine($"Download progress: {e}");
                    remoteCell.DownloadProgress = e / 100;
                };
                await this.I18N.ChangeLocale(cell.Locale, progress);
                remoteCell.UpdateDownloadLabel();
                remoteCell.IsDownload = false;
            }
            else
            {
                await this.I18N.ChangeLocale(cell.Locale);
            }

            cell.IsChecked = true;
        }
    }
}