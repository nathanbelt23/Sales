

namespace Sales.ViewModels
{    using System;
    using System.Windows.Input;
    using Xamarin.Forms;
    using GalaSoft.MvvmLight.Command;
    using Sales.Helpers;
    using Sales.Services;
    using Sales.Views;

    public class LoginViewModel:BaseViewModel
    {


        #region Atributos

        private bool isRunning;
        private bool isEnabled;

        private ApiService apiService;
        #endregion

        #region Propiedades

        public string Email { get; set; }
        public  string Password { get; set; }
        public bool IsRemembered { get; set; }
        public bool IsRunning { get { return this.isRunning; } set { SetValue(ref this.isRunning, value); } }
        public bool IsEnabled { get { return this.isEnabled; } set { SetValue(ref this.isEnabled, value); } }
        #endregion

        #region Constructor
        public LoginViewModel()
        {

            
            this.IsEnabled = true;
            this.IsRunning = false;
            this.IsRemembered = true;
            this.apiService = new ApiService();

        }

        #endregion

        #region Comandos


        public ICommand LoginCommand { get { return  new  RelayCommand(Login); } }

        private async void Login()
        {

            if (string.IsNullOrEmpty(Email))
            {

                await Application.Current.MainPage.DisplayAlert(Languages.Error,
                        Languages.EmailValidation,
                    Languages.Accept
                    );
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {

                await Application.Current.MainPage.DisplayAlert(Languages.Error,
                    Languages.PasswordValidation, 
                    Languages.Accept
                    );
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;
            var isconnection = await this.apiService.CheckConnection();

            if (!isconnection.IsSuccess)
            {

                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, isconnection.Message, Languages.Accept);
                return;
            }

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var token = await this.apiService.GetToken(url, this.Email, this.Password);

            if (token == null || string.IsNullOrEmpty(token.AccessToken))
            {

                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.SomethingWrong,
                    Languages.Accept
                     );

                return;
            }


            //await Application.Current.MainPage.DisplayAlert(Languages.Error,"Siiiiiiiiiiiiiiii", Languages.Accept);

            Settings.TokenType = token.TokenType;
            Settings.AccessToken = token.AccessToken;
            Settings.IsRemenber = this.IsRemembered;
            Settings.ExpiresDate = token.Expires;


            this.IsRunning = false;
            this.IsEnabled = true;



            MainViewModel.GetInstance().Products = new ProductsViewModel();

             Application.Current.MainPage = new ProductsPage();





        }

        public ICommand RegisterCommand { get { return new RelayCommand(Register); } }
       private async void Register()
        {
            throw new NotImplementedException();
        }


        public ICommand LoginFacebookComand { get { return new RelayCommand(LoginFacebook); } }

        private async void LoginFacebook()
        {

        }

        public ICommand LoginTwitterComand { get { return new RelayCommand(LoginTwitter); } }

        private  async void LoginTwitter()
        {
           
        }

        public ICommand LoginInstagramComand { get { return new RelayCommand(LoginInstagram); } }

        private async void LoginInstagram()
        {

        }

        


        #endregion
    }

       
    }
