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
        public List<Client> Clients { get; set; } = new List<Client>();
        public List<Administrateur> Admins { get; set; } = new List<Administrateur>();
        public Utilisateur? CurrentUser { get; set; }
        public Livre? CurrentLivre { get; set; }
        public ElementTheme CurrentTheme { get; set; } = ElementTheme.Light; // Default a light
        public bool IsGridLayout { get; set; } = true;

        public Appli()
        {
            //create test user
            var admin = new Administrateur("e", "e");
            Admins.Add(admin);
        }

        public void AddClient(string nom, string pwd)
        {
            var client = new Client(nom, pwd);
            Clients.Add(client);
        }

        public void AddAdmin(string nom, string pwd)
        {
            var admin = new Administrateur(nom, pwd);
            Admins.Add(admin);
        }

        public bool CheckLoginClient(string nom, string pwd)
        {
            foreach (var client in Clients)
            {
                if (client.Nom == nom && client.VerifierPassword(pwd))
                {
                    CurrentUser = client;
                    return true;
                }
            }
            return false;
        }
        public bool CheckLoginAdmin(string nom, string pwd)
        {
            foreach (var admin in Admins)
            {
                if (admin.Nom == nom && admin.VerifierPassword(pwd))
                {
                    CurrentUser = admin;
                    return true;
                }
            }
            return false;
        }
    }
}

