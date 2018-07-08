using KMCCC.Authentication;
using KMCCC.Launcher;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace ModernLauncherMinecraft
{
    class Launch
    {
        public static void VLW(string username)
        {
            string launch_name = username;
            var versions = Program.Core.GetVersions().ToArray();
            var core = LauncherCore.Create();
            var ver = core.GetVersion("1.8");
            var result = Program.Core.Launch(new LaunchOptions
            {
                Version = ver, //Ver为Versions里你要启动的版本名字
                MaxMemory = 1024, //最大内存，int类型
                Authenticator = new OfflineAuthenticator(launch_name), //离线启动，设置的游戏名
                                                                       //Authenticator = new YggdrasilLogin("邮箱", "密码", true), // 正版启动，最后一个为是否twitch登录
                Mode = LaunchMode.MCLauncher, //启动模式
                Server = new ServerInfo { Address = "play.vl.cstu.gq", Port = 25565 }, //设置启动游戏后，自动加入指定IP的服务器，可以不要
                Size = new WindowSize { Height = 768, Width = 1280 } //设置窗口大小，可以不要
            });
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
