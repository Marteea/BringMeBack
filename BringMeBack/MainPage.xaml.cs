using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.System;
using Windows.Devices.Geolocation;
using BringMeBack.Library;
using Windows.Media.SpeechSynthesis;
using System.Threading.Tasks;
using Windows.Media.SpeechRecognition;
using Windows.UI.Core;
using System.Collections.Generic;
// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BringMeBack
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page{

        private GeolocalisationLibrary geolocalisationLibrary = new GeolocalisationLibrary();
        internal GeolocalisationLibrary GeolocalisationLibrary { get => geolocalisationLibrary; set => geolocalisationLibrary = value; }
        SpeechSynthesizer speechsynthesizer;
        SpeechRecognizer recognizer;
        CoreDispatcher dispatcher;

        public  MainPage()
        {
            this.InitializeComponent();
            speechsynthesizer = new SpeechSynthesizer();
            

            



        }

        

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (App.globaluser == null)
            {
                App.globaluser = (Class.User)e.Parameter;
            }
                        
        }

        private async void BMB_Click(object sender, RoutedEventArgs e)
        {

            Geopoint geopoint = await geolocalisationLibrary.Located();
            Geopoint point = geopoint;

            Uri uri = new Uri("ms-call:+330678594398");
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

            Uri requestUri = new Uri("http://businesswallet.fr/Bmb/User_Historical.php?id_user=" + App.globaluser.id_user + "&name_historical=Appel chauffeur&text_historical=Votre Enfant " + App.globaluser.name_user + " - " + App.globaluser.firstname_user +" a appelé un chauffeur le " + DateTime.Today + " a l'heure de " + DateTime.Now + "&lattitude_historical=" + point.Position.Latitude + "&longitude_historical=" + point.Position.Longitude);

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


            //Demande du nom avant d'aller sur la page call
            //SpeechSynthesisStream stream = await speechsynthesizer.SynthesizeTextToStreamAsync("Appel du chauffeur le plus proche : veuillez confirmer votre prénom ");
            //media.SetSource(stream, stream.ContentType);
            //media.Play();


            //test
            SpeechRecognizer speechRecognizer = new SpeechRecognizer();
            speechRecognizer.Constraints.Add(
                new SpeechRecognitionListConstraint(new List<String>() { "Start Listening" }));

            // Compile the new constraints
            SpeechRecognitionCompilationResult compilationResult =
                                                await speechRecognizer.CompileConstraintsAsync();

            // Subscribe to event when command is recognized
            speechRecognizer.ContinuousRecognitionSession.ResultGenerated +=
                ContinuousRecognitionSession_ResultGenerated;

            // Start recognizing
            await speechRecognizer.ContinuousRecognitionSession.StartAsync();



            //await Task.Delay(TimeSpan.FromSeconds(10));
            //Frame rootFrame = Window.Current.Content as Frame;
            //rootFrame.Navigate(typeof(BMB.Call),e);
        }

        private async void SOS_Click(object sender, RoutedEventArgs e)
        {
            Geopoint geopoint = await geolocalisationLibrary.Located();
            Geopoint point = geopoint;


            Uri uri = new Uri("ms-call:+112");
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

            Uri requestUri = new Uri("http://businesswallet.fr/Bmb/User_Historical.php?id_user=" + App.globaluser.id_user + "&name_historical=Appel Urgence&text_historical=Votre Enfant " + App.globaluser.name_user + " - " + App.globaluser.firstname_user + " a appelé les secours le " + DateTime.Today + " a l'heure de " + DateTime.Now + "&lattitude_historical=" + point.Position.Latitude + "&longitude_historical="+point.Position.Longitude);

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
            rootFrame.Navigate(typeof(BMB.SOS));

           
        }

        private  void Ethy_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(BMB.Ethylotest));
        }


        private void ContinuousRecognitionSession_ResultGenerated(
                        SpeechContinuousRecognitionSession sender,
                        SpeechContinuousRecognitionResultGeneratedEventArgs args)
        {
            if (args.Result.Text == "Start Listening")
            {
                // if you need to do something on the UI thread, 
                // make sure to use a dispatcher
                
            }
        }







    }

}
    
