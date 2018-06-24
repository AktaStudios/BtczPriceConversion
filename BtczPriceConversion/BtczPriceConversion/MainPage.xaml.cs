using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using BtczPriceConversion.Controls;
using System.IO;
using System.Reflection;
using Android.Content.Res;
using Android;
using Android.App;
using System.Xml.Linq;
using System.Collections.Specialized;

namespace BtczPriceConversion
{
	public partial class MainPage : TabbedPage
	{
        string cmcListingsUri = @"https://api.coinmarketcap.com/v2/listings/";
        string availableCurrenciesXmlFileName = "availablecurrencies.xml";
        string selectedCurrenciesXmlFileName = "selectedcurrencies.xml";
        Currencies availableCurrencies;
        Currencies selectedCurrencies;
        bool addCurrencyButtonClicked = false;
        CurrencySelectPage currencySelectPage;
        //public event EventHandler CalculatePrices;

        public MainPage()
		{
            GetAvailableCurrencies();
            GetSelectedCurrencies();
            
            selectedCurrencies.CurrencyList.CollectionChanged += AddedCurrenciesChanged;

            InitializeComponent();
            currencySelectPage = new CurrencySelectPage(availableCurrencies, selectedCurrencies, stackLayout, btczControl);
            btczControl.AddedCurrencies = selectedCurrencies;
            btczControl.CcyStackLayout = stackLayout;
            NavigationPage.SetHasNavigationBar(this, false);
            //Creating TapGestureRecognizers  
            var tapImage = new TapGestureRecognizer();
            //Binding events  
            tapImage.Tapped += addCurrencyButtonImage_Tapped;
            //Associating tap events to the image buttons  
            addCurrencyButtonImage.GestureRecognizers.Add(tapImage);
        }

        void addCurrencyButtonImage_Tapped(object sender, EventArgs e)
        {
            //// handle the tap  
            //DisplayAlert("Alert", "This is an image button", "OK");
            if (addCurrencyButtonClicked)
            {
                return;
            }
            addCurrencyButtonClicked = true;
            Navigation.PushAsync(currencySelectPage);
            addCurrencyButtonClicked = false;
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
        }

        private void GetSelectedCurrencies()
        {
            if (!SaveAndLoad.FileExists(selectedCurrenciesXmlFileName))
            {
                XDocument xdoc = XDocument.Load(Android.App.Application.Context.Assets.Open(selectedCurrenciesXmlFileName));
                SaveAndLoad.SaveText(selectedCurrenciesXmlFileName, xdoc.ToString());
            }

            var xmlString = SaveAndLoad.LoadText(selectedCurrenciesXmlFileName);

            var serializer = new XmlSerializer(typeof(Currencies));
            var strReader = new StringReader(xmlString);
            var xmlReader = new XmlTextReader(strReader);
            //XmlReader reader = XmlReader.Create(Android.App.Application.Context.Assets.Open(selectedCurrenciesXmlFileName));
            selectedCurrencies = (Currencies)serializer.Deserialize(xmlReader);
        }

        private void GetAvailableCurrencies()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Currencies));
            XmlReader reader = XmlReader.Create(Android.App.Application.Context.Assets.Open(availableCurrenciesXmlFileName));
            availableCurrencies = (Currencies)serializer.Deserialize(reader);
        }

      

        public void AddedCurrenciesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //This will get called when the currencies change
            StringWriter sw = new StringWriter();
            XmlTextWriter tw = null;
            XmlSerializer serializer = new XmlSerializer(typeof(Currencies));

            tw = new XmlTextWriter(sw);
            serializer.Serialize(tw, selectedCurrencies);
            SaveAndLoad.SaveText("selectedcurrencies.xml", sw.ToString());
        }



    }
}