using EEEEReader.Data.Models;
using EEEEReader.Models;
using System.Collections.Generic;

public interface IDataProvider
{
    void AjouterUtilisateur(Utilisateur utilisateur);
    void AjouterLivreToUtilisateur(Livre livre);
    List<Utilisateur> GetUtilisateursData();
    void ChangerPageDuLivre(int id);
}