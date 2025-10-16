using System;

namespace EEEEReader.Models
{
    public abstract class Utilisateur
    {
        public string Nom { get; set; }
        public string Pwd { get; set; }
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

        public void AjouterLivre(Livre livre)
        {
            if (livre == null) throw new ArgumentNullException(nameof(livre));
            Librairie.AjouterLivre(livre);
        }

        public void AjouterLivre(string content, string titre, string auteur, string date, string isbn, string langue = "", string resume = "")
        {
            var livre = new Livre(content, titre, auteur, date, isbn, langue, resume);
            Librairie.AjouterLivre(livre);
        }
    }
}
