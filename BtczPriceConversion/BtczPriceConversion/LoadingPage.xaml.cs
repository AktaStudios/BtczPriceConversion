using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BtczPriceConversion
{
    public partial class LoadingPage : ContentPage
    {

        public LoadingPage()
        {
            InitializeComponent();
            progressBar.ProgressTo(100, 5000, Easing.Linear);
        }

        public void ClosePage()
        {
            Navigation.PopAsync();
        }

}
}