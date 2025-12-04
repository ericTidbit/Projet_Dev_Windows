using EEEEReader.Data;
using EEEEReader.Data.data;
using EEEEReader.Data.Models;
using EEEEReader.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace EEEEReader.Tests.Models
{
    [TestClass]
    public class LivreDataProviderTest
    {
        private EEEEReaderDbContext _context;
        private DataProvider _dataProvider;

        [TestInitialize]
        public void Setup()
        {

            var options = new DbContextOptionsBuilder<EEEEReaderDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            _context = new EEEEReaderDbContext(options);
            _context.Database.OpenConnection();
            _context.Database.EnsureCreated();

            _dataProvider = new DataProvider(_context);


        }

        [TestCleanup]
        public void Nettoyer()
        {
            _context.Database.CloseConnection();
            _context.Dispose();
        }

        // Créé une seule fois et utilisé dans les tests (c'est plus simple)
        // par copilote
        private Livre CreateLivre(string titre, string auteur, int utilisateurId, int currentPage = 0)
        {
            return new Livre
            {
                Titre = titre,
                Auteur = auteur,
                FichierEpub = new byte[0],
                CurrentPage = currentPage,
                Pourcentage = 0,
                UtilisateurId = utilisateurId
            };
        }


        // Eric
        [TestMethod]
        public void TestAjouterLivreToUtilisateur()
        {
            // Arrange
            Utilisateur utilisateur = new("felix", "12345678");
            _dataProvider.AjouterUtilisateur(utilisateur);
            List<Utilisateur> utilisateurDansLaDb = _dataProvider.GetUtilisateursData();
            Livre livreTest = CreateLivre("Echoes of Silksong", "Eric", utilisateurDansLaDb[0].Id);

            // Act
            _dataProvider.AjouterLivreToUtilisateur(livreTest);
            List<Livre> livres = _dataProvider.GetUtilisateurLivreData(utilisateurDansLaDb[0].Id);

            // Assert
            Assert.AreEqual(1, livres.Count);
            Assert.AreEqual("Echoes of Silksong", livres[0].Titre);
            Assert.AreEqual("Eric", livres[0].Auteur);
            Assert.AreEqual(utilisateurDansLaDb[0].Id, livres[0].UtilisateurId);
        }

        [TestMethod]
        public void GetUtilisateurLivreData_SansLivres_RetourneVide()
        {
            // Arrange
            Utilisateur utilisateur = new("user2", "pwd");
            _dataProvider.AjouterUtilisateur(utilisateur);
            List<Utilisateur> utilisateurDansLaDb = _dataProvider.GetUtilisateursData();

            // Act
            List<Livre> livres = _dataProvider.GetUtilisateurLivreData(utilisateurDansLaDb[0].Id);

            // Assert
            Assert.IsNotNull(livres);
            Assert.AreEqual(0, livres.Count);
        }

        [TestMethod]
        public void ChangerPageDuLivre_MetAJourLaPageActuelle()
        {
            // Arrange
            Utilisateur utilisateur = new("user3", "pwd");
            _dataProvider.AjouterUtilisateur(utilisateur);
            List<Utilisateur> utilisateurDansLaDb = _dataProvider.GetUtilisateursData();

            Livre livreTest = CreateLivre("Echoes of Silksong", "Eric", utilisateurDansLaDb[0].Id);
            _dataProvider.AjouterLivreToUtilisateur(livreTest);

            Livre livreFromDb = _dataProvider.GetUtilisateurLivreData(utilisateurDansLaDb[0].Id).First();

            // Act
            _dataProvider.ChangerPageDuLivre(livreFromDb.Id, 42);
            Livre updated = _dataProvider.GetUtilisateurLivreData(utilisateurDansLaDb[0].Id).First();

            // Assert
            Assert.AreEqual(42, updated.CurrentPage);
        }
    }
}
