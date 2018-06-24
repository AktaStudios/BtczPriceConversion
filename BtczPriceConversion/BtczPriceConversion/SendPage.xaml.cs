using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BtczPriceConversion
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SendPage : ContentPage
	{
        Contacts _contacts;
        string amountToSend;
        public ObservableCollection<Contact> FilteredContacts { get; set; }

        public SendPage (string amount)
		{
            if (String.IsNullOrEmpty(amount) || amount == "0")
            {
                amount = "1";
            }
            amountToSend = amount;

            _contacts = new Contacts();
            _contacts.Iniitialise();
            FilteredContacts = _contacts.FilteredContacts;

            InitializeComponent ();

            contactList.ItemsSource = FilteredContacts;
            NavigationPage.SetHasNavigationBar(this, false);

        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filteredContactsList = _contacts.ContactList.Where(x => x.Name.ToLower().Contains(e.NewTextValue.ToLower()) || x.BtczAddress.ToLower().Contains(e.NewTextValue.ToLower()) || x.BtczMeAddress.ToLower().Contains(e.NewTextValue.ToLower())).ToList();
            _contacts.UpdateFilteredContacts(filteredContactsList);
        }

        private async void sendButton_Clicked(object sender, EventArgs e)
        {            
            var contact = contactList.SelectedItem;
            if(contact != null)
            {
                SendBtcz(((Contact)contact).BtczAddress);
            }
            else 
            {
                var searchBartext = contactSearchBar.Text;
                if (String.IsNullOrEmpty(searchBartext))
                {
                    await DisplayAlert("Error", "Please enter an address or select a contact", "Ok");
                }
                else
                {
                    if (searchBartext.Contains("https://btcz.me/"))
                    {
                        try
                        {
                            var html = await GetResource.GetHtmlSource(searchBartext);
                            var btczAdd = html.Substring(html.IndexOf("bitcoinz:"));
                            var btczAdd2 = btczAdd.Substring(btczAdd.IndexOf(":") + 1);
                            var btczAdd3 = btczAdd2.Substring(0, btczAdd2.IndexOf("\""));

                            SendBtcz(btczAdd3);
                        }
                        catch (Exception ex)
                        {
                            await DisplayAlert("Error", String.Format("Unable to retrieve BitcoinZ address from {0}. Please enter BitcoinZ address manually", searchBartext), "Ok");
                        }
                    }
                    else
                    {
                        SendBtcz(searchBartext);
                    }
                }
            }
            //bitcoinz: t1ShY9WZhePe29znStcQjsgek6dtHkHy8gz
        }

        private async void SendBtcz(string address)
        {
            try
            {
                Device.OpenUri(new Uri(String.Format("bitcoinz:{0}?amount={1}", address, Double.Parse(amountToSend, NumberStyles.Any, CultureInfo.InvariantCulture).ToString())));
            }
            catch
            {
                await DisplayAlert("Error", "Cannot send.  BitcoinZ Anroid Wallet or Coinomi is not installed on device", "Ok");
            }
        }

        private void contactSearchBar_Focused(object sender, FocusEventArgs e)
        {
            contactList.SelectedItem = null;
        }
    }
}