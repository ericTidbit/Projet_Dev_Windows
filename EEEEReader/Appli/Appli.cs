using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Protection.PlayReady;
using Microsoft.UI.Xaml;
using EEEEReader.Data.Models;
using EEEEReader.ViewModels;
using EEEEReader.ViewModels.Pages;
using System.ComponentModel.DataAnnotations.Schema;

namespace EEEEReader.Models
{
    public class Appli
    {
        public LoginUtilisateursViewModel LoginViewModel { get; set; }
        public Utilisateur? CurrentUser { get; set; }
        public Livre? CurrentLivre { get; set; }
        public bool IsDarkMode { get; set; } = false; // par défaut pas dark mode
        public bool IsGridLayout { get; set; } = true;
        //public LivreViewModel? CurrentLivreViewModel { get; set; }

        public Appli()
        {
            LoginViewModel = new LoginUtilisateursViewModel();
            
            // Create test user
            LoginViewModel.AddClient("e", "e");
        }

        public void AddClient(string nom, string pwd)
        {
            LoginViewModel.AddClient(nom, pwd);
        }

        public bool CheckLoginUtilisateur(string nom, string pwd)
        {
            bool result = LoginViewModel.CheckLoginUtilisateur(nom, pwd);
            if (result)
            {
                CurrentUser = LoginViewModel.CurrentUser?._user;
            }
            return result;
        }
    }
}

