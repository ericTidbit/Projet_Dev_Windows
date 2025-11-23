using EEEEReader.Data.Models;
using EEEEReader.ViewModels;
using EEEEReader.Views.HomePages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace EEEEReader.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PreviewPage : Page

    {
        public UtilisateursViewModel? CurrentUser { get; private set; }

        public LivreViewModel PreviewedLivre { get; private set; }

        public PreviewPage()
        {
            InitializeComponent();
        }

      
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var data = ((LivreViewModel Livre, UtilisateursViewModel User))e.Parameter;

            PreviewedLivre = data.Livre;
            CurrentUser = data.User;

        }

        public void LireLivre_Click(object sender, RoutedEventArgs e)
        {
            if (App.MainWindow?.Content is Frame mainFrame && PreviewedLivre is not null)
            {
                
               

                //App.AppReader.CurrentUser.AjouterLivreRecent(PreviewedLivre.Livre);
                mainFrame.Navigate(typeof(ReadingPage), (PreviewedLivre, User: CurrentUser));
            }
        }

        public void SuprimmerLivreClick(object sender, RoutedEventArgs e)
        {
            if (App.MainWindow?.Content is Frame mainFrame && PreviewedLivre is not null)
            {
                CurrentUser.Librairie.SupprimerLivre(PreviewedLivre.Livre);
                this.Frame?.Navigate(typeof(Biblio), CurrentUser);
            }
        }
    }
}
