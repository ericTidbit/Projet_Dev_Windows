using EEEEReader.Data;
using EEEEReader.Data.Models;
using EEEEReader.Models;
using EEEEReader.ViewModels.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EEEEReader.Views
{
    public sealed partial class RegisterPage : Page
    {
        public RegisterViewModel ViewModel;
        public RegisterPage()
        {
            this.InitializeComponent();
            EEEEReaderDbContext dbContext = new EEEEReaderDbContext();
            DataProvider dataProvider = new DataProvider(dbContext);
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

                // Récupérer les messages d'erreur du ViewModel
                string errorMessage = GetValidationErrors();

                // message d'erreur
                ContentDialog dialog = new ContentDialog()
                {
                    Title = "Erreur d'inscription",
                    Content = errorMessage,
                    CloseButtonText = "OK",
                    XamlRoot = this.Content.XamlRoot
                };
                await dialog.ShowAsync();
            }
        }

        /// <summary>
        /// Récupère tous les messages d'erreur de validation du ViewModel
        /// </summary>
        private string GetValidationErrors()
        {
            List<string> errors = new List<string>();

            // Récupérer les erreurs pour chaque propriété
            IEnumerable<ValidationResult> usernameErrors = ViewModel.GetErrors(nameof(ViewModel.Username))
                .Cast<ValidationResult>();
            IEnumerable<ValidationResult> passwordErrors = ViewModel.GetErrors(nameof(ViewModel.Password))
                .Cast<ValidationResult>();
            IEnumerable<ValidationResult> confirmPasswordErrors = ViewModel.GetErrors(nameof(ViewModel.ConfirmPassword))
                .Cast<ValidationResult>();

            // Ajouter les messages d'erreur
            foreach (ValidationResult error in usernameErrors)
                errors.Add(error.ErrorMessage);
            foreach (ValidationResult error in passwordErrors)
                errors.Add(error.ErrorMessage);
            foreach (ValidationResult error in confirmPasswordErrors)
                errors.Add(error.ErrorMessage);

            // Retourner un message par défaut si aucune erreur spécifique n'est trouvée
            return errors.Any() 
                ? string.Join("\n", errors) 
                : "Veuillez vérifier les informations saisies.";
        }

        private void AnnulerClick(object sender, RoutedEventArgs e)
        {
            this.Frame?.Navigate(typeof(LoginPage));
        }
    }
}