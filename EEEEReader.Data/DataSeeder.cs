using EEEEReader.Data.Models;
using EEEEReader.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Linq;
using VersOne.Epub; // Optionnel : uniquement si tu veux extraire la couverture

namespace EEEEReader.Data
{
    public class DataSeeder
    {
        private readonly EEEEReaderDbContext _context;

        public DataSeeder(EEEEReaderDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            // Ne seed que si la base est vide
            if (_context.Utilisateurs.Any() || _context.Livres.Any())
                return;

            var admin = new Utilisateur("admin", "3f79bb7b435b05321651daefd374cdc681dc06faa65e374e38337b88ca046dea");
            _context.Utilisateurs.Add(admin);
            _context.SaveChanges(); 
            // donc pour prendre le fichier epub j'ai mis directement dans une propriété de classe toute le binaire pour éviter le probleme de path
            byte[] fichierEpub = TestBooks.ConingsbyEpub;

            byte[] coverRaw = null;

            var livreTest = new Livre(
                fichierEpub: fichierEpub,
                titre: "Coningsby",
                auteur: "Benjamin Disraeli",
                date: "1844",
                isbn: null,
                langue: "en",
                resume: "Roman politique de Benjamin Disraeli publié en 1844 sous le pseudonyme de « un jeune auteur ». Une œuvre clé du conservatisme britannique.",
                coverRaw: coverRaw,
                utilisateurId: admin.Id
            );

            _context.Livres.Add(livreTest);
            _context.SaveChanges();

            Console.WriteLine("[DataSeeder] Utilisateur 'admin' (mdp: e) et livre 'Coningsby' ajoutés avec succès (fichier embarqué) !");
        }
    }
}