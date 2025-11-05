using EEEEReader.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEEEReader.ViewModels
{
    public class ClientViewModel : BaseViewModel
    {
        private Client _client;

        public ClientViewModel(string nom, string pwd)
        {
            _client = new Client(nom, pwd);
        }
    }
}
