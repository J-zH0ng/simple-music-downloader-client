using System.Threading.Tasks;

namespace 项目_网络音乐播放器.Services
{
    interface IRemoteDataService
    {
        MusicService.song_view[] GetSongs();
        Task<bool> DownLoadAsync(int id, string title);
    }
}
