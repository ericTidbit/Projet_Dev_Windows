using EEEEReader.Data;
using EEEEReader.Data.data;
using EEEEReader.Data.Models;
using EEEEReader.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EEEEReader.Tests.Models
{
    [TestClass]
    public class LivreDataProviderTest
    {
        private EEEEReaderDbContext _context;
        private DataProviderUtilisateur _utilisateurDataProvider;
        private DataProviderLivre _livreDataProvider;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<EEEEReaderDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            _context = new EEEEReaderDbContext(options);
            _context.Database.OpenConnection();
            _context.Database.EnsureCreated();

            _utilisateurDataProvider = new DataProviderUtilisateur(_context);
            _livreDataProvider = new DataProviderLivre(_context);
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
            _utilisateurDataProvider.AjouterUtilisateur(utilisateur);
            List<Utilisateur> utilisateurDansLaDb = _utilisateurDataProvider.GetUtilisateursData();
            Livre livreTest = CreateLivre("Echoes of Silksong", "Eric", utilisateurDansLaDb[0].Id);

            // Act
            _livreDataProvider.AjouterLivreToUtilisateur(livreTest);
            List<Livre> livres = _livreDataProvider.GetUtilisateurLivreData(utilisateurDansLaDb[0].Id);

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
            _utilisateurDataProvider.AjouterUtilisateur(utilisateur);
            List<Utilisateur> utilisateurDansLaDb = _utilisateurDataProvider.GetUtilisateursData();

            // Act
            List<Livre> livres = _livreDataProvider.GetUtilisateurLivreData(utilisateurDansLaDb[0].Id);

            // Assert
            Assert.IsNotNull(livres);
            Assert.AreEqual(0, livres.Count);
        }

        [TestMethod]
        public void ChangerPageDuLivre_MetAJourLaPageActuelle()
        {
            // Arrange
            Utilisateur utilisateur = new("user3", "pwd");
            _utilisateurDataProvider.AjouterUtilisateur(utilisateur);
            List<Utilisateur> utilisateurDansLaDb = _utilisateurDataProvider.GetUtilisateursData();

            Livre livreTest = CreateLivre("Echoes of Silksong", "Eric", utilisateurDansLaDb[0].Id);
            _livreDataProvider.AjouterLivreToUtilisateur(livreTest);

            Livre livreFromDb = _livreDataProvider.GetUtilisateurLivreData(utilisateurDansLaDb[0].Id).First();

            // Act
            _livreDataProvider.ChangerPageDuLivre(livreFromDb.Id, 42);
            Livre updated = _livreDataProvider.GetUtilisateurLivreData(utilisateurDansLaDb[0].Id).First();

            // Assert
            Assert.AreEqual(42, updated.CurrentPage);
        }

        [TestMethod]
        public void SupprimerLivre_EnleverDeLaDatabase()
        {
            // Arrange
            Utilisateur utilisateur = new("felix", "12345678");
            _utilisateurDataProvider.AjouterUtilisateur(utilisateur);
            List<Utilisateur> utilisateurDansLaDb = _utilisateurDataProvider.GetUtilisateursData();
            Livre livreTest = CreateLivre("Echoes of Silksong", "Eric", utilisateurDansLaDb[0].Id);
            _livreDataProvider.AjouterLivreToUtilisateur(livreTest);
            List<Livre> livres = _livreDataProvider.GetUtilisateurLivreData(utilisateurDansLaDb[0].Id);

            // Act
            _livreDataProvider.SupprimerLivre(livres[0].Id);
            List<Livre> livresSupprime = _livreDataProvider.GetUtilisateurLivreData(utilisateurDansLaDb[0].Id);

            // Assert
            Assert.AreEqual(0, livresSupprime.Count);
        }

        [TestMethod]
        public void GetUtilisateurLivreDataParAuteur_RetourneTriParAuteur()
        {
            Utilisateur utilisateur = new("trieur", "pwd");
            _utilisateurDataProvider.AjouterUtilisateur(utilisateur);
            int userId = _utilisateurDataProvider.GetUtilisateursData().First().Id;

            var livreA = CreateLivre("Titre A", "Zed", userId);
            var livreB = CreateLivre("Titre B", "Anna", userId);
            var livreC = CreateLivre("Titre C", "Bob", userId);

            _livreDataProvider.AjouterLivreToUtilisateur(livreA);
            _livreDataProvider.AjouterLivreToUtilisateur(livreB);
            _livreDataProvider.AjouterLivreToUtilisateur(livreC);

            List<Livre> livresTries = _livreDataProvider.GetUtilisateurLivreDataParAuteur(userId);

            Assert.AreEqual(3, livresTries.Count);
            Assert.AreEqual("Anna", livresTries[0].Auteur);
            Assert.AreEqual("Bob", livresTries[1].Auteur);
            Assert.AreEqual("Zed", livresTries[2].Auteur);
        }

        // Felix
        [TestMethod]
        public void AjouterPlusieursLivresPourUtilisateur_RetourneBonNombre()
        {
            Utilisateur u1 = new("u1", "pwd");
            Utilisateur u2 = new("u2", "pwd");
            _utilisateurDataProvider.AjouterUtilisateur(u1);
            _utilisateurDataProvider.AjouterUtilisateur(u2);
            var users = _utilisateurDataProvider.GetUtilisateursData();

            int id1 = users.First(u => u.Nom == "u1").Id;
            int id2 = users.First(u => u.Nom == "u2").Id;

            _livreDataProvider.AjouterLivreToUtilisateur(new Livre { Titre = "L1", Auteur = "A", FichierEpub = new byte[0], CurrentPage = 0, Pourcentage = 0, UtilisateurId = id1 });
            _livreDataProvider.AjouterLivreToUtilisateur(new Livre { Titre = "L2", Auteur = "B", FichierEpub = new byte[0], CurrentPage = 0, Pourcentage = 0, UtilisateurId = id1 });
            _livreDataProvider.AjouterLivreToUtilisateur(new Livre { Titre = "L3", Auteur = "C", FichierEpub = new byte[0], CurrentPage = 0, Pourcentage = 0, UtilisateurId = id2 });

            Assert.AreEqual(2, _livreDataProvider.GetUtilisateurLivreData(id1).Count);
            Assert.AreEqual(1, _livreDataProvider.GetUtilisateurLivreData(id2).Count);
        }

        [TestMethod]
        public void ChangerPageDuLivre_IdInexistant_LanceInvalidOperationException()
        {
            int inexistId = 99999;
            Assert.ThrowsException<InvalidOperationException>(() => _livreDataProvider.ChangerPageDuLivre(inexistId, 10));
        }

        [TestMethod]
        public void AjouterLivre_AssigneIdNonNul()
        {
            Utilisateur utilisateur = new("idtester", "pwd");
            _utilisateurDataProvider.AjouterUtilisateur(utilisateur);
            int userId = _utilisateurDataProvider.GetUtilisateursData().First().Id;

            var livre = new Livre
            {
                Titre = "AvecId",
                Auteur = "AuteurId",
                FichierEpub = new byte[0],
                CurrentPage = 0,
                Pourcentage = 0,
                UtilisateurId = userId
            };
            _livreDataProvider.AjouterLivreToUtilisateur(livre);

            Livre livreFromDb = _livreDataProvider.GetUtilisateurLivreData(userId).First();
            Assert.IsTrue(livreFromDb.Id > 0);
        }

        [TestMethod]
        public void GetUtilisateurLivreData_IsoleLesLivresParUtilisateur()
        {
            Utilisateur u1 = new("iso1", "pwd");
            Utilisateur u2 = new("iso2", "pwd");
            _utilisateurDataProvider.AjouterUtilisateur(u1);
            _utilisateurDataProvider.AjouterUtilisateur(u2);

            var users = _utilisateurDataProvider.GetUtilisateursData();
            int id1 = users.First(u => u.Nom == "iso1").Id;
            int id2 = users.First(u => u.Nom == "iso2").Id;

            _livreDataProvider.AjouterLivreToUtilisateur(new Livre { Titre = "U1-L1", Auteur = "A", FichierEpub = new byte[0], CurrentPage = 0, Pourcentage = 0, UtilisateurId = id1 });
            _livreDataProvider.AjouterLivreToUtilisateur(new Livre { Titre = "U2-L1", Auteur = "B", FichierEpub = new byte[0], CurrentPage = 0, Pourcentage = 0, UtilisateurId = id2 });
            _livreDataProvider.AjouterLivreToUtilisateur(new Livre { Titre = "U1-L2", Auteur = "C", FichierEpub = new byte[0], CurrentPage = 0, Pourcentage = 0, UtilisateurId = id1 });

            var livresU1 = _livreDataProvider.GetUtilisateurLivreData(id1);
            var livresU2 = _livreDataProvider.GetUtilisateurLivreData(id2);

            Assert.AreEqual(2, livresU1.Count);
            Assert.AreEqual(1, livresU2.Count);
            Assert.IsTrue(livresU1.All(l => l.UtilisateurId == id1));
            Assert.IsTrue(livresU2.All(l => l.UtilisateurId == id2));
        }
    }
}
