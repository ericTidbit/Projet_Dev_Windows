using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EEEEReader.Data.Models;

namespace EEEEReader.Data
{
    public class EEEEReaderDbContext : DbContext
    {
        public EEEEReaderDbContext(DbContextOptions<EEEEReaderDbContext> options) : base(options) { }

        public EEEEReaderDbContext() : base() { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Livre> Livres { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string dbPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "EEEEreader", "EEEEReader.db");

                Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);

                optionsBuilder.UseSqlite($"Data Source={dbPath}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
