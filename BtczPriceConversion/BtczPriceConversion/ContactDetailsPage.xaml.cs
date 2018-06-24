using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BtczPriceConversion
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ContactDetailsPage : ContentPage
	{
        Contact newContact;
        Contact _contact;
        Contacts _contacts;

        public ContactDetailsPage(Contacts contacts)
        {
            _contacts = contacts;
            InitializeComponent();
            deleteContactButton.IsVisible = false;
            newContact = new Contact();
            BindingContext = newContact;
            NavigationPage.SetHasNavigationBar(this, false);
        }

        public ContactDetailsPage (Contacts contacts, Contact contact)
		{
            _contacts = contacts;
            _contact = contact;
			InitializeComponent ();
            newContact = new Contact();
            BindingContext = newContact;
            UpdateContact(_contact, newContact);
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private void saveContactButton_Clicked(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(newContact.Name))
            {
                DisplayAlert("Error", "Please Enter a Valid Name", "Ok");
                return;
            }
            if (String.IsNullOrEmpty(newContact.BtczAddress) && String.IsNullOrEmpty(newContact.BtczMeAddress))
            {
                DisplayAlert("Error", "Please Enter a Btcz.Me or Btcz Address", "Ok");
                return;
            }
            if(!String.IsNullOrEmpty(newContact.BtczMeAddress))
            {
                if(!newContact.BtczMeAddress.Contains("https://btcz.me/"))
                {
                    DisplayAlert("Error", "Btcz.Me Address is invalid.  Address should be in the format https://btcz.me/Username", "Ok");
                    return;
                }
            }

            if (_contacts.ContactList.Contains(_contact))
            {
                UpdateContact(newContact, _contact);
                Navigation.PopAsync();
            }
            else
            {
                if(_contacts.ContactList.Where(x => x.Name == newContact.Name).Any())
                {
                    DisplayAlert("Error", String.Format("There is already a contact with the name, {0}",newContact.Name), "Ok");
                    return;
                }
                _contacts.ContactList.Add(newContact);
                Navigation.PopAsync();
            }

        }

        void UpdateContact(Contact oldContact, Contact newContact)
        {
            newContact.Name = oldContact.Name;
            newContact.BtczMeAddress = oldContact.BtczMeAddress;
            newContact.BtczAddress = oldContact.BtczAddress;
            newContact.EmailAddress = oldContact.EmailAddress;
        }

        private void deleteContactButton_Clicked(object sender, EventArgs e)
        {
            if (_contacts.ContactList.Contains(_contact))
            {
                _contacts.ContactList.Remove(_contact);
                Navigation.PopAsync();
            }
        }

        private async void contactBtczMeAddress_Completed(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(newContact.BtczMeAddress))
            {
                if (!newContact.BtczMeAddress.Contains("https://btcz.me/"))
                {
                    await DisplayAlert("Error", "Btcz.Me Address is invalid.  Address should be in the format https://btcz.me/Username", "Ok");
                    newContact.BtczMeAddress = "";
                    return;
                }
                else
                {
                    if (String.IsNullOrEmpty(newContact.BtczAddress))
                    {
                        try
                        {
                            var html = await GetResource.GetHtmlSource(newContact.BtczMeAddress);
                            var btczAdd = html.Substring(html.IndexOf("bitcoinz:"));
                            var btczAdd2 = btczAdd.Substring(btczAdd.IndexOf(":")+1);
                            var btczAdd3 = btczAdd2.Substring(0, btczAdd2.IndexOf("\""));

                            newContact.BtczAddress = btczAdd3;
                        }
                        catch(Exception ex)
                        {
                            await DisplayAlert("Error", String.Format("Unable to retrieve BitcoinZ address from {0}. Please enter BitcoinZ address manually", newContact.BtczMeAddress), "Ok");
                        }
                        //bitcoinz:t1ShY9WZhePe29znStcQjsgek6dtHkHy8gz
                    }

                }
            }
        }
    }
}