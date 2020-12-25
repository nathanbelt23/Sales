
namespace Sales.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Sales.Common.Models;
    using Sales.Helpers;
    using Sales.Services;
    using Sales.Views;
    using System;
    using System.Linq;
    using System.Windows.Input;
    using Xamarin.Forms;


    public class ProductItemViewModel:Product
    {

        #region Atributes

       private  ApiService apiService;

        #endregion


        #region Comandos

        public ICommand EditProductCommand { get { return new RelayCommand(EditProduct);   } }

        private async void EditProduct()
        {
           
            MainViewModel.GetInstance().EditProduct = new EditProductViewModel(this);
           await  Application.Current.MainPage.Navigation.PushAsync(new  EditProductPage());
         
        }

        public ICommand DeleteProductCommand {

            get {

                return new RelayCommand( DeleteProduct);

            }
        }

        private async void DeleteProduct()
        {

            var answer = await Application.Current.MainPage.DisplayAlert(Languages.Confirmation, Languages.DeleteConfirmation, Languages.Yes, Languages.No);


            if (!answer)
            {

                return;
            }

            var isconnection = await this.apiService.CheckConnection();

            if (!isconnection.IsSuccess)
            {


                await Application.Current.MainPage.DisplayAlert(Languages.Error, isconnection.Message, Languages.Accept);
                return;
            }

            var response = await apiService.Delete(Application.Current.Resources["UrlAPI"].ToString(), Application.Current.Resources["Prefix"].ToString(), Application.Current.Resources["ControllerProduct"].ToString(), this.ProductId, Settings.TokenType, Settings.AccessToken);
            if (!response.IsSuccess)
            {

                await Application.Current.MainPage.DisplayAlert(Languages.Error,response.Message,Languages.Accept);
                return;
            }


            var productsViewModel = ProductsViewModel.GetInstance();
            var deleteProduct = productsViewModel.Products.Where(p=>p.ProductId== this.ProductId).FirstOrDefault();

            if (deleteProduct != null)
            {

                productsViewModel.Products.Remove(deleteProduct);

            }

      


        }


        #endregion


        #region Constructor

        public ProductItemViewModel()
        {
            this.apiService = new ApiService();
          
        }
        #endregion

     

    }
}
