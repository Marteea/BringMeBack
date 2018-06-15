
using BringMeBack;
using BringMeBack.Class;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BMB
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class Connexion : Page
    {

        
        private string email;
        private string password;
        private User user = null;

        public Connexion()
        {
            this.InitializeComponent(); 
            
        }     


        private void Button_Switch(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(BMB.Inscription));
        }

        private async void Button_Connecter(object sender, RoutedEventArgs e)
        {
            if(Email.Text!="" && Password.Password != "")
            {
                // recuperation des valeurs email et mdp 

                email = Email.Text;
                password = Password.Password;

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

                Uri requestUri = new Uri("http://businesswallet.fr/Bmb/User_Connexion.php?email=" + email + "&password=" + password);

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
                    App.globaluser = user;

                    if (user.isparent == 0)// si un enfant se connecte
                    {
                        Frame rootFrame = Window.Current.Content as Frame;
                        rootFrame.Navigate(typeof(MainPage), user);
                    }
                    else// si un parent se connecte
                    {
                        Frame rootFrame = Window.Current.Content as Frame;
                        rootFrame.Navigate(typeof(MainParent), user);
                    }




                }
                else
                {
                    libelleErreur.Text = "Veuillez saisir le bon email et mot de passe : ce couple d'identifiant est inconnu";
                }

            }
            else
            {
                libelleErreur.Text = "Veuillez remplir les deux champs afin de vous connecter";
            }
            
            
        }

        
    }
}
