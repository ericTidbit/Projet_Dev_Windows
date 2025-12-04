using EEEEReader.Data.Models;
using HtmlAgilityPack;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VersOne.Epub;
using VersOne.Epub.Options;
namespace EEEEReader.Data.Models

{
    public class Livre
    {
        public Livre() { }
        public int Id { get; set; }
        public byte[] FichierEpub { get; set; }
        public string Titre { get; set; }
        public string? Auteur { get; set; }
        public string? Date { get; set; }
        public string? ISBN { get; set; }
        public string? Langue { get; set; }
        public string? Resume { get; set; }
        public byte[]? CoverRaw { get; set; }
        public int CurrentPage { get; set; }
        public int Pourcentage { get; set; }
        public int UtilisateurId { get; set; }
        
        [NotMapped]
        public EpubContent RawContent { get; set; }

        [NotMapped]
        public Image CoverImage { get; set; }

        [NotMapped]
        public List<HtmlDocument> HtmlContentList { get; set; }

        public Livre(byte[] FichierEpubComplet, EpubContent RawContent, string Titre, string Auteur, string Date, string ISBN, string Langue, string Resume, byte[] CoverRaw, Image CoverImage, List<HtmlDocument> HtmlContentList, int utilisateurId)
        {
            this.FichierEpub = FichierEpubComplet;
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
            UtilisateurId = utilisateurId;
        }
        public Livre(byte[] fichierEpub, string titre, string auteur, string date,
                    string isbn, string langue, string resume, byte[] coverRaw, int utilisateurId)
        {
            FichierEpub = fichierEpub;
            Titre = titre;
            Auteur = auteur;
            Date = date;
            ISBN = isbn;
            Langue = langue;
            Resume = resume;
            CoverRaw = coverRaw;
            CurrentPage = 0;
            Pourcentage = 0;
            UtilisateurId = utilisateurId;

            // Les propriétés [NotMapped] restent null par défaut
            // Elles seront remplies plus tard si besoin (ex: lors du chargement pour lecture)
            RawContent = null;
            CoverImage = null;
            HtmlContentList = null;
        }

    }
}
