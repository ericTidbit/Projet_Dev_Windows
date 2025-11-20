using EEEEReader.Data.Models;
using EEEEReader.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEEEReader.Data.data
{
    public class DBUtilisateursDataProvider : IDataProvider
    {

        private readonly EEEEReaderDbContext _context;

        public DBUtilisateursDataProvider()
        {
            _context = new EEEEReaderDbContext();
        }

        public void AjouterUtilisateur(Utilisateur utilisateur)
        {
            _context.Utilisateurs.Add(utilisateur);
            _context.SaveChanges();
        }


        public List<Utilisateur> GetUtilisateursData()
        {
            return _context.Utilisateurs.ToList();
        }
        public void AjouterLivreToUtilisateur(Livre livre, Utilisateur utilisateur) 
        { 
            _context.Livres.Add(livre);
        }
    }
}
