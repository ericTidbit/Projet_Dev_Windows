using System;

namespace EEEEReader.Models
{
    /// <summary>
    /// Mod�le repr�sentant un message
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
        /// Date de cr�ation du message
        /// </summary>
        public DateTime DateCreation { get; set; } = DateTime.Now;

        /// <summary>
        /// Auteur du message
        /// </summary>
        public string Auteur { get; set; } = string.Empty;
    }
}
