using BringMeBack.Class;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Windows.Foundation.Metadata;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BringMeBack
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class AddChildren : Page
    {
        private string name_children;
        private string firstname_children;
        private User user = null;
        private List<User> Users = new List<User>();



        public AddChildren()
        {
            this.InitializeComponent();           

        }

        

        private void BackMainParent(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;            
            rootFrame.Navigate(typeof(MainParent));
            
        }

        private async void Button_Rechercher(object sender, RoutedEventArgs e)
        {
            
            name_children = Name.Text;
            firstname_children = FirstName.Text;

            //Create an HTTP client object
            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();

            //Add a user-agent header to the GET request. 
            var headers = httpClient.DefaultRequestHeaders;

            //The safe way to add a header value is to use the TryParseAdd method and verify the return value is true,
            //especially if the header value is coming from user input.
            string header = "ie";
            if (!headers.UserAgent.TryParseAdd(header))
            {
                throw new Exception("Invalid header value: " + header);
            }

            header = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)";
            if (!headers.UserAgent.TryParseAdd(header))
            {
                throw new Exception("Invalid header value: " + header);
            }

            Uri requestUri = new Uri("http://businesswallet.fr/Bmb/Get_Children_ByName.php?name_children=" + name_children + "&firstname_children=" + firstname_children);

            //Send the GET request asynchronously and retrieve the response as a string.
            Windows.Web.Http.HttpResponseMessage httpResponse = new Windows.Web.Http.HttpResponseMessage();
            string httpResponseBody = "";

            try
            {
                //Send the GET request
                httpResponse = await httpClient.GetAsync(requestUri);

                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();

                user = JsonConvert.DeserializeObject<User>(httpResponseBody);
            }
            catch (Exception ex)
            {
                httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;

            }
            if (!(user is null))
            {
                ListViewUsers.Visibility = Visibility.Visible;                
                Users.Add(user);
                ListViewUsers.ItemsSource = Users;

            }
            else
            {
                libelleErreur.Text = "Aucun utilisateur existe avec ce couple Nom/Prenom";
            }
        }

        private void Confir_ItemClick(object sender, ItemClickEventArgs e)
        {

            dialog_Confirm();
            


        }

        private async void dialog_Confirm()
        {

            var title = "Confirmation de l'ajout d'un enfant";
            var content = "Ëtes vous sur de vouloir ajouter cet utilisateur en tant qu'enfant ?";

            var yesCommand = new UICommand("oui", cmd => {});
            
            var cancelCommand = new UICommand("Annuler", cmd => {});

            var dialog = new MessageDialog(content, title);
            dialog.Options = MessageDialogOptions.None;
            dialog.Commands.Add(yesCommand);

            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 0;           

            if (cancelCommand != null)
            {
                
                var t_hardwareBackButton = "Windows.Phone.UI.Input.HardwareButtons";

                if (ApiInformation.IsTypePresent(t_hardwareBackButton))
                {
                    // disable the default Cancel command index
                    // so that dialog.ShowAsync() returns null
                    // in that case

                    dialog.CancelCommandIndex = UInt32.MaxValue;
                }
                else
                {
                    dialog.Commands.Add(cancelCommand);
                    dialog.CancelCommandIndex = (uint)dialog.Commands.Count - 1;
                }
            }

            var command = await dialog.ShowAsync();

            if (command == null && cancelCommand != null)
            {        

                cancelCommand.Invoked(cancelCommand);
            }

            if (command == yesCommand)
            {
                // ajout de l'enfant selectionné
                var id = ListViewUsers.SelectedIndex;
                User Userselected = Users[0];
                AddDBChildrien(Userselected, App.globaluser);
            }            
            
        }



        private async void AddDBChildrien(User usertoADD,User UserParent)
        {

            //Create an HTTP client object
            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();

            //Add a user-agent header to the GET request. 
            var headers = httpClient.DefaultRequestHeaders;

            //The safe way to add a header value is to use the TryParseAdd method and verify the return value is true,
            //especially if the header value is coming from user input.
            string header = "ie";
            if (!headers.UserAgent.TryParseAdd(header))
            {
                throw new Exception("Invalid header value: " + header);
            }

            header = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)";
            if (!headers.UserAgent.TryParseAdd(header))
            {
                throw new Exception("Invalid header value: " + header);
            }

            Uri requestUri = new Uri("http://businesswallet.fr/Bmb/Add_Children.php?idenfant=" + usertoADD.id_user + "&idparent=" + UserParent.id_user);


            //Send the GET request asynchronously and retrieve the response as a string.
            Windows.Web.Http.HttpResponseMessage httpResponse = new Windows.Web.Http.HttpResponseMessage();
            string httpResponseBody = "";

            try
            {
                //Send the GET request
                httpResponse = await httpClient.GetAsync(requestUri);

                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();

                //user = JsonConvert.DeserializeObject<User>(httpResponseBody);
            }
            catch (Exception ex)
            {
                httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;

            }

        }


    }
}
