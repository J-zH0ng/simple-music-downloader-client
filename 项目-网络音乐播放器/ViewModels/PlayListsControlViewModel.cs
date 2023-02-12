using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace 项目_网络音乐播放器.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class PlayListsControlViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public string NewName { get; set; }
        public string NewDescription { get; set; }
        public BitmapImage NewImage { get; set; }
        public string ResultMessage { get; set; }
        public bool IsResultDialogOpen { get; set; }

        private RelayCommand<WrapPanel> setContainer;
        public RelayCommand<WrapPanel> SetContainer
        {
            get
            {
                if (setContainer == null)
                {
                    setContainer = new RelayCommand<WrapPanel>((wp) =>
                      {
                          wrapPanel = wp;
                          ResetContainer();
                      });
                }
                return setContainer;
            }
        }

        //private RelayCommand setNewImage;
        //public RelayCommand SetNewImage
        //{
        //    get
        //    {
        //        if (setNewImage == null)
        //        {
        //            setNewImage = new RelayCommand(() =>
        //            {
        //                OpenFileDialog open = new OpenFileDialog();
        //                open.Filter = "图片 | *.jpg; *.png; *.gif;*.jpeg";
        //                if (open.ShowDialog() == true)
        //                {
        //                    Uri uri = new Uri(open.FileName);
        //                    NewImage = new BitmapImage(uri);
        //                }
        //            });
        //        }
        //        return setNewImage;
        //    }
        //}
        public void ResetContainer()
        {
            wrapPanel.Children.Clear();
            string directory = Environment.CurrentDirectory + @"\Data\PlayLists";
            if(!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string[] paths = Directory.GetDirectories(Environment.CurrentDirectory + @"\Data\PlayLists");
            IEnumerable<string> names = paths.Select((path) => { string[] ss = path.Split('\\'); return ss[ss.Length - 1]; });
            foreach (var name in names)
            {
                Models.PlayList playList = new Models.PlayList(name);
                wrapPanel.Children.Add(new Views.PlayListCard()
                {
                    DataContext = new PlayListCardViewModel(playList)
                });
            }
        }


        private RelayCommand createNewPlayList;
        public RelayCommand CreateNewPlayList
        {
            get
            {
                if (createNewPlayList == null)
                {
                    createNewPlayList = new RelayCommand(() =>
                    {
                        if (NewName != null)
                        {
                            //获取Environment.CurrentDirectory\Data\PlayLists 目录下所有文件夹的名字（歌单名）
                            string[] paths = Directory.GetDirectories(Environment.CurrentDirectory + @"\Data\PlayLists");
                            IEnumerable<string> names = paths.Select((path) => { string[] ss = path.Split('\\'); return ss[ss.Length - 1]; });
                            bool isExist = false;
                            foreach (var name in names)
                            {
                                if (NewName == name)
                                {
                                    ResultMessage = TITIL_EXIST;
                                    IsResultDialogOpen = true;
                                    isExist = true;
                                    break;
                                }
                            }
                            if (!isExist)
                            {
                                Models.PlayList playList = new Models.PlayList(NewName)
                                {
                                    Image = NewImage,
                                    Description = NewDescription
                                };
                                playList.Save();
                                ResetContainer();
                            }
                        }
                    });
                }
                return createNewPlayList;
            }
        }
        private RelayCommand refresh;

        public RelayCommand Refresh
        {
            get
            {
                if (refresh == null)
                {
                    refresh = new RelayCommand(() =>
                    {
                        ResetContainer();
                    });
                }
                return refresh;
            }
        }

        private const string TITIL_EXIST = "标题已存在,换一个吧`(*>﹏<*)′";
        private WrapPanel wrapPanel;
    }
}
