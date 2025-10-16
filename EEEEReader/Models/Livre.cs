using EEEEReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        public Livre(string content)
        {
            Content = content;
        }
    }
}
