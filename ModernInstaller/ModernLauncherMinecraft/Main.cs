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
using IWshRuntimeLibrary;

namespace ModernInstallerMinecraft
{


    public partial class Main : Form
    {
        bool form_mov = false;
        int form_x;
        int form_y;
        string destination;
        bool installed = false;
        bool unpacked = false;

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
                    return true;
                }
            }
            catch (Exception)
            {
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
                MessageBox.Show("Check your Internet connection.", "Connect to the Internet", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                sortie();
            }
        }

        void checkenable()
        {
            bool checkenable_result = checkfile("http://cqweb.iask.in:2222/server/vast-light/support/installer/enable.txt");
            if (checkenable_result != true)
            {
                MessageBox.Show("This installer is disabled by the web administrator.", "Disabled", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                sortie();
            }
        }

        void checkinstallerver()
        {
            if (Application.ProductVersion != getmotd("http://cqweb.iask.in:2222/server/vast-light/support/installer/ver.txt"))
            {
                MessageBox.Show("This installer is outdated, please update it.\n\nDetails:\n" + getmotd("http://cqweb.iask.in:2222/server/vast-light/support/installer/upd_log.txt"), "Require to Update", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                sortie();
            }
        }

        string getlastlinetext(string gllt_file)
        {
            string st = string.Empty;
            if (System.IO.File.Exists(@gllt_file))
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
                string destinationFile = Application.StartupPath + "\\install.log";
                if (System.IO.File.Exists(destinationFile))
                {
                    FileInfo fi = new FileInfo(destinationFile);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    System.IO.File.Delete(destinationFile);
                }
                destinationFile = Application.StartupPath + "\\install.err";
                if (System.IO.File.Exists(destinationFile))
                {
                    FileInfo fi = new FileInfo(destinationFile);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    System.IO.File.Delete(destinationFile);
                }
                destinationFile = Application.StartupPath + "\\git.dat";
                if (System.IO.File.Exists(destinationFile))
                {
                    FileInfo fi = new FileInfo(destinationFile);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    System.IO.File.Delete(destinationFile);
                }
                destinationFile = Application.StartupPath + "\\unpack.exe";
                if (System.IO.File.Exists(destinationFile))
                {
                    FileInfo fi = new FileInfo(destinationFile);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    System.IO.File.Delete(destinationFile);
                }
            }
            catch (Exception)
            {
                lb_status.Text = "TEMP: DEL_TRASH_ERROR";
            }
        }

        void bef_getclient()
        {
            killcmd();
            del_trash();
        }

        void getclient()
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = "/c TITLE Get Client Console&&CD /d " + destination + "&&" + Application.StartupPath + "\\temp\\git-cmd.exe " + Application.StartupPath + "\\temp\\runtime\\install.bat 1>" + Application.StartupPath + "\\install.log 2>" + Application.StartupPath + "\\install.err";
            p.Start();//启动程序
            lb_status.Visible = true;
            time_status.Enabled = true;
        }

        void unpack_getclient()
        {
            lb_status.Text = "Extracting components..";
            byte[] Save1 = global::ModernInstallerMinecraft.Properties.Resources.destination;
            FileStream fsObj1 = new FileStream(Application.StartupPath + "\\git.dat", FileMode.CreateNew);
            fsObj1.Write(Save1, 0, Save1.Length);
            fsObj1.Close();
            byte[] Save2 = global::ModernInstallerMinecraft.Properties.Resources.unpack;
            FileStream fsObj2 = new FileStream(Application.StartupPath + "\\unpack.exe", FileMode.CreateNew);
            fsObj2.Write(Save2, 0, Save2.Length);
            fsObj2.Close();
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = @"/c TITLE 7zip Unpacking (DO NOT CLOSE IT!)&&CLS&&unpack.exe x git.dat -y 1>install.log 2>install.err";
            p.Start();//启动程序
            lb_status.Visible = true;
            time_status.Enabled = true;
        }

        void checkgit()
        {
            if (!System.IO.File.Exists(Application.StartupPath + "\\temp\\git-cmd.exe"))
            {
                MessageBox.Show("The program detected your git with installation was not supported.", "Components Missed", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            bef_getclient();
            checkconn();
            checkenable();
            checkinstallerver();
            lb_submain.Text = "Notice: " + getmotd("http://cqweb.iask.in:2222/server/vast-light/support/installer/motd.txt");
            tb_dest.Text = Application.StartupPath + "\\destination";
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

        void callauncher()
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = @"/c TITLE Calling Launcher..&&CD /d " + destination + "&&START Launcher.exe&&EXIT";
            p.Start();//启动程序
        }

        private void bt_install_Click(object sender, EventArgs e)
        {
            if (installed == false)
            {
                destination = tb_dest.Text;
                if (!Directory.Exists(destination))
                {
                    // Create the directory it does not exist.
                    Directory.CreateDirectory(destination);
                }
                tb_dest.Visible = false;
                bt_install.Enabled = false;
                lb_dest.Visible = false;
                lb_close.Visible = false;
                Thread.Sleep(50);
                unpack_getclient();
            }
            else
            {
                callauncher();
                Thread.Sleep(150);
                sortie();
            }
        }

        bool CrtShortCut(string FilePath, string fileName, string WorkingDirectory, string Discription)
        {
            //MessageBox.Show(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory).ToString());
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
        void fin()
        {
            time_status.Enabled = false;
            del_trash();
            if (!System.IO.File.Exists(destination + "\\Launcher.exe"))
            {
                lb_main.Text = "Failed.";
                lb_submain.Text = "You can contact your administrator or reinstall that.";
            }
            else
            {
                CrtShortCut(destination + "\\git\\uninstall\\main.bat", "Uninstall Vast-Light Client", destination + "\\git\\uninstall", "Uninstall Vast-Light Client");
                CrtShortCut(destination + "\\Launcher.exe", "Vast-Light Launcher", destination, "Launch Vast-Light Client");
                Thread.Sleep(70);
                lb_status.Visible = false;
                killcmd();
                Thread.Sleep(50);
                del_trash();
                del_temp();
                lb_main.Text = "Succeeded.";
                lb_submain.Text = "You can use your client now!";
                bt_install.Enabled = true;
                bt_install.Text = "Launch";
                lb_close.Visible = true;
                installed = true;
            }
        }

        void del_temp()
        {
            try
            {
                if (Directory.Exists(Application.StartupPath + "\\temp"))
                {
                    DirectoryInfo di = new DirectoryInfo(Application.StartupPath + "\\temp");
                    di.Delete(true);
                }
            }
            catch (Exception)
            {
                lb_status.Text = "TEMP: DEL_TRASH_ERROR";
            }
        }

        void unpack_fin()
        {
            time_status.Enabled = false;
            checkgit();
            getclient();
        }

        private void time_status_Tick(object sender, EventArgs e)
        {
            lb_status.Text = "Status: " + getlastlinetext(Application.StartupPath + "\\install.log") + "\n        " + getlastlinetext(Application.StartupPath + "\\install.err");
            if (getlastlinetext(Application.StartupPath + "\\install.log") == "You must install Java before you play this game." && installed == false)
            {
                fin();
            }
            if (getlastlinetext(Application.StartupPath + "\\install.log").Contains("Compressed: ") && unpacked == false)
            {
                unpack_fin();
                unpacked = true;
            }
        }

        void killcmd()
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //!是否使用操作系统shell启动
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = "/c TASKKILL /fi 'WINDOWTITLE eq Get Client Console' /im cmd.exe /f&&EXIT";
            p.Start();//启动程序
        }

        void sortie()
        {
            killcmd();
            Thread.Sleep(50);
            del_trash();
            del_temp();
            Thread.Sleep(100);
            System.Environment.Exit(0);
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            sortie();
            Thread.Sleep(500);
        }

        void nocmd()
        {
            time_checkcmd.Enabled = false;
            MessageBox.Show("Has the cleaning script closed?\nYou must restart this installation, or it will left some unuseful files.", "Cleaning script not detected.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            sortie();
        }

        private void time_checkcmd_Tick(object sender, EventArgs e)
        {
            if (Process.GetProcessesByName("cmd").Length <= 0) nocmd();
        }
    }
}
