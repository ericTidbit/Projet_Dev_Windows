using EEEEReader.Data.Models;
using EEEEReader.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Protection.PlayReady;

namespace EEEEReader.ViewModels
{
    public class UtilisateursViewModel : BaseViewModel
    {
        public Utilisateur _user;
        
        public UtilisateursViewModel(Utilisateur user)
        {
            ObservableCollection<ValidationResult> listeValidations = new ObservableCollection<ValidationResult>();
            _user = user;
        }

        public int Id
        {
            get => _user.Id;
        }


        public string Nom
        {
            get => _user.Nom;
            set
            {
                if (_user.Nom != value)
                {
                    _user.Nom = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Pwd
        {
            get => _user.Pwd;
            set
            {
                if (_user.Pwd != value)
                {
                    _user.Pwd = HashPassword(value);
                    RaisePropertyChanged();
                }
            }
        }

        public Librairie Librairie
        {
            get => _user.Librairie;
            set
            {
                if (_user.Librairie != value)
                {
                    _user.Librairie = value;
                    RaisePropertyChanged();
                }
            }
        }

        public DateTime? Date
        {
            get => _user.Date;
        }

        public bool IsAdmin
        {
            get => _user.IsAdmin;
            set
            {
                if (_user.IsAdmin != value)
                {
                    _user.IsAdmin = value;
                    RaisePropertyChanged();
                }
            }
        }


        public string DateToString(DateTime date, string format)
        {
            return date.ToString(format);
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
            return HashPassword(password) == Pwd;
        }
    }
}

