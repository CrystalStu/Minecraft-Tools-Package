using KMCCC.Authentication;
using KMCCC.Launcher;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace ModernLauncherMinecraft
{
    class Launch
    {
        public static void changeOption(int lang)
        {
            if (!File.Exists(Application.StartupPath + @"\.minecraft\versions\1.13\options.txt"))
            {
                FileInfo fi = new FileInfo(Application.StartupPath + @"\.minecraft\versions\1.13\options.txt.tmpl");
                if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                    fi.Attributes = FileAttributes.Normal;
                fi.MoveTo(Application.StartupPath + @"\.minecraft\versions\1.13\options.txt");
            }
            string langStr;
            switch (lang)
            {
                default:
                    langStr = "en_us";
                    break;
                case 1:
                    langStr = "ja_jp";
                    break;
                case 2:
                    langStr = "zh_cn";
                    break;
            }
            string[] properties = File.ReadAllLines(Application.StartupPath + @"\.minecraft\versions\1.13\options.txt");
            for(int t = 0; t < properties.Length; t++)
            {
                if(properties[t].Split(':')[0] == "gamma")
                {
                    properties[t] = properties[t].Replace("1", "0").Replace("2", "0").Replace("3", "0").Replace("4", "0").Replace("5", "0").Replace("6", "0").Replace("7", "0").Replace("8", "0").Replace("9", "0");
                    continue;
                }
                if(properties[t].Split(':')[0] == "lang")
                {
                    properties[t] = "lang:" + langStr;
                    continue;
                }
            }
            File.WriteAllLines(Application.StartupPath + @"\.minecraft\versions\1.13\options.txt", properties);
        }

        public static void VLW(string username, string password, string memory, string path, bool fullScreen)
        {
            string serverStr = "28f8f58a8a7f11e88feb525400b59b6a";
            // var versions = Program.Core.GetVersions().ToArray();
            var core = LauncherCore.Create();
            var ver = core.GetVersion("1.13");
            var option = new LaunchOptions
            {
                Version = ver, //Ver为Versions里你要启动的版本名字
                MaxMemory = Convert.ToInt32(memory), //最大内存，int类型
                AgentPath = @"nide8auth.jar=" + serverStr,
                // Authenticator = new OfflineAuthenticator(username), //离线启动，设置的游戏名
                Authenticator = new YggdrasilLogin(username, password, true, null, "https://auth2.nide8.com:233/" + serverStr + "/authserver"), // 伪正版启动，最后一个为是否twitch登录
                Mode = LaunchMode.MCLauncher, //启动模式
                // Server = new ServerInfo { Address = "ali.cge.hm", Port = 30033 }, //设置启动游戏后，自动加入指定IP的服务器，可以不要
                /*
                Size = new WindowSize
                {
                    Height = 720,
                    Width = 1280,
                } //设置窗口大小，可以不要
                */
            };
            if (fullScreen) option.Size.FullScreen = true;
            var result = Program.Core.Launch(option);
            if (!result.Success)
            {
                switch (result.ErrorType)
                {
                    case ErrorType.NoJAVA:
                        MessageBox.Show("Java Error, try to reinstall the client.", "Java Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case ErrorType.AuthenticationFailed:
                        MessageBox.Show("Please check your username and password.", "Check Profile", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                    case ErrorType.UncompressingFailed:
                        MessageBox.Show("Depressing Failed!\nCheck your client or reinstall.", "Uncompressing Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    default:
                        MessageBox.Show(
                            result.ErrorMessage + "\n" +
                            (result.Exception == null ? string.Empty : result.Exception.StackTrace),
                            "Error Occured: Unknown");
                        break;
                }
            }
            Thread.Sleep(50);
        }
    }
}
