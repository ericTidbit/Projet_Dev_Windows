using Microsoft.ML.OnnxRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EEEEReader.Models;

namespace EEEEReader.Models
{
    public abstract class Utilisateur
    {
        public string nom {  get; set; }

        public string pwd { get; set; }
        public Librairie? Libraire { get; set; }

        public DateTime? Date { get; set; }

        public bool isAdmin { get; set; }

        public Utilisateur(string nom, string pwd)
        {
            this.nom = nom;
            this.pwd = pwd;
            Libraire = null;
            Date = DateTime.Now;
            isAdmin = false;

        }
        public UAjouterLivre(string text)

        {
            Livre livre = new Livre();
            Libraire.AjouterLivre(livre);
        }
    }
}
