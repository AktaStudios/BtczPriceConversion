using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using System.Xml.Serialization;

namespace BtczPriceConversion
{
    [XmlRoot("Currencies")]
    public class Currencies
    {
        
        public Currencies()
        {
            CurrencyList = new ObservableCollection<Currency>();
            //CurrencyList.CollectionChanged += CurrenciesChanged;
        }

        [XmlElement("Currency")]
        public ObservableCollection<Currency> CurrencyList { get; set; }

        [XmlElement("ConversionUri")]
        public string ConversionUri { get; set; }

        //public void CurrenciesChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    //This will get called when the currencies change
        //}

    }
}
