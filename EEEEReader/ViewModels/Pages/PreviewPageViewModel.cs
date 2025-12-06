using EEEEReader.Data;
using EEEEReader.Data.Models;
using EEEEReader.Views.HomePages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VersOne.Epub;
using WinRT.Interop;

namespace EEEEReader.ViewModels.Pages
{
    public class PreviewPageViewModel : MainViewModel
    {
        public LivreViewModel PreviewedLivre { get; private set; }


        private IDataProviderLivre _livreDataProvider;

        public PreviewPageViewModel(IDataProviderLivre DataProvider, LivreViewModel previewedLivre)
        {
            _livreDataProvider = DataProvider;
            PreviewedLivre = previewedLivre;
        }
        public void SuprimmerLivreClick()
        { 
           _livreDataProvider.SupprimerLivre(PreviewedLivre.Livre.Id);
            
        }




    }
}
