using MaterialDesignThemes.Wpf;
using System.Windows;
using 项目_网络音乐播放器.ViewModels;

namespace 项目_网络音乐播放器
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly PaletteHelper _paletteHelper = new PaletteHelper();
        public bool IsDark { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            SetWindowSize();
            playArea.DataContext = new PlayAreaViewModel();
        }

        private void SetWindowSize()
        {
            Rect rc = SystemParameters.WorkArea;//获取工作区大小
            this.Width = rc.Width;
            this.Height = rc.Height;
        }

        private void Goodbye_DialogHost_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            if (Equals(eventArgs.Parameter, true))
            {
                this.Close();
            }
        }

        private void Login_DialogHost_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {

        }

        private void Signin_DialogHost_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {

        }

        private void Logout_DialogHost_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {

        }

        private void ChangeModeIcon(object sender, RoutedEventArgs e)
        {
            if (modeIcon.Kind == PackIconKind.Repeat)
            {
                modeIcon.Kind = PackIconKind.RepeatOne;
                modeBtn.ToolTip = "单曲循环";
            }
            else if (modeIcon.Kind == PackIconKind.RepeatOne)
            {
                modeIcon.Kind = PackIconKind.RepeatVariant;
                modeBtn.ToolTip = "随机播放";
            }
            else
            {
                modeIcon.Kind = PackIconKind.Repeat;
                modeBtn.ToolTip = "顺序播放";
            }
        }

        private void ChangePlayIcon(object sender, RoutedEventArgs e)
        {
            if (playIcon.Kind == PackIconKind.Play)
            {
                playIcon.Kind = PackIconKind.Pause;
                playBtn.ToolTip = "暂停";
            }
            else
            {
                playIcon.Kind = PackIconKind.Play;
                playBtn.ToolTip = "开始";
            }
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsDark)
            {
                ApplyBase(!IsDark);
                dayOrNight.Text = "day";
                IsDark = false;
            }
            else
            {
                ApplyBase(!IsDark);
                dayOrNight.Text = "night";
                IsDark = true;
            }
        }

        private void ApplyBase(bool isDark)
        {
            ITheme theme = _paletteHelper.GetTheme();
            IBaseTheme baseTheme = isDark ? new MaterialDesignDarkTheme() : (IBaseTheme)new MaterialDesignLightTheme();
            theme.SetBaseTheme(baseTheme);
            _paletteHelper.SetTheme(theme);
        }

        //private void SelectBgImg_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog open = new OpenFileDialog();
        //    open.Filter = "图片 | *.jpg; *.png; *.gif;*.jpeg";
        //    if (open.ShowDialog() == true)
        //    {
        //        Uri uri = new Uri(open.FileName);
        //        background.Background = new ImageBrush() { ImageSource = new BitmapImage(uri) };
        //    }
        //}

    }
}
