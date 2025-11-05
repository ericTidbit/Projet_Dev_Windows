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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EEEEReader.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class ReadingPage : Page
{
    private LivreViewModel _currentLivreViewModel { get; set; }
    private Livre _currentLivre { get; set; }
    public string FooterText { get; set; }
    public ReadingPage()
    {
        InitializeComponent();
        _currentLivreViewModel = App.AppReader.CurrentLivreViewModel;
        //LoadContent(_currentLivreViewModel.HtmlContentList[_currentLivreViewModel.CurrentPage]);
        LoadEpubContent(_currentLivreViewModel.HtmlContentList[_currentLivre.CurrentPage]);
        UpdateFooter();
    }

    public void LoadContent(HtmlDocument docToLoad)
    {
        ContentPanel.Children.Clear();

        TextBlock textBlock = new TextBlock
        {
            Text = docToLoad.Text,
            TextWrapping = TextWrapping.Wrap,
            Margin = new Thickness(0, 0, 0, 20)
        };

        ContentPanel.Children.Add(textBlock);
    }

    public void LoadEpubContent(HtmlDocument docToLoad)
    {
        ContentPanel.Children.Clear();
        ContentPanel.Children.Add(_currentLivreViewModel.HtmlDocParser(docToLoad));
    }

    public void UpdateFooter()
    {
        // TODO: Ajouter le chapitre
        // N'UPDATE PAS EN TEMPS RÉEL, À IMPLÉMENTER DANS VIEWMODEL
        FooterText = $"{_currentLivre.Titre} — {_currentLivre.Auteur}  |  Page {_currentLivre.CurrentPage + 1}  |  Progression : {_currentLivre.Pourcentage}%";
        this.Bindings.Update();
    }

    public void ButtonPrev_OnClick(object sender, RoutedEventArgs e)
    {
        //LoadContent(_currentLivre.HtmlContentList[_currentLivre.PrevPage()]);
        LoadEpubContent(_currentLivreViewModel.HtmlContentList[_currentLivreViewModel.PrevPage()]);
        _currentLivreViewModel.pourcentageLivre();
        UpdateFooter();
    }

    public async void ButtonNext_OnClick(object sender, RoutedEventArgs e)
    {
        if (_currentLivreViewModel.IsBookFinished() == false)
        {
            //LoadContent(_currentLivre.HtmlContentList[_currentLivre.NextPage()]);
            LoadEpubContent(_currentLivreViewModel.HtmlContentList[_currentLivreViewModel.NextPage()]);
            _currentLivreViewModel.pourcentageLivre();
            UpdateFooter();
        }
        else
        {
            _currentLivreViewModel.pourcentageLivre();
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
