namespace Sample.MvvmCross.Core.ViewModels
{
    using Acr.UserDialogs;
    using global::MvvmCross.Core.ViewModels;
    using global::MvvmCross.Localization;
    using global::MvvmCross.Platform;

    public class BaseViewModel : MvxViewModel, IMvxLocalizedTextSourceOwner
    {
        private IMvxLanguageBinder localizedTextSource;

        public virtual IMvxLanguageBinder LocalizedTextSource =>
            this.localizedTextSource ?? (this.localizedTextSource = new MvxLanguageBinder("", this.GetType().Name));
        
        public IUserDialogs UserDialogs => Mvx.Resolve<IUserDialogs>();
    }
}