using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace BtczPriceConversion
{
    public class Currency
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Code")]
        public string Code { get; set; }

        public double Price { get; set; }

        public string ConversionUri { get; set; }

        //public int ID { get; set; }

    }
}
