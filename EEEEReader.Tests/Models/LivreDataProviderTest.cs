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


        // Felix
        [TestMethod]
        public void AjouterPlusieursLivresPourUtilisateur_RetourneBonNombre()
        {
            Utilisateur u1 = new("u1", "pwd");
            Utilisateur u2 = new("u2", "pwd");
            _dataProvider.AjouterUtilisateur(u1);
            _dataProvider.AjouterUtilisateur(u2);
            var users = _dataProvider.GetUtilisateursData();

            int id1 = users.First(u => u.Nom == "u1").Id;
            int id2 = users.First(u => u.Nom == "u2").Id;

            _dataProvider.AjouterLivreToUtilisateur(new Livre { Titre = "L1", Auteur = "A", FichierEpub = new byte[0], CurrentPage = 0, Pourcentage = 0, UtilisateurId = id1 });
            _dataProvider.AjouterLivreToUtilisateur(new Livre { Titre = "L2", Auteur = "B", FichierEpub = new byte[0], CurrentPage = 0, Pourcentage = 0, UtilisateurId = id1 });
            _dataProvider.AjouterLivreToUtilisateur(new Livre { Titre = "L3", Auteur = "C", FichierEpub = new byte[0], CurrentPage = 0, Pourcentage = 0, UtilisateurId = id2 });

            Assert.AreEqual(2, _dataProvider.GetUtilisateurLivreData(id1).Count);
            Assert.AreEqual(1, _dataProvider.GetUtilisateurLivreData(id2).Count);
        }

        [TestMethod]
        public void ChangerPageDuLivre_IdInexistant_LanceInvalidOperationException()
        {
            int inexistId = 99999;
            Assert.Throws<InvalidOperationException>(() => _dataProvider.ChangerPageDuLivre(inexistId, 10));
        }

        [TestMethod]
        public void AjouterLivre_AssigneIdNonNul()
        {
            Utilisateur utilisateur = new("idtester", "pwd");
            _dataProvider.AjouterUtilisateur(utilisateur);
            int userId = _dataProvider.GetUtilisateursData().First().Id;

            var livre = new Livre
            {
                Titre = "AvecId",
                Auteur = "AuteurId",
                FichierEpub = new byte[0],
                CurrentPage = 0,
                Pourcentage = 0,
                UtilisateurId = userId
            };
            _dataProvider.AjouterLivreToUtilisateur(livre);

            Livre livreFromDb = _dataProvider.GetUtilisateurLivreData(userId).First();
            Assert.IsTrue(livreFromDb.Id > 0);
        }

        [TestMethod]
        public void GetUtilisateurLivreData_IsoleLesLivresParUtilisateur()
        {
            Utilisateur u1 = new("iso1", "pwd");
            Utilisateur u2 = new("iso2", "pwd");
            _dataProvider.AjouterUtilisateur(u1);
            _dataProvider.AjouterUtilisateur(u2);

            var users = _dataProvider.GetUtilisateursData();
            int id1 = users.First(u => u.Nom == "iso1").Id;
            int id2 = users.First(u => u.Nom == "iso2").Id;

            _dataProvider.AjouterLivreToUtilisateur(new Livre { Titre = "U1-L1", Auteur = "A", FichierEpub = new byte[0], CurrentPage = 0, Pourcentage = 0, UtilisateurId = id1 });
            _dataProvider.AjouterLivreToUtilisateur(new Livre { Titre = "U2-L1", Auteur = "B", FichierEpub = new byte[0], CurrentPage = 0, Pourcentage = 0, UtilisateurId = id2 });
            _dataProvider.AjouterLivreToUtilisateur(new Livre { Titre = "U1-L2", Auteur = "C", FichierEpub = new byte[0], CurrentPage = 0, Pourcentage = 0, UtilisateurId = id1 });

            var livresU1 = _dataProvider.GetUtilisateurLivreData(id1);
            var livresU2 = _dataProvider.GetUtilisateurLivreData(id2);

            Assert.AreEqual(2, livresU1.Count);
            Assert.AreEqual(1, livresU2.Count);
            Assert.IsTrue(livresU1.All(l => l.UtilisateurId == id1));
            Assert.IsTrue(livresU2.All(l => l.UtilisateurId == id2));
        }
    }
}
