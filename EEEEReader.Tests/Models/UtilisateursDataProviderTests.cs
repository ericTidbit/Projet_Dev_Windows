using EEEEReader.Data;
using EEEEReader.Data.data;
using EEEEReader.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

// Félix

namespace EEEEReader.Tests.Models
{
    [TestClass]
    public class UtilisateursDataProviderTests
    {
        private EEEEReaderDbContext _context;
        private DataProviderUtilisateur _dataProvider;

        // private clientDataProvider 

        [TestInitialize]
        public void Setup()
        {

            var options = new DbContextOptionsBuilder<EEEEReaderDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            _context = new EEEEReaderDbContext(options);
            _context.Database.OpenConnection();
            _context.Database.EnsureCreated();

            _dataProvider = new DataProviderUtilisateur(_context);


        }
        [TestCleanup]
        public void Nettoyer()
        {
            _context.Database.CloseConnection();
            _context.Dispose();
        }

        [TestMethod]
        public void AjouterUtilisateur()
        {
            // Arrange
            Utilisateur utilisateur = new("felix", "12345678");

            // Act
            _dataProvider.AjouterUtilisateur(utilisateur);
            List<Utilisateur> utilisateurDansLaDb = _dataProvider.GetUtilisateursData();

            // Assert
            Assert.AreEqual(1, utilisateurDansLaDb.Count);
        }

        // Julien

        [TestMethod]
        public void AjouterPlusieursUtilisateurs()
        {
            // Arrange
            Utilisateur utilisateur1 = new("felix", "12345678");
            Utilisateur utilisateur2 = new("eric", "12345678");
            Utilisateur utilisateur3 = new("julien", "12345678");

            // Act
            _dataProvider.AjouterUtilisateur(utilisateur1);
            _dataProvider.AjouterUtilisateur(utilisateur2);
            _dataProvider.AjouterUtilisateur(utilisateur3);
            List<Utilisateur> utilisateurs = _dataProvider.GetUtilisateursData();

            // Assert
            Assert.AreEqual(3, utilisateurs.Count);
            Assert.IsTrue(utilisateurs.Any(u => u.Nom == "felix"));
            Assert.IsTrue(utilisateurs.Any(u => u.Nom == "eric"));
            Assert.IsTrue(utilisateurs.Any(u => u.Nom == "julien"));
        }

        [TestMethod]
        public void GetUtilisateursData_SansUtilisateurs()
        {
            // Act
            List<Utilisateur> utilisateurs = _dataProvider.GetUtilisateursData();

            // Assert
            Assert.IsNotNull(utilisateurs); // une liste vide doit pas etre nulle
            Assert.AreEqual(0, utilisateurs.Count);
        }

        [TestMethod]
        public void AjouterUtilisateurConserverMotDePasse()
        {
            // Arrange
            string nomUtilisateur = "eric";
            string motDePasse = "12345678";
            Utilisateur utilisateur = new(nomUtilisateur, motDePasse);

            // Act
            _dataProvider.AjouterUtilisateur(utilisateur);
            List<Utilisateur> utilisateurs = _dataProvider.GetUtilisateursData();
            Utilisateur utilisateurRecupere = utilisateurs.First();

            // Assert
            Assert.AreEqual(nomUtilisateur, utilisateurRecupere.Nom);
            Assert.IsTrue(utilisateurRecupere.VerifierPassword(motDePasse));
        }

        [TestMethod]
        public void VerifierPasswordMauvaisMotDePasse()
        {
            // Arrange
            Utilisateur utilisateur = new("julien", "12345678");
            _dataProvider.AjouterUtilisateur(utilisateur);
            List<Utilisateur> utilisateurs = _dataProvider.GetUtilisateursData();
            Utilisateur utilisateurRecupere = utilisateurs.First();

            // Act
            bool resultat = utilisateurRecupere.VerifierPassword("unmauvaismotdepasse");

            // Assert
            Assert.IsFalse(resultat);
        }

        [TestMethod]
        public void AjouterUtilisateurIdUnique()
        {
            // Arrange
            Utilisateur utilisateur1 = new("felix", "12345678");
            Utilisateur utilisateur2 = new("eric", "12345678");

            // Act
            _dataProvider.AjouterUtilisateur(utilisateur1);
            _dataProvider.AjouterUtilisateur(utilisateur2);
            List<Utilisateur> utilisateurs = _dataProvider.GetUtilisateursData();

            // Assert
            Assert.AreEqual(2, utilisateurs.Count);
            Assert.AreNotEqual(utilisateurs[0].Id, utilisateurs[1].Id);
            Assert.IsTrue(utilisateurs[0].Id > 0);
            Assert.IsTrue(utilisateurs[1].Id > 0);
        }
    }
}
