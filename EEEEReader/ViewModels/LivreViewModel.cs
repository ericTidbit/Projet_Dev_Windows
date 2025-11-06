using EEEEReader.Data.Models;
using HtmlAgilityPack;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VersOne.Epub;
using Windows.Storage.Streams;
using Windows.UI.Text;
using Microsoft.UI.Xaml.Media.Imaging;
using SixLabors;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Microsoft.UI.Xaml.Media.Animation;
using System.IO;
using SixLabors.ImageSharp.Formats.Png;

namespace EEEEReader.ViewModels
{
    public class LivreViewModel : BaseViewModel
    {
        private Livre _livre;

        public LivreViewModel(Livre livre)
        {
            _livre = livre;
        }
        public LivreViewModel(EpubContent content, string Titre, string Auteur = null, string Date = null, string ISBN = null, string Langue = null, string Resume = null, byte[] cover = null)
        {
            _livre.RawContent = content;
            _livre.Titre = Titre;
            _livre.Auteur = Auteur;
            _livre.Date = Date;
            _livre.ISBN = ISBN;
            _livre.Langue = Langue;
            _livre.Resume = Resume;
            _livre.CoverRaw = cover;
            _livre.CoverImage = LoadImageFromByteArray(cover);
            _livre.CurrentPage = 0;
            _livre.Pourcentage = 0;
            _livre.HtmlContentList = Librairie.LoadXamlContent(content);
        }

        public Livre Livre 
        { 
            get => _livre; 
            set => _livre = value;
        }
        public string Titre => _livre.Titre;
        public string Auteur => _livre.Auteur;
        public string Resume => _livre.Resume ?? "Aucun résumé disponible.";
        public BitmapImage CoverImage => ImageSharpToBitmapImage(_livre.CoverImage);
        public List<HtmlDocument> HtmlContentList => _livre.HtmlContentList;
        public int CurrentPage => _livre.CurrentPage;
        public int Pourcentage => _livre.Pourcentage;


        // TODO: est une méthode pour compatibilité avec ancien code -- à corriger plus tard
        public static SixLabors.ImageSharp.Image LoadImageFromByteArray(byte[] data)
        {
            return SixLabors.ImageSharp.Image.Load<Rgba32>(data);
        }

        public void pourcentageLivre()
        {
            _livre.Pourcentage = ((_livre.CurrentPage) * 100) / (_livre.HtmlContentList.Count - 1);



        }
        public int NextPage()
        {
            if (_livre.CurrentPage < _livre.HtmlContentList.Count - 1)
            {
                _livre.CurrentPage++;
            }

            return _livre.CurrentPage;
        }
        public int PrevPage()
        {
            if (_livre.CurrentPage > 0)
            {
                _livre.CurrentPage--;
            }


            return _livre.CurrentPage;
        }

        public bool IsBookFinished()
        {
            return _livre.CurrentPage >= _livre.HtmlContentList.Count - 1;
        }

        public RichTextBlock HtmlDocParser(HtmlDocument rawXml)
        {
            RichTextBlock parsedNode = new RichTextBlock();

            List<HtmlNode> childNodes = LivreViewModel.FlattenHtmlDocument(rawXml);

            foreach (HtmlNode node in childNodes)
            {
                Paragraph? parsedNodeParagraph = ParserXmlSwitch(node);

                if (parsedNodeParagraph != null)
                {
                    parsedNode.Blocks.Add(parsedNodeParagraph);
                }
            }

            // debug, cause beaucoup de temps de chargement
            /*
            Paragraph debugPara = new Paragraph();
            debugPara.Inlines.Add(new Run { Text = "\n--------- RAW XML ----------\n" + rawXml.Text });
            Run flatXmlRun = new Run { Text = "\n--------- FLATTENED NODES ----------\n" };
            foreach (HtmlNode node in childNodes)
            {
                flatXmlRun.Text += node.OuterHtml + "\n";
            }
            debugPara.Inlines.Add(flatXmlRun);
            parsedNode.Blocks.Add(debugPara);
            */
            // --

            return parsedNode;

        }

        // TODO: extrêmement inefficace, à améliorer
        public Paragraph? ParserXmlSwitch(HtmlNode node)
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
                        SixLabors.ImageSharp.Image shImg = GetImgFromSrc(node.GetAttributeValue("src", ""));
                        // TODO: taille dynamique
                        int Width = 500;
                        int height = (int)(shImg.Height * (500.0 / shImg.Width));

                        shImg.Mutate(x => x.Resize(Width, height));

                        // convertir ImageSharp à BitmapImage
                        Image img = new Image();
                        img.Source = ImageSharpToBitmapImage(shImg);

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

                        List<Run> styledRuns = LivreViewModel.ApplyStyle(node);

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

                        List<Run> styledRuns = LivreViewModel.ApplyStyle(node, new List<string>() { node.Name });

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

        public SixLabors.ImageSharp.Image? GetImgFromSrc(string src)
        {
            Match match = Regex.Match(src, @"[^\/]+\.\w\S*");
            if (match.Success)
            {
                foreach (EpubLocalByteContentFile img in _livre.RawContent.Images.Local)
                {
                    if (img.FilePath.EndsWith(match.Value))
                    {
                        return LivreViewModel.LoadImageFromByteArray(img.Content);
                    }
                }
            }

            return null;

        }

        // à moitié gpt
        public static BitmapImage ImageSharpToBitmapImage(SixLabors.ImageSharp.Image img)
        {
            using MemoryStream ms = new MemoryStream();
            img.Save(ms, new PngEncoder());
            ms.Seek(0, SeekOrigin.Begin);

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.SetSource(ms.AsRandomAccessStream());

            return bitmapImage;
        }

        public static List<Run> ApplyStyle(HtmlNode rootNode, List<string> startFlags = null)
        {
            List<Run> outputRuns = new List<Run>();

            List<HtmlNode> flatRootNode = LivreViewModel.FlattenHtmlNode(rootNode);
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

                        Run run = new Run { Text = LivreViewModel.XmlPatternReplacer(node.InnerText) };
                        if (styleFlags.Contains("em"))
                        {
                            run.FontStyle = FontStyle.Italic;
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
    }
}
