using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using BringMeBack.Class;
using Newtonsoft.Json;

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

        public async void GoToSeeMyChildrien()// recuperation du dernier emplacement de son enfant
        {

            // recuperation de la derniere position de l'enfant 
            //Create an HTTP client object
            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();
            Uri requestUri = new Uri("http://businesswallet.fr/Bmb/Get_LastPosition_Children.php?id_user=" + App.principalChildren.id_user);
            //Send the GET request asynchronously and retrieve the response as a string.
            Windows.Web.Http.HttpResponseMessage httpResponse = new Windows.Web.Http.HttpResponseMessage();
            string httpResponseBody = "";
            String latitude = "";
            String longitude = "";
        
            try
            {
                //Send the GET request
                httpResponse = await httpClient.GetAsync(requestUri);

                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
                List<Historical> historicals = JsonConvert.DeserializeObject<List<Historical>>(httpResponseBody);
                foreach (Historical his in historicals)
                {
                    latitude = his.latitude_historical;
                    longitude = his.longitude_historical;

                }
                

                

            }
            catch (Exception ex)
            {
                httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;

            }

            // creation de la vue bingmap avec ses coordonnées
            var uriNewYork = new Uri(@"bingmaps:?cp="+latitude+"~"+longitude+"&ss=1");

            // Launch the Windows Maps app
            var launcherOptions = new Windows.System.LauncherOptions();
            launcherOptions.TargetApplicationPackageFamilyName = "Microsoft.WindowsMaps_8wekyb3d8bbwe";
            var success = await Windows.System.Launcher.LaunchUriAsync(uriNewYork, launcherOptions);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GoToSeeMyChildrien();
        }
    }
}
