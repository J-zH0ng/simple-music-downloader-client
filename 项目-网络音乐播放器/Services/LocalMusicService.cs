using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Windows.Threading;
using 项目_网络音乐播放器.Models;

namespace 项目_网络音乐播放器.Services
{
    /// <summary>
    /// 所有属性都是静态变量，封装了操控音乐播放需要的属性和方法
    /// </summary>
    public class LocalMusicService : ILocalMusicService
    {
        #region 属性
        /// <summary>
        /// 设置或获取播放模块中的播放列表，静态变量由所有实例共享
        /// </summary>
        public List<Song> Songs
        {
            get { return songs; }
            set { songs = value; }
        }
        /// <summary>
        /// 获取正在播放的歌在PlayList中的序号
        /// </summary>
        public int CurrentIndex
        {
            get { return currentIndex; }
        }
        /// <summary>
        /// 获取或设置正在播放的歌的进度(s)
        /// </summary>
        public double Position
        {
            get { return mediaPlayer.Position.TotalSeconds; }
            set { mediaPlayer.Position = TimeSpan.FromSeconds(value); }
        }
        /// <summary>
        /// 获取或设置当前音量值
        /// </summary>
        public double Volume
        {
            get { return mediaPlayer.Volume; }
            set { mediaPlayer.Volume = value; }
        }
        /// <summary>
        /// 获取当前正播放歌曲的时长(s)
        /// </summary>
        public double MaxPosition
        {
            get
            {
                if (mediaPlayer.NaturalDuration.HasTimeSpan)
                {
                    return mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// 获取是否有歌曲正在播放
        /// </summary>
        public bool IsPlaying
        {
            get
            {
                return mediaPlayerIsPlaying;
            }
        }
        /// <summary>
        /// 获取播放模式
        /// </summary>
        public PlayMode PlayMode
        {
            get { return playMode; }
            set { playMode = value; }
        }
        #endregion



        #region 方法
        /// <summary>
        /// 播放已经加载好的音乐
        /// </summary>
        /// <returns></returns>
        public bool Play()
        {
            if (mediaPlayer != null && mediaPlayer.Source != null)
            {
                mediaPlayer.Play();
                mediaPlayerIsPlaying = true;
                return true;
            }
            Console.WriteLine("[LocalMusicService] 歌曲播放失败");
            return false;
        }

        /// <summary>
        /// 暂停正在播放的音乐
        /// </summary>
        /// <returns></returns>
        public bool Pause()
        {
            if (mediaPlayerIsPlaying)
            {
                mediaPlayer.Pause();
                mediaPlayerIsPlaying = false;
                return true;
            }
            Console.WriteLine("[LocalMusicService] 歌曲暂停失败");
            return false;
        }

        /// <summary>
        /// 加载一首音乐，可以不在当前播放列表中
        /// </summary>
        /// <param name="song"></param>
        /// <returns></returns>
        public bool Prepare(Song song)
        {
            if (File.Exists(song.file_path) && song.file_path.EndsWith("mp3"))
            {
                try
                {
                    mediaPlayer.Open(new Uri(song.file_path));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
                return true;
            }
            else
            {
                Console.WriteLine("[LocalMusicService] 歌曲加载失败");
                return false;
            }
        }

        /// <summary>
        /// 加载一首音乐并播放，可以不在当前播放列表中
        /// </summary>
        /// <param name="song"></param>
        /// <returns></returns>
        public bool PrepareAndPlay(Song song)
        {
            if (Prepare(song))
            {
                Play();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 加载一首音乐并播放，必须已在当前播放列表中
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool PrepareAndPlay(int index)
        {
            if (Prepare(Songs[index]))
            {
                Play();
                currentIndex = index;
                return true;
            }
            else
            {
                currentIndex = 0;
                return false;
            }
        }


        #region 废弃
        public void SetPlayMode(PlayMode mode)
        {
            playMode = mode;
        }

        public void SetPosition(double value)
        {
            Position = value;
        }

        public void SetVolume(double value)
        {
            Volume = value;
        }
        #endregion

        /// <summary>
        /// 改变播放模式
        /// 循环、单曲循环、随机
        /// </summary>
        public void ChangePlayMode()
        {
            switch (PlayMode)
            {
                case PlayMode.Repeat:
                    PlayMode = PlayMode.RepeatOne;
                    break;
                case PlayMode.RepeatOne:
                    PlayMode = PlayMode.RepeatVariant;
                    break;
                case PlayMode.RepeatVariant:
                    PlayMode = PlayMode.Repeat;
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// 根据播放模式跳转到下一首歌
        /// </summary>
        /// <returns></returns>
        public bool SkipNext()
        {
            if (mediaPlayer != null && songs.Count > 1)
            {
                switch (playMode)
                {
                    case PlayMode.Repeat:
                        if (songs.Count > 1)
                        {
                            preIndex = currentIndex;
                            currentIndex++;
                        }
                        if (currentIndex < songs.Count)
                        {
                            return PrepareAndPlay(currentIndex);
                        }
                        else
                        {
                            currentIndex = 0;
                            return PrepareAndPlay(currentIndex);
                        }
                    //break;
                    case PlayMode.RepeatOne:
                        Position = 0;
                        return true;
                    //break;
                    case PlayMode.RepeatVariant:
                        int temp = random.Next(0, songs.Count);
                        if (temp != currentIndex)
                        {
                            preIndex = currentIndex;
                            currentIndex = temp;
                            return PrepareAndPlay(currentIndex);
                        }
                        break;
                }
            }
            return false;
        }

        /// <summary>
        /// 根据播放模式跳转到上一首歌，随机模式下上一首是刚刚播放的那一首，不停的上一首只会在两首歌之间切换
        /// </summary>
        /// <returns></returns>
        public bool SkipPrevious()
        {
            if (mediaPlayer != null && songs.Count > 1)
            {
                if (playMode == PlayMode.Repeat)
                {
                    if (songs.Count > 1)
                        currentIndex--;
                    if (currentIndex >= 0)
                    {
                        return PrepareAndPlay(CurrentIndex);
                    }
                    else
                    {
                        currentIndex = songs.Count - 1;
                        return PrepareAndPlay(currentIndex);
                    }
                }
                else
                {
                    int temp = currentIndex;
                    currentIndex = preIndex;
                    preIndex = currentIndex;
                    return PrepareAndPlay(currentIndex);
                }
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 如果计时器没有启动,启动计时器
        /// </summary>
        public LocalMusicService()
        {
            if (!timer.IsEnabled)
            {
                timer.Interval = TimeSpan.FromSeconds(0.5);
                timer.Tick += timer_Tick;
                timer.Start();
            }
        }

        /// <summary>
        /// 计时器的工作：切歌
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            if (mediaPlayer.Source != null && mediaPlayer.NaturalDuration.HasTimeSpan)
            {
                if (mediaPlayer.Position == mediaPlayer.NaturalDuration.TimeSpan)
                    SkipNext();
            }
        }
        #endregion

        private static DispatcherTimer timer = new DispatcherTimer();
        private static MediaPlayer mediaPlayer = new MediaPlayer();
        private static bool mediaPlayerIsPlaying = false;//判断歌曲是否正在播放
        private static int currentIndex = 0;
        private static int preIndex;
        private static PlayMode playMode = PlayMode.Repeat;
        private static Random random = new Random();
        private static List<Song> songs = new List<Song>();//当前播放列表
    }
}
