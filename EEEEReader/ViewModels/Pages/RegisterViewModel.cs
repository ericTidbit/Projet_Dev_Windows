using EEEEReader.Data;
using EEEEReader.Models;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEEEReader.ViewModels.Pages
{

    public class RegisterViewModel : BaseViewModel
    {
        private IDataProvider _utilisateurDataProvider;
        private ObservableCollection<UtilisateursViewModel> _utilisateurs;


        public RegisterViewModel(IDataProvider utilisateurDataProvider)
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
        public void ChargerUtilisateurs()
        {
            _utilisateurs.Clear();

            List<Utilisateur> utilisateursData = _utilisateurDataProvider.GetUtilisateursData();

            foreach (var utilisateur in utilisateursData)
            {
                _utilisateurs.Add(new UtilisateursViewModel(utilisateur));
            }
        }
        public bool RegardeMDP(string username, string password, string confirm)
        {
            if (username != "" && password != "")
            {
                if (password == confirm)
                {
                    // faire la meme chose que pour la mais avec la création 

                    Utilisateur Utilisateur = new Utilisateur(username, password);
                    _utilisateurDataProvider.AjouterUtilisateur(Utilisateur);
                    
                    return true;
                }
            }
            return false;
        }
    }
}
