using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace EEEEReader.Views
{
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private async void OnLoginClick(object sender, RoutedEventArgs e)
        {
            string username = UserNameBox.Text;
            string password = PasswordBox.Password;


            if (CheckLoginUtilisateur(username, password))
            {
                this.Frame?.Navigate(typeof(Home));
                
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
                    Content = "Nom d'utilisateur ou mot de passe invalide.",
                    CloseButtonText = "OK",
                    XamlRoot = this.Content.XamlRoot
                };
                await dialog.ShowAsync();
            }
        }
        // mettre pour que ca respecte le viewmodel Changer
        public bool CheckLoginUtilisateur(string nom, string pwd)
        {
            foreach (var user in App.AppReader.Utilisateurs)
            {
                if (user.Nom == nom && user.VerifierPassword(pwd))
                {
                    App.AppReader.CurrentUser = user;
                    return true;
                }
            }
            return false;
        }


        private void OnRegisterClick(object sender, RoutedEventArgs e)
        {
            this.Frame?.Navigate(typeof(RegisterPage));
        }
    }
}