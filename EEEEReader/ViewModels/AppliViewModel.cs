using EEEEReader.Data.Models;
using EEEEReader.Views.HomePages;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEEEReader.ViewModels
{
    public class AppliViewModel : BaseViewModel
    {
        private Appli _appli;
        // ici car utilise UI

        public Utilisateur? CurrentUser;

        public AppliViewModel()
        {
            _appli = new Appli();
            CurrentUser = _appli.CurrentUser;
        }
    }
}
