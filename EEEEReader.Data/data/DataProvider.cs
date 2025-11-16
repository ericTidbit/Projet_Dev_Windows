using EEEEReader.Data.Models;
using EEEEReader.Models;
using System;
using System.Collections.Generic;

namespace EEEEReader.Data
{
    public class DataProvider : IDataProvider
    {
        private readonly List<Utilisateur> _utilisateurs;
        private readonly List<Livre> _livres;

        public DataProvider(EEEEReaderDbContext dbContext)
        {
            _utilisateurs = new List<Utilisateur>();
            _livres = new List<Livre>();

            SeedData();
        }

        public void AjouterUtilisateur(Utilisateur utilisateur)
        {
            if (utilisateur == null)
                throw new ArgumentNullException(nameof(utilisateur));

            _utilisateurs.Add(utilisateur);
        }

        // Retourne tous les utilisateurs
        public List<Utilisateur> GetUtilisateursData()
        {
            return new List<Utilisateur>(_utilisateurs);
        }

        private void SeedData()
        {
            _utilisateurs.Add(new Utilisateur("e", "3f79bb7b435b05321651daefd374cdc681dc06faa65e374e38337b88ca046dea") { IsAdmin = true });
            _utilisateurs.Add(new Utilisateur("test", "test") { IsAdmin = false });

        }
    }
}
