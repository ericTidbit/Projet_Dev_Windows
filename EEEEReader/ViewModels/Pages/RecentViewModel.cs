using EEEEReader.Data;
using EEEEReader.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEEEReader.ViewModels.Pages
{
    public class RecentViewModel : BaseViewModel
    {
        private readonly DataProvider _dataProvider;
        public Livre PreviewedLivre { get; set; }
        public UtilisateursViewModel? CurrentUser { get; private set; }

        public RecentViewModel(UtilisateursViewModel user)
        {
            CurrentUser = user;
            _dataProvider = App.DataProvider;
        }

        public List<LivreViewModel> GenererLivresRecent()
        {
            List<LivreViewModel> LivresVM = new();
            List<Livre> livres = _dataProvider.GetUtilisateurLivreData(CurrentUser.Id);
            foreach (Livre LivreNormal in livres)
            {
                LivreViewModel livreVM = new LivreViewModel(LivreNormal, LivreNormal.FichierEpub);
                LivresVM.Add(livreVM);
            }

            return LivresVM;
        }
    }
}
