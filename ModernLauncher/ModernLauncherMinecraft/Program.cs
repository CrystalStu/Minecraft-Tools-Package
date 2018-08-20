using System;
using System.Windows.Forms;
using KMCCC.Launcher;

namespace ModernLauncherMinecraft
{
    static class Program
    {
        public static LauncherCore Core = LauncherCore.Create();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(System.Globalization.CultureInfo.CurrentUICulture.Name); // new System.Globalization.CultureInfo("zh-CN"); // 
            Application.Run(new Main());
        }
    }
}
