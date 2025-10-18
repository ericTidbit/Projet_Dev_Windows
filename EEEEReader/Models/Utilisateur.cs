using System;
using System.Security.Cryptography;
using System.Text;

namespace EEEEReader.Models
{
    public abstract class Utilisateur
    {
        public string Nom { get; set; }

        private string _pwd;
        public string Pwd 
        { 
            get => _pwd;
            set => _pwd = HashPassword(value);
        }
        
        public Librairie Librairie { get; set; }
        public DateTime? Date { get; set; }
        public bool IsAdmin { get; set; }

        public Utilisateur(string nom, string pwd)
        {
            Nom = nom ?? throw new ArgumentNullException(nameof(nom));
            Pwd = pwd ?? throw new ArgumentNullException(nameof(pwd));
            Librairie = new Librairie();
            Date = DateTime.Now;
            IsAdmin = false;
        }

        private static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public bool VerifierPassword(string password)
        {
            return HashPassword(password) == _pwd;
        }
    }
}
