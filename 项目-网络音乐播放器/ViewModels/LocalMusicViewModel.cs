using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using 项目_网络音乐播放器.Models;
using 项目_网络音乐播放器.Services;

namespace 项目_网络音乐播放器.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class LocalMusicViewModel : ViewModelBase, INotifyPropertyChanged
    {        
        private List<Song> songs;
        public List<SongsItemViewModel> SongItems { get; set; }

        private DataGrid datagrid;
        private RelayCommand<DataGrid> setDataGrid;
        public RelayCommand<DataGrid> SetDataGrid
        {
            get
            {
                if (setDataGrid == null)
                {
                    setDataGrid = new RelayCommand<DataGrid>((mydatagrid) =>
                    {
                        datagrid = mydatagrid;
                    });
                }
                return setDataGrid;
            }
        }

        private LocalMusicService musicService;
        private RelayCommand play;
        public RelayCommand Play
        {
            get
            {
                if (play == null)
                {
                    play = new RelayCommand(() =>
                    {
                        if (musicService.Songs != songs)
                        {
                            musicService.Songs = songs;
                        }
                        if (datagrid.SelectedIndex != -1)
                        {
                            musicService.PrepareAndPlay(datagrid.SelectedIndex);
                        }
                    });
                }
                return play;
            }
        }

        private RelayCommand deleteSongs;
        public RelayCommand DeleteSongs
        {
            get
            {
                if (deleteSongs == null)
                {
                    deleteSongs = new RelayCommand(() =>
                    {
                        for (int i = 0; i < SongItems.Count; i++)
                        {
                            SongsItemViewModel item = SongItems[i];
                            if (item.IsChecked)
                            {
                                if(File.Exists(item.song.file_path))
                                    File.Delete(item.song.file_path);
                                SongItems.Remove(item);
                                datagrid.ItemsSource = null;
                                datagrid.ItemsSource = SongItems;
                            }
                        }
                    });
                }
                return deleteSongs;
            }
        }
        private ListBox listBox;
        private RelayCommand<ListBox> getListBox;

        public RelayCommand<ListBox> GetListBox
        {
            get
            {
                if (getListBox == null)
                {
                    getListBox = new RelayCommand<ListBox>((lb) =>
                    {
                        listBox = lb;
                    });
                }
                return getListBox;
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
                        GetPlayListNames();
                        listBox.ItemsSource = null;
                        listBox.ItemsSource = PlayListNames;
                        songs = GetSongs();
                        GetSongItems();
                        datagrid.ItemsSource = null;
                        datagrid.ItemsSource = SongItems;
                    });
                }
                return refresh;
            }
        }

        private RelayCommand openDirectory;
        public RelayCommand OpenDirectory
        {
            get
            {
                if (openDirectory == null)
                {
                    openDirectory = new RelayCommand(() =>
                    {
                        string path = Environment.CurrentDirectory + @"\Songs";
                        System.Diagnostics.Process.Start(path);
                    });
                }
                return openDirectory;
            }
        }
        private string[] playListNames;
        public string[] PlayListNames
        {
            get
            {
                GetPlayListNames();
                return playListNames;
            }
        }

        private void GetPlayListNames()
        {
            string[] paths = Directory.GetDirectories(Environment.CurrentDirectory + @"\Data\PlayLists");
            IEnumerable<string> names = paths.Select((path) => { string[] ss = path.Split('\\'); return ss[ss.Length - 1]; });
            playListNames = names.ToArray();
        }

        private RelayCommand<ListBox> addSong;
        public RelayCommand<ListBox> AddSong
        {
            get
            {
                if (addSong == null)
                {
                    addSong = new RelayCommand<ListBox>((lb) =>
                    {
                        int index = lb.SelectedIndex;
                        if (index != -1)
                        {
                            PlayList playList = new PlayList(PlayListNames[index]);
                            foreach (var item in SongItems)
                            {
                                if (item.IsChecked)
                                {
                                    if(!playList.Songs.Contains(item.song, new Song.SongComparator()))
                                        playList.Songs.Add(item.song);
                                }
                            }
                            playList.Save();
                        }
                    });
                }
                return addSong;
            }
        }

        public LocalMusicViewModel()
        {
            musicService = new LocalMusicService();
            songs = GetSongs();
            GetSongItems();
        }
        private List<Song> GetSongs()
        {
            List<Song> songs = new List<Song>();
            string[] x = System.IO.Directory.GetFiles(Environment.CurrentDirectory + @"\Songs", "*.mp3", SearchOption.AllDirectories);
            foreach (string path in x)
            {
                songs.Add(new Song(path));
            }
            foreach (var song in songs)
            {
                Console.WriteLine(song.ToString());
            }
            return songs;
        }
        private void GetSongItems()
        {
            SongItems = new List<SongsItemViewModel>();
            foreach (var song in songs)
            {
                SongItems.Add(new SongsItemViewModel()
                {
                    song = song,
                    IsChecked = false
                });
            }
        }
        

    }
}
