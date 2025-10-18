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
using EEEEReader.ViewModels.Pages;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EEEEReader.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class Biblio : Page
{
    public Biblio()
    {
        InitializeComponent();
   
    }
    public async void SelectionFichier(object sender, RoutedEventArgs e)
    {
        string? path = await choisirFichierUtilisateur(App.MainWindow!);
        if (path != null)
        {
            biblioViewModels extraire = new ViewModels.Pages.biblioViewModels();
            extraire.extraireMetaData(path);


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
