using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using 项目_网络音乐播放器.Services;

namespace 项目_网络音乐播放器.Models
{
    class PlayList
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public BitmapImage Image { get; set; }
        public List<Song> Songs { get; set; }
        public void Save()
        {
            string directory = Environment.CurrentDirectory + @"\Data\PlayLists\" + Name+@"\";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            //if(Image!=null)
            //    localDataService.SaveSingle(directory + IMAGE, ReturnBytes(Image));
            if (!String.IsNullOrEmpty(Description))
            {
                localDataService.SaveSingle(directory + DESCRIPTION, Description);
            }
            else
            {
                if (File.Exists(directory + DESCRIPTION))
                    File.Delete(directory + DESCRIPTION);
            }
            if (Songs.Count > 0)
            {
                localDataService.SaveMultiple(directory + SONGS, Songs);
            }
            else
            {
                if (File.Exists(directory + SONGS))
                    File.Delete(directory + SONGS);
            }
                
        }
        public PlayList(string name)
        {
            Name = name;
            Songs = new List<Song>();
            Load();
        }

        public void Load()
        {
            string directory = Environment.CurrentDirectory + @"\Data\PlayLists\" + Name + @"\";
            localDataService.LoadMultiple(directory + SONGS, Songs);
            //byte[] streamBytes = localDataService.LoadSingle<byte[]>(directory + IMAGE);
            //BitmapImage image = ReturnPhoto(streamBytes);
            if (/*image == null && */Songs.Count > 0)
            {
                Image = Songs[0].Image;
            }
            else
            {
                Image = new BitmapImage();
            }
            Description = localDataService.LoadSingle<string>(directory + DESCRIPTION);
        }

        public void Delete()
        {
            string directory = Environment.CurrentDirectory + @"\Data\PlayLists\" + Name + @"\";
            Directory.Delete(directory, true);
        }
        private const string IMAGE = "Image.bin";
        private const string DESCRIPTION = "Description.bin";
        private const string SONGS = "Songs.bin";
        private LocalDataService localDataService = new LocalDataService();

        private BitmapImage ReturnPhoto(byte[] streamByte)
        {
            if (streamByte != null)
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream(streamByte);
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.StreamSource = ms;
                img.EndInit();
                return img;
            }
            else
            {
                return null;
            }

        }

        //private byte[] ReturnBytes(BitmapImage image)
        //{
        //    byte[] ByteArray = null;

        //    try
        //    {
        //        Stream stream = image.StreamSource;
        //        if (stream != null && stream.Length > 0)
        //        {
        //            stream.Position = 0;
        //            using (BinaryReader br = new BinaryReader(stream))
        //            {
        //                ByteArray = br.ReadBytes((int)stream.Length);
        //            }
        //        }
        //    }
        //    catch(Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //        return null;
        //    }

        //    return ByteArray;
        //}
    }
}
