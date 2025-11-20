using ABI.System.ComponentModel;
using EEEEReader.Data.Models;
using EEEEReader.ViewModels;
using EEEEReader.ViewModels.Pages;
using EEEEReader.Views;
using EEEEReader.Views.HomePages;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Windows.Storage.Pickers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Display.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinRT.Interop;
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
        string? path = await choisirFichierUtilisateur(App.MainWindow!);
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

    public async Task<string?> choisirFichierUtilisateur(Window window)
    {
        var hwnd = WindowNative.GetWindowHandle(window);
        var winId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hwnd);
        var appWin = AppWindow.GetFromWindowId(winId);

        var picker = new FileOpenPicker(appWin.Id);
        picker.FileTypeFilter.Add(".epub");

        var file = await picker.PickSingleFileAsync();
        return file?.Path;
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
