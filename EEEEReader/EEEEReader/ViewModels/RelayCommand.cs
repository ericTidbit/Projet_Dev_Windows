using System;
using System.Windows.Input;

namespace EEEEReader.ViewModels
{
    /// <summary>
    /// Impl�mentation de ICommand pour le pattern MVVM
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _executer;
        private readonly Func<object?, bool>? _peutExecuter;

        /// <summary>
        /// Cr�e une nouvelle commande
        /// </summary>
        /// <param name="executer">Action � ex�cuter</param>
        /// <param name="peutExecuter">Fonction d�terminant si la commande peut �tre ex�cut�e</param>
        public RelayCommand(Action<object?> executer, Func<object?, bool>? peutExecuter = null)
        {
            _executer = executer ?? throw new ArgumentNullException(nameof(executer));
            _peutExecuter = peutExecuter;
        }

        /// <summary>
        /// D�termine si la commande peut �tre ex�cut�e
        /// </summary>
        public bool CanExecute(object? parametre) 
            => _peutExecuter == null || _peutExecuter(parametre);

        /// <summary>
        /// Ex�cute la commande
        /// </summary>
        public void Execute(object? parametre) 
            => _executer(parametre);

        /// <summary>
        /// �v�nement d�clench� quand CanExecute change
        /// </summary>
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// L�ve l'�v�nement CanExecuteChanged pour forcer la r��valuation de CanExecute
        /// </summary>
        public void LeverChangementPeutExecuter()
            => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
