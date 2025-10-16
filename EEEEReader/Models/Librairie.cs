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
        public void AjouterLivre(Livre livre)
        {
            Livres.Add(livre);
        }
    }
}