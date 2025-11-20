using EEEEReader.Data.Models;
using EEEEReader.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace EEEEReader.Data
{
    public class DataProvider : IDataProvider
    {
        private readonly EEEEReaderDbContext _dbContext;

        private readonly List<Utilisateur> _utilisateurs;
        private readonly List<Livre> _livres;

        public DataProvider(EEEEReaderDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        }

        public void AjouterUtilisateur(Utilisateur utilisateur)
        {
            // ne fonctionne pas 
            if (utilisateur == null)
                throw new ArgumentNullException(nameof(utilisateur));

            _dbContext.Utilisateurs.Add(utilisateur);
            _dbContext.SaveChanges();
        }

        public List<Utilisateur> GetUtilisateursData()
        {
            return _dbContext.Utilisateurs.ToList();
        }

        
    }
}
