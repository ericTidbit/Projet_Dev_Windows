using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Demo.ViewModels

    // tout copier de la demo

{
    public abstract class ValidableViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        /// <summary>
        /// Évènement lancé lorsque le dictionnaire d'erreurs est modifié
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <summary>
        /// Dictionnaire des erreurs d'un objet. Chaque propriété est une clé du dictionnaire. À chaque clé est associée une collection observable d'erreurs
        /// </summary>
        protected Dictionary<string, ObservableCollection<ValidationResult>> _errors = new Dictionary<string, ObservableCollection<ValidationResult>>();

        /// <summary>
        /// Retourne Vrai si des erreurs sont présentes dans le dictionnaire
        /// </summary>
        /// <value>Vrai si le dictionnaire n'est pas vide</value>
        public bool HasErrors
        {
            get
            {
                bool hasErrors = false;
                foreach (ObservableCollection<ValidationResult> listeErreurs in _errors.Values)
                {
                    if (listeErreurs.Any())
                    {
                        hasErrors = true;
                        break;
                    }
                }
                return hasErrors;
            }
        }

        /// <summary>
        /// Retourne Vrai si l'objet est valide (s'il n'y a pas d'erreurs de validation)
        /// </summary>
        public bool EstValide => !HasErrors;

        /// <summary>
        /// Retourne la collection d'erreurs de la propriété passée en paramètre à partir du dictionnaire d'erreurs
        /// </summary>
        /// <param name="propriete">Propriété pour laquelle on veut obtenir la collection d'erreurs</param>
        public IEnumerable GetErrors(string propriete)
        {
            if (_errors.ContainsKey(propriete))
            {
                return _errors[propriete];
            }
            else
            {
                return new ObservableCollection<ValidationResult>();
            }
        }

        /// <summary>
        /// Permet de valider la valeur passée en paramètre selon les annotations de validation de la propriété passée en paramètre
        /// </summary>
        /// <param name="value">Valeur qu'on veut attribuer à la propriété</param>
        /// <param name="propriete">Propriété (qui contient les annotations de validation)</param>
        public bool Validate<T>(T value, [CallerMemberName] string propriete = "")
        {
            EffacerErreurs(propriete);
            List<ValidationResult> erreursValidation = new List<ValidationResult>();
            ValidationContext contexte = new ValidationContext(this, null, null) { MemberName = propriete };
            bool proprieteValide = Validator.TryValidateProperty(value, contexte, erreursValidation);

            if (!proprieteValide)
            {
                AjoutErreurs(erreursValidation, propriete);
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Déclenche l'événement ErrorsChanged pour notifier les changements d'erreurs de validation
        /// </summary>
        /// <param name="propriete">Nom de la propriété dont les erreurs ont changé</param>
        protected void OnErrorsChanged([CallerMemberName] string propriete = "")
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propriete));
            RaisePropertyChanged(nameof(EstValide));
        }

        /// <summary>
        /// Ajoute au dictionnaire des erreurs la liste d'erreurs (passée en paramètre) à la propriété passée en paramètre
        /// </summary>
        /// <param name="erreursValidation">Liste des erreurs de validation retournée par le validateur</param>
        /// <param name="propriete">Propriété pour laquelle on ajoute des erreurs</param>
        private void AjoutErreurs(List<ValidationResult> erreursValidation, [CallerMemberName] string propriete = "")
        {
            if (!_errors.ContainsKey(propriete))
            {
                _errors.Add(propriete, new ObservableCollection<ValidationResult>());
            }

            foreach (ValidationResult erreur in erreursValidation)
            {
                _errors[propriete].Add(erreur);
            }
            OnErrorsChanged(propriete);
        }

        /// <summary>
        /// Efface dans le dictionnaire d'erreurs la collection d'erreurs pour la propriété passée en paramètre
        /// </summary>
        /// <param name="propriete">La propriété pour laquelle on veut vider la collection d'erreurs</param>
        private void EffacerErreurs([CallerMemberName] string propriete = "")
        {
            if (_errors.ContainsKey(propriete))
            {
                _errors[propriete].Clear();
                OnErrorsChanged(propriete);
            }
        }
    }
}
