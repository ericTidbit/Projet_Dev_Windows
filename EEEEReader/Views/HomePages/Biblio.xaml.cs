using EEEEReader.ViewModels.Pages;
using EEEEReader.Views.HomePages;
using EEEEReader.Views;
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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinRT.Interop;
using Windows.Devices.Display.Core;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EEEEReader.Views.HomePages;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class Biblio : Page
{
    public Biblio()
    {
        InitializeComponent();
        BiblioGridView.ItemsSource = App.AppReader.CurrentUser.Librairie.Livres;
        this.DataContext = this;
        applyLayout();
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


    private void OnItemClick(object sender, ItemClickEventArgs e)
    {
        App.AppReader.CurrentLivre = (EEEEReader.Models.Livre)e.ClickedItem;
        this.Frame?.Navigate(typeof(EEEEReader.Views.PreviewPage));
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
