using System.Collections.Generic;

namespace 项目_网络音乐播放器.Services
{
    interface ILocalDataService
    {
        void LoadMultiple<T>(string path, List<T> list);
        void SaveMultiple<T>(string path, List<T> list);
        void SaveSingle<T>(string path, T o);
        T LoadSingle<T>(string path);

    }
}
