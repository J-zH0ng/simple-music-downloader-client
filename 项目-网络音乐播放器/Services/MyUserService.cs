using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 项目_网络音乐播放器.Models;
using 项目_网络音乐播放器.UserService;

namespace 项目_网络音乐播放器.Services
{
    public class MyUserService
    {
        private LocalDataService localDataService = new LocalDataService();
        private UserServiceClient client = new UserServiceClient();
        public bool Login(string username,string password)
        {
            try
            {
                if (client.LogIn(username, password))
                {
                    User user = new User()
                    {
                        Name = username,
                        Password = password
                    };

                    string path = Environment.CurrentDirectory + @"\Data";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    path += @"\User.bin";
                    localDataService.SaveSingle(path, user);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            
        }
        public bool SignIn(string username,string password)
        {
            try
            {
                return client.SignIn(username, password);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
        public bool Logout()
        {
            string path = Environment.CurrentDirectory + @"\Data\User.bin";
            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
            return false;
        }

    }
}
