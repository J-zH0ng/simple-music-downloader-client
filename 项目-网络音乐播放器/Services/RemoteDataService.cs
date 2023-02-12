using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace 项目_网络音乐播放器.Services
{
    public class RemoteDataService : IRemoteDataService
    {
        MusicService.MusicServiceClient client;
        public RemoteDataService()
        {
            client = new MusicService.MusicServiceClient();
        }
        public async Task<bool> DownLoadAsync(int id, string title)
        {
            string path = Environment.CurrentDirectory + @"\Songs\";
            //string message = "";
            //Stream filestream = new MemoryStream();
            //long filesize = client.DownLoadFile(id, out issuccess, out message, out filestream);
            MusicService.DownFileResult downFileResult = await client.DownLoadFileAsync(id);
            if (downFileResult.IsSuccess)
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //byte[] buffer = new byte[filesize];

                FileStream fs = new FileStream($"{path}{title}-{id}.mp3", FileMode.Create, FileAccess.Write);
                fs.Write(downFileResult.FileBytes, 0, downFileResult.FileBytes.Length);
                //int count = 0;
                //while ((count = filestream.Read(buffer, 0, buffer.Length)) > 0)
                //{
                //    fs.Write(buffer, 0, count);
                //}

                //清空缓冲区
                fs.Flush();
                //关闭流
                fs.Close();
                Debug.Print("下载成功！");
                return true;
            }
            else
            {

                Debug.Print("下载失败");
                return false;
            }
        }

        public MusicService.song_view[] GetSongs()
        {
            MusicService.song_view[] songs = null;
            try
            {
                songs = client.GetSongViews();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return songs;
        }
    }
}
