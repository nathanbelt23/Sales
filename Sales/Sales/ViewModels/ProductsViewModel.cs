



namespace Sales.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Sales.Common.Models;
    using Sales.Helpers;
    using Sales.Services;
    using Xamarin.Forms;

    public class ProductsViewModel : BaseViewModel
    {

        #region Atributos
        private ApiService apiService;
        //  Con la p minuscula es private
        private ObservableCollection<ProductItemViewModel> products;
        // IsRefreshing uno lo  ve en el listview de products
       
        private bool isRefreshing;

        private  string filter;
        //
        private DataService dataService;


        #endregion

        #region Propiedades
       

        public List<Product> MyProducts { get; set; }
        public string Filter {
            get {
                return this.filter;
                    }
            set {
                this.filter = value;
                this.RefreshList();
            }
        }
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }

            set
            {
                SetValue(ref this.isRefreshing, value);
            }
        }

        public ObservableCollection<ProductItemViewModel> Products
        {
            get
            {
                return this.products;
            }
            set
            {
                this.SetValue(ref this.products, value);
            }
        }
        #endregion

        #region  Constructor
        public ProductsViewModel()
        {
            instance = this;
            this.apiService = new ApiService();
            this.dataService = new DataService();
            this.LoadProducts();
        }

        #endregion

        #region Metodos
        private async void LoadProducts()
        {

            ApiService api = new ApiService();


            var isConecction = await api.CheckConnection();


            if (isConecction.IsSuccess)
            {
                var answer = await LoadProductsFromAPI();
                if (answer)
                {
                   this.SaveProductsDB();
                }

            }
            else
            {
                await this.LoadProductsDB();
        
            }

         

            if (this.MyProducts.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, Languages.NoProductsMessage, Languages.Accept);

                return;
            }
         
            this.RefreshList();
            this.IsRefreshing = false;
        }

        private async Task LoadProductsDB()
        {
          this.MyProducts = await this.dataService.GetAllProducts();
        }

        private async void SaveProductsDB()
        {

            //await 
            try
            {
              await  dataService.DeleteAllProducts();
            }
            catch (Exception E)
            {
                var str = E.Message;
            }
                dataService.Insert(MyProducts);
        }

        private async Task<bool> LoadProductsFromAPI()
        {
            string strUrl = Application.Current.Resources["UrlAPI"].ToString();


            this.IsRefreshing = true;
            //string strUrl = await Application.Current.Resources["UrlAPI"].ToString();
            var response = await apiService.GetList<Product>(strUrl, "/Sales.API/Api", "/Products", Settings.TokenType, Settings.AccessToken);
            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return false;
            }

            this.MyProducts = (List<Product>)response.Result;
            return true;
        }

        public void RefreshList()
        {

            if (string.IsNullOrEmpty(Filter))
            {
                var myListProductItemViewModel = this.MyProducts.Select(p => new ProductItemViewModel()
                {

                    ProductId = p.ProductId,
                    Description = p.Description,
                    Price = p.Price,
                    ImageArray = p.ImageArray,
                    IsVariable = p.IsVariable,
                    ImagePath = p.ImagePath,
                    PublishOn = p.PublishOn,
                    Remarks = p.Remarks

                });

                this.Products = new ObservableCollection<ProductItemViewModel>(myListProductItemViewModel.OrderBy(P => P.Description));

            }

            else

            {
                var myListProductItemViewModel = this.MyProducts.Select(p => new ProductItemViewModel()
                {

                    ProductId = p.ProductId,
                    Description = p.Description,
                    Price = p.Price,
                    ImageArray = p.ImageArray,
                    IsVariable = p.IsVariable,
                    ImagePath = p.ImagePath,
                    PublishOn = p.PublishOn,
                    Remarks = p.Remarks

                }).Where(p=>p.Description.ToLower().Contains(Filter.ToLower() ) || p.Remarks.ToLower().Contains(Filter.ToLower())).ToList();

                this.Products = new ObservableCollection<ProductItemViewModel>(myListProductItemViewModel.OrderBy(P => P.Description));

            }
        }

        public ICommand RefreshCommand {

            get
            {
                return new RelayCommand(LoadProducts);
            }
        }



        public ICommand SearchComand { get
            {
                return  new RelayCommand(RefreshList);
            }

        }

       



        #endregion

        #region Singleton

        public static ProductsViewModel instance;

        public static ProductsViewModel GetInstance()
        {
            if (instance == null)
                return new ProductsViewModel();
            return instance;

        }

        #endregion
    }
}

