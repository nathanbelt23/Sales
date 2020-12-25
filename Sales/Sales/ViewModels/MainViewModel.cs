

namespace Sales.ViewModels
{

    using System.Windows.Input;
    #region Using
    using Xamarin.Forms;
    using GalaSoft.MvvmLight.Command;
    using Sales.Views;

    #endregion
    public class MainViewModel
    {

        #region Propiedades
        public ProductsViewModel Products { get; set; }
        public AddProductsViewModel AddProduct { get; set; }
        public EditProductViewModel EditProduct { get; set; }
        public LoginViewModel Login { get; set; }
        #endregion

        #region Constructores


        public MainViewModel()
        {

          
            instance = this;
            //  quito esta  linea  porque ahora  comienza  desde el login
            //this.Products = new ProductsViewModel();

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
            await Application.Current.MainPage.Navigation.PushAsync(new AddProductPage("Nathan"));
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
