using EEEEReader.Models;
using HtmlAgilityPack;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using VersOne.Epub;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EEEEReader.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class ReadingPage : Page
{
    public Livre _currentLivre { get; set; }
    public string FooterText { get; set; }
    public ReadingPage()
    {
        InitializeComponent();
        _currentLivre = App.AppReader.CurrentLivre;
        //LoadContent(_currentLivre.HtmlContentList[_currentLivre.CurrentPage]);
        LoadEpubContent(_currentLivre.HtmlContentList[_currentLivre.CurrentPage]);
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
        ContentPanel.Children.Add(HtmlDocParserr(_currentLivre, docToLoad));
    }
    // ContentPanel logic 
    public RichTextBlock HtmlDocParserr(Livre livre,HtmlDocument rawXml)
    {
        RichTextBlock parsedNode = new RichTextBlock();

        List<HtmlNode> childNodes = Livre.FlattenHtmlDocument(rawXml);

        foreach (HtmlNode node in childNodes)
        {
            Paragraph? parsedNodeParagraph = ParserXmlSwitch(livre,node);

            if (parsedNodeParagraph != null)
            {
                parsedNode.Blocks.Add(parsedNodeParagraph);
            }
        }
        return parsedNode;
    }
        public Paragraph? ParserXmlSwitch(Livre livre,HtmlNode node)
    {
        switch (node.Name)
        {
            // TODO: h1, h2, h3, h4, h5, h6
            // ignorés
            case "#comment":
            case "span":
            case "div":
            case "meta":
            case "style":
            case "body":
            case "head":
            case "html":
            case "section":
            // traités dans p
            case "#text":
            case "em":
            case "strong":
                { return null; }

            case "img":
                {
                    // TODO: livres d'amazon ont des images dupliquées, ignorer les doublons (propriétés data-amznremoved-m8 et data-amznremoved)
                    Image img = new Image();
                    img.Source = GetImgFromSrc(livre, node.GetAttributeValue("src", ""));
                    // TODO: taille dynamique
                    img.Width = 500;

                    // il faut faire le container pour mettre une image dans un paragraphe
                    InlineUIContainer container = new InlineUIContainer();
                    container.Child = img;

                    Paragraph para = new Paragraph();
                    para.Inlines.Add(container);

                    return para;
                }
            case "p":
                {
                    Paragraph para = new Paragraph();

                    List<Run> styledRuns = Livre.ApplyStyle(node);

                    foreach (Run styledRun in styledRuns)
                    {
                        para.Inlines.Add(styledRun);
                    }

                    return para;
                }
            case "h1":
            case "h2":
            case "h3":
            case "h4":
            case "h5":
            case "h6":
                {
                    Paragraph para = new Paragraph();

                    List<Run> styledRuns = Livre.ApplyStyle(node, new List<string>() { node.Name });

                    foreach (Run styledRun in styledRuns)
                    {
                        para.Inlines.Add(styledRun);
                    }

                    return para;
                }
            default:
                {
                    // Pour debug
                    Paragraph para = new Paragraph();

                    para.Inlines.Add(new Run { Text = "Unsupported node in ParserXmlSwitch -- Node type : " + node.Name });

                    return para;
                }
        }
    }

    public BitmapImage? GetImgFromSrc(Livre livre ,string src)
    {
        Match match = Regex.Match(src, @"[^\/]+\.\w\S*");

        if (match.Success)
        {
            foreach (EpubLocalByteContentFile img in livre.RawContent.Images.Local)
            {
                if (img.FilePath.EndsWith(match.Value))
                {
                    return Livre.LoadImageFromByteArray(img.Content);
                }
            }
        }

        return null;

    }


    // fin  content Panel Logic

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
        LoadEpubContent(_currentLivre.HtmlContentList[_currentLivre.PrevPage()]);
        _currentLivre.pourcentageLivre();
        UpdateFooter();
    }

    public async void ButtonNext_OnClick(object sender, RoutedEventArgs e)
    {
        if (_currentLivre.IsBookFinished() == false)
        {
            //LoadContent(_currentLivre.HtmlContentList[_currentLivre.NextPage()]);
            LoadEpubContent(_currentLivre.HtmlContentList[_currentLivre.NextPage()]);
            _currentLivre.pourcentageLivre();
            UpdateFooter();
        }
        else
        {
            _currentLivre.pourcentageLivre();
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
