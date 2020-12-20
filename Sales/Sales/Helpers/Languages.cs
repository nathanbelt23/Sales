namespace Sales.Helpers
{
    using Xamarin.Forms;
    using Sales.interfaces;
    using Resources;

    public static class Languages
    {
        static Languages()
        {
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        public static string Accept
        {
            get { return Resource.Acept; }
        }

        public static string Error
        {
            get { return Resource.Error; }
        }

        public static string NoInternet
        {
            get { return Resource.NoInthernet; }
        }

        public static string Products
        {
            get { return Resource.Products; }
        }

        public static string TurnOnInternet
        {
            get { return Resource.TurnOnInterner; }
        }
    }
}