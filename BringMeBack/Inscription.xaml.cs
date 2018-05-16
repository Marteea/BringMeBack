﻿
using BringMeBack;
using BringMeBack.Class;
using Newtonsoft.Json;
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
using Windows.Web.Http;
using System.Diagnostics;


// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BMB
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class Inscription : Page
    {

        private string Email;        
        private string Password;
        private string Firstname;
#pragma warning disable CS0108 // 'Inscription.Name' masque le membre hérité 'FrameworkElement.Name'. Utilisez le mot clé new si le masquage est intentionnel.
        private string Name;
#pragma warning restore CS0108 // 'Inscription.Name' masque le membre hérité 'FrameworkElement.Name'. Utilisez le mot clé new si le masquage est intentionnel.
        private string Birthdate;
        public User user;

        public Inscription()
        {
            this.InitializeComponent();


        }

        private void Button_Click(object sender, RoutedEventArgs e)       {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(BMB.Connexion));
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {

            // recuperation des valeurs email et mdp 
            Name = name.Text;
            Firstname = firstname.Text;
            Email = email.Text;
            Password = password.Text;
            Birthdate = birthdate.Text;



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

            Uri requestUri = new Uri("http://businesswallet.fr/Bmb/User_Inscription.php?name=" + Name + "&firstname=" + Firstname+"&birthdate="+Birthdate+"&email="+Email+"&password="+Password);

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

                Frame rootFrame = Window.Current.Content as Frame;
                rootFrame.Navigate(typeof(MainPage));

            }
            
        }
    }
}