using EEEEReader.Data.Models;
using EEEEReader.Models;
using Microsoft.EntityFrameworkCore;
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

        public void Seed()
        {
            // Ne seed que si BD vide
            if (_context.Livres.Any())
                return;

            List<Utilisateur> utilisateurs = new List<Utilisateur>
            {
                new Utilisateur("e", "e")
                {
                    IsAdmin = true
                },
                new Utilisateur("test", "3f79bb7b435b05321651daefd374cdc681dc06faa65e374e38337b88ca046dea")
                {
                    IsAdmin = false
                }

            };

            _context.Utilisateurs.AddRange(utilisateurs);
            _context.SaveChanges();
        }
    }
}
