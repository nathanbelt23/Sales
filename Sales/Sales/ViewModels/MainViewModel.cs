

namespace Sales.ViewModels
{


    #region Using

    using System.Windows.Input;
    using System.Collections.ObjectModel;
    using GalaSoft.MvvmLight.Command;
    using Sales.Views;
    using Xamarin.Forms;
    using System;
    using Sales.Helpers;
    using Sales.Common.Models;
    #endregion
    public class MainViewModel
    {

        #region Propiedades
        public ProductsViewModel Products { get; set; }
        public AddProductsViewModel AddProduct { get; set; }
        public EditProductViewModel EditProduct { get; set; }
        public LoginViewModel Login { get; set; }
        public RegisterViewModel Register { get; set; }
        public ObservableCollection<MenuItemViewModel> Menu { get; set; }
        public MyUserASP UserASP { get; set; }

        public string ImgLogo {
            get {
                return "sales.png";
            }
        }


        public string UserFullName
        {
            get
            {
                if (this.UserASP != null && this.UserASP.Claims != null && this.UserASP.Claims.Count > 1)
                {
                    return $"{this.UserASP.Claims[0].ClaimValue} {this.UserASP.Claims[1].ClaimValue}";
                }

                return null;
            }
        }

        public string UserImageFullPath
        {
            get
            {
                foreach (var claim in this.UserASP.Claims)
                {
                    if (claim.ClaimType == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/uri")
                    {
                        if (claim.ClaimValue.StartsWith("~"))
                        {
                            return $"{Application.Current.Resources["UrlSitio"].ToString() }{claim.ClaimValue.Substring(1)}";
                        }

                        return claim.ClaimValue;
                    }
                }

                return null;
            }
        }

        #endregion

        #region Constructores


        public MainViewModel()
        {

          
            instance = this;
            //  quito esta  linea  porque ahora  comienza  desde el login
            //this.Products = new ProductsViewModel();
            this.LoadMenu();

        }

     
        #endregion

        #region Comandos
        public ICommand AddProductCommand
        {
            get
            {
                return new RelayCommand(GoToAddProduct);

            }
        }

        #endregion


        #region  Metodos
        private async void GoToAddProduct()
        {
            this.AddProduct = new AddProductsViewModel();
            await App.Navigator.PushAsync(new AddProductPage("Nathan"));
        }

        private void LoadMenu()
        {

            this.Menu = new ObservableCollection<MenuItemViewModel>();

            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_info",
                PageName = "AboutPage",
                Title = Languages.About,
            });

            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_phonelink_setup",
                PageName = "SetupPage",
                Title = Languages.Setup,
            });

            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_exit_to_app",
                PageName = "LoginPage",
                Title = Languages.Exit,
            });

        }

        #endregion


        #region Singleton

        public static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
                return new MainViewModel();
            return instance;

        }

        #endregion
    }

}
