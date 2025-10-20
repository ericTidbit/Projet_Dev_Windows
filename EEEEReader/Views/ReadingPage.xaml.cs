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
using EEEEReader.Models;
using VersOne.Epub;
using HtmlAgilityPack;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EEEEReader.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class ReadingPage : Page
{
    public Livre _currentLivre { get; set; }
    public ReadingPage()
    {
        InitializeComponent();
        _currentLivre = App.AppReader.CurrentLivre;
        LoadContent();
    }

    public void LoadContent()
    {
        foreach (EpubLocalTextContentFile contentPage in _currentLivre.RawContent.Html.Local)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(contentPage.Content);

            TextBlock textBlock = new TextBlock
            {
                Text = doc.Text,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 20)
            };
            ContentPanel.Children.Add(textBlock);
        }
    }
}
