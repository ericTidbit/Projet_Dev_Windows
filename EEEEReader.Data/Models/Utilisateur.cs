using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using EEEEReader.Data.Models;

namespace EEEEReader.Models
{
    public class Utilisateur
    {
        public int Id { get; set; }
        public string Nom { get; set; }

        private string _pwd;

        public List<Livre> LivresRecent { get; set; } = new List<Livre>();
        public string Pwd {  get; set; }
        public Librairie Librairie { get; set; }
        public DateTime? Date { get; set; }
        public bool IsAdmin { get; set; }

        public Utilisateur(string nom, string pwd)
        {
            Nom = nom ?? throw new ArgumentNullException(nameof(nom));
            Pwd = pwd ?? throw new ArgumentNullException(nameof(pwd));
            Librairie = new Librairie();
            Date = DateTime.Now;
            IsAdmin = false;
        }

        // TODO mettre ça ailleur
        public void AjouterLivreRecent(Livre livre)
        {
            // regarder la logique pour etre certain que tous fonctionne 
            if (!LivresRecent.Contains(livre))
            {
                if (LivresRecent.Count > 5)
                {
                    LivresRecent.RemoveAt(0);
                    LivresRecent.Add(livre);
                }
                else
                {
                    //change la prioriter du livre le remettre comme 5eme
                    LivresRecent.Add(livre);
                }
            }
            


        }
    }
}
