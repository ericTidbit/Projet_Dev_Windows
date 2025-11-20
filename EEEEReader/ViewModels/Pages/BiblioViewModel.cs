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


        // devrais être effectuer a chaque fois lorsque l'utilisateur entre dans biblio
        

        // lors de la création d'un l'objet est crée en premier ensuite elle va se sauvegarder dans la base de
        // donc il faut dans la base donnée juste le fichier ainsi que la l'id de l'utilisateur
        
        // ensutie pour extraire les metadeta il faut utiliser cette fonction et passé a travers chacun 
        // des livre possèder par cette utlisateur
        public bool extraireMetaData(string Path)
        {
            try
            {
                byte[] epubEnByte = File.ReadAllBytes(Path);
                var livremetadata = EpubReader.ReadBook(Path);
                var dateee = livremetadata.Schema.Package.Metadata.Dates;
                var langue = livremetadata.Schema.Package.Metadata.Languages;

                /* string content, string Titre, string Auteur, string Date, string ISBN, string Langue, string Resume*/

                /* ajout ISBN a la place de 667*/
                if (dateee.Count != 0)
                {
                    Livre livre= CurrentUser.Librairie.AjouterLivre(CurrentUser.Id,epubEnByte,livremetadata.Content, livremetadata.Title, livremetadata.Author, dateee[0].Date, "667", langue[0].Language, livremetadata.Description, livremetadata.CoverImage);
                    
                
                }
                else
                {
                    Livre livre = CurrentUser.Librairie.AjouterLivre(CurrentUser.Id, epubEnByte, livremetadata.Content, livremetadata.Title, livremetadata.Author, null, "667", langue[0].Language, livremetadata.Description, livremetadata.CoverImage);
                
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

        // non actually c'est stackoverflow 
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
