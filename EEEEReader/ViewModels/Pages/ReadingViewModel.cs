using EEEEReader.Data.Models;
using HtmlAgilityPack;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EEEEReader.ViewModels.Pages
{
    public class ReadingViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

        private LivreViewModel _currentLivreViewModel;
        private Livre _currentLivre;
        private HtmlDocument _currentHtml;
        private string _footerText;

        public LivreViewModel currentLivreViewModel => _currentLivreViewModel;
        public Livre currentLivre => _currentLivre;

        public HtmlDocument CurrentHtml
        {
            get => _currentHtml;
            private set
            {
                if (_currentHtml != value)
                {
                    _currentHtml = value;
                    OnPropertyChanged();
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
                    OnPropertyChanged();
                }
            }
        }

        public ReadingViewModel(LivreViewModel livreViewModel)
        {
            _currentLivreViewModel = livreViewModel ?? throw new ArgumentNullException(nameof(livreViewModel));
            _currentLivre = _currentLivreViewModel.Livre
                            ?? throw new ArgumentNullException(nameof(_currentLivreViewModel.Livre));

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
