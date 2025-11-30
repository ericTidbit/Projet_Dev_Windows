using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EEEEReader.Data.Models;
using EEEEReader.Models;
using System.Diagnostics;

namespace EEEEReader.Data
{
    public class EEEEReaderDbContext : DbContext
    {
        public EEEEReaderDbContext(DbContextOptions<EEEEReaderDbContext> options) : base(options) { }

        public EEEEReaderDbContext() : base() { }

        public DbSet<Utilisateur> Utilisateurs => Set<Utilisateur>();
        public DbSet<Livre> Livres => Set<Livre>();
        //public DbSet<Librairie> Librairies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string dbPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "EEEEreader", "EEEEReader.db");

                Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);
                Debug.WriteLine("path de la db", dbPath);

                optionsBuilder.UseSqlite($"Data Source={dbPath}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Ignore<Librairie>();
            // la librairy sert a rien est fait bugger la release
        }
    }
}
