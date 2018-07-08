using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ModernLauncherMinecraft
{
    public static class Utility
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public static string GetLastLine(string file)
        {
            string st = string.Empty;
            if (File.Exists(file))
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
    }
}
