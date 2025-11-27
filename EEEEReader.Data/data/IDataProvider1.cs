using EEEEReader.Data.Models;
using EEEEReader.Models;
using System.Collections.Generic;

namespace EEEEReader.Data
{
    public interface IDataProvider
    {
        void AjouterUtilisateur(Utilisateur utilisateur);
        void AjouterLivreToUtilisateur(Livre livre);
        List<Utilisateur> GetUtilisateursData();
        List<Livre> GetUtilisateurLivreData(int utilisateurId);
        void ChangerPageDuLivre(int id, int NouvellePage);
    }
}