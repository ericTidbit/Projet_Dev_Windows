using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEEEReader.Models
{
    public class Librairie
    {
        public ObservableCollection<Livre> Livres { get; } = new();

        public Librairie()
        {
            Livres = new ObservableCollection<Livre>();
        
        }
        public void AjouterLivre(string content, string titre, string auteur, string date, string isbn, string langue = "", string resume = "", byte[] cover = null)
        {
            var livre = new Livre(content, titre, auteur, date, isbn, langue, resume, cover);
            Livres.Add(livre);
        }
    }
}