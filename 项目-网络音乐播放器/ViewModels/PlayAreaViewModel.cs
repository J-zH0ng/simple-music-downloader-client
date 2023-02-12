using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MaterialDesignThemes.Wpf;
using PropertyChanged;
using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using 项目_网络音乐播放器.Models;
using 项目_网络音乐播放器.Services;

namespace 项目_网络音乐播放器.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class PlayAreaViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private RelayCommand<Slider> setSlider;
        public RelayCommand<Slider> SetSlider
        {
            get
            {
                if (setSlider == null)
                {
                    setSlider = new RelayCommand<Slider>((slider) =>
                      {
                          Slider = slider;
                      });
                }
                return setSlider;
            }
            set { setSlider = value; }
        }
        private Slider Slider { get; set; }

        public PlayAreaViewModel()
        {
            MusicService = new LocalMusicService();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.5);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        public PackIconKind PlayIconKind { get; set; }
        private void timer_Tick(object sender, EventArgs e)
        {
            if (Slider != null)
            {
                Slider.Value = MusicService.Position;
                Slider.Maximum = MusicService.MaxPosition;
                Position = TimeSpan.FromSeconds(MusicService.Position).ToString(@"hh\:mm\:ss");
                MaxPosition = TimeSpan.FromSeconds(MusicService.MaxPosition).ToString(@"hh\:mm\:ss");
                if (MusicService.IsPlaying)
                {
                    PlayIconKind = PackIconKind.Pause;
                }
                else
                {
                    PlayIconKind = PackIconKind.Play;
                }
                CurrentSong = GetCurrentSong();
            }

            //Console.WriteLine($"Position:{Position} MaxPosition:{MaxPosition}");
        }
        //private RelayCommand sliderDragStarted;
        //public RelayCommand SliderDragStarted
        //{
        //    get
        //    {
        //        if (sliderDragStarted == null)
        //        {
        //            sliderDragStarted = new RelayCommand(() =>
        //            {
        //                userIsDraging = true;
        //            });
        //        }
        //        return sliderDragStarted;
        //    }
        //    set { sliderDragStarted = value; }
        //}

        private RelayCommand sliderValueChanged;
        public RelayCommand SliderValueChanged
        {
            get
            {
                if (sliderValueChanged == null)
                {
                    sliderValueChanged = new RelayCommand(() =>
                    {
                        MusicService.Position = Slider.Value;
                    });
                }
                return sliderValueChanged;
            }
            set { sliderValueChanged = value; }
        }

        //private RelayCommand sliderDragEnded;
        //public RelayCommand SliderDragEnded
        //{
        //    get
        //    {
        //        if (sliderDragEnded == null)
        //        {
        //            sliderDragEnded = new RelayCommand(() =>
        //            {
        //                userIsDraging = false;
        //                MusicService.Position = Slider.Value;
        //            });
        //        }
        //        return sliderDragEnded;
        //    }
        //    set { sliderDragEnded = value; }
        //}


        public LocalMusicService MusicService { get; set; }

        public Song CurrentSong { get; set; }

        private Song GetCurrentSong()
        {
            if (MusicService.Songs != null && MusicService.Songs.Count > 0)
                return MusicService.Songs[MusicService.CurrentIndex];
            else
                return null;
        }

        public string Position { get; set; }

        public string MaxPosition { get; set; }

        public double Volume
        {
            get { return MusicService.Volume; }
            set { MusicService.Volume = value; }
        }

        private RelayCommand playAndPause;
        public RelayCommand PlayAndPause
        {
            get
            {
                if (playAndPause == null)
                {
                    playAndPause = new RelayCommand(() =>
                    {
                        if (PlayIconKind == PackIconKind.Play)
                        {
                            if (MusicService.Play())
                            {
                                PlayIconKind = PackIconKind.Pause;
                            }
                        }
                        else
                        {
                            if (MusicService.Pause())
                            {
                                PlayIconKind = PackIconKind.Play;
                            }
                        }
                    });
                }
                return playAndPause;
            }
        }


        private RelayCommand skipNext;
        public RelayCommand SkipNext
        {
            get
            {
                if (skipNext == null)
                {
                    skipNext = new RelayCommand(() => MusicService.SkipNext());
                }
                return skipNext;
            }
        }

        private RelayCommand skipPrevious;
        public RelayCommand SkipPrivious
        {
            get
            {
                if (skipPrevious == null)
                {
                    skipPrevious = new RelayCommand(() => MusicService.SkipPrevious());
                }
                return skipPrevious;
            }
        }

        public RelayCommand changePlayMode;
        public RelayCommand ChangePlayMode
        {
            get
            {
                if (changePlayMode == null)
                {
                    changePlayMode = new RelayCommand(() =>
                     {
                         MusicService.ChangePlayMode();
                     });
                }
                return changePlayMode;
            }
        }

    }
}
