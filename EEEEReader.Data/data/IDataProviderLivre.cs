using EEEEReader.Data.Models;
using System.Collections.Generic;

namespace EEEEReader.Data
{
    public interface IDataProviderLivre
    {
        void AjouterLivreToUtilisateur(Livre livre);
        List<Livre> GetUtilisateurLivreData(int utilisateurId);
        List<Livre> GetUtilisateurLivreDataParAuteur(int utilisateurId);
        void ChangerPageDuLivre(int livreId, int nouvellePage);
        void SupprimerLivre(int livreId);
    }
}
