


namespace Sales.ViewModels
{
    using System.Windows.Input;
    using Xamarin.Forms;
    using GalaSoft.MvvmLight.Command;
    using Sales.Helpers;
    using Sales.Services;
    using Sales.Common.Models;
    using System;
    using System.Linq;
    using Plugin.Media.Abstractions;
    using Plugin.Media;

    public class AddProductsViewModel:BaseViewModel
    {

        #region Attributes
        private bool isRunning, isEnabled;
        private ApiService apiService;
        // La  imagen puede ser local  o de internet
        private ImageSource imageSource;
       // esta linea la añado por la camara MediaFile, la foto queda  almacenada en el atributo
        private MediaFile file;

        #endregion


        #region Properties

        public string Description { get; set; }
        public string Price { get; set; }
        public string Remarks { get; set; }


        public ImageSource ImageSource{get { return this.imageSource; }set { SetValue(ref this.imageSource, value); }}
        public bool IsRunning { get { return this.isRunning; } set { SetValue(ref this.isRunning, value); } }
        public bool IsEnabled { get { return this.isEnabled; } set { SetValue(ref this.isEnabled, value); } }

        #endregion

        #region Constructors
        public AddProductsViewModel()
        {

            this.IsEnabled = true;
            this.apiService = new ApiService();
            this.ImageSource = "NoProduct.png";

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

        public ICommand SaveCommand
        {
            get{
                return new RelayCommand(Save);
            }
        }

        private async void Save()
        {

   

            if (string.IsNullOrEmpty(Description))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.DescriptionError,
                    Languages.Accept
                    );
                return;
            }
            if (string.IsNullOrEmpty(Price))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PriceError,
                    Languages.Accept
                    );
                return;
            }

            decimal price=0;
            if (!decimal.TryParse(Price,out price))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PriceError,
                    Languages.Accept
                    );
                return;
            }

             price = decimal.Parse(Price);
            if (price<0)
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
            }


            string strUrl = Application.Current.Resources["UrlAPI"].ToString();
            string strPrefix = Application.Current.Resources["Prefix"].ToString();
            string strControllerProduct = Application.Current.Resources["ControllerProduct"].ToString();
            var product = new Product() {
                Description=this.Description,
                Price=Convert.ToDecimal(this.Price),
                Remarks= this.Remarks,
                ImageArray = imageArrayAux

            };



            var  post= await  apiService.Post(strUrl, strPrefix, strControllerProduct,product, Settings.TokenType, Settings.AccessToken);

            if (!post.IsSuccess)
            {

                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, post.Message, Languages.Accept);
                return;
            }

            /*
             ESTO LO HAGO POR EL PATRON  SINGLETON  OBTENGO  EL PRODUCT NUEVO Y LLO AÑADO  A LIST
             */

            var newProduct = (Product)post.Result;
            var productViewModel = ProductsViewModel.GetInstance();
            productViewModel.MyProducts.Add(newProduct);
            productViewModel.RefreshList();


            /*

            ProductItemViewModel itemProductNew = new ProductItemViewModel()
            {

                ProductId = newProduct.ProductId,
                Description = newProduct.Description,
                Price = newProduct.Price,
                ImageArray = newProduct.ImageArray,
                IsVariable = newProduct.IsVariable,
                ImagePath = newProduct.ImagePath,
                PublishOn = newProduct.PublishOn,
                Remarks = newProduct.Remarks
            };



            productViewModel.Products.Add(itemProductNew);
            productViewModel.Products = productViewModel.Products;*/

            this.isEnabled = true;
            this.isRunning = false;

            await Application.Current.MainPage.Navigation.PopAsync();
       

        }

        #endregion

    }
}
