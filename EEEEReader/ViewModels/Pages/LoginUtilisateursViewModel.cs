using EEEEReader.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Protection.PlayReady;
using Windows.System;

namespace EEEEReader.ViewModels.Pages
{
    public class LoginUtilisateursViewModel : BaseViewModel
    {
        private ObservableCollection<UtilisateursViewModel> _utilisateurs;
        
        private UtilisateursViewModel? _currentUser;

        private IDataProvider _utilisateurDataProvider;

        public LoginUtilisateursViewModel(IDataProvider utilisateurDataProvider)
        {
            _utilisateurDataProvider = utilisateurDataProvider
                                       ?? throw new ArgumentNullException(nameof(utilisateurDataProvider));

            _utilisateurs = new ObservableCollection<UtilisateursViewModel>();
            ChargerUtilisateurs();
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

        public UtilisateursViewModel? CurrentUser
        {
            get => _currentUser;
            set
            {
                if (_currentUser != value)
                {
                    _currentUser = value;
                    RaisePropertyChanged();
                }
            }
        }

        public (bool, UtilisateursViewModel?) CheckLoginUtilisateur(string nom, string pwd)
        {
            foreach (var user in Utilisateurs)
            {
                if (user.Nom == nom && user.VerifierPassword(pwd))
                {
                    // ici mettre la donné pour que  
                    _currentUser = user;
                    return (true,user);
                }
            }
            return (false, null);
        }

        public void AddClient(string nom, string pwd)
        {
            // ajouter dans 
            string hashedPassword = HashPassword(pwd);
            var utilisateur = new Utilisateur(nom, hashedPassword);

            _utilisateurDataProvider.AjouterUtilisateur(utilisateur);

            var utilisateurViewModel = new UtilisateursViewModel(utilisateur);
            Utilisateurs.Add(utilisateurViewModel);

        }

        private static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
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
    }
}
