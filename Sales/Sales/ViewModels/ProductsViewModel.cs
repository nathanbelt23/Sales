



namespace Sales.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Sales.Common.Models;
    using Sales.Helpers;
    using Sales.Services;
    using Xamarin.Forms;

    public class ProductsViewModel : BaseViewModel
    {


        private ApiService apiService;
        //  Con la p minuscula es private
        private ObservableCollection<Product> products;

        // IsRefreshing uno lo  ve en el listview de products

        private bool isRefreshing;

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }

            set
            {
                SetValue(ref this.isRefreshing, value);
            }
        }

        public ObservableCollection<Product> Products
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

        public ProductsViewModel()
        {

            this.apiService = new ApiService();
            this.LoadProducts();
        }

        private async void LoadProducts()
        {

            ApiService api = new ApiService();

            var isConecction = await api.CheckConnection();

            if (!isConecction.IsSuccess)
            {

                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, isConecction.Message, Languages.Accept);
                return;
            }


                string strUrl =  Application.Current.Resources["UrlAPI"].ToString();
          

          this.IsRefreshing = true;
            //string strUrl = await Application.Current.Resources["UrlAPI"].ToString();
            var response = await apiService.GetList<Product>(strUrl, "/Sales.API/Api", "/Products");
            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }
            var list = (List<Product>)response.Result;
            this.Products = new ObservableCollection<Product>(list);
            this.IsRefreshing = false;
        }

        public ICommand RefreshCommand {

            get
            {
                return new RelayCommand(LoadProducts);
            }

              


                }
    }
}
