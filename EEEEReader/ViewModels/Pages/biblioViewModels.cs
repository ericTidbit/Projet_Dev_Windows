using System;
using System.Collections.Generic;
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
            var livre = EpubReader.ReadBook(Path);

        }

    }
}
