using EEEEReader.Models;
using HtmlAgilityPack;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using VersOne.Epub;
using VersOne.Epub.Options;
using Windows.Storage.Streams;
namespace EEEEReader.Models
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
        public string? Resume{ get; set; }
        public byte[]? CoverRaw { get; set; }
        public BitmapImage? CoverImage { get; set; }
        public int CurrentPage { get; set; }

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
            this.CoverImage = cover != null ? LoadCoverImage(cover) : null;

            
            this.CoverImage = LoadCoverImage(cover);
            

            this.CurrentPage = 0;
        }


        public Livre()
        {
        }

        // code de Andrei Ashikhmin, https://stackoverflow.com/questions/42523593/convert-byte-to-windows-ui-xaml-media-imaging-bitmapimage
        // modifié
        // soit cette méthode ne marche pas, ou EpubReader est cooked
        public BitmapImage LoadCoverImage(byte[] data)
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
        // ici pour bug de depassé nombre de page
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

        public static StackPanel HtmlDocParser(HtmlDocument rawXml)
        {
            StackPanel returnStackPanel = new StackPanel();

            List<HtmlNode> childNodes = Livre.FlattenNestedHtmlNode(rawXml);

            foreach (HtmlNode node in childNodes)
            {
                returnStackPanel.Children.Add(ParserXmlSwitch(node));
            }

            return returnStackPanel;

        }

        public static StackPanel? ParserXmlSwitch(HtmlNode node)
        {
            switch(node.Name)
            {
                case "p":
                    {
                        StackPanel returnStackPanel = new StackPanel();
                        returnStackPanel.Children.Add(new TextBlock { Text = node.InnerText});
                        return returnStackPanel;

                    }
                default:
                    {
                        StackPanel returnStackPanel = new StackPanel();
                        if (node.HasChildNodes)
                        {
                            returnStackPanel.Children.Add(new TextBlock { Text = "Unsupported node: " + node.Name + " | InnerText: Child nodes present" });

                        }
                        else
                        {
                            returnStackPanel.Children.Add(new TextBlock { Text = "Unsupported node: " + node.Name + " | InnerText: " + node.InnerText });
                        }
                        return returnStackPanel;
                    }
            }
        }

        // généré par copilot wth
        public static List<HtmlNode> FlattenNestedHtmlNode(HtmlDocument MainDocument)
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

    }
}
