using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MaterialDesignThemes.Wpf;
using PropertyChanged;
using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace 项目_网络音乐播放器.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    class ShopDiaplayCardViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public ShopDiaplayCardViewModel(int id, string title, string artist, string album, BitmapImage bitmapImage)
        {
            Id = id;
            Title = title;
            Artist = artist;
            Album = album;
            MyImage = bitmapImage;
            RemoteDataService = new Services.RemoteDataService();
            DownloadEnable = true;
            DownloadIcon = PackIconKind.Download;
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public BitmapImage MyImage { get; set; }

        public string Message { get; set; }
        public PackIconKind DialogIcon { get; set; }
        public bool Open { get; set; }

        public bool DownloadEnable { get; set; }
        public PackIconKind DownloadIcon { get; set; }

        public Services.RemoteDataService RemoteDataService { get; set; }

        private RelayCommand download;

        public RelayCommand Download
        {
            get
            {
                if (download == null)
                {
                    download = new RelayCommand(() => downloadMethod(Id, Title));
                }
                return download;
            }
        }

        private async void downloadMethod(int id, string title)
        {
            bool isSuccess = await RemoteDataService.DownLoadAsync(id, title);
            if (isSuccess)
            {
                Message = "下载成功";
                DialogIcon = PackIconKind.Check;
                Open = true;
                DownloadIcon = PackIconKind.Check;
                DownloadEnable = false;
            }
            else
            {
                Message = "下载失败";
                DialogIcon = PackIconKind.EmoticonConfused;
                Open = true;
            }
        }
    }
}
