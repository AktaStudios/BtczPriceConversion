using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BtczPriceConversion
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ErrorsPage : ContentPage
	{
		public ErrorsPage ()
		{
			InitializeComponent ();
		}

        public ErrorsPage(string errorMessage)
        {
            InitializeComponent();
            errorLabel.Text = errorMessage;
        }
    }
}