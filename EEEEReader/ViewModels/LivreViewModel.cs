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
// faire property 



namespace EEEEReader.ViewModels
{
    public class LivreViewModel : BaseViewModel
    {
        private Livre _livre;
        VersOne.Epub.EpubBook livremetadata;

        public LivreViewModel(Livre livre)
        {
            _livre = livre;
        }
        public LivreViewModel(Livre livre,byte[] FichierEpub)
        {
            using var ms = new MemoryStream(FichierEpub);
            livremetadata = EpubReader.ReadBook(ms);
            _livre = livre;
        }
        public Livre Livre 
        { 
            get => _livre; 
            set => _livre = value;
        }

        //Livre livre = CurrentUser.Librairie.AjouterLivre(CurrentUser.Id, epubEnByte, livremetadata.Content, livremetadata.Title, livremetadata.Author, dateee[0].Date, "667", langue[0].Language, livremetadata.Description, livremetadata.CoverImage);
        public int Id => _livre.Id;
        public string Titre => livremetadata.Title;
        public string Auteur => livremetadata.Author;
        public string Resume => livremetadata.Description ?? "Aucun résumé disponible.";
        // cover image n'existe pas encore c'est pour ca que ca crée un bug il faut le crée a pratir d'ici
        public BitmapImage CoverImage => ImageSharpToBitmapImage(LoadImageFromByteArray(livremetadata.CoverImage));
        // il ne passe pas HtmlContent parce qu'il faut le faire dans le Viewmodel
        public List<HtmlDocument> HtmlContentList => LoadXamlContent(livremetadata.Content);
        public EpubContent RawContent => livremetadata.Content;

        public int CurrentPage
        {
            get => _livre.CurrentPage;
            set
            {
                if (_livre.CurrentPage != value)
                {
                    _livre.CurrentPage = value;
                    RaisePropertyChanged(nameof(CurrentPage));
                }
            }
        } 

        
        // TODO: est une méthode pour compatibilité avec ancien code -- à corriger plus tard
        public static SixLabors.ImageSharp.Image LoadImageFromByteArray(byte[] data)
        {
            return SixLabors.ImageSharp.Image.Load<Rgba32>(data);
        }
        public static List<HtmlDocument> LoadXamlContent(EpubContent rawContent)
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
        public int NextPage()
        {
            if (_livre.CurrentPage < HtmlContentList.Count - 1)
            {
                CurrentPage++;
            }

            return _livre.CurrentPage;
        }
        public int PrevPage()
        {
            if (_livre.CurrentPage > 0)
            {
                CurrentPage--;
            }
            

            return _livre.CurrentPage;
        }

        public bool IsBookFinished()
        {
            return _livre.CurrentPage >= HtmlContentList.Count - 1;
        }

        // TODO: remove duplicate function in HtmlDocumentToRichTextBlockConverter
        public static BitmapImage ImageSharpToBitmapImage(SixLabors.ImageSharp.Image img)
        {
            using MemoryStream ms = new MemoryStream();
            img.Save(ms, new PngEncoder());
            ms.Seek(0, SeekOrigin.Begin);

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.SetSource(ms.AsRandomAccessStream());

            return bitmapImage;
        }
        
        public bool extraireMetaDataDuLivre(byte[] FichierEpub)
        {
            using var ms = new MemoryStream(FichierEpub);
            var livremetadata = EpubReader.ReadBook(ms);
            var dateee = livremetadata.Schema.Package.Metadata.Dates;
            var langue = livremetadata.Schema.Package.Metadata.Languages;

            /* string content, string Titre, string Auteur, string Date, string ISBN, string Langue, string Resume*/

            /* ajout ISBN a la place de 667*/
            /*
            if (dateee.Count != 0)
            {
                Livre livre = CurrentUser.Librairie.AjouterLivre(CurrentUser.Id, epubEnByte, livremetadata.Content, livremetadata.Title, livremetadata.Author, dateee[0].Date, "667", langue[0].Language, livremetadata.Description, livremetadata.CoverImage);

                
                return true;
            }
            else
            {
                Livre livre = CurrentUser.Librairie.AjouterLivre(CurrentUser.Id, epubEnByte, livremetadata.Content, livremetadata.Title, livremetadata.Author, null, "667", langue[0].Language, livremetadata.Description, livremetadata.CoverImage);
                            }
            */
            return false;
        }
    
    }
}
