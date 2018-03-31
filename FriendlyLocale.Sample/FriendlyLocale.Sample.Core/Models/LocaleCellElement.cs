namespace FriendlyLocale.Sample.Models
{
    using System.Windows.Input;
    using FriendlyLocale.Interfaces;

    public class LocaleCellElement : BaseViewModel
    {
        private bool isChecked;

        public LocaleCellElement(ILocale locale)
        {
            this.Locale = locale;
            this.Title = locale.DisplayName;
        }

        public ILocale Locale { get; }

        public string Description => this.Locale.Source;

        public virtual bool IsRemote => false;

        public ICommand Command { get; set; }

        public bool IsChecked
        {
            get => this.isChecked;
            set => this.SetProperty(ref this.isChecked, value);
        }
    }
}