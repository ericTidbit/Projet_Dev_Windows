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
    public class DataProviderLivre : IDataProviderLivre
    {
        private readonly EEEEReaderDbContext _context;

        public DataProviderLivre()
        {
            _context = new EEEEReaderDbContext();
        }

        public DataProviderLivre(EEEEReaderDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
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
            return LivreToUtilisateur;
        }

        public List<Livre> GetUtilisateurLivreDataParAuteur(int UtilisateurId)
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

            List<Livre> livresSortedByAuteur = LivreToUtilisateur.OrderBy(l => l.Auteur).ToList();
            return livresSortedByAuteur;
        }

        public void AjouterLivreToUtilisateur(Livre livre)
        {
            _context.Livres.Add(livre);
            _context.SaveChanges();
        }

        public void ChangerPageDuLivre(int LivreId, int NouvellePage)
        {
            var book = _context.Livres.Single(l => l.Id == LivreId);
            book.CurrentPage = NouvellePage;
            _context.SaveChanges();
        }

        public void SupprimerLivre(int LivreId)
        {
            Livre? LivreASupprimer = _context.Livres.FirstOrDefault(c => c.Id == LivreId);
            if (LivreASupprimer != null)
            {
                _context.Livres.Remove(LivreASupprimer);
                _context.SaveChanges();
            }
        }
    }
}
