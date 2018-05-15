using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour en savoir plus sur le modèle d'élément Contrôle utilisateur, consultez la page https://go.microsoft.com/fwlink/?LinkId=234236

namespace BringMeBack.UserControls
{
    public sealed partial class MenuUserControl : UserControl
    {
        

        private String myVar;

        public String CurrentPage
        {
            get { return myVar; }
            set {
                myVar = value;
                switch (value)
                {
                    case "Call":
                        this.rect.Fill = new SolidColorBrush(Color.FromArgb(180, 255, 0,0));
                        break;
                    case "SOS":
                        this.rect.Fill = new SolidColorBrush(Color.FromArgb(255, 0, 166, 255));
                        break;
                    case "Ethylotest":
                        this.rect.Fill = new SolidColorBrush(Color.FromArgb(180, 255, 0, 0));
                        break;
                    default:
                        break;
                }
            }
        }

        public MenuUserControl()
        {
            this.InitializeComponent();
            //this.rect.Fill = new SolidColorBrush(Color.FromArgb((byte)230, (byte)47, (byte)47,3));
        }
        


        private void Retour(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(MainPage));
        }
    }
}
