using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using EEEEReader.Models;

namespace EEEEReader.ViewModels.Pages
{
    /// <summary>
    /// ViewModel pour une page de messages
    /// </summary>
    public class MessagesPageViewModel : ViewModelBase
    {
        private Message? _messageSelectionne;
        private string _nouveauMessageTexte = string.Empty;

        /// <summary>
        /// Collection de messages
        /// </summary>
        public ObservableCollection<Message> Messages { get; }

        /// <summary>
        /// Message actuellement sélectionné
        /// </summary>
        public Message? MessageSelectionne
        {
            get => _messageSelectionne;
            set => SetProperty(ref _messageSelectionne, value);
        }

        /// <summary>
        /// Texte du nouveau message à ajouter
        /// </summary>
        public string NouveauMessageTexte
        {
            get => _nouveauMessageTexte;
            set => SetProperty(ref _nouveauMessageTexte, value);
        }

        /// <summary>
        /// Commande pour ajouter un message
        /// </summary>
        public ICommand CommandeAjouterMessage { get; }

        /// <summary>
        /// Commande pour supprimer le message sélectionné
        /// </summary>
        public ICommand CommandeSupprimerMessage { get; }

        public MessagesPageViewModel()
        {
            Messages = new ObservableCollection<Message>();

            CommandeAjouterMessage = new RelayCommand(
                _ => AjouterMessage(),
                _ => !string.IsNullOrWhiteSpace(NouveauMessageTexte));

            CommandeSupprimerMessage = new RelayCommand(
                _ => SupprimerMessage(),
                _ => MessageSelectionne != null);

            // Ajouter quelques messages d'exemple
            ChargerMessagesExemple();
        }

        private void AjouterMessage()
        {
            var nouveauMessage = new Message
            {
                Id = Messages.Count + 1,
                Contenu = NouveauMessageTexte,
                Auteur = "Utilisateur",
                DateCreation = DateTime.Now
            };

            Messages.Add(nouveauMessage);
            NouveauMessageTexte = string.Empty;
        }

        private void SupprimerMessage()
        {
            if (MessageSelectionne != null)
            {
                Messages.Remove(MessageSelectionne);
                MessageSelectionne = null;
            }
        }

        private void ChargerMessagesExemple()
        {
            Messages.Add(new Message
            {
                Id = 1,
                Contenu = "Bienvenue dans l'application !",
                Auteur = "Système",
                DateCreation = DateTime.Now.AddMinutes(-10)
            });

            Messages.Add(new Message
            {
                Id = 2,
                Contenu = "Ceci est un message d'exemple",
                Auteur = "Admin",
                DateCreation = DateTime.Now.AddMinutes(-5)
            });
        }
    }
}
