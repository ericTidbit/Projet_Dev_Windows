using Demo.ViewModels;
using EEEEReader.Data;
using EEEEReader.Models;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEEEReader.ViewModels.Pages
{

    public class RegisterViewModel : ValidableViewModel
    {
        private IDataProviderUtilisateur _utilisateurDataProvider;
        private ObservableCollection<UtilisateursViewModel> _utilisateurs;

        // Backing fields pour les propriétés validées
        private string _username = "";
        private string _password = "";
        private string _confirmPassword = "";

        public RegisterViewModel(IDataProviderUtilisateur utilisateurDataProvider)
        {
            _utilisateurDataProvider = utilisateurDataProvider
                                       ?? throw new ArgumentNullException(nameof(utilisateurDataProvider));

            _utilisateurs = new ObservableCollection<UtilisateursViewModel>();
            ChargerUtilisateurs();

            // Initialiser les collections d'erreurs pour le binding (comme dans l'exemple)
            _errors.Add(nameof(Username), new ObservableCollection<ValidationResult>());
            _errors.Add(nameof(Password), new ObservableCollection<ValidationResult>());
            _errors.Add(nameof(ConfirmPassword), new ObservableCollection<ValidationResult>());
        }

        public ObservableCollection<UtilisateursViewModel> Utilisateurs
        {
            get => _utilisateurs;
            set
            {
                if (_utilisateurs != value)
                {
                    _utilisateurs = value;
                    RaisePropertyChanged();
                }
            }
        }

        public void ChargerUtilisateurs()
        {
            _utilisateurs.Clear();

            List<Utilisateur> utilisateursData = _utilisateurDataProvider.GetUtilisateursData();

            foreach (var utilisateur in utilisateursData)
            {
                _utilisateurs.Add(new UtilisateursViewModel(utilisateur));
            }
        }

        // Propriétés avec annotations de validation (pattern identique à MainAjouterModifierGateauViewModel)
        [Required(ErrorMessage = "Le nom d'utilisateur est requis.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Le nom d'utilisateur doit contenir entre 3 et 100 caractères")]
        public string Username
        {
            get => _username;
            set
            {
                if (_username != value && Validate(value, nameof(Username)))
                {
                    _username = value;
                    RaisePropertyChanged();
                }
            }
        }

        [Required(ErrorMessage = "Le mot de passe est requis.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Le mot de passe doit contenir au moins 6 caractères")]
        public string Password
        {
            get => _password;
            set
            {
                if (_password != value && Validate(value, nameof(Password)))
                {
                    _password = value;
                    RaisePropertyChanged();
                }
            }
        }

        [Required(ErrorMessage = "La confirmation du mot de passe est requise.")]
        [Compare(nameof(Password), ErrorMessage = "Les mots de passe ne correspondent pas.")]
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                if (_confirmPassword != value && Validate(value, nameof(ConfirmPassword)))
                {
                    _confirmPassword = value;
                    RaisePropertyChanged();
                }
            }
        }

        // Méthode existante — validation appliquée sans changer la signature
        public bool RegardeMDP(string username, string password, string confirm)
        {
            // Valider Username et Password
            bool validUsername = Validate(username, nameof(Username));
            bool validPassword = Validate(password, nameof(Password));
            
            // Valider que les mots de passe correspondent
            bool passwordsMatch = password == confirm;
            
            // Gérer l'erreur de confirmation manuellement
            var confirmErrors = _errors[nameof(ConfirmPassword)];
            confirmErrors.Clear();
            
            if (string.IsNullOrWhiteSpace(confirm))
            {
                confirmErrors.Add(new ValidationResult("La confirmation du mot de passe est requise."));
                OnErrorsChanged(nameof(ConfirmPassword));
            }
            else if (!passwordsMatch)
            {
                confirmErrors.Add(new ValidationResult("Les mots de passe ne correspondent pas."));
                OnErrorsChanged(nameof(ConfirmPassword));
            }
            
            // Vérifier toutes les validations
            if (!validUsername || !validPassword || !passwordsMatch || confirmErrors.Count > 0)
                return false;

            // Mettre à jour les propriétés du ViewModel AVANT de créer l'utilisateur
            // pour éviter les erreurs de validation lors de l'assignation
            _username = username;
            _password = password;
            _confirmPassword = confirm;
            
            // Nettoyer les erreurs pour toutes les propriétés
            _errors[nameof(Username)].Clear();
            _errors[nameof(Password)].Clear();
            _errors[nameof(ConfirmPassword)].Clear();
            OnErrorsChanged(nameof(Username));
            OnErrorsChanged(nameof(Password));
            OnErrorsChanged(nameof(ConfirmPassword));

            // Créer et ajouter l'utilisateur
            Utilisateur utilisateur = new Utilisateur(username, password);
            _utilisateurDataProvider.AjouterUtilisateur(utilisateur);

            return true;
        }
    }
}
