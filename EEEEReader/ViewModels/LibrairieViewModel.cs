using EEEEReader.Data.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VersOne.Epub;

namespace EEEEReader.ViewModels
{
    public class LibrairieViewModel : BaseViewModel
    {
        private Librairie _librairie;

        public LibrairieViewModel()
        {
            _librairie = new Librairie();
        }


    }
}
