using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BtczPriceConversion.Controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CurrencyControl : Xamarin.Forms.Grid
    {

        public static readonly BindableProperty CurrencyNameProperty = BindableProperty.Create(nameof(CurrencyName), typeof(string), typeof(CurrencyControl), default(string), BindingMode.TwoWay);
        public static readonly BindableProperty CurrencyCodeProperty = BindableProperty.Create(nameof(CurrencyCode), typeof(string), typeof(CurrencyControl), default(string), BindingMode.TwoWay);
        public static readonly BindableProperty CurrencyAmountProperty = BindableProperty.Create(nameof(CurrencyAmount), typeof(string), typeof(CurrencyControl), default(string), BindingMode.TwoWay);
        public static readonly BindableProperty CurrencyRateProperty = BindableProperty.Create(nameof(CurrencyRate), typeof(string), typeof(CurrencyControl), default(string), BindingMode.TwoWay);
        public Currencies AddedCurrencies { get; set; }
        public StackLayout CcyStackLayout { get; set; }
        CurrencyControl _btczControl;

        public CurrencyControl()
		{
			InitializeComponent();
            SetupTapGesture();
        }

        public CurrencyControl(Currency currency, Currencies addedCurrencies, StackLayout stackLayout, CurrencyControl btczControl)
        {
            InitializeComponent();
            Currency = currency;
            CurrencyName = String.Format("({0})", currency.Name);
            CurrencyCode = currency.Code;
            AddedCurrencies = addedCurrencies;
            CcyStackLayout = stackLayout;
            _btczControl = btczControl;
            SetupTapGesture();
            CurrencyRate = "";
        }

        void SetupTapGesture()
        {
            //Creating TapGestureRecognizers  
            var tapImage = new TapGestureRecognizer();
            //Binding events  
            tapImage.Tapped += removeButton_Tapped;
            //Associating tap events to the image buttons  
            removeButton.GestureRecognizers.Add(tapImage);
        }

        void removeButton_Tapped(object sender, EventArgs e)
        {
            //// handle the tap  
            //DisplayAlert("Alert", "This is an image button", "OK");
            AddedCurrencies.CurrencyList.Remove(Currency);
            CcyStackLayout.Children.Remove(this);
        }

        public Currency Currency { get; set; }

        public string CurrencyName
        {
            get
            {
                return (string)GetValue(CurrencyNameProperty);
            }

            set
            {
                SetValue(CurrencyNameProperty, value);
            }
        }

        public string CurrencyCode
        {
            get
            {
                return (string)GetValue(CurrencyCodeProperty);
            }

            set
            {
                SetValue(CurrencyCodeProperty, value);
            }
        }

        public string CurrencyAmount
        {
            get
            {
                return (string)GetValue(CurrencyAmountProperty);
            }

            set
            {
                SetValue(CurrencyAmountProperty, value);
            }
        }

        public string CurrencyRate
        {
            get
            {
                return (string)GetValue(CurrencyRateProperty);
            }

            set
            {
                SetValue(CurrencyRateProperty, value);
            }
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == CurrencyCodeProperty.PropertyName)
            {
                currencyCode.Text = CurrencyCode;
                if (CurrencyCode == "BTCZ")
                {
                    removeButton.IsVisible = false;
                    currencyRate.IsVisible = false;
                    sendButton.IsVisible = true;
                    //receiveButton.IsVisible = true;
                }
            }
            else if (propertyName == CurrencyNameProperty.PropertyName)
            {
                currencyName.Text = CurrencyName;
                //if (CurrencyCode == "BTCZ")
                //{
                //    removeButton.IsVisible = false;
                //}
            }
            else if (propertyName == CurrencyAmountProperty.PropertyName)
            {
                currencyAmount.Text = CurrencyAmount.ToString();
            }
            else if (propertyName == CurrencyRateProperty.PropertyName)
            {
                currencyRate.Text = String.Format("Rate: {0}", CurrencyRate.ToString());
            }

        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {            
            CurrencyAmount = String.IsNullOrEmpty(e.NewTextValue) ? "0" : e.NewTextValue;
        }

        private async void currencyAmount_Completed(object sender, EventArgs e)
        {
          
            var loadingPage = new LoadingPage();

            try
            {
                PushControlToTopOfStack();
                if (String.IsNullOrEmpty(CurrencyAmount))
                {
                    return;
                }
                Navigation.PushAsync(loadingPage);

                await GetPrices();
                if (CurrencyCode == "BTCZ")
                {
                    foreach (var ccy in AddedCurrencies.CurrencyList)
                    {
                        var numberFormat = ccy.Code == "BTC" || ccy.Code == "LTC" || ccy.Code == "BCH" || ccy.Code == "ETH" || ccy.Code == "XRP" ? "N8" : "N2";
                        CurrencyControl ccyControl = (CurrencyControl)CcyStackLayout.Children.Where(x => ((CurrencyControl)x).Currency.Code == ccy.Code).First();
                        ccyControl.CurrencyAmount = (ccy.Price * Double.Parse(CurrencyAmount, NumberStyles.Any, CultureInfo.InvariantCulture)).ToString(numberFormat, CultureInfo.InvariantCulture);
                        ccyControl.CurrencyRate = ccy.Price.ToString("N8", CultureInfo.InvariantCulture);
                    }
                }
                else
                {

                    _btczControl.CurrencyAmount = (Double.Parse(CurrencyAmount, NumberStyles.Any, CultureInfo.InvariantCulture) / Currency.Price).ToString("N2", CultureInfo.InvariantCulture);


                    foreach (var ccy in AddedCurrencies.CurrencyList)
                    {
                        var numFormat = ccy.Code == "BTC" || ccy.Code == "LTC" || ccy.Code == "BCH" || ccy.Code == "ETH" || ccy.Code == "XRP" ? "N8" : "N2";
                        CurrencyControl ccyControl = (CurrencyControl)CcyStackLayout.Children.Where(x => ((CurrencyControl)x).Currency.Code == ccy.Code).First();
                        ccyControl.CurrencyAmount = (ccy.Price * Double.Parse(_btczControl.CurrencyAmount, NumberStyles.Any, CultureInfo.InvariantCulture)).ToString(numFormat, CultureInfo.InvariantCulture);
                        ccyControl.CurrencyRate = ccy.Price.ToString("N8", CultureInfo.InvariantCulture);
                    }

                }
                loadingPage.ClosePage();
            }
            catch (Exception ex)
            {
                loadingPage.ClosePage();
                var errorsPage = new ErrorsPage(String.Format("An Error Occurred retrieving prices.  Check internet connection and try again later.  Please send below error to developer for further investigation. \n {0} \n {1}", ex.Message, ex.StackTrace));
                Navigation.PushAsync(errorsPage);
                //DisplayAlert("Error", "Error Retrieving Prices", "OK");
            }
        }

        private void PushControlToTopOfStack()
        {
            if (CurrencyCode != "BTCZ" && CcyStackLayout.Children.Count > 1)
            {

                CcyStackLayout.Children.Remove(this);
                CcyStackLayout.Children.Insert(0, this);
            }
        }

        private async Task GetPrices()
        {
            foreach(var ccy in AddedCurrencies.CurrencyList)
            {
                //dynamic ccyData = JsonConvert.DeserializeObject<dynamic>(await GetResource.GetJsonResource(ccy.ConversionUri));
                JObject ccyData;
                try
                { 
                    ccyData = JObject.Parse(await GetResource.GetJsonResource(ccy.ConversionUri));
                }
                catch (Exception ex)
                {
                    throw new Exception(String.Format("Could not retrieve prices from api: {0}.  Please check your internet connection.", ccy.ConversionUri));
                }

                foreach (var currency in ccyData.First)
                {
                    var price = (string)currency["quotes"][ccy.Code]["price"];
                    ccy.Price = Double.Parse(price, NumberStyles.Any, CultureInfo.InvariantCulture);
                }
            }
            
        }

        private void sendButton_Clicked(object sender, EventArgs e)
        {            
            Navigation.PushAsync(new SendPage(CurrencyAmount));
        }

        private void receiveButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}