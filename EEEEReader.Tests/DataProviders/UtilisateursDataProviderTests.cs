using EEEEReader.Data;
using EEEEReader.Data.data;
using EEEEReader.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Security.Authentication.OAuth;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEEEReader.Tests.DataProviders
{
    public class UtilisateursDataProviderTests
    {
        private EEEEReaderDbContext _context;
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

            // _clientDataProvider = new CBDClientDataprovider(_context)

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


            // Assert
            // Assert.IsNotNull(saved_things);
            // Assert.AreEqual(nom, "felix");
            // Assert.AreEqual(pwd, "1234");
            // Assert.AreEqual(, 1);

            Assert.AreEqual(1,1);
        }
        [TestMethod]
        public void GetUtilisateursData()
        {
            Utilisateur utilisateur = new Utilisateur("felix", "1234");

        }
    }
}
