using KMCCC.Authentication;
using KMCCC.Launcher;
using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace ModernLauncherMinecraft
{
    class Launch
    {
        public static void VLW(string username, string password, string memory, string path, bool fullScreen)
        {
            string serverStr = "28f8f58a8a7f11e88feb525400b59b6a";
            var versions = Program.Core.GetVersions().ToArray();
            var core = LauncherCore.Create();
            var ver = core.GetVersion("1.12.2-forge1.12.2-14.23.2.2611");
            var option = new LaunchOptions
            {
                Version = ver, //Ver为Versions里你要启动的版本名字
                MaxMemory = Convert.ToInt32(memory), //最大内存，int类型
                AgentPath = @"nide8auth.jar=" + serverStr,
                //Authenticator = new OfflineAuthenticator(username), //离线启动，设置的游戏名
                Authenticator = new YggdrasilLogin(username, password, true, null, "https://auth2.nide8.com:233/" + serverStr + "/authserver"), // 伪正版启动，最后一个为是否twitch登录
                Mode = LaunchMode.MCLauncher, //启动模式
                Server = new ServerInfo { Address = "218.93.208.142", Port = 11314 }, //设置启动游戏后，自动加入指定IP的服务器，可以不要
                Size = new WindowSize
                {
                    Height = 720,
                    Width = 1280,
                } //设置窗口大小，可以不要
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
                        MessageBox.Show("Check your user profile!", "Check Profile", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                    case ErrorType.UncompressingFailed:
                        MessageBox.Show("Uncompressing Failed!\nCheck your client or reinstall.", "Uncompressing Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
