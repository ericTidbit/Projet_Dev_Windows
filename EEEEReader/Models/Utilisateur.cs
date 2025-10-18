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

        

        
    }
}
