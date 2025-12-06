using EEEEReader.Data;
using EEEEReader.Data.data;
using EEEEReader.Data.Models;
using EEEEReader.ViewModels;
using EEEEReader.ViewModels.Pages;
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
        private readonly IDataProviderLivre _livreDataProvider;
        public UtilisateursViewModel? CurrentUser { get; private set; }

        public LivreViewModel PreviewedLivre { get; private set; }

        public PreviewPage()
        {
            InitializeComponent();
            _livreDataProvider = App.DataProviderLivre;
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
                mainFrame.Navigate(typeof(ReadingPage), (PreviewedLivre, User: CurrentUser));
            }
        }

        public void SuprimmerLivreClick(object sender, RoutedEventArgs e)
        {
            if (App.MainWindow?.Content is Frame mainFrame && PreviewedLivre is not null)
            {
                _livreDataProvider.SupprimerLivre(PreviewedLivre.Livre.Id);
                this.Frame?.Navigate(typeof(Biblio), CurrentUser);
            }
        }
    }
}
