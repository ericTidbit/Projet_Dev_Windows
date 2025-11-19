using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EEEEReader.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace EEEEReader.Models
{
    public class Appli
    {
        public List<Utilisateur> Utilisateurs { get; set; } = new List<Utilisateur>();
        public Utilisateur? CurrentUser { get; set; }
        public Livre? CurrentLivre { get; set; }
        public bool IsDarkMode { get; set; } = false; 
        public bool IsGridLayout { get; set; } = true;

        public Appli()
        {

        }
    }
}

