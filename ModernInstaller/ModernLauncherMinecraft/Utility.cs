using IWshRuntimeLibrary;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ModernInstallerMinecraft
{
    public static class Utility
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        public static string GetLastLine(string file)
        {
            string st = string.Empty;
            if (System.IO.File.Exists(file))
            {
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        while (!sr.EndOfStream)
                        {
                            st = sr.ReadLine();
                        }
                    }
                }
            }
            return st;
        }

        public static bool CrtShortCut(string FilePath, string fileName, string WorkingDirectory, string Discription)
        {
            //从COM中引用 Windows Script Host Object Model
            //再using IWshRuntimeLibrary;
            WshShell shell = new WshShell();

            //创建桌面快捷方式
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\" + fileName + ".lnk");
            shortcut.TargetPath = FilePath;
            shortcut.WorkingDirectory = WorkingDirectory;
            shortcut.WindowStyle = 1;
            shortcut.Description = Discription;
            shortcut.Save();

            //创建开始菜单快捷方式
            IWshShortcut shortcut1 = (IWshShortcut)shell.CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu) + "\\" + fileName + ".lnk");
            shortcut1.TargetPath = FilePath;
            shortcut1.WorkingDirectory = WorkingDirectory;
            shortcut1.WindowStyle = 1;
            shortcut1.Description = Discription;
            shortcut1.Save();
            return true;
        }
    }
}
