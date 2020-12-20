

namespace Sales.Views
{


    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddProductPage : ContentPage
	{

        public AddProductPage()
        {
            InitializeComponent();

        }


            public AddProductPage (string  strNombre)
		{
			InitializeComponent ();
            lblHome.Text = strNombre;
		}
	}
}