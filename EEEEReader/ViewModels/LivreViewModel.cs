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

    }
}
