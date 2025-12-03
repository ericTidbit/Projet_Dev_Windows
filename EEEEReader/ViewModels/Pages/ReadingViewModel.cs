using EEEEReader.Data;
using EEEEReader.Data.Models;
using HtmlAgilityPack;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EEEEReader.ViewModels.Pages
{
    public class ReadingViewModel : BaseViewModel
    {

        private LivreViewModel _currentLivreViewModel;
        private LivreViewModel _currentLivre;
        private HtmlDocument _currentHtml;
        private IDataProvider _utilisateurDataProvider;

        private string _footerText;

        public LivreViewModel currentLivreViewModel => _currentLivreViewModel;
        public LivreViewModel currentLivre => _currentLivre;

        // fonctionnalité pour changé de page
        public HtmlDocument CurrentHtml
        {
            get => _currentHtml;
            private set
            {
                if (_currentHtml != value)
                {
                    _currentHtml = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string FooterText
        {
            get => _footerText;
            private set
            {
                if (_footerText != value)
                {
                    _footerText = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ReadingViewModel(LivreViewModel livreViewModel, IDataProvider utilisateurDataProvider)
        {
            _utilisateurDataProvider = utilisateurDataProvider;

            _currentLivreViewModel = livreViewModel ?? throw new ArgumentNullException(nameof(livreViewModel));
            _currentLivre = _currentLivreViewModel
                            ?? throw new ArgumentNullException(nameof(_currentLivreViewModel));

            CurrentHtml = _currentLivre.HtmlContentList[_currentLivre.CurrentPage];
            UpdateFooter();
        }

        public void UpdateFooter()
        {
            FooterText = $"{_currentLivre.Titre} — {_currentLivre.Auteur}  |  Page {_currentLivre.CurrentPage + 1}  |  Progression : {_currentLivre.Pourcentage}%";
        }

        public void NextPage()
        {
            _currentLivreViewModel?.NextPage();
            CurrentHtml = _currentLivre.HtmlContentList[_currentLivre.CurrentPage];
            _utilisateurDataProvider.ChangerPageDuLivre(_currentLivre.Id, _currentLivre.CurrentPage);
            UpdateFooter();
        }

        public void PrevPage()
        {
            _currentLivreViewModel.PrevPage();
            CurrentHtml = _currentLivre.HtmlContentList[_currentLivre.CurrentPage];
            UpdateFooter();
        }
    }
}
