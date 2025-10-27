using EEEEReader.ViewModels.Pages;
using EEEEReader.Views.HomePages;
using EEEEReader.Views;
using EEEEReader.Models;
using EEEEReader.ViewModels.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinRT.Interop;
using Microsoft.Windows.Storage.Pickers;
using Microsoft.UI.Windowing;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EEEEReader.Views.HomePages;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class Recent : Page
{
    public Livre PreviewedLivre { get; set; }
    public Recent()
    {
        InitializeComponent();
        BiblioGridView.ItemsSource = App.AppReader.CurrentUser.LivresRecent;
        this.DataContext = this;
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
        App.AppReader.CurrentLivre = (EEEEReader.Models.Livre)e.ClickedItem;
        this.Frame?.Navigate(typeof(EEEEReader.Views.PreviewPage));
    }
    
    public async void SelectionFichier(object sender, RoutedEventArgs e)
    {
        string? path = await choisirFichierUtilisateur(App.MainWindow!);
        if (path != null)
        {
            biblioViewModels extraire = new ViewModels.Pages.biblioViewModels();
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
