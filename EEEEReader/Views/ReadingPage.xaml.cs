using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using EEEEReader.Data.Models;
using VersOne.Epub;
using HtmlAgilityPack;
using EEEEReader.ViewModels;
using EEEEReader.ViewModels.Pages;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EEEEReader.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class ReadingPage : Page
{
    public ReadingViewModel ReadingViewModel { get; set; }
    public ReadingPage()
    {
        InitializeComponent();
    }
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        
        if (e.Parameter is LivreViewModel livreVm)
        {
            ReadingViewModel = new ReadingViewModel(livreVm);
            DataContext = ReadingViewModel;
        }
        else
        {
            // ici tu peux loguer / debugger si besoin
        }
    }

    public void ButtonPrev_OnClick(object sender, RoutedEventArgs e)
    {
        ReadingViewModel.PrevPage();
        ReadingViewModel.currentLivreViewModel.pourcentageLivre();
    }

    public async void ButtonNext_OnClick(object sender, RoutedEventArgs e)
    {
        if (ReadingViewModel.currentLivreViewModel.IsBookFinished() == false)
        {
            ReadingViewModel.NextPage();
            ReadingViewModel.currentLivreViewModel.pourcentageLivre();
        }
        else
        {
            ReadingViewModel.currentLivreViewModel.pourcentageLivre();
            /*quand tu arrive a la fin du livre :) */
            ContentDialog dialog = new ContentDialog()
            {
                Title = "fin du livre",
                Content = "tu es arrivé a la fin du livre.",
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };
            await dialog.ShowAsync();

        }
        }


    public void ButtonBack_OnClick(object sender, RoutedEventArgs e)
    {
        // TODO: Aller a la page preview du livre au lieu de biblio
        this.Frame.Navigate(typeof(Home));
    }
}
