using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using 项目_网络音乐播放器.ViewModels;

namespace 项目_网络音乐播放器.Views
{
    /// <summary>
    /// Shop.xaml 的交互逻辑
    /// </summary>
    public partial class Shop : UserControl
    {
        public MusicService.song_view[] Songs { get; set; }
        public Services.RemoteDataService RemoteDataService { get; set; }
        public Shop()
        {
            InitializeComponent();
            RemoteDataService = new Services.RemoteDataService();
            Songs = RemoteDataService.GetSongs();
            if (Songs != null)
            {
                string path = Environment.CurrentDirectory + @"\Songs\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string[] filenames = Directory.GetFiles(path, "*.mp3", SearchOption.AllDirectories);
                int[] ids = new int[filenames.Length];
                for (int i = 0; i < filenames.Length; i++)
                {
                    string realName = filenames[i].Substring(0, filenames[i].Length - 4);
                    string[] ss = realName.Split('-');
                    ids[i] = int.Parse(ss[ss.Length - 1]);
                }
                foreach (var SongView in Songs)
                {
                    if (!IsExist(SongView.song_id, ids))
                    {
                        container.Children.Add(new ShopDisplayCard()
                        {
                            DataContext = new ShopDiaplayCardViewModel(
                                SongView.song_id,
                                SongView.song_name,
                                SongView.artist_name,
                                SongView.album_name,
                                ReturnPhoto(SongView.album_image))
                        });
                    }

                    //Console.WriteLine($"{SongView.song_name} {SongView.song_id} {SongView.album_image.Length}");
                }
            }

        }
        private BitmapImage ReturnPhoto(byte[] streamByte)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream(streamByte);
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.StreamSource = ms;
            img.EndInit();
            return img;
        }

        private bool IsExist(int id, int[] ids)
        {
            for (int i = 0; i < ids.Length; i++)
            {
                if (id == ids[i])
                    return true;
            }
            return false;
        }
    }
}
