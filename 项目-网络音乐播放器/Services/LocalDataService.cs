using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using 项目_网络音乐播放器.Models;

namespace 项目_网络音乐播放器.Services
{
    public class LocalDataService : ILocalDataService
    {
        //public void SavePlayList(string fileName, List<Song> songs)
        //{
        //    string path = Path.Combine(Environment.CurrentDirectory, @"Data\" + fileName + ".bin");
        //    SaveMultiple<Song>(path, songs);
        //}
        //public List<Song> LoadPlayList(string fileName)
        //{
        //    List<Song> songs = new List<Song>();
        //    string path = Path.Combine(Environment.CurrentDirectory, @"Data\" + fileName + ".bin");
        //    LoadMultiple<Song>("path", songs);
        //    return songs;
        //}
        //public void SaveUserInfo(User u)
        //{
        //    string path = Path.Combine(Environment.CurrentDirectory, @"Data\user.bin");
        //    using (FileStream writer = new FileStream(path, FileMode.Create))
        //    {
        //        IFormatter formatter = new BinaryFormatter();
        //        formatter.Serialize(writer, u);
        //    }
        //}
        //public User LoadUserInfo()
        //{
        //    string path = Path.Combine(Environment.CurrentDirectory, @"Data\user.bin");
        //    if (File.Exists(path))
        //    {
        //        using (FileStream fs = new FileStream(path, FileMode.Open))
        //        {
        //            IFormatter formatter = new BinaryFormatter();

        //            return (User)formatter.Deserialize(fs);
        //        }
        //    }
        //    else
        //    {
        //        return new User();
        //    }
        //}



        #region 序列化List到路径、重新加载序列化的List到程序
        public void SaveMultiple<T>(string path, List<T> list)
        {
            using (FileStream writer = new FileStream(path, FileMode.Create))
            {
                IFormatter formatter = new BinaryFormatter();
                foreach (var item in list)
                {
                    formatter.Serialize(writer, item);
                }
            }
        }

        public void LoadMultiple<T>(string path, List<T> list)
        {
            try
            {
                if (File.Exists(path))
                {
                    using (FileStream fs = new FileStream(path, FileMode.Open))
                    {
                        IFormatter formatter = new BinaryFormatter();

                        while (fs.Position != fs.Length)
                        {
                            list.Add((T)formatter.Deserialize(fs));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">完整路径</param>
        /// <param name="o"></param>
        public void SaveSingle<T>(string path, T o)
        {
            using (FileStream writer = new FileStream(path, FileMode.Create))
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(writer, o);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">完整路径</param>
        /// <returns></returns>
        public T LoadSingle<T>(string path)
        {
            if (File.Exists(path))
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    IFormatter formatter = new BinaryFormatter();

                    return (T)formatter.Deserialize(fs);
                }
            }
            else
            {
                return default;
            }
        }
    }
}
