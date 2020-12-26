
using Sales.Helpers;
using Sales.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using Sales.Views;
using Newtonsoft.Json;
using Sales.Common.Models;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Sales
{

    public partial class App : Application
    {
        public static NavigationPage Navigator { get; internal set; }

        public App()
        {
            InitializeComponent();

            if (Settings.IsRemenber & !  string.IsNullOrEmpty(Settings.AccessToken)&& Settings.ExpiresDate>DateTime.UtcNow)
            {
               var   mainView  =MainViewModel.GetInstance();
                mainView.UserASP = JsonConvert.DeserializeObject<MyUserASP>(Settings.UserASP);

                MainPage =new MasterPage();
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
