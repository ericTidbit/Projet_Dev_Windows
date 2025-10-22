using EEEEReader.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VersOne.Epub;
namespace EEEEReader.ViewModels.Pages
{
    public class biblioViewModels
    {
        public void extraireMetaData(string Path )
        {
            var livremetadata = EpubReader.ReadBook(Path);
            var dateee = livremetadata.Schema.Package.Metadata.Dates;
            var langue = livremetadata.Schema.Package.Metadata.Languages;

            /* string content, string Titre, string Auteur, string Date, string ISBN, string Langue, string Resume*/
            /*debug d*/
            if (dateee.Count != 0) { 
            App.AppReader.CurrentUser.Librairie.AjouterLivre(livremetadata.Content, livremetadata.Title, livremetadata.Author, dateee[0].Date, "667", langue[0].Language, livremetadata.Description, livremetadata.CoverImage);
            }
            else
            {
                App.AppReader.CurrentUser.Librairie.AjouterLivre(livremetadata.Content, livremetadata.Title, livremetadata.Author, null, "667", langue[0].Language, livremetadata.Description, livremetadata.CoverImage);
            }
        }

    }
}
