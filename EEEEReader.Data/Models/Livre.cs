using EEEEReader.Data.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VersOne.Epub;
using SixLabors.ImageSharp;
using VersOne.Epub.Options;
namespace EEEEReader.Data.Models

{
    public class Livre
    {
        // temporaire en attendant l'intégration sql
        // id est également l'index dans la librairie
        // pas d'id si le livre n'est pas dans une librairie
        public int? Id { get; set; }
        // --
        public EpubContent RawContent { get; set; }
        public string Titre { get; set; }
        public string? Auteur { get; set; }
        public string? Date { get; set; }
        public string? ISBN { get; set; }
        public string? Langue { get; set; }
        public string? Resume { get; set; }
        public byte[]? CoverRaw { get; set; }
        public Image CoverImage { get; set; }
        public int CurrentPage { get; set; }
        public int Pourcentage { get; set; }
        public List<HtmlDocument>? HtmlContentList;

        public Livre(EpubContent RawContent, string Titre, string Auteur, string Date, string ISBN, string Langue, string Resume, byte[] CoverRaw, Image CoverImage, List<HtmlDocument> HtmlContentList)
        {
            this.RawContent = RawContent;
            this.Titre = Titre;
            this.Auteur = Auteur;
            this.Date = Date;
            this.ISBN = ISBN;
            this.Langue = Langue;
            this.Resume = Resume;
            this.CoverRaw = CoverRaw;
            this.CoverImage = CoverImage;
            this.CurrentPage = 0;
            this.Pourcentage = 0;
            this.HtmlContentList = HtmlContentList;
        }
    }
}
