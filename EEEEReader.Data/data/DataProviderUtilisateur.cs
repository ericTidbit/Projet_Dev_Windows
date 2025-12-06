using EEEEReader.Data.Models;
using EEEEReader.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace EEEEReader.Data
{
    public class DataProviderUtilisateur : IDataProviderUtilisateur
    {
        private readonly EEEEReaderDbContext _dbContext;

        public DataProviderUtilisateur(EEEEReaderDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public void AjouterUtilisateur(Utilisateur utilisateur)
        {
            if (utilisateur == null)
                throw new ArgumentNullException(nameof(utilisateur));
            utilisateur.Pwd = HashPassword(utilisateur.Pwd);
            _dbContext.Utilisateurs.Add(utilisateur);
            _dbContext.SaveChanges();
        }

        public List<Utilisateur> GetUtilisateursData()
        {
            return _dbContext.Utilisateurs.ToList();
        }

        private static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
