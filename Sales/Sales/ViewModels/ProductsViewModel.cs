



namespace Sales.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Sales.Common.Models;
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


          this.IsRefreshing = true;
            //string strUrl = await Application.Current.Resources["UrlAPI"].ToString();
            var response = await apiService.GetList<Product>("http://192.168.0.11", "/Sales.API/Api", "/Products");
            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
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
