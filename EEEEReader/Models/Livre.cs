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
        public List<StackPanel> ParsedContent { get; set; }
        public string Titre { get; set; }
        public string Auteur { get; set; }
        public string Date { get; set; }
        public string ISBN { get; set; }
        public string Langue { get; set; }
        public string Resume{ get; set; }
        public byte[] CoverRaw { get; set; }
        public BitmapImage? CoverImage { get; set; }

        public Livre(EpubContent content, string Titre, string Auteur, string Date, string ISBN, string Langue, string Resume, byte[] cover)
        {
            this.RawContent = content;
            this.ParsedContent = LoadXamlContent(content);
            this.Titre = Titre;
            this.Auteur = Auteur;
            this.Date = Date;
            this.ISBN = ISBN;
            this.Langue = Langue;
            this.Resume = Resume;
            this.CoverRaw = cover;
            this.CoverImage = LoadCoverImage(cover);
        }


        public Livre()
        {
        }

        // code de Andrei Ashikhmin, https://stackoverflow.com/questions/42523593/convert-byte-to-windows-ui-xaml-media-imaging-bitmapimage
        // modifié
        // soit cette méthode ne marche pas, ou EpubReader est cooked
        public BitmapImage LoadCoverImage(byte[] data)
        {
            

            var bmp = new BitmapImage();

            using var stream = new InMemoryRandomAccessStream();
            stream.WriteAsync(data.AsBuffer()).AsTask().Wait(); 
            stream.Seek(0);
            bmp.SetSource(stream); 
            return bmp;
        }

        public List<StackPanel> LoadXamlContent(EpubContent rawContent)
        {
            List<StackPanel> chapterList = new List<StackPanel>();

            foreach (EpubLocalTextContentFile chapter in rawContent.Html.Local)
            {
                string chapterString = chapter.Content;
                HtmlDocument chapterHtml = new HtmlDocument();
                chapterHtml.LoadHtml(chapterString);

                HtmlNode link = chapterHtml.CreateElement("link");

                foreach (HtmlNode node in chapterHtml.DocumentNode.ChildNodes)
                {
                    RichTextBlock rtb = new RichTextBlock();
                    if (node.NodeType == HtmlNodeType.Element) {
                        // TODO
                    }
                    // Removed to lower lag
                    //Debug.WriteLine("Name: " + node.Name.ToString() + " | Type: " + node.NodeType.ToString());
                    //Debug.WriteLine("Name: " + node.Name.ToString() + " | Type: " + node.NodeType.ToString() + " | InnerText: " + node.InnerText.ToString() + " | InnerHtml: " + node.InnerHtml.ToString());

                    // TODO
                    //switch (node.Name)
                    //{
                    //}
                }
            }

            return chapterList;
        }
    }
}
