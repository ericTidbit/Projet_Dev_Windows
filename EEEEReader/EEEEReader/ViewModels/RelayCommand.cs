using System;
using System.Windows.Input;

namespace EEEEReader.ViewModels
{
    /// <summary>
    /// Implémentation de ICommand pour le pattern MVVM
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _executer;
        private readonly Func<object?, bool>? _peutExecuter;

        /// <summary>
        /// Crée une nouvelle commande
        /// </summary>
        /// <param name="executer">Action à exécuter</param>
        /// <param name="peutExecuter">Fonction déterminant si la commande peut être exécutée</param>
        public RelayCommand(Action<object?> executer, Func<object?, bool>? peutExecuter = null)
        {
            _executer = executer ?? throw new ArgumentNullException(nameof(executer));
            _peutExecuter = peutExecuter;
        }

        /// <summary>
        /// Détermine si la commande peut être exécutée
        /// </summary>
        public bool CanExecute(object? parametre) 
            => _peutExecuter == null || _peutExecuter(parametre);

        /// <summary>
        /// Exécute la commande
        /// </summary>
        public void Execute(object? parametre) 
            => _executer(parametre);

        /// <summary>
        /// Événement déclenché quand CanExecute change
        /// </summary>
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// Lève l'événement CanExecuteChanged pour forcer la réévaluation de CanExecute
        /// </summary>
        public void LeverChangementPeutExecuter()
            => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
