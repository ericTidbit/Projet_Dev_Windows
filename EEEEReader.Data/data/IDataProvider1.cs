using EEEEReader.Data.Models;
using EEEEReader.Models;
using System.Collections.Generic;

namespace EEEEReader.Data
{
    public interface IDataProviderUtilisateur
    {
        void AjouterUtilisateur(Utilisateur utilisateur);
        List<Utilisateur> GetUtilisateursData();
    }
}