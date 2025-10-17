using EEEEReader.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace EEEEReader.Views
{
    public sealed partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            this.InitializeComponent();
        }

        private async void OnRegisterClick(object sender, RoutedEventArgs e)
        {
            string username = UserNameBox.Text;
            string password = PasswordBox.Password;

            if (username != "" && password != "")
            {
                Client NewClient = new Client(username, password);
                App.AppReader.AddClient(NewClient.Nom, NewClient.Pwd);

                //retourne au login
                this.Frame?.Navigate(typeof(LoginPage));
            }
            else
            {
                // clear
                UserNameBox.Text = "";
                PasswordBox.Password = "";

                // message d'erreur
                ContentDialog dialog = new ContentDialog()
                {
                    Title = "Erreur",
                    Content = "Les informations ne peuves pas être vides.",
                    CloseButtonText = "OK",
                    XamlRoot = this.Content.XamlRoot
                };
                await dialog.ShowAsync();
            }
        }
    }
}