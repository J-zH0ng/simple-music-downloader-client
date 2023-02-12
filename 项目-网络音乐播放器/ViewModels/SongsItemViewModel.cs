using GalaSoft.MvvmLight;
using PropertyChanged;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using 项目_网络音乐播放器.Models;

namespace 项目_网络音乐播放器.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class SongsItemViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public Song song { get; set; }
        public BitmapImage Image { get; set; }
        public bool IsChecked { get; set; }
    }
}
