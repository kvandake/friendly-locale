namespace Sample.MvvmCross.Core.Extensions
{
    using global::MvvmCross.Binding.Binders;
    using global::MvvmCross.Binding.BindingContext;
    using global::MvvmCross.Localization;
    using global::MvvmCross.Platform;

    public static class MvxFluentBindingDescriptionExtensions
    {
        public static MvxFluentBindingDescription<TTarget, TSource> ToFlyLocalizationId<TTarget, TSource>(
            this MvxFluentBindingDescription<TTarget, TSource> bindingDescription,
            string localizationId)
            where TSource : IMvxLocalizedTextSourceOwner
            where TTarget : class
        {
            var valueConverter = Mvx.Resolve<IMvxValueConverterLookup>().Find("Language");
            return bindingDescription.To(vm => vm.LocalizedTextSource)
                .OneWay()
                .WithConversion(valueConverter, localizationId);
        }
    }
}