using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEEEReader.Models
{
    public class Administrateur : Utilisateur
    {
        public Administrateur(string nom, string pwd) : base(nom, pwd)
        {
            isAdmin = true;
        }
    }
}
