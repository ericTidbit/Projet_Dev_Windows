using EEEEReader.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEEEReader.Data
{
    public class DataSeeder
    {
        private readonly EEEEReaderDbContext _context;
        public DataSeeder(EEEEReaderDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            // Ne seed que si BD vide
            if (_context.Livres.Any())
                return;

            List<Livre> Livres = new List<Livre>
            {
                new Livre(
                    RawContent: null,
                    Titre: "Livre exemple",
                    Auteur: "Auteur exemple",
                    Date: "2024",
                    ISBN: "978-0000000000",
                    Langue: "fr",
                    Resume: "Résumé exemple",
                    CoverRaw: null,
                    CoverImage: null,
                    HtmlContentList: null
                )
            };

            _context.Livres.AddRange(Livres);
            await _context.SaveChangesAsync();
        }
    }
}
