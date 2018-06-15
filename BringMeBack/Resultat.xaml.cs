
using BringMeBack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BMB
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class Resultat : Page
    {

        private double resultat;
        private Uri urlImage;

        public Resultat()
        {
            this.InitializeComponent();
            resultat = AleaResult();

            taux.Text=resultat.ToString()+" G ";
            
            if (resultat > 0.5){
                urlImage = new Uri("ms-appx:///Assets/redcross.png", UriKind.Absolute);
                LibelleResultat.Text = "Votre Taux est trop élevé veuillez contacter un chauffeur";
            }
            else
            {
                urlImage = new Uri("ms-appx:///Assets/ok.png", UriKind.Absolute);
                LibelleResultat.Text = "Votre Taux est correcte vous pouvez rentrer en sécurité";
            }
            BitmapImage sourceImage = new BitmapImage(urlImage);
            Logo.Source = sourceImage;
        }

        private double  AleaResult()
        {
            Random rnd = new Random();

            return Math.Round(GetRandomNumber(0, 1),2);

        }

        public double GetRandomNumber(double minimum, double maximum)
        {
            Random random = new Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }

        private async void Button_RamenezMoi(object sender, RoutedEventArgs e)
        {


            Uri uri = new Uri("ms-call:+330987654356");
            await Launcher.LaunchUriAsync(uri);


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

            Uri requestUri = new Uri("http://businesswallet.fr/Bmb/User_Historical.php?id_user=" + App.globaluser.id_user + "&name_historical=Appel chauffeur&text_historical=Votre Enfant " + App.globaluser.name_user + " - " + App.globaluser.firstname_user + " a appelé un chauffeur le " + DateTime.Today + " a l'heure de " + DateTime.Now);

            //Send the GET request asynchronously and retrieve the response as a string.
            Windows.Web.Http.HttpResponseMessage httpResponse = new Windows.Web.Http.HttpResponseMessage();
            string httpResponseBody = "";
            try
            {
                //Send the GET request
                httpResponse = await httpClient.GetAsync(requestUri);
                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;

            }

            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(BMB.Call), e);
        }

        private void MenuUc_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
