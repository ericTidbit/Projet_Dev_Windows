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
    RecentViewModel ViewModel { get; set; }    

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        if (e.Parameter is UtilisateursViewModel user)
        {
            ViewModel = new RecentViewModel(user);
            Charger();
        }
    }

    public Recent()
    {
        InitializeComponent();
        var dbContext = new EEEEReaderDbContext();

        // doit être une liste de livre <list>Livre
        //BiblioGridView.ItemsSource = App.AppReader.CurrentUser.LivresRecent;
    }
   
    public void Charger()
    {
        BiblioGridView.ItemsSource = ViewModel.GenererLivresRecent();
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
        var selectedLivre = e.ClickedItem as LivreViewModel;
        var navigationTuple = (Livre: selectedLivre, CurrentUser: ViewModel.CurrentUser);
        this.Frame?.Navigate(typeof(EEEEReader.Views.PreviewPage), navigationTuple);
    }
    
}
