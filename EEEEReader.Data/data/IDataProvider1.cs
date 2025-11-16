using EEEEReader.Data.Models;
using EEEEReader.Models;
using System.Collections.Generic;

public interface IDataProvider
{
    void AjouterUtilisateur(Utilisateur utilisateur);
    List<Utilisateur> GetUtilisateursData();
}