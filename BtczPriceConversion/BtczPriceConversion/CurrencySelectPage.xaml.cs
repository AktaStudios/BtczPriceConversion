using BtczPriceConversion.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BtczPriceConversion
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CurrencySelectPage : ContentPage
	{

        Currencies selectedCurrencies;
        IList<Currency> filteredCurrencies;
        StackLayout _stackLayout;
        CurrencyControl _btczControl;
        string conversionUri;

        public CurrencySelectPage(Currencies availableCurrencies, Currencies preSelectedCurrencies, StackLayout stackLayout, CurrencyControl btczControl)
		{
            _btczControl = btczControl;
            _stackLayout = stackLayout;
            conversionUri = availableCurrencies.ConversionUri;

            selectedCurrencies = preSelectedCurrencies;

            var addedCurrencies = new List<Currency>(selectedCurrencies.CurrencyList.ToList());

            AvailableCurrencies = availableCurrencies.CurrencyList;
            filteredCurrencies = availableCurrencies.CurrencyList.Select(x => x).ToList();
            InitializeComponent();
            currencyList.ItemsSource = FilteredCurrencies;
            NavigationPage.SetHasNavigationBar(this, false);

            if (selectedCurrencies != null)
            {
                foreach (var ccy in addedCurrencies)
                {
                    ccy.ConversionUri = String.Format("{0}{1}", conversionUri, ccy.Code);
                    AddCurrencyControl(ccy);
                }
            }

        }

        public IList<Currency> AvailableCurrencies { get; set; }
        public ObservableCollection<Currency> FilteredCurrencies { get { return new ObservableCollection<Currency>(filteredCurrencies); } }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            filteredCurrencies = AvailableCurrencies.Where(x => x.Name.ToLower().Contains(e.NewTextValue.ToLower()) || x.Code.ToLower().Contains(e.NewTextValue.ToLower())).ToList();
            currencyList.ItemsSource = FilteredCurrencies;
        }

        private void currencyList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var ccy = ((Currency)(e.Item));
            ccy.ConversionUri = String.Format("{0}{1}", conversionUri, ccy.Code);
            if (selectedCurrencies.CurrencyList.Contains(ccy))
            {
                Navigation.PopAsync();
                return;
            }
            AddCurrencyControl(ccy);
            Navigation.PopAsync();
        }

        private void AddCurrencyControl(Currency ccy)
        {
            if(!selectedCurrencies.CurrencyList.Contains(ccy))
            {
                selectedCurrencies.CurrencyList.Add(ccy);
            }
            var ccyControl = new CurrencyControl(ccy, selectedCurrencies, _stackLayout, _btczControl);
            _stackLayout.Children.Add(ccyControl);
        }
    }
}