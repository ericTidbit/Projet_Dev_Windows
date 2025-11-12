using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Protection.PlayReady;
using Microsoft.UI.Xaml;

namespace EEEEReader.Models
{
    public class Appli
    {
        public List<Utilisateur> Utilisateurs { get; set; } = new List<Utilisateur>();
        public Utilisateur? CurrentUser { get; set; }
        public Livre? CurrentLivre { get; set; }
        public ElementTheme CurrentTheme { get; set; } = ElementTheme.Light; // Default a light
        public bool IsGridLayout { get; set; } = true;

        public Appli()
        {
            //create test user
            var utilisateur = new Utilisateur("e", "e");
            Utilisateurs.Add(utilisateur);
        }

        public void AddClient(string nom, string pwd)
        {
            var user = new Utilisateur(nom, pwd);
            Utilisateurs.Add(user);
        }


        public bool CheckLoginClient(string nom, string pwd)
        {
            foreach (var user in Utilisateurs)
            {
                if (user.Nom == nom && user.VerifierPassword(pwd))
                {
                    CurrentUser = user;
                    return true;
                }
            }
            return false;
        }
    }
}

