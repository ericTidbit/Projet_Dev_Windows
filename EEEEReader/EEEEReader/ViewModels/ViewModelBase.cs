using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EEEEReader.ViewModels
{
    /// <summary>
    /// Classe de base pour tous les ViewModels implémentant INotifyPropertyChanged
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Notifie qu'une propriété a changé
        /// </summary>
        /// <param name="nomPropriete">Nom de la propriété (automatique avec CallerMemberName)</param>
        protected void OnPropertyChanged([CallerMemberName] string? nomPropriete = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nomPropriete));
        }

        /// <summary>
        /// Définit la valeur d'un champ et notifie si elle a changé
        /// </summary>
        /// <typeparam name="T">Type de la propriété</typeparam>
        /// <param name="champ">Référence au champ de sauvegarde</param>
        /// <param name="valeur">Nouvelle valeur</param>
        /// <param name="nomPropriete">Nom de la propriété (automatique)</param>
        /// <returns>True si la valeur a changé, False sinon</returns>
        protected bool SetProperty<T>(ref T champ, T valeur, [CallerMemberName] string? nomPropriete = null)
        {
            if (Equals(champ, valeur))
                return false;

            champ = valeur;
            OnPropertyChanged(nomPropriete);
            return true;
        }
    }
}
