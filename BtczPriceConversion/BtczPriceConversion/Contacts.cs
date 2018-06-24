using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace BtczPriceConversion
{
    [XmlRoot("Contacts")]
    public class Contacts
    {
        string contactsXmlFileName = "contacts.xml";
        public ObservableCollection<Contact> FilteredContacts { get; set; }

        [XmlElement("Contact")]
        public ObservableCollection<Contact> ContactList { get; set; }

        public Contacts()
        {
        }

        public void Iniitialise()
        {
            GetContacts();
            FilteredContacts = new ObservableCollection<Contact>();
            UpdateFilteredContacts(ContactList);
            ContactList.CollectionChanged += ContactsChanged;
        }


        public void UpdateFilteredContacts(IList<Contact> contactList)
        {
            FilteredContacts.Clear();
            foreach (var x in contactList)
            {
                FilteredContacts.Add(x);
            }
        }

        public void ContactsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //This will get called when the currencies change
            StringWriter sw = new StringWriter();
            XmlTextWriter tw = null;
            XmlSerializer serializer = new XmlSerializer(typeof(Contacts));

            tw = new XmlTextWriter(sw);
            serializer.Serialize(tw, this);
            SaveAndLoad.SaveText("contacts.xml", sw.ToString());

            UpdateFilteredContacts(ContactList);
        }

        private void GetContacts()
        {
            if (!SaveAndLoad.FileExists(contactsXmlFileName))
            {
                XDocument xdoc = XDocument.Load(Android.App.Application.Context.Assets.Open(contactsXmlFileName));
                SaveAndLoad.SaveText(contactsXmlFileName, xdoc.ToString());
            }

            var xmlString = SaveAndLoad.LoadText(contactsXmlFileName);

            var serializer = new XmlSerializer(typeof(Contacts));
            var strReader = new StringReader(xmlString);
            var xmlReader = new XmlTextReader(strReader);
            var _contacts = (Contacts)serializer.Deserialize(xmlReader);
            ContactList = new ObservableCollection<Contact>(_contacts.ContactList);
        }

    }
}
