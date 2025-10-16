using System.ComponentModel;
using System.Runtime.CompilerServices;

public class BaseViewModel : INotifyPropertyChanged
    {
       
        public event PropertyChangedEventHandler? PropertyChanged;

        
        protected virtual void RaisePropertyChanged([CallerMemberName] string? propriete = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propriete));
        }
    }