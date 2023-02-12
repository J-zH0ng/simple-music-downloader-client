using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using PropertyChanged;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using 项目_网络音乐播放器.Models;
using 项目_网络音乐播放器.Services;

namespace 项目_网络音乐播放器.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public ThemeSetting ThemeSetting { get; set; }
        public BitmapImage Image { get; set; }
        public string UserName { get; set; }
        public SnackbarMessageQueue MyMessageQueue { get; set; }
        public bool IsLogin { get; set; }

        private const string SIGNIN_SUCCESS = "注册成功ヾ(≧▽≦*)o";
        private const string LOGIN_ERROR = "登录失败`(*>﹏<*)′";
        private const string LOGIN_SUCCESS = "登录成功ヾ(≧▽≦*)o";
        private const string SIGNIN_ERROR = "注册失败，换一个用户名试试<(＿　＿)>";
        private const string NETWORK_ERROR = "网络好像出了点问题╮(╯-╰)╭";
        private MyUserService userService = new MyUserService();
        private RelayCommand<PasswordBox> login;
        public RelayCommand<PasswordBox> Login
        {
            get
            {
                if (login == null)
                {
                    login = new RelayCommand<PasswordBox>((pb) =>
                    { 
                        bool issucceed = false;
                        try
                        {
                            issucceed = userService.Login(UserName, pb.Password);
                        }
                        catch (Exception)
                        {
                            MyMessageQueue.Enqueue(NETWORK_ERROR);
                            UserName = "未登录";
                            return;
                        }
                        
                        if (issucceed)
                        {
                            IsLogin = true;
                            MyMessageQueue.Enqueue(LOGIN_SUCCESS);
                        }
                        else
                        {
                            MyMessageQueue.Enqueue(LOGIN_ERROR);
                            UserName = "未登录";
                        }
                    });
                }
                return login;
            }
        }
        private RelayCommand<PasswordBox> signIn;

        public RelayCommand<PasswordBox> SignIn
        {
            get
            {
                if (signIn == null)
                {
                    signIn = new RelayCommand<PasswordBox>((pb) =>
                    {
                        bool issucceed = false;
                        try
                        {
                            issucceed = userService.SignIn(UserName, pb.Password);
                        }
                        catch (Exception)
                        {
                            MyMessageQueue.Enqueue(NETWORK_ERROR);
                            return;
                        }
                        if (issucceed)
                        {
                            MyMessageQueue.Enqueue(SIGNIN_SUCCESS);
                            UserName = "未登录";
                        }
                        else
                        {
                            MyMessageQueue.Enqueue(SIGNIN_ERROR);
                            UserName = "未登录";
                        }
                    });
                }
                return signIn;
            }
        }
        private RelayCommand logOut;

        public RelayCommand LogOut
        {
            get
            {
                if (logOut == null)
                {
                    logOut = new RelayCommand(() =>
                    {
                        userService.Logout();
                        IsLogin = false;
                        UserName = "未登录";
                    });
                }
                return logOut;
            }
        }


        private RelayCommand setBackgroundImg;
        public RelayCommand SetBackgroundImg
        {
            get
            {
                if (setBackgroundImg == null)
                {
                    setBackgroundImg = new RelayCommand(() => selectImg());
                }
                return setBackgroundImg;
            }
        }
        private void selectImg()
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "图片 | *.jpg; *.png; *.gif;*.jpeg";
            if (open.ShowDialog() == true)
            {
                ThemeSetting.ImgPath = open.FileName;
                string path = Environment.CurrentDirectory + @"\Data\ThemeSetting.bin";
                localDataService.SaveSingle<ThemeSetting>(path, ThemeSetting);//选择图片的同时直接保存设置了
                Uri uri = new Uri(ThemeSetting.ImgPath);
                Image = new BitmapImage(uri);
                //background.Background = new ImageBrush() { ImageSource = new BitmapImage(uri) };
            }
        }

        private RelayCommand saveThemeSetting;
        public RelayCommand SaveThemeSetting
        {
            get
            {
                if (saveThemeSetting == null)
                {
                    saveThemeSetting = new RelayCommand(() =>
                    {
                        string path = Environment.CurrentDirectory + @"\Data\ThemeSetting.bin";
                        localDataService.SaveSingle<ThemeSetting>(path, ThemeSetting);
                    });
                }
                return saveThemeSetting;
            }
        }

        public MainWindowViewModel()
        {
            MyMessageQueue = new SnackbarMessageQueue();
            ThemeSetting = new ThemeSetting();
            string path = Environment.CurrentDirectory + @"\Data\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            
            if (File.Exists(path + "ThemeSetting.bin"))
            {
                ThemeSetting = localDataService.LoadSingle<ThemeSetting>(path + "ThemeSetting.bin");
                Uri uri = new Uri(ThemeSetting.ImgPath);
                Image = new BitmapImage(uri);
            }
            if (File.Exists(path + "User.bin"))
            {
                User user = localDataService.LoadSingle<User>(path + "User.bin");
                UserName = user.Name;
                IsLogin = true;
            }
            else
            {
                UserName = "未登录";
            }
            
        }
        private Services.LocalDataService localDataService = new Services.LocalDataService();
    }

    [Serializable]
    public class ThemeSetting
    {
        public string ImgPath { get; set; }
        public bool IsDark { get; set; }
    }
}
