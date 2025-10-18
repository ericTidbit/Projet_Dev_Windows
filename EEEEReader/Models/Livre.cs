using EEEEReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VersOne.Epub.Options;

namespace EEEEReader.Models
{
    public class Livre
    {
        public string Content { get; set; }
        public string Titre { get; set; }
        public string Auteur { get; set; }
        public string Date { get; set; }
        public string ISBN { get; set; }
        public string Langue { get; set; }
        public string Resume{ get; set; }
        public byte[] cover { get; set; }


        public Livre(string content, string Titre, string Auteur, string Date, string ISBN, string Langue, string Resume,byte[] cover)
        {
            this.Content = content;
            this.Titre = Titre;
            this.Auteur = Auteur;
            this.Date = Date;
            this.ISBN = ISBN;
            this.Langue = Langue;
            this.Resume = Resume;
            this.cover = cover;
        }

        public Livre()
        {
        }
    }
}
