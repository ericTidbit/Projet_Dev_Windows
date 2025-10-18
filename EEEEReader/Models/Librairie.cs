using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEEEReader.Models
{
    public class Librairie
    {
        public List<Livre> Livres { get; set; }

        public Librairie()
        {
            Livres = new List<Livre>();
        
        }
        public void AjouterLivre(string content, string titre, string auteur, string date, string isbn, string langue = "", string resume = "", byte[] cover = null)
        {
            var livre = new Livre(content, titre, auteur, date, isbn, langue, resume, cover);
            Livres.Add(livre);
        }
    }
}