using EEEEReader.Models;
using System.Reflection.Metadata;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestModels.Models
{
    public class TestUtilisateur
    {
        [Fact]
        public void UtilisateurAjouterLivre()
        {
            // Arrange
            var utilisateur = new Client("testUser", "testPwd");
            var livre = new Livre("Content", "Titre", "Auteur", "2023", "1234567890", "Français", "Résumé");
            // Act
            utilisateur.AjouterLivre(livre);
            // Assert
            Assert.Contains(livre, utilisateur.Librairie.Livres);
        }
    }
}