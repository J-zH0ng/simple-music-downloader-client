using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;

namespace 项目_网络音乐播放器.Models
{

    [Serializable]
    public class Song:IComparable
    {
        public string song_name { get; set; }
        public string artist_name { get; set; }
        public string album_name { get; set; }
        public string file_path { get; set; }
        public long file_length { get; set; }

        private byte[] imagebytes;
        public BitmapImage Image
        {
            get { return ReturnPhoto(imagebytes); }
        }
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
                return new BitmapImage();
            }
        }
        public Song(string path)
        {
            LoadFromMP3File(path);
        }
        public void LoadFromMP3File(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            TagLib.File songFileInfo = TagLib.File.Create(path);
            artist_name = songFileInfo.Tag.FirstPerformer;
            song_name = songFileInfo.Tag.Title;
            album_name = songFileInfo.Tag.Album;
            if (songFileInfo.Tag.Pictures.Length >= 1)
            {
                //tag中的图片信息为byte数组，需要用函数进行转化
                imagebytes = songFileInfo.Tag.Pictures[0].Data.Data;
                //pictureBox2.Image = ReturnPhoto(bin);//转化函数
            }
            file_path = path;
            file_length = fileInfo.Length;
        }


        //private byte[] ReturnBytes(BitmapImage image)
        //{
        //    if (image != null)
        //    {
        //        MemoryStream ms = image.StreamSource as MemoryStream;
        //        return ms.ToArray();
        //    }
        //    else
        //    {
        //        return new byte[0];
        //    }
        //}
        public override string ToString()
        {
            return $"{song_name} {artist_name} {artist_name} {file_path} {file_length}";
        }

        public int CompareTo(object obj)
        {
            return song_name.CompareTo(obj);
        }

        public class SongComparator : IEqualityComparer<Song>
        {
            public bool Equals(Song x, Song y)
            {
                return x.file_path == y.file_path;
            }

            public int GetHashCode(Song obj)
            {
                return GetHashCode();
            }
        }

    }
}
