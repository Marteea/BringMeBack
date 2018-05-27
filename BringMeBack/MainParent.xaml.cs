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
using Windows.System;
using BringMeBack.Class;
using BringMeBack.UserControls;
using Newtonsoft.Json;
using Windows.UI;
using Xamarin.Forms.Platform.UWP;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BringMeBack
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainParent : Page
    {
        private int id_parent;
        private Class.User user = null;


        public MainParent()
        {
            this.InitializeComponent();
            GetChildren();

        }

        public async void GetChildren()
        {



            id_parent = App.globaluser.id_user;

            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();

            var headers = httpClient.DefaultRequestHeaders;

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

            Uri requestUri = new Uri("http://businesswallet.fr/Bmb/Get_Children.php?id_parent=" + id_parent);

            //Send the GET request asynchronously and retrieve the response as a string.
            Windows.Web.Http.HttpResponseMessage httpResponse = new Windows.Web.Http.HttpResponseMessage();
            string httpResponseBody = "";

            try
            {
                //Send the GET request
                httpResponse = await httpClient.GetAsync(requestUri);

                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
                user = JsonConvert.DeserializeObject<Class.User>(httpResponseBody);
                
                
            }
            catch (Exception ex)
            {
                httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;

            }

            if (user == null)
            {
                ChildName.Text = "Aucun enfant associé a votre compte";
                ChildName.FontSize = 14;                
                ChildrenPicture.Visibility = Visibility.Collapsed;
            }
            else
            {
                ChildName.Text = user.firstname_user;
                App.principalChildren = user;
            }
            

        }

        private void BMB_DetailsChildren(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(DetailsChildren));
        }












    }
}
