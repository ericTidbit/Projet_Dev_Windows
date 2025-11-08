using EEEEReader.Models;
using HtmlAgilityPack;
using Microsoft.UI.Text;
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
using Windows.UI.Text;

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

                    List<Run> styledRuns = ApplyStyle(node);

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

                    List<Run> styledRuns = ApplyStyle(node, new List<string>() { node.Name });

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
        Match match = Regex.Match(src, @"[^\\/]+\.\w\S*");

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

    // style
    // partiellement généré par copilot
    public static List<HtmlNode> FlattenHtmlDocument(HtmlDocument MainDocument)
    {
        List<HtmlNode> flatList = new List<HtmlNode>();
        void Traverse(HtmlNode node)
        {
            flatList.Add(node);
            if (node.HasChildNodes)
            {
                foreach (HtmlNode child in node.ChildNodes)
                {
                    Traverse(child);
                }
            }
        }

        foreach (HtmlNode parentNode in MainDocument.DocumentNode.ChildNodes)
        {
            Traverse(parentNode);
        }

        return flatList;
    }
    public static List<HtmlNode> FlattenHtmlNode(HtmlNode rootNode)
    {
        List<HtmlNode> flatList = new List<HtmlNode>();
        void Traverse(HtmlNode node)
        {
            flatList.Add(node);
            if (node.HasChildNodes)
            {
                foreach (HtmlNode child in node.ChildNodes)
                {
                    Traverse(child);
                }
            }
        }

        Traverse(rootNode);

        return flatList;
    }



    public static List<Run> ApplyStyle(HtmlNode rootNode, List<string> startFlags = null)
    {
        List<Run> outputRuns = new List<Run>();

        List<HtmlNode> flatRootNode = Livre.FlattenHtmlNode(rootNode);
        List<string> nextStyleFlags = new List<string>();

        if (startFlags != null)
        {
            nextStyleFlags.AddRange(startFlags);
        }

        foreach (HtmlNode node in flatRootNode)
        {
            (Run run, List<string> updatedStyleFlags) = ApplyStyleSwitch(node, nextStyleFlags);
            nextStyleFlags = updatedStyleFlags;
            if (run != null)
            {
                outputRuns.Add(run);
            }
        }

        return outputRuns;

    }


    public static (Run, List<string>) ApplyStyleSwitch(HtmlNode node, List<string> styleFlags)
    {
        // TODO: traiter paragraphes avec des styles imbriqués (ex. <strong> du <em> italique </em> en gras </strong>)
        switch (node.Name)
        {
            // n'enlève pas les styles, car est inline
            case "span":
            case "sup":
                { return (null, styleFlags); }
            // enlève les styles, pour éviter d'affecter les prochaines nodes
            case "p":
            case "div":
                { return (null, new List<string>()); }
            case "#text":
                {

                    Run run = new Run { Text = Livre.XmlPatternReplacer(node.InnerText) };
                    if (styleFlags.Contains("em"))
                    {
                        run.FontStyle = Windows.UI.Text.FontStyle.Italic;
                    }
                    if (styleFlags.Contains("strong"))
                    {
                        run.FontWeight = FontWeights.Bold;
                    }
                    // ne peut pas avoir plusieurs headings en même temps
                    if (styleFlags.Contains("h1"))
                    {
                        run.FontSize = 32;
                        run.FontWeight = FontWeights.Bold;
                    }
                    else if (styleFlags.Contains("h2"))
                    {
                        run.FontSize = 28;
                        run.FontWeight = FontWeights.Bold;
                    }
                    else if (styleFlags.Contains("h3"))
                    {
                        run.FontSize = 24;
                        run.FontWeight = FontWeights.Bold;
                    }
                    else if (styleFlags.Contains("h4"))
                    {
                        run.FontSize = 20;
                        run.FontWeight = FontWeights.Bold;
                    }
                    else if (styleFlags.Contains("h5"))
                    {
                        run.FontSize = 16;
                        run.FontWeight = FontWeights.Bold;
                    }
                    else if (styleFlags.Contains("h6"))
                    {
                        run.FontSize = 14;
                        run.FontWeight = FontWeights.Bold;
                    }

                    // note: n'enlève pas les flags, les flags sont enlevés par les éléments en block (pas inline)
                    return (run, styleFlags);
                }
            case "em":
                {
                    styleFlags.Add("em");
                    return (null, styleFlags);
                }
            case "strong":
                {
                    styleFlags.Add("strong");
                    return (null, styleFlags);
                }
            case "h1":
            case "h2":
            case "h3":
            case "h4":
            case "h5":
            case "h6":
                {
                    styleFlags.Add(node.Name);
                    return (null, styleFlags);
                }
            default:
                {
                    // enlève les styles, pour éviter d'affecter les prochaines nodes
                    styleFlags.Clear();
                    return (new Run { Text = "Unsupported node in ApplyStyle -- Node Type : " + node.Name }, styleFlags);
                }

        }
    }

    public static string XmlPatternReplacer(string input)
    {
        string output = input;
        Dictionary<string, string> patternMap = new Dictionary<string, string>();

        // TODO: plus de patterns
        // https://www.ascii-code.com/
        // format (pattern, remplacement)
        patternMap.Add(@"&amp;", "&");
        patternMap.Add(@"&#160;", "");
        // ne marche pas rn
        patternMap.Add(@"<\/?\w*>", "");

        foreach (string pattern in patternMap.Keys)
        {
            output = Regex.Replace(output, pattern, patternMap[pattern]);
        }

        return output;
    }


    // style 





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
