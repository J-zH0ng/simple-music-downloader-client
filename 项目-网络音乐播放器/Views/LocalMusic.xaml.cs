using System.Windows.Controls;
using 项目_网络音乐播放器.ViewModels;

namespace 项目_网络音乐播放器.Views
{
    /// <summary>
    /// LocalMusic.xaml 的交互逻辑
    /// </summary>
    public partial class LocalMusic : UserControl
    {
        public LocalMusic()
        {
            InitializeComponent();
            DataContext = new LocalMusicViewModel();
        }
    }
}
