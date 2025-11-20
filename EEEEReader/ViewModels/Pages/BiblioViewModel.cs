using EEEEReader.Data.Models;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.Windows.Storage.Pickers;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using VersOne.Epub;
using WinRT.Interop;
namespace EEEEReader.ViewModels.Pages
{
    public class BiblioViewModel : MainViewModel
    {
        private ObservableCollection<Livre> _livres;
        private ObservableCollection<LivreViewModel> _livreViewModel;


        public ObservableCollection<Livre> Livres { get { return _livres; } set { _livres = value; } }
        public ObservableCollection<LivreViewModel> LivreViewModels { get { return _livreViewModel; } set { _livreViewModel = value; } }
        public UtilisateursViewModel CurrentUser { get; set; }


        /*retourn vrai si le livre est dans un bon format et non si le livre peux pas etre extre */
        public bool extraireMetaData(string Path)
        {
            try
            {
                var livremetadata = EpubReader.ReadBook(Path);
                var dateee = livremetadata.Schema.Package.Metadata.Dates;
                var langue = livremetadata.Schema.Package.Metadata.Languages;

                /* string content, string Titre, string Auteur, string Date, string ISBN, string Langue, string Resume*/

                /* ajout ISBN a la place de 667*/
                if (dateee.Count != 0)
                {
                    CurrentUser.Librairie.AjouterLivre(livremetadata.Content, livremetadata.Title, livremetadata.Author, dateee[0].Date, "667", langue[0].Language, livremetadata.Description, livremetadata.CoverImage);
                }
                else
                {
                    CurrentUser.Librairie.AjouterLivre(livremetadata.Content, livremetadata.Title, livremetadata.Author, null, "667", langue[0].Language, livremetadata.Description, livremetadata.CoverImage);
                }
                return true;
            }
            catch
            {
                Debug.WriteLine("c'est INotifyPropertyChangedpas bon ton affaire la ");
                return false;
            }
        }

        public void SetCurrentUser(UtilisateursViewModel utilisateursViewModel)
        {
            CurrentUser = utilisateursViewModel;
            _livres = CurrentUser.Librairie.Livres;

            _livreViewModel = new ObservableCollection<LivreViewModel>();
            foreach (Livre livre in _livres)
            {
                _livreViewModel.Add(new LivreViewModel(livre));
            }
        }

        // 100% c'est chatgpt qui a fait cette fonction
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
