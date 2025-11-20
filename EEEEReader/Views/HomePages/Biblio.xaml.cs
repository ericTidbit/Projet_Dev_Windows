using EEEEReader.Data.Models;
using EEEEReader.ViewModels;
using EEEEReader.ViewModels.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Specialized;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EEEEReader.Views.HomePages;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class Biblio : Page
{
    public BiblioViewModel ViewModel { get; set; }
    public Biblio()
    {
        InitializeComponent();
        // doesnt load the good thing it should load The dataProvider 
        // Wrap chaque livre avec un LivreViewModel pour être compatible avec le layout

        //this._livres = App.AppReader.CurrentUser.Librairie.Livres;

        // prof
        this.DataContext = this;
        ViewModel = new BiblioViewModel();
    }
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        if (e.Parameter is UtilisateursViewModel user)
        {
            ViewModel.SetCurrentUser(user);
            BiblioGridView.ItemsSource = ViewModel.LivreViewModels;
            ViewModel.Livres.CollectionChanged += Livres_CollectionChanged;
            applyLayout();
        }
    }


    // de la prof
    private void Livres_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            if (e.NewItems != null)
            {
                foreach (Livre livre in e.NewItems)
                {
                    ViewModel.LivreViewModels.Add(new LivreViewModel(livre));
                }
            }
        }
    }

    public async void SelectionFichier(object sender, RoutedEventArgs e)
    {
        string? path = await ViewModel.ChoisirFichierUtilisateur(App.MainWindow!);
        if (path != null && ViewModel.CurrentUser != null)
        {
            var extraire = new ViewModels.Pages.BiblioViewModel
            {
                CurrentUser = ViewModel.CurrentUser
            };

            bool result = extraire.extraireMetaData(path);
            if (!result)
            {
                ContentDialog dialog = new ContentDialog()
                {
                    Title = "Erreur",
                    Content = "format du livre pas bon erreur d'extraction.",
                    CloseButtonText = "OK",
                    XamlRoot = this.Content.XamlRoot
                };
                await dialog.ShowAsync();
            }
        }
    }



    private void OnItemClick(object sender, ItemClickEventArgs e)
    {
        // faut changer ca pour que ca soit pas dans le Appli directement
        //App.AppReader.CurrentLivreViewModel = (EEEEReader.ViewModels.LivreViewModel)e.ClickedItem;
        var livreVM = (EEEEReader.ViewModels.LivreViewModel)e.ClickedItem;
        this.Frame?.Navigate(typeof(EEEEReader.Views.PreviewPage), (Livre: livreVM, User: ViewModel.CurrentUser));
    }

    private void changeLayout(object sender, RoutedEventArgs e)
    {
        
        if (layoutBtn.Content is FontIcon icon)
        {
            
            if (App.AppReader.IsGridLayout)
            {
                // Switch to list layout
                icon.Glyph = "\uE8BA";
                BiblioGridView.ItemTemplate = (DataTemplate)this.Resources["ListItemTemplate"];
                BiblioGridView.ItemsPanel = (ItemsPanelTemplate)this.Resources["ListLayoutPanel"];
                App.AppReader.IsGridLayout = false;
            }
            else
            {
                // Switch to grid layout
                icon.Glyph = "\uE8FD";
                BiblioGridView.ItemTemplate = (DataTemplate)this.Resources["GridItemTemplate"];
                BiblioGridView.ItemsPanel = (ItemsPanelTemplate)this.Resources["GridLayoutPanel"];
                App.AppReader.IsGridLayout = true;
            }
        }   
    }
    private void applyLayout()
    {
        if (App.AppReader.IsGridLayout)
        {
            BiblioGridView.ItemTemplate = (DataTemplate)this.Resources["GridItemTemplate"];
            BiblioGridView.ItemsPanel = (ItemsPanelTemplate)this.Resources["GridLayoutPanel"];
        }
        else
        {
            BiblioGridView.ItemTemplate = (DataTemplate)this.Resources["ListItemTemplate"];
            BiblioGridView.ItemsPanel = (ItemsPanelTemplate)this.Resources["ListLayoutPanel"];
        }
    }
}
