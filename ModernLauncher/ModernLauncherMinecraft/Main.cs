// #define NOCHECK // ONLY FOR THE TEST MODE
using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;
using static ModernLauncherMinecraft.Web;
using static ModernLauncherMinecraft.Utility;

namespace ModernLauncherMinecraft
{
    public partial class Main : Form
    {
        #region UI Component Handler
        private void Main_MouseDown(object sender, MouseEventArgs e)
        {
            WindowMoveHandle(ref e);
        }

        private void lb_close_MouseDown(object sender, MouseEventArgs e)
        {
            lb_close.ForeColor = Color.Red;
        }

        private void lb_close_MouseUp(object sender, MouseEventArgs e)
        {
            lb_close.ForeColor = Color.White;
        }

        private void lb_close_Click(object sender, EventArgs e)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = "/c TASKKILL /im conhost.exe /f";
            p.Start();//启动程序
            Thread.Sleep(100);
            CleanUpdateLog();
            Thread.Sleep(50);
            Sortie();
        }

        private void lb_mini_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void lb_mini_MouseDown(object sender, MouseEventArgs e)
        {
            lb_mini.ForeColor = Color.LimeGreen;
        }

        private void lb_mini_MouseUp(object sender, MouseEventArgs e)
        {
            lb_mini.ForeColor = Color.White;
        }

        private void lb_title_Click(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog();
        }

        private void lb_title_MouseDown(object sender, MouseEventArgs e)
        {
            lb_title.ForeColor = Color.Lavender;
        }

        private void lb_title_MouseUp(object sender, MouseEventArgs e)
        {
            lb_title.ForeColor = Color.White;
        }

        #endregion
        
        #region Event Component Handler
        private void launch_vlw_Click(object sender, EventArgs e)
        {
            Launch.VLW(tb_user.Text, tb_pass.Text, tb_memory.Text, Application.StartupPath);
            Sortie();
        }

        private void time_status_Tick(object sender, EventArgs e)
        {
            lb_status.Text = "Status: " + GetLastLine(Application.StartupPath + "\\update.log") + "\n" + GetLastLine(Application.StartupPath + "\\update.err");
            if (GetLastLine(Application.StartupPath + "\\update.log").Contains("Complete"))
            {
                lb_close.Visible = true;
                launch_vlw.Enabled = true;
                // launch_quartz.Enabled = true;
                time_status.Enabled = false;
            }
        }

        private void bt_register_Click(object sender, EventArgs e)
        {
            Process.Start("https://login2.nide8.com:233/28f8f58a8a7f11e88feb525400b59b6a/register");
        }
        #endregion

        #region UI Event Handler
        private void WindowMoveHandle(ref MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        #endregion

        #region Launcher Base Handler

        void CheckEnable()
        {
            bool checkenable_result = CheckFile("https://vl.cstu.gq/support/launcher/enable.txt");
            if (checkenable_result != true)
            {
                MessageBox.Show("This launcher is disabled by the web administrator or you have disconnected from the Internet.", "Disabled", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Sortie();
            }
        }

        void LauncherUpdate()
        {
            if (Application.ProductVersion != DownloadText("https://vl.cstu.gq/support/launcher/ver.txt"))
            {
                MessageBox.Show("This launcher is outdated, confirm to update it.\n\nDetails:\n" + DownloadText("https://vl.cstu.gq/support/launcher/upd_log.txt"), "Require to Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process p = new Process();
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.Arguments = "/c TITLE Launcher Update Console&&DEL .git\\index.lock /f /q&&CLS&&git\\git-cmd.exe git\\runtime\\launcher_update.bat";
                p.Start();//启动程序
                Sortie();
            }
        }

        void CleanUpdateLog()
        {
            try
            {
                string destinationFile = Application.StartupPath + "\\update.log";
                if (File.Exists(destinationFile))
                {
                    FileInfo fi = new FileInfo(destinationFile);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(destinationFile);
                }
                destinationFile = Application.StartupPath + "\\update.err";
                if (File.Exists(destinationFile))
                {
                    FileInfo fi = new FileInfo(destinationFile);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(destinationFile);
                }
            }
            catch (Exception)
            {
                lb_status.Text = "TEMP: DEL_TRASH_ERROR";
            }
        }

        void KillJava()
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = "/c TASKKILL /im javaw.exe /f /t";
            p.Start();//启动程序
            Thread.Sleep(50);
            CleanUpdateLog();
        }

        void ClientUpdate()
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = "/c TITLE Client Update Console&&DEL .git\\index.lock /f /q&&CLS&&git\\git-cmd.exe git\\runtime\\update.bat 1>update.log 2>update.err";
            p.Start();//启动程序
            Thread.Sleep(100);
            time_status.Enabled = true;
        }

        void CheckGit()
        {
            if (!File.Exists(@Application.StartupPath+@"\git\git-cmd.exe"))
            {
                MessageBox.Show("The program detected your git with installation was not supported.","Components Missed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Sortie();
            }
        }
        #endregion

        #region Main Form
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            KillJava();
#if (!NOCHECK)
            CheckEnable();
            CheckGit();
            LauncherUpdate();
            ClientUpdate();
            lb_submain.Text = "Notice: " + DownloadText("https://vl.cstu.gq/support/launcher/motd.txt");
#endif
            UI.PlayBGM();
#if (!NOCHECK)
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = "/c TASKKILL /im javaw.exe /f";
            p.Start();//启动程序
#endif
#if (NOCHECK)
            lb_status.Text = "THIS VERSION IS ONLY FOR TEST.";
            lb_close.Visible = true;
            launch_vlw.Enabled = true;
#endif
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Sortie();
            Thread.Sleep(200);
        }

        void Sortie()
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = "/c TASKKILL /fi 'WINDOWTITLE eq Launcher Update Console' /im cmd.exe /f";
            p.Start();//启动程序
            Thread.Sleep(10);
            p.StartInfo.Arguments = "/c TASKKILL /fi 'WINDOWTITLE eq Client Update Console' /im cmd.exe /f";
            p.Start();//启动程序
            Thread.Sleep(50);
            CleanUpdateLog();
            Thread.Sleep(100);
            Environment.Exit(0);
        }
        #endregion
    }
}
