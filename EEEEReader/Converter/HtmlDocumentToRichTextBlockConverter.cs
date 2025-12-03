using EEEEReader.ViewModels;
using HtmlAgilityPack;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Media.Imaging;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VersOne.Epub;
using Windows.UI.Text;

namespace EEEEReader.Converter
{
    internal class HtmlDocumentToRichTextBlockConverter : IValueConverter
    {
        public EpubContent? CurrentContent { get; set; }
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is HtmlDocument rawXml)
            {
                return HtmlDocParser(rawXml);
            }
            // TODO: Fix return
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        public RichTextBlock HtmlDocParser(HtmlDocument rawXml)
        {
            RichTextBlock parsedNode = new RichTextBlock();

            List<HtmlNode> childNodes = HtmlDocumentToRichTextBlockConverter.FlattenHtmlDocument(rawXml);

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
                case "img":
                    {
                        // TODO: livres d'amazon ont des images dupliquées, ignorer les doublons (propriétés data-amznremoved-m8 et data-amznremoved)
                        SixLabors.ImageSharp.Image shImg = GetImgFromSrc(node.GetAttributeValue("src", ""));

                        // convertir ImageSharp à BitmapImage
                        Image img = new Image();
                        img.Source = ImageSharpToBitmapImage(shImg);

                        // il faut faire le container pour mettre une image dans un paragraphe
                        InlineUIContainer container = new InlineUIContainer();
                        img.Width = 300;
                        container.Child = img;

                        Paragraph para = new Paragraph();
                        para.Inlines.Add(container);

                        return para;
                    }
                case "p":
                    {
                        Paragraph para = new Paragraph();

                        List<Run> styledRuns = HtmlDocumentToRichTextBlockConverter.ApplyStyle(node);

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

                        List<Run> styledRuns = HtmlDocumentToRichTextBlockConverter.ApplyStyle(node, new List<string>() { node.Name });

                        foreach (Run styledRun in styledRuns)
                        {
                            para.Inlines.Add(styledRun);
                        }

                        return para;
                    }
                case "ol":
                case "ul":
                    {
                        Paragraph para = new Paragraph();

                        //para.Inlines.Add(new Run { Text = node.Name + node.OuterHtml });

                        foreach (HtmlNode childNode in node.ChildNodes)
                        {
                            if (childNode.Name == "li")
                            {
                                Run liElement = new Run { Text = "- " };

                                foreach (HtmlNode flatChildNode in FlattenHtmlNode(childNode))
                                {
                                    if (flatChildNode.Name == "#text")
                                    {
                                        liElement.Text += flatChildNode.InnerHtml + "\n";
                                    }
                                }

                                para.Inlines.Add(liElement);
                            }
                        }

                        return para;
                    }

                default:
                    { return null; }
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
                //change pour pas utiliser appli directement
                foreach (EpubLocalByteContentFile img in CurrentContent.Images.Local)
                {
                    if (img.FilePath.EndsWith(match.Value))
                    {
                        // TODO: Call livreViewModel awkward
                        return LivreViewModel.LoadImageFromByteArray(img.Content);
                    }
                }
            }

            return null;
        }

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

            List<HtmlNode> flatRootNode = HtmlDocumentToRichTextBlockConverter.FlattenHtmlNode(rootNode);
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

                        Run run = new Run { Text = HtmlDocumentToRichTextBlockConverter.XmlPatternReplacer(node.InnerText) };
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
 
                        // enlève les styles, pour éviter d'affecter les prochaines nodes
                        { return (null, new List<string>()); }

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
