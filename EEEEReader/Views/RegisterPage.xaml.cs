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
            string confirm = PasswordConfirmationBox.Password;

            if (username != "" && password != "")
            {
                if (password == confirm)
                {
                    App.AppReader.AddClient(username, password);

                    //retourne au login
                    this.Frame?.Navigate(typeof(LoginPage));
                }
                else
                {
                    //clear passwords
                    PasswordBox.Password = "";
                    PasswordConfirmationBox.Password = "";

                    // message d'erreur
                    ContentDialog dialog = new ContentDialog()
                    {
                        Title = "Erreur",
                        Content = "Les mots de passes ne sont pas identiques.",
                        CloseButtonText = "OK",
                        XamlRoot = this.Content.XamlRoot
                    };
                    await dialog.ShowAsync();
                }
            }
            else
            {
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