using EEEEReader.Data;
using EEEEReader.Data.Models;
using EEEEReader.ViewModels;
using EEEEReader.ViewModels.Pages;
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
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Windows.Storage.Pickers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinRT.Interop;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EEEEReader.Views.HomePages;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class Recent : Page
{
    
    private readonly DataProvider _dataProvider;

    public Livre PreviewedLivre { get; set; }
    public UtilisateursViewModel? CurrentUser { get; private set; }
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        if (e.Parameter is UtilisateursViewModel user)
        {
            CurrentUser = user;
            ChargerLivreRecent();
        }
    }

    public Recent()
    {
        InitializeComponent();
        var dbContext = new EEEEReaderDbContext();
        _dataProvider = new DataProvider(dbContext);
        

        // doit être une liste de livre <list>Livre
        //BiblioGridView.ItemsSource = App.AppReader.CurrentUser.LivresRecent;
    }
    public void ChargerLivreRecent()
    {
        List<Livre> livres = _dataProvider.GetUtilisateurLivreData(CurrentUser.Id);
        BiblioGridView.ItemsSource = livres;
    }
   



        public void LireLivre_Click(object sender, RoutedEventArgs e)
    {
        if (App.MainWindow?.Content is Frame mainFrame)
        {
            mainFrame.Navigate(typeof(ReadingPage));
        }
    }
    private void OnItemClick(object sender, ItemClickEventArgs e)
    {
        this.Frame?.Navigate(typeof(EEEEReader.Views.PreviewPage));
    }


    


    // recent ne devrait pas selectionner des chose 
    public async void SelectionFichier(object sender, RoutedEventArgs e)
    {
        string? path = await choisirFichierUtilisateur(App.MainWindow!);
        if (path != null)
        {
            BiblioViewModel extraire = new ViewModels.Pages.BiblioViewModel(_dataProvider);
            bool result = extraire.extraireMetaData(path);
            if (result == false)
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
    
    
}
