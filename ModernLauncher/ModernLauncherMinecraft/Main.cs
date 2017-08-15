using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Runtime.InteropServices;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Threading;
using KMCCC.Launcher;
using KMCCC.Authentication;
using KMCCC.Modules;
using KMCCC.Tools;

namespace ModernLauncherMinecraft
{
    public partial class Main : Form
    {
        bool form_mov = false;
        int form_x;
        int form_y;

        //Functions

        [DllImport("winmm.dll")]
        public static extern uint mciSendString(string lpstrCommand,
        string lpstrReturnString, uint uReturnLength, uint hWndCallback);
        void bgm_play()
        {
            mciSendString(@"close temp_alias", null, 0, 0);
            mciSendString(@"open ""http://cqweb.iask.in:2222/server/vast-light/misc/misc.mp3"" alias temp_alias", null, 0, 0);
            mciSendString("play temp_alias repeat", null, 0, 0);
        }
        
        /*
         * Closed: The lowest Windows of .NET Framework 4.5 System Requirements is Windows Vista SP2.
        void checksys()
        {
            if (System.Environment.OSVersion.Version.Major < 6)
            {
                MessageBox.Show("Please update your system to NT6.0 or higher (Windows Vista/2008 or higher, recommend Windows 10.)","Modern Launcher Minecraft cannot run in this system");
                sortie();
            }
        }
        */

        public static bool checkfile(string fileUrl)
        {
            HttpWebRequest re = null;
            HttpWebResponse res = null;
            try
            {
                re = (HttpWebRequest)WebRequest.Create(fileUrl);
                res = (HttpWebResponse)re.GetResponse();
                if (res.ContentLength != 0)
                {
                    //MessageBox.Show("文件存在");
                    return true;
                }
            }
            catch (Exception)
            {
                //MessageBox.Show("无此文件");
                return false;
            }
            finally
            {
                if (re != null)
                {
                    re.Abort();//销毁关闭连接
                }
                if (res != null)
                {
                    res.Close();//销毁关闭响应
                }
            }
            return false;
        }

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(int Description, int ReservedValue);
        public static bool checknet()
        {
            int Description = 0;
            return InternetGetConnectedState(Description, 0);
        }


        void checkconn()
        {
            bool checkconn_result = checknet();
            if (checkconn_result != true)
            {
                MessageBox.Show("Check your Internet connection.","Connect to the Internet", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                sortie();
            }
        }

        void checkenable()
        {
            bool checkenable_result = checkfile("http://cqweb.iask.in:2222/server/vast-light/support/launcher/enable.txt");
            if (checkenable_result != true)
            {
                MessageBox.Show("This launcher is disabled by the web administrator.","Disabled", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                sortie();
            }
        }

        void checklauncherver()
        {
            //MessageBox.Show(Application.ProductVersion);
            //MessageBox.Show(getmotd("http://cqweb.iask.in:2222/server/vast-light/support/launcher/ver.txt"));
            if (Application.ProductVersion != getmotd("http://cqweb.iask.in:2222/server/vast-light/support/launcher/ver.txt"))
            {
                MessageBox.Show("This launcher is outdated, confirm to update it.\n\nDetails:\n" + getmotd("http://cqweb.iask.in:2222/server/vast-light/support/launcher/upd_log.txt"), "Require to Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.Arguments = "/c TITLE Launcher Update Console&&DEL .git\\index.lock /f /q&&CLS&&git\\git-cmd.exe git\\runtime\\launcher_update.bat";
                p.Start();//启动程序
                sortie();
            }
        }

        string getlastlinetext(string gllt_file)
        {
            string st = string.Empty;
            if (File.Exists(@gllt_file))
            {
                using (FileStream fs = new FileStream(@gllt_file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
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

        void del_trash()
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

        void bef_checkver()
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = "/c TASKKILL /im javaw.exe /f /t";
            p.Start();//启动程序
            Thread.Sleep(50);
            del_trash();
        }
        void checkver()
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            //p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
            //p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = "/c TITLE Client Update Console&&DEL .git\\index.lock /f /q&&CLS&&git\\git-cmd.exe git\\runtime\\update.bat 1>update.log 2>update.err";
            p.Start();//启动程序
                      //Thread.Sleep(500);
            //string sOutput = p.StandardOutput.ReadToEnd();
            //p.StandardInput.WriteLine("n");
            //p.StandardInput.AutoFlush = true;
            Thread.Sleep(100);
            time_status.Enabled = true;
        }

        void checkgit()
        {
            if (!File.Exists(@Application.StartupPath+@"\git\git-cmd.exe"))
            {
                MessageBox.Show("The program detected your git with installation was not supported.","Components Missed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                sortie();
            }
        }

        string getmotd(string getmotd_url)
        {
            WebClient client = new WebClient();
            byte[] buffer = client.DownloadData(getmotd_url);
            return System.Text.ASCIIEncoding.ASCII.GetString(buffer);
        }

        //Programs

        public Main()
        {
            InitializeComponent();
        }

        private void Main_MouseDown(object sender, MouseEventArgs e)
        {
            form_mov = true;
            form_x = MousePosition.X;//鼠标的x坐标为当前窗体左上角x坐标
            form_y = MousePosition.Y;//鼠标的y坐标为当前窗体左上角y坐标
        }

        private void Main_MouseMove(object sender, MouseEventArgs e)
        {
            if (form_mov)
            {
                this.Left += MousePosition.X - form_x;//根据鼠标x坐标确定窗体的左边坐标x
                this.Top += MousePosition.Y - form_y;//根据鼠标的y坐标窗体的顶部，即Y坐标
                form_x = MousePosition.X;
                form_y = MousePosition.Y;
            }
        }

        private void Main_MouseUp(object sender, MouseEventArgs e)
        {
            form_mov = false;//停止移动
        }

        private void Main_Load(object sender, EventArgs e)
        {
            bef_checkver();
            checkconn();
            checkenable();
            checkgit();
            checklauncherver();
            checkver();
            lb_submain.Text = "Notice: " + getmotd("http://cqweb.iask.in:2222/server/vast-light/support/launcher/motd.txt");
            bgm_play();
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
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = "/c TASKKILL /im conhost.exe /f";
            p.Start();//启动程序
            Thread.Sleep(100);
            del_trash();
            Thread.Sleep(50);
            sortie();
        }

        private void lb_mini_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
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

        
        private void launch_vlw_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "It's not avaliable now.", "Warning", MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            /*
            string launch_name = tb_user.Text;
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
                Server = new ServerInfo { Address = "vast-light.iask.in", Port = 25565 }, //设置启动游戏后，自动加入指定IP的服务器，可以不要
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
                        MessageBox.Show(this, "Check your user profile!", "Check Profile" , MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                    case ErrorType.UncompressingFailed:
                        MessageBox.Show(this, "Uncompressing Failed!\nCheck your client or reinstall.", "Uncompressing Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    default:
                        MessageBox.Show(this,
                            result.ErrorMessage + "\n" +
                            (result.Exception == null ? string.Empty : result.Exception.StackTrace),
                            "Error Occured: Unknown");
                        break;
                }
            }
            Thread.Sleep(50);
            sortie();
            */
        }

        private void time_status_Tick(object sender, EventArgs e)
        {
            lb_status.Text = "Status: " + getlastlinetext(Application.StartupPath + "\\update.log") + "\n" + getlastlinetext(Application.StartupPath + "\\update.err");
            if (getlastlinetext(Application.StartupPath + "\\update.log").Contains("Complete"))
            {
                lb_close.Visible = true;
                launch_vlw.Enabled = true;
                launch_quartz.Enabled = true;
                time_status.Enabled = false;
            }
        }

        void sortie()
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = "/c TASKKILL /fi 'WINDOWTITLE eq Launcher Update Console' /im cmd.exe /f";
            p.Start();//启动程序
            Thread.Sleep(10);
            p.StartInfo.Arguments = "/c TASKKILL /fi 'WINDOWTITLE eq Client Update Console' /im cmd.exe /f";
            p.Start();//启动程序
            Thread.Sleep(50);
            del_trash();
            Thread.Sleep(100);
            System.Environment.Exit(0);
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            sortie();
            Thread.Sleep(200);
        }

        private void launch_quartz_Click(object sender, EventArgs e)
        {
            string launch_name = tb_user.Text;
            var versions = Program.Core.GetVersions().ToArray();
            var core = LauncherCore.Create();
            var ver = core.GetVersion("1.7.2");
            var result = Program.Core.Launch(new LaunchOptions
            {
                Version = ver, //Ver为Versions里你要启动的版本名字
                MaxMemory = 1024, //最大内存，int类型
                Authenticator = new OfflineAuthenticator(launch_name), //离线启动，设置的游戏名
                                                                       //Authenticator = new YggdrasilLogin("邮箱", "密码", true), // 正版启动，最后一个为是否twitch登录
                Mode = LaunchMode.MCLauncher, //启动模式
                Server = new ServerInfo { Address = "game.cstu.gq", Port = 25565 }, //设置启动游戏后，自动加入指定IP的服务器，可以不要
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
                        MessageBox.Show(this, "Check your user profile!", "Check Profile", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                    case ErrorType.UncompressingFailed:
                        MessageBox.Show(this, "Uncompressing Failed!\nCheck your client or reinstall.", "Uncompressing Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    default:
                        MessageBox.Show(this,
                            result.ErrorMessage + "\n" +
                            (result.Exception == null ? string.Empty : result.Exception.StackTrace),
                            "Error Occured: Unknown");
                        break;
                }
            }
            Thread.Sleep(50);
            sortie();
        }
    }
}
