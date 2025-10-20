using Microsoft.UI.Xaml.Documents;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VersOne.Epub;
using Windows.ApplicationModel.Store.Preview.InstallControl;

namespace EEEEReader.Models
{
    public class Librairie
    {
        public ObservableCollection<Livre> Livres { get; } = new();

        public Librairie()
        {
            Livres = new ObservableCollection<Livre>();
        
        }
        public void AjouterLivre(EpubContent content, string titre, string auteur, string date, string isbn, string langue = "", string resume = "", byte[] cover = null)
        {
            Livre livre = new Livre(content, titre, auteur, date, isbn, langue, resume, cover);
            // temporaire en attendant l'intégration sql
            livre.Id = Livres.IndexOf(livre);
            // --
            Livres.Add(livre);
        }
    }
}