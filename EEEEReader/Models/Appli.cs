using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEEEReader.Models
{
    public class Appli
    {
        public List<Client> Clients { get; set; } = new List<Client>();
        public List<Administrateur> Admins { get; set; } = new List<Administrateur>();

        public Appli()
        {
            
        }

        public void AddClient(string nom, string pwd)
        {
            var client = new Client(nom, pwd);
            Clients.Add(client);
        }

        public void AddAdmin(string nom, string pwd)
        {
            var admin = new Administrateur(nom, pwd);
            Admins.Add(admin);
        }

        public bool CheckLogin(string nom, string pwd)
        {
            foreach (var client in Clients)
            {
                if (client.Nom == nom && client.Pwd == pwd)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
