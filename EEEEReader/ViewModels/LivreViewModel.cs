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
        public LivreViewModel(byte[] FichierEpub)
        {
            using var ms = new MemoryStream(FichierEpub);
            livremetadata = EpubReader.ReadBook(ms);

        }
        /*
        public LivreViewModel(EpubContent content, string Titre, string Auteur = null, string Date = null, string ISBN = null, string Langue = null, string Resume = null, byte[] cover = null)
        {
            _livre.
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
        */
        public Livre Livre 
        { 
            get => _livre; 
            set => _livre = value;
        }

        //Livre livre = CurrentUser.Librairie.AjouterLivre(CurrentUser.Id, epubEnByte, livremetadata.Content, livremetadata.Title, livremetadata.Author, dateee[0].Date, "667", langue[0].Language, livremetadata.Description, livremetadata.CoverImage);

        public string Titre => livremetadata.Title;
        public string Auteur => livremetadata.Author;
        public string Resume => livremetadata.Description ?? "Aucun résumé disponible.";
        // cover image n'existe pas encore c'est pour ca que ca crée un bug il faut le crée a pratir d'ici
        public BitmapImage CoverImage => ImageSharpToBitmapImage(LoadImageFromByteArray(livremetadata.CoverImage));
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
