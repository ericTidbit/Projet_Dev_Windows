using EEEEReader.Models;
using EEEEReader.ViewModels;
using System;
using System.Collections.Generic;

// fonctionnalité de tri va être un select ici qui va le montré
namespace EEEEReader.data
{
    public class DataProvider : IDataProvider
    {
        private Appli _application;
        private List<Client> _clients;
        private List<Livre> _livres;

        public DataProvider()
        {
            _clients = new List<Client>();
            _livres = new List<Livre>();
            _application = InitialiserDonnees();
        }

        // permet de récupérer les données (ton application)
        public Appli GetData()
        {
            return _application;
        }

        List<Data> IDataProvider.GetData()
        {
            throw new NotImplementedException();
        }

        private Appli InitialiserDonnees()
        {
            var app = new Appli();
            app.AddClient("test", "ouioui");
            return app;
        }
    }
}
