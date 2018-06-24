using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BtczPriceConversion
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ContactsPage : ContentPage
	{
        Contacts _contacts;
        public ObservableCollection<Contact> FilteredContacts { get; set; }

        public ContactsPage()
        {
            _contacts = new Contacts();
            _contacts.Iniitialise();
            FilteredContacts = _contacts.FilteredContacts;
            InitializeComponent();
            contactsList.ItemsSource = FilteredContacts;
            NavigationPage.SetHasNavigationBar(this, false);
        }      

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var   filteredContactsList = _contacts.ContactList.Where(x => x.Name.ToLower().Contains(e.NewTextValue.ToLower()) || x.BtczAddress.ToLower().Contains(e.NewTextValue.ToLower()) || x.BtczMeAddress.ToLower().Contains(e.NewTextValue.ToLower())).ToList();
            _contacts.UpdateFilteredContacts(filteredContactsList);
        }

        private void contactsList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var ctc = ((Contact)(e.Item));
            Navigation.PushAsync(new ContactDetailsPage(_contacts, ctc));
        }

        private void addContactButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ContactDetailsPage(_contacts));
        }
    }
}