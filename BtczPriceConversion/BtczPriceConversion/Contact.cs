using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace BtczPriceConversion
{    
    public class Contact : INotifyPropertyChanged
    {
        string name;
        string btczMeAddress;
        string btczAddress;
        string emailAddress;

        [XmlElement("Name")]
        public string Name {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        [XmlElement("BtczMeAddress")]
        public string BtczMeAddress
        {
            get { return btczMeAddress; }
            set
            {
                if (btczMeAddress != value)
                {
                    btczMeAddress = value;
                    OnPropertyChanged("BtczMeAddress");
                }
            }
        }

        [XmlElement("BtczAddress")]
        public string BtczAddress
        {
            get { return btczAddress; }
            set
            {
                if (btczAddress != value)
                {
                    btczAddress = value;
                    OnPropertyChanged("BtczAddress");
                }
            }
        }

        [XmlElement("EmailAddress")]
        public string EmailAddress
        {
            get { return emailAddress; }
            set
            {
                if (emailAddress != value)
                {
                    emailAddress = value;
                    OnPropertyChanged("EmailAddress");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
