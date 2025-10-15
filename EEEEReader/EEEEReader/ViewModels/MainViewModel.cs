using System;
using System.Windows.Input;

namespace EEEEReader.ViewModels
{
    /// <summary>
    /// ViewModel pour la fenêtre principale
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private string _texteEntree = string.Empty;
        private string _texteAffiche = string.Empty;

        /// <summary>
        /// Texte saisi par l'utilisateur
        /// </summary>
        public string TexteEntree
        {
            get => _texteEntree;
            set => SetProperty(ref _texteEntree, value);
        }

        /// <summary>
        /// Texte affiché à l'écran
        /// </summary>
        public string TexteAffiche
        {
            get => _texteAffiche;
            set => SetProperty(ref _texteAffiche, value);
        }

        /// <summary>
        /// Commande pour envoyer le texte
        /// </summary>
        public ICommand CommandeEnvoyer { get; }

        public MainViewModel()
        {
            CommandeEnvoyer = new RelayCommand(_ => TexteAffiche = TexteEntree);
        }
    }
}