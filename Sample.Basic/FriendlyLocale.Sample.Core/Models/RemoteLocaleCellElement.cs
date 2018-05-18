namespace FriendlyLocale.Sample.Models
{
    using FriendlyLocale.Models;

    public class RemoteLocaleCellElement : LocaleCellElement
    {
        private float downloadProgress;
        private bool isDownload;

        public RemoteLocaleCellElement(RemoteLocale remoteLocale)
            : base(remoteLocale)
        {
            this.Locale = remoteLocale;
        }

        public override bool IsRemote => true;

        public new RemoteLocale Locale { get; }

        public string DownloadLabel => this.Locale.Downloaded ? "Downloaded" : "NeedDownload";

        public bool IsDownload
        {
            get => this.isDownload;
            set
            {
                this.isDownload = value;
                this.OnPropertyChanged();
            }
        }

        public float DownloadProgress
        {
            get => this.downloadProgress;
            set
            {
                this.downloadProgress = value;
                this.OnPropertyChanged();
            }
        }

        public void UpdateDownloadLabel()
        {
            this.OnPropertyChanged("DownloadLabel");
        }
    }
}