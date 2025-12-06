using EEEEReader.Data;
using EEEEReader.Data.Models;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.Windows.Storage.Pickers;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using VersOne.Epub;
using Windows.ApplicationModel.Appointments.AppointmentsProvider;
using WinRT.Interop;

namespace EEEEReader.ViewModels.Pages
{
    public class BiblioViewModel : MainViewModel
    {
        private ObservableCollection<Livre> _livres;
        private ObservableCollection<LivreViewModel> _livreViewModel;

        public ObservableCollection<Livre> Livres 
        { 
            get { return _livres; } 
            set 
            { 
                _livres = value; 
                RaisePropertyChanged();
            } 
        }
        
        public ObservableCollection<LivreViewModel> LivreViewModels 
        { 
            get { return _livreViewModel; } 
            set 
            { 
                _livreViewModel = value; 
                RaisePropertyChanged();
            } 
        }
        
        public UtilisateursViewModel CurrentUser { get; set; }

        private IDataProviderLivre _livreDataProvider;

        public BiblioViewModel(IDataProviderLivre livreDataProvider)
        {
            _livreDataProvider = livreDataProvider;
            _livreViewModel = new ObservableCollection<LivreViewModel>();
        }

        public bool extraireMetaData(string Path)
        {
            byte[] epubEnByte = File.ReadAllBytes(Path);
            var livremetadata = EpubReader.ReadBook(Path);
            var dateee = livremetadata.Schema.Package.Metadata.Dates;
            var langue = livremetadata.Schema.Package.Metadata.Languages;

            /* string content, string Titre, string Auteur, string Date, string ISBN, string Langue, string Resume*/

            /* ajout ISBN a la place de 667*/
            if (dateee.Count != 0)
            {
                Livre livre =  new Livre(epubEnByte, livremetadata.Title, livremetadata.Author, dateee[0].Date, "667", langue[0].Language, livremetadata.Description, livremetadata.CoverImage, CurrentUser.Id);

                _livreDataProvider.AjouterLivreToUtilisateur(livre);
                
                LivreViewModels.Add(new LivreViewModel(livre, epubEnByte));
                
                return true;
            }
            else
            {
                Livre livre = new Livre( epubEnByte, livremetadata.Title, livremetadata.Author, null, "667", langue[0].Language, livremetadata.Description, livremetadata.CoverImage, CurrentUser.Id);
                _livreDataProvider.AjouterLivreToUtilisateur(livre);
                
                LivreViewModels.Add(new LivreViewModel(livre, epubEnByte));
                
                return true;
            }
            return false;
        }

        public void SetCurrentUser(UtilisateursViewModel utilisateursViewModel)
        {
            CurrentUser = utilisateursViewModel;
            
            var livresFromDb = _livreDataProvider.GetUtilisateurLivreData(utilisateursViewModel.Id);
            
            /*
            CurrentUser.Librairie.Livres.Clear();
            foreach (var livre in livresFromDb)
            {
                CurrentUser.Librairie.Livres.Add(livre);
            }
            
            _livres = CurrentUser.Librairie.Livres;
            */
            _livreViewModel = new ObservableCollection<LivreViewModel>();
            foreach (Livre livre in livresFromDb)
            {
                _livreViewModel.Add(new LivreViewModel(livre, livre.FichierEpub));
            }
            
            RaisePropertyChanged(nameof(Livres));
            RaisePropertyChanged(nameof(LivreViewModels));
        }

        public async Task<string?> ChoisirFichierUtilisateur(Window window)
        {
            var hwnd = WindowNative.GetWindowHandle(window);
            var winId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hwnd);
            var appWin = AppWindow.GetFromWindowId(winId);

            var picker = new FileOpenPicker(appWin.Id);
            picker.FileTypeFilter.Add(".epub");

            var file = await picker.PickSingleFileAsync();
            return file?.Path;
        }
    }
}
