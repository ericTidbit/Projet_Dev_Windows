using EEEEReader.Data;
using EEEEReader.ViewModels;
using EEEEReader.ViewModels.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace EEEEReader.Views
{
    public sealed partial class LoginPage : Page
    {
        public LoginUtilisateursViewModel ViewModel;

        public LoginPage()
        {
            this.InitializeComponent();
            var dbContext = new EEEEReaderDbContext();
            var dataProvider = new DataProvider(dbContext);
            ViewModel = new LoginUtilisateursViewModel(dataProvider);
            this.DataContext = ViewModel;
        }

        private async void OnLoginClick(object sender, RoutedEventArgs e)
        {
            string username = UserNameBox.Text;
            string password = PasswordBox.Password;

            (bool, UtilisateursViewModel? user) LoginArgument = ViewModel.CheckLoginUtilisateur(username, password);
            bool succes = LoginArgument.Item1;
            UtilisateursViewModel? CurrentUser = LoginArgument.Item2;
            if (LoginArgument.Item1)
            {
                // tuple changer juste essayer pour que ca fonctionne :)
                (bool success, UtilisateursViewModel? user) test = ViewModel.CheckLoginUtilisateur(username, password);
                this.Frame?.Navigate(typeof(Home), CurrentUser);
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
        /*
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
        */


        private void OnRegisterClick(object sender, RoutedEventArgs e)
        {
            this.Frame?.Navigate(typeof(RegisterPage));
        }
    }
}