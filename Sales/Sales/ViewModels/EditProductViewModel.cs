

namespace Sales.ViewModels
{
    using System;
    using System.Linq;
    using System.Windows.Input;
    using Xamarin.Forms;
    using GalaSoft.MvvmLight.Command;
    using Sales.Helpers;
    using Sales.Services;
    using Sales.Common.Models;
    using Plugin.Media.Abstractions;
    using Plugin.Media;


    public  class EditProductViewModel : BaseViewModel
    {
        #region Atributos
        private Product product;
        private bool isRunning, isEnabled;
        private ApiService apiService;
        // La  imagen puede ser local  o de internet
        private ImageSource imageSource;
        // esta linea la añado por la camara MediaFile, la foto queda  almacenada en el atributo
        private MediaFile file;

        #endregion

        #region Propiedades

        public Product Product { get { return this.product; } set { SetValue(ref product, value); } }

        public ImageSource ImageSource { get { return this.imageSource; } set { SetValue(ref this.imageSource, value); } }
        public bool IsRunning { get { return this.isRunning; } set { SetValue(ref this.isRunning, value); } }
        public bool IsEnabled { get { return this.isEnabled; } set { SetValue(ref this.isEnabled, value); } }
        #endregion



        #region Constructor
        public EditProductViewModel(Product product)
        {
            this.Product = product;

            this.IsEnabled = true;
            this.apiService = new ApiService();
            this.ImageSource = this.product.ImageFullPath;

        }
        #endregion


        #region Commands



        public ICommand ChangueImageCommand
        {
            get
            {
                return new RelayCommand(ChangueImage);
            }
        }

        private async void ChangueImage()
        {
            // Inicializa la libreria de fotos
            await Plugin.Media.CrossMedia.Current.Initialize();
            // este muestra un mensaje  un popoup
            var source = await Application.Current.MainPage.DisplayActionSheet(
                Languages.ImageSource,
                Languages.Cancel,
                null,
                Languages.FromGallery,
                Languages.NewPicture);

            if (source == Languages.Cancel)
            {
                this.file = null;
                return;
            }


            if (source == Languages.NewPicture)
            {
                // ESTA linea tomo  la foto
                this.file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = "test.jpg",
                        PhotoSize = PhotoSize.Small,
                    }
                );
            }
            else
            {
                //Busca la  foto en la libreria
                this.file = await CrossMedia.Current.PickPhotoAsync();
            }

            //  esta  sacala imagen
            if (this.file != null)
            {
                this.ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = this.file.GetStream();
                    return stream;
                });
            }
        }

        public ICommand SaveUpdateCommand
        {
            get
            {
                return new RelayCommand(SaveUpdate);
            }
        }

        private async void SaveUpdate()
        {



            if (string.IsNullOrEmpty(Product.Description))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.DescriptionError,
                    Languages.Accept
                    );
                return;
            }

            if (this.Product.Price< 0)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PriceError,
                    Languages.Accept
                    );
                return;
            }
            apiService = new ApiService();
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


            byte[] imageArrayAux = null;
            if (this.file != null)
            {
                imageArrayAux = FilesHelper.ReadFully(this.file.GetStream());
                this.Product.ImageArray = imageArrayAux;
            }


            string strUrl = Application.Current.Resources["UrlAPI"].ToString();
            string strPrefix = Application.Current.Resources["Prefix"].ToString();
            string strControllerProduct = Application.Current.Resources["ControllerProduct"].ToString();
              var put = await apiService.Put(strUrl, strPrefix, strControllerProduct, this.Product, this.Product.ProductId, Settings.TokenType, Settings.AccessToken);

            if (!put.IsSuccess)
            {

                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, put.Message, Languages.Accept);
                return;
            }

            /*
             ESTO LO HAGO POR EL PATRON  SINGLETON  OBTENGO  EL PRODUCT NUEVO Y LLO AÑADO  A LIST
             */

            var newProduct = (Product) put.Result;
            var productViewModel = ProductsViewModel.GetInstance();
            var oldProduct = productViewModel.MyProducts.Where(p => p.ProductId == newProduct.ProductId).FirstOrDefault();   

            if (oldProduct != null)
            {
                productViewModel.MyProducts.Remove(oldProduct);
            }


            productViewModel.MyProducts.Add(newProduct);
            productViewModel.RefreshList();



            this.isEnabled = true;
            this.isRunning = false;

            await Application.Current.MainPage.Navigation.PopAsync();


        }

        #endregion

        public ICommand DeleteCommand
        {

            get
            {

                return new RelayCommand(DeleteProduct);

            }
        }

        private async void DeleteProduct()
        {

            var answer = await Application.Current.MainPage.DisplayAlert(Languages.Confirmation, Languages.DeleteConfirmation, Languages.Yes, Languages.No);


            this.IsRunning = true;
            this.IsEnabled = false;

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

            var response = await apiService.Delete(Application.Current.Resources["UrlAPI"].ToString(), Application.Current.Resources["Prefix"].ToString(), Application.Current.Resources["ControllerProduct"].ToString(), this.Product.ProductId, Settings.TokenType, Settings.AccessToken);
            if (!response.IsSuccess)
            {

                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }


            var productsViewModel = ProductsViewModel.GetInstance();
            var deleteProduct = productsViewModel.Products.Where(p => p.ProductId == this.Product.ProductId).FirstOrDefault();

            if (deleteProduct != null)
            {

                productsViewModel.Products.Remove(deleteProduct);

            }

            this.IsRunning = false;
            this.IsEnabled = true; 
            productsViewModel.RefreshList();

            await Application.Current.MainPage.Navigation.PopAsync();


        }


    }
}
