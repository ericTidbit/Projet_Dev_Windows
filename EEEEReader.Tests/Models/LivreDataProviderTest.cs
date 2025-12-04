using EEEEReader.Data;
using EEEEReader.Data.data;
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
    }
}
