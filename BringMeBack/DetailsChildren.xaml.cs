using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;
using Windows.System.UserProfile;
using BringMeBack.Class;
using BringMeBack.UserControls;
using Newtonsoft.Json;
using Windows.Devices.Geolocation;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BringMeBack
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class DetailsChildren : Page
    {
        private List<Historical> listOfHistorical= new List<Historical>();

        

        public DetailsChildren()
        {
            this.InitializeComponent();

            Get_historical(App.principalChildren.id_user);
            NameChilren.Text = App.principalChildren.name_user;

        }

        public async void Get_historical(int id_user)
        {           

            //Create an HTTP client object
            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();
            Uri requestUri = new Uri("http://businesswallet.fr/Bmb/Get_Historical.php?id_user=" + id_user);
            //Send the GET request asynchronously and retrieve the response as a string.
            Windows.Web.Http.HttpResponseMessage httpResponse = new Windows.Web.Http.HttpResponseMessage();
            string httpResponseBody = "";

            try
            {
                //Send the GET request
                httpResponse = await httpClient.GetAsync(requestUri);

                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
                List<Historical> historicals = JsonConvert.DeserializeObject<List<Historical>>(httpResponseBody);
                foreach (  Historical h in  historicals) {
                    listOfHistorical.Add(h);
                }
                
                ListViewHistorical.ItemsSource = listOfHistorical;

            }
            catch (Exception ex)
            {
                httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;

            }

        }

        private void BackMainParent(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;            
            rootFrame.Navigate(typeof(MainParent));
            
        }

       




    }
}
