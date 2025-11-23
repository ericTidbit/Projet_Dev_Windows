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
        public List<Livre> GetUtilisateurLivreData(int UtilisateurId)
        {
            
            List<Livre> TousLesLivresDispo = _context.Livres.ToList();

            List<Livre> LivreToUtilisateur = new List<Livre>();

            foreach (Livre livre in TousLesLivresDispo)
            {
               if (livre.UtilisateurId == UtilisateurId)
                {
                    LivreToUtilisateur.Add(livre);
                }
            
            }
            return TousLesLivresDispo;
        }

        public void AjouterLivreToUtilisateur(Livre livre, Utilisateur utilisateur) 
        { 
            _context.Livres.Add(livre);
        }

        public void AjouterLivreToUtilisateur(Livre livre)
        {
            throw new NotImplementedException();
        }

        public void ChangerPageDuLivre(int id, int NouvellePage)
        {
            throw new NotImplementedException();
        }
    }
}
