using EEEEReader.Data;
using EEEEReader.Data.Models;
using EEEEReader.Models;
using EEEEReader.ViewModels.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace EEEEReader.Views
{
    public sealed partial class RegisterPage : Page
    {
        public RegisterViewModel ViewModel;
        public RegisterPage()
        {
            this.InitializeComponent();
            var dbContext = new EEEEReaderDbContext();
            var dataProvider = new DataProvider(dbContext);
            ViewModel = new RegisterViewModel(dataProvider);
            this.DataContext = ViewModel;
        }

        // viewModel
        private async void OnRegisterClick(object sender, RoutedEventArgs e)
        {
            string username = UserNameBox.Text;
            string password = PasswordBox.Password;
            string confirm = PasswordConfirmationBox.Password;
            bool correcte = ViewModel.RegardeMDP(username, password, confirm);
            
                if (correcte)
                {
                    
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
         
        
        

        private void AnnulerClick(object sender, RoutedEventArgs e)
        {
            this.Frame?.Navigate(typeof(LoginPage));
        }
    }
}