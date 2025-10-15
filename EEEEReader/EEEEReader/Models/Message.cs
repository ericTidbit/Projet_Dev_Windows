using System;

namespace EEEEReader.Models
{
    /// <summary>
    /// Modèle représentant un message
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Identifiant unique du message
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Contenu du message
        /// </summary>
        public string Contenu { get; set; } = string.Empty;

        /// <summary>
        /// Date de création du message
        /// </summary>
        public DateTime DateCreation { get; set; } = DateTime.Now;

        /// <summary>
        /// Auteur du message
        /// </summary>
        public string Auteur { get; set; } = string.Empty;
    }
}
