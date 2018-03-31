namespace FriendlyLocale.Models
{
    using System.ComponentModel;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using FriendlyLocale.Annotations;
    using FriendlyLocale.Interfaces;

    public class Locale : ILocale, INotifyPropertyChanged
    {
        private string displayName;

        public Locale(string key, string source)
        {
            this.Key = key;
            this.Source = source;
        }

        public string Key { get; }

        public string Source { get; }

        public virtual string DisplayName => this.displayName ?? (this.displayName = new CultureInfo(this.Key).NativeName.ToUpper());
        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString()
        {
            return this.DisplayName;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}