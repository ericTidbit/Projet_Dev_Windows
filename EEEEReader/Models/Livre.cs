using EEEEReader.Models;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VersOne.Epub.Options;
using Windows.Storage.Streams;

namespace EEEEReader.Models
{
    public class Livre
    {
        public string Content { get; set; }
        public string Titre { get; set; }
        public string Auteur { get; set; }
        public string Date { get; set; }
        public string ISBN { get; set; }
        public string Langue { get; set; }
        public string Resume{ get; set; }
        public byte[] CoverRaw { get; set; }
        public BitmapImage? CoverImage { get; set; }


        public Livre(string content, string Titre, string Auteur, string Date, string ISBN, string Langue, string Resume, byte[] cover)
        {
            this.Content = content;
            this.Titre = Titre;
            this.Auteur = Auteur;
            this.Date = Date;
            this.ISBN = ISBN;
            this.Langue = Langue;
            this.Resume = Resume;
            this.CoverRaw = cover;
            this.LoadCoverImage(cover);
        }


        public Livre()
        {
        }

        // code de Andrei Ashikhmin, https://stackoverflow.com/questions/42523593/convert-byte-to-windows-ui-xaml-media-imaging-bitmapimage
        // modifié
        // soit cette méthode ne marche pas, ou EpubReader est cooked
        public async void LoadCoverImage(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                this.CoverImage = null;
            }

            BitmapImage bitmapImage = new BitmapImage();

            using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
            {
                using (DataWriter writer = new DataWriter(stream))
                {
                    writer.WriteBytes(data);
                    await writer.StoreAsync();
                    await writer.FlushAsync();
                    writer.DetachStream();
                }

                stream.Seek(0);
                await bitmapImage.SetSourceAsync(stream);
            }

            this.CoverImage = bitmapImage;
        }
    }
}
