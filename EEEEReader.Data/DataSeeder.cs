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

            
        }
    }
}
