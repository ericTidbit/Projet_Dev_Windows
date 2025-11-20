using HtmlAgilityPack;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VersOne.Epub;


namespace EEEEReader.Data.Models
{
    public class Librairie
    {
        public ObservableCollection<Livre> Livres { get; } = new();
        public int Id { get; set; }

        public Librairie()
        {
            Livres = new ObservableCollection<Livre>();
        
        }
        // doit être dans le ViewModel pour process le epub  faudra changer un peu l'architecture mais 
        // mais ca devrais être correcte 
        public Livre AjouterLivre(int currentUserId,byte[] EpubToByte, EpubContent content, string titre, string auteur, string date, string isbn, string langue = "", string resume = "", byte[] cover = null)
        {
            Livre livre = new Livre(EpubToByte,content, titre, auteur, date, isbn, langue, resume, cover, SixLabors.ImageSharp.Image.Load<Rgba32>(cover), new List<HtmlDocument>(LoadXamlContent(content)),currentUserId );
            // temporaire en attendant l'intégration sql
            livre.Id = Livres.IndexOf(livre);
            // --
          
            Livres.Add(livre);
            // devrais changer de place mais en attendant pour que tous fonctionne important
            return livre;
        }

        public void SupprimerLivre(Livre livre)
        {
            if (Livres.Contains(livre))
            {
                Livres.Remove(livre);
            }
            else
            {
                Debug.WriteLine("il n'est pas dans la liste");
            }
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
    }
}