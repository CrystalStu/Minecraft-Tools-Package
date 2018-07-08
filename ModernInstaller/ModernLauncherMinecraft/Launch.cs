using System.Diagnostics;

namespace ModernInstallerMinecraft
{
    class Launch
    {
        public static void Launcher(string destination)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = @"/c TITLE Calling Launcher..&&CD /d " + destination + "&&START Launcher.exe&&EXIT";
            p.Start();//启动程序
        }
    }
}
