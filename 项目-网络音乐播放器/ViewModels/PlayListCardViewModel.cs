using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using 项目_网络音乐播放器.Models;
using 项目_网络音乐播放器.Services;

namespace 项目_网络音乐播放器.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    class PlayListCardViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public PlayListCardViewModel(PlayList playList)
        {
            PlayList = playList;
        }
        public PlayList PlayList { get; set; }

        private const string SONG_LOAD_ERROR = "歌曲失效，可能歌曲文件位置发生变化，已从歌单中清除";
        private ListBox listBox;

        private RelayCommand<ListBox> setListBox;
        public RelayCommand<ListBox> SetListBox 
        {
            get
            {
                if (setListBox == null)
                {
                    setListBox = new RelayCommand<ListBox>((listbox) =>
                    {
                        listBox = listbox;
                    });
                }
                return setListBox;
            }
        }

        private LocalMusicService musicService = new LocalMusicService();
        private RelayCommand play;
        public RelayCommand Play
        {
            get
            {
                if (play == null)
                {
                    play = new RelayCommand(() =>
                      {
                          if(musicService.Songs != PlayList.Songs)
                          {
                              musicService.Songs = PlayList.Songs;
                          }
                          if (listBox.SelectedIndex != -1)
                          {
                              int index = listBox.SelectedIndex;
                              bool IsSucceed = musicService.PrepareAndPlay(index);
                              if (!IsSucceed)
                              {
                                  PlayList.Songs.RemoveAt(index);
                                  PlayList.Save();
                                  listBox.ItemsSource = null;
                                  listBox.ItemsSource = PlayList.Songs;
                                  if (musicService.Songs == PlayList.Songs)
                                      musicService.Songs = PlayList.Songs;
                                  DialogMessage = SONG_LOAD_ERROR;
                                  IsDialogOpen = true;
                              }
                          }
                      });
                }
                return play;
            }
        }
        public bool IsDialogOpen { get; set; }
        public string DialogMessage { get; set; }

        //private RelayCommand setPlayListImage;
        //public RelayCommand SetPlayListImage
        //{
        //    get
        //    {
        //        if (setPlayListImage == null)
        //        {
        //            setPlayListImage = new RelayCommand(() =>
        //            {
        //                OpenFileDialog open = new OpenFileDialog();
        //                open.Filter = "图片 | *.jpg; *.png; *.gif;*.jpeg";
        //                if (open.ShowDialog() == true)
        //                {
        //                    Uri uri = new Uri(open.FileName);
        //                    PlayList.Image = new BitmapImage(uri);
        //                    PlayList.Save();
        //                }
        //            });
        //        }
        //        return setPlayListImage;
        //    }
        //}

        private RelayCommand savePlayListChange;

        public RelayCommand SavePlayListChange
        {
            get
            {
                if (savePlayListChange == null)
                {
                    savePlayListChange = new RelayCommand(() =>
                    {
                        PlayList.Save();
                    });
                }
                return savePlayListChange;
            }
        }

        private RelayCommand<Grid> deletePlayList;
        public RelayCommand<Grid> DeletePlayList
        {
            get
            {
                if (deletePlayList == null)
                {
                    deletePlayList = new RelayCommand<Grid>((grid) =>
                    {
                        grid.Visibility = System.Windows.Visibility.Collapsed;
                        PlayList.Delete();
                    });
                }
                return deletePlayList;
            }
        }

        private RelayCommand<ListBoxItem> deleteSong;
        public RelayCommand<ListBoxItem> DeleteSong
        {
            get
            {
                if (deleteSong == null)
                {
                    deleteSong = new RelayCommand<ListBoxItem>((listboxitem) =>
                    {
                        listboxitem.IsSelected = true;
                        int index = listBox.SelectedIndex;
                        if (index != -1)
                        {
                            PlayList.Songs.RemoveAt(index);
                            PlayList.Save();
                            listBox.ItemsSource = null;
                            listBox.ItemsSource = PlayList.Songs;
                            if(musicService.Songs == PlayList.Songs)
                                musicService.Songs = PlayList.Songs;
                        }
                    });
                }
                return deleteSong;
            }
        }

        private RelayCommand addSong;
        public RelayCommand AddSong
        {
            get
            {
                if (addSong == null)
                {
                    addSong = new RelayCommand(() =>
                    {
                        OpenFileDialog open = new OpenFileDialog();
                        open.InitialDirectory = Environment.CurrentDirectory + @"\Songs";
                        open.Filter = "音乐 | *.mp3";
                        if (open.ShowDialog() == true)
                        {
                            Song song = new Song(open.FileName);
                            if(!PlayList.Songs.Contains(song,new Song.SongComparator()))
                            {
                                PlayList.Songs.Add(new Song(open.FileName));
                                PlayList.Save();
                                listBox.ItemsSource = null;
                                listBox.ItemsSource = PlayList.Songs;
                                if (musicService.Songs == PlayList.Songs)
                                    musicService.Songs = PlayList.Songs;
                            }
                        }
                    });
                }
                return addSong;
            }
        }

    }
}
