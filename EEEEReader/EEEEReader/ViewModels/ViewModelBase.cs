using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EEEEReader.ViewModels
{
    /// <summary>
    /// Classe de base pour tous les ViewModels impl�mentant INotifyPropertyChanged
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Notifie qu'une propri�t� a chang�
        /// </summary>
        /// <param name="nomPropriete">Nom de la propri�t� (automatique avec CallerMemberName)</param>
        protected void OnPropertyChanged([CallerMemberName] string? nomPropriete = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nomPropriete));
        }

        /// <summary>
        /// D�finit la valeur d'un champ et notifie si elle a chang�
        /// </summary>
        /// <typeparam name="T">Type de la propri�t�</typeparam>
        /// <param name="champ">R�f�rence au champ de sauvegarde</param>
        /// <param name="valeur">Nouvelle valeur</param>
        /// <param name="nomPropriete">Nom de la propri�t� (automatique)</param>
        /// <returns>True si la valeur a chang�, False sinon</returns>
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
