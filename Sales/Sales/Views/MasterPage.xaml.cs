using Sales.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sales.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MasterPage : MasterDetailPage
	{
		public    MasterPage ()
		{
            MainViewModel.GetInstance().Products = new ProductsViewModel();

            InitializeComponent ();
           // cargarDefecto();
            
		}

        private async void cargarDefecto()
        {
            MainViewModel.GetInstance().Products = new ProductsViewModel();
            await    this.Navigator.PushAsync(new ProductsPage());
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.Navigator = Navigator;
        }

    }
}