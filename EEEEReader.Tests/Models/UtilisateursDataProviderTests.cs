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
        private DataProvider _dataProvider;

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

            _dataProvider = new DataProvider(_context);


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
            Utilisateur utilisateur = new Utilisateur("felix", "1234");

            // Act
            // DBUtilisateursDataProvider.
            _dataProvider.AjouterUtilisateur(utilisateur);
            List<Utilisateur> UtilisateurDansLaDb = _dataProvider.GetUtilisateursData();


            // Assert
            // Assert.IsNotNull(saved_things);
            // Assert.AreEqual(nom, "felix");
            // Assert.AreEqual(pwd, "1234");
            // Assert.AreEqual(, 1);

            Assert.AreEqual(1, UtilisateurDansLaDb.Count);
        }
    }
}
