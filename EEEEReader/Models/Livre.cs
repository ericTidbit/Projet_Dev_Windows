using EEEEReader.Models;
using HtmlAgilityPack;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VersOne.Epub;
using VersOne.Epub.Options;
using Windows.Storage.Streams;
using Windows.UI.Text;
namespace EEEEReader.Models

// dependance a changer d'endroit :)
{
    public class Livre
    {
        // temporaire en attendant l'intégration sql
        // id est également l'index dans la librairie
        // pas d'id si le livre n'est pas dans une librairie
        public int? Id { get; set; }
        // --
        public EpubContent RawContent { get; set; }
        public List<HtmlDocument> HtmlContentList { get; set; }
        public string Titre { get; set; }
        public string? Auteur { get; set; }
        public string? Date { get; set; }
        public string? ISBN { get; set; }
        public string? Langue { get; set; }
        public string? Resume { get; set; }
        public byte[]? CoverRaw { get; set; }
        public BitmapImage? CoverImage { get; set; }
        public int CurrentPage { get; set; }
        public int Pourcentage { get; set; }

        public Livre(EpubContent content, string Titre, string Auteur = null, string Date = null, string ISBN = null, string Langue = null, string Resume = null, byte[] cover = null)
        {
            this.RawContent = content;
            this.HtmlContentList = LoadXamlContent(content);
            this.Titre = Titre;
            this.Auteur = Auteur;
            this.Date = Date;
            this.ISBN = ISBN;
            this.Langue = Langue;
            this.Resume = Resume;
            this.CoverRaw = cover;
            this.CoverImage = cover != null ? LoadImageFromByteArray(cover) : null;


            this.CoverImage = LoadImageFromByteArray(cover);


            this.CurrentPage = 0;
            this.Pourcentage = 0;
        }


        public Livre()
        {
        }

        // code de Andrei Ashikhmin, https://stackoverflow.com/questions/42523593/convert-byte-to-windows-ui-xaml-media-imaging-bitmapimage
        // modifié
        // soit cette méthode ne marche pas, ou EpubReader est cooked
        
        // mettre dans viewModel
        public static BitmapImage LoadImageFromByteArray(byte[] data)
        {
            if (data == null)
            {
                var bmp = new BitmapImage(new Uri("ms-appx:///Assets/Wide310x150Logo.scale-200.png"));
                return bmp;
            }

            try
            {
                var bmp = new BitmapImage();

                using var stream = new InMemoryRandomAccessStream();
                stream.WriteAsync(data.AsBuffer()).AsTask().GetAwaiter().GetResult();
                stream.Seek(0);
                bmp.SetSource(stream);
                Debug.WriteLine("Cover image loaded successfully.");
                return bmp;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LoadCoverImage failed: {ex}");
                return null;
            }
        }

        public List<HtmlDocument> LoadXamlContent(EpubContent rawContent)
        {
            List<HtmlDocument> chapterList = new List<HtmlDocument>();

            foreach (EpubLocalTextContentFile chapter in rawContent.Html.Local)
            {
                string chapterString = chapter.Content;
                HtmlDocument chapterHtml = new HtmlDocument();
                chapterHtml.LoadHtml(chapterString);

                chapterList.Add(chapterHtml);
            }

            return chapterList;
        }
        public void pourcentageLivre()
        {
            this.Pourcentage = ((this.CurrentPage) * 100) / (this.HtmlContentList.Count - 1);



        }
        public int NextPage()
        {
            if (this.CurrentPage < this.HtmlContentList.Count - 1)
            {
                this.CurrentPage++;
            }

            return this.CurrentPage;
        }
        public int PrevPage()
        {
            if (this.CurrentPage > 0)
            {
                this.CurrentPage--;
            }


            return this.CurrentPage;
        }

        public bool IsBookFinished()
        {
            return this.CurrentPage >= this.HtmlContentList.Count - 1;
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
