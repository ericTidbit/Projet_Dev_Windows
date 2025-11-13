using EEEEReader.Data.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEEEReader.ViewModels.Pages
{
    public class ReadingViewModel
    {
        private LivreViewModel _currentLivreViewModel { get; set; }
        private Livre _currentLivre { get; set; }
        private HtmlDocument _currentHtml {  get; set; }
        public string FooterText { get; set; }

        public LivreViewModel currentLivreViewModel => _currentLivreViewModel;
        public Livre currentLivre => _currentLivre;
        public HtmlDocument CurrentHtml => _currentHtml;

        public ReadingViewModel()
        {
            _currentLivreViewModel = App.AppReader.CurrentLivreViewModel;
            _currentLivre = _currentLivreViewModel.Livre;
            _currentHtml = _currentLivre.HtmlContentList[_currentLivre.CurrentPage];
            //LoadContent(_currentLivreViewModel.HtmlContentList[_currentLivreViewModel.CurrentPage]);
        }

        public void UpdateFooter()
        {
            FooterText = $"{_currentLivre.Titre} — {_currentLivre.Auteur}  |  Page {_currentLivre.CurrentPage + 1}  |  Progression : {_currentLivre.Pourcentage}%";
        }

        public void NextPage()
        {
            _currentLivreViewModel?.NextPage();
            _currentHtml = _currentLivre.HtmlContentList[_currentLivre.CurrentPage];
        }

        public void PrevPage()
        {
            _currentLivreViewModel.PrevPage();
            _currentHtml = _currentLivre.HtmlContentList[_currentLivre.CurrentPage];
        }
    }
}
