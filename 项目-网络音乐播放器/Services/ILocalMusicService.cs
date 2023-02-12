using 项目_网络音乐播放器.Models;

namespace 项目_网络音乐播放器.Services
{
    interface  ILocalMusicService
    {
        bool Prepare(Song song);
        bool Play();
        bool Pause();
        bool SkipNext();
        bool SkipPrevious();
        void SetVolume(double value);
        void SetPlayMode(PlayMode mode);
        void SetPosition(double value);

    }

    public enum PlayMode
    {
        Repeat,
        RepeatOne,
        RepeatVariant
    }
}
