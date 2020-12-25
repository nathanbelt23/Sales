
using Sales.Helpers;
using Sales.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;


[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Sales
{

    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            if (Settings.IsRemenber & !  string.IsNullOrEmpty(Settings.AccessToken)&& Settings.ExpiresDate>DateTime.UtcNow)
            {

                MainViewModel.GetInstance().Products = new ProductsViewModel();
                MainPage = new NavigationPage(new Views.ProductsPage());
            }
            else
            {
                MainViewModel.GetInstance().Login = new LoginViewModel();
                MainPage = new NavigationPage(new Views.LoginPage());
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
