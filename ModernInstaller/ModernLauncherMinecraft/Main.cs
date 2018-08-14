using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;
using static ModernInstallerMinecraft.Web;
using static ModernInstallerMinecraft.Utility;

namespace ModernInstallerMinecraft
{


    public partial class Main : Form
    {
        string destination;
        bool installed = false;
        bool unpacked = false;

        #region UI Component Handler
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

        #region Main Form
        public Main()
        {
            InitializeComponent();
        }

        private void Main_MouseDown(object sender, MouseEventArgs e)
        {
            WindowMoveHandle(ref e);
        }

        private void Main_Load(object sender, EventArgs e)
        {
            DelTrashAndKillCmd();
            // CheckEnable();
            CheckVer();
            lb_submain.Text = "Notice: " + DownloadText("https://vl.cstu.gq/support/installer/motd.txt");
            tb_dest.Text = Application.StartupPath + "\\destination";
            // UI.PlayBGM();
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Sortie();
            Thread.Sleep(500);
        }
        #endregion

        #region Event Component Handler
        private void bt_install_Click(object sender, EventArgs e)
        {
            if(tb_dest.Text.Contains(" "))
            {
                if(Thread.CurrentThread.CurrentUICulture.Name.Contains("zh"))
                {
                    MessageBox.Show("请不要在安装路径中含有空格(' ')，请修改您的选项并重试。");
                }
                MessageBox.Show("Please don't contain space(' ') in the destination path, please modify your option and retry.");
                return;
            }
            if (installed == false)
            {
                destination = tb_dest.Text;
                if (!Directory.Exists(destination))
                {
                    Directory.CreateDirectory(destination);
                }
                tb_dest.Visible = false;
                bt_install.Enabled = false;
                lb_dest.Visible = false;
                lb_close.Visible = false;
                Thread.Sleep(50);
                UnpackGit();
            }
            else
            {
                Launch.Launcher(destination);
                Thread.Sleep(150);
                Sortie();
            }
        }

        private void time_status_Tick(object sender, EventArgs e)
        {
            lb_status.Text = "Status: " + GetLastLine(Application.StartupPath + "\\install.log") + "\n        " + GetLastLine(Application.StartupPath + "\\install.err");
            if (GetLastLine(Application.StartupPath + "\\install.log") == "You must install Java before you play this game." && installed == false)
            {
                FinishInstall();
            }
            if (GetLastLine(Application.StartupPath + "\\install.log").Contains("Compressed: ") && unpacked == false)
            {
                FinishUnpack();
                unpacked = true;
            }
        }

        private void time_checkcmd_Tick(object sender, EventArgs e)
        {
            if (Process.GetProcessesByName("cmd").Length <= 0) CmdNotFound();
        }
        #endregion

        #region Installer Base Handler
        void CheckEnable()
        {
            bool checkenable_result = CheckFile("https://vl.cstu.gq/support/installer/enable.txt");
            if (checkenable_result != true)
            {
                MessageBox.Show("This launcher is disabled by the web administrator or you have disconnected from the Internet.", "Disabled", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Sortie();
            }
        }

        void CheckVer()
        {
            string serverVer = DownloadText("https://vl.cstu.gq/support/installer/ver.txt");
            if (serverVer.Contains("ERR_RTV_M: ")) return;
            if (Application.ProductVersion != serverVer)
            {
                MessageBox.Show("This installer is outdated, please update it.\n\nDetails:\n" + DownloadText("https://vl.cstu.gq/support/installer/upd_log.txt"), "Require to Update", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Sortie();
            }
        }

        void CheckGitExists()
        {
            if (!File.Exists(Application.StartupPath + "\\temp\\git-cmd.exe"))
            {
                MessageBox.Show("The program detected your git with installation was not supported.", "Components Missed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Sortie();
            }
        }

        void DelTrashAndKillCmd()
        {
            KillGetClientConsole();
            DelTrash();
        }

        void DownloadFromGit()
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = "/c TITLE Get Client Console&&CD /d " + destination + "&&" + Application.StartupPath + "\\temp\\git-cmd.exe " + Application.StartupPath + "\\temp\\runtime\\install.bat 1>" + Application.StartupPath + "\\install.log 2>" + Application.StartupPath + "\\install.err";
            p.Start();//启动程序
            lb_status.Visible = true;
            time_status.Enabled = true;
        }

        void UnpackGit()
        {
            lb_status.Text = "Extracting components..";
            byte[] Save1 = Properties.Resources.destination;
            FileStream fsObj1 = new FileStream(Application.StartupPath + "\\git.dat", FileMode.CreateNew);
            fsObj1.Write(Save1, 0, Save1.Length);
            fsObj1.Close();
            byte[] Save2 = Properties.Resources.unpack;
            FileStream fsObj2 = new FileStream(Application.StartupPath + "\\unpack.exe", FileMode.CreateNew);
            fsObj2.Write(Save2, 0, Save2.Length);
            fsObj2.Close();
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = @"/c TITLE 7zip Unpacking (DO NOT CLOSE IT!)&&CLS&&unpack.exe x git.dat -y 1>install.log 2>install.err";
            p.Start();//启动程序
            lb_status.Visible = true;
            time_status.Enabled = true;
        }

        void FinishUnpack()
        {
            time_status.Enabled = false;
            CheckGitExists();
            DownloadFromGit();
        }

        void FinishInstall()
        {
            time_status.Enabled = false;
            DelTrash();
            if (!File.Exists(destination + "\\Launcher.exe"))
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
                KillGetClientConsole();
                Thread.Sleep(50);
                DelTrash();
                DelTempDir();
                lb_main.Text = "Succeeded.";
                lb_submain.Text = "You can use your client now!";
                bt_install.Enabled = true;
                bt_install.Text = "Launch";
                lb_close.Visible = true;
                installed = true;
            }
        }

        void DelTrash()
        {
            try
            {
                string destinationFile = Application.StartupPath + "\\install.log";
                if (File.Exists(destinationFile))
                {
                    FileInfo fi = new FileInfo(destinationFile);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(destinationFile);
                }
                destinationFile = Application.StartupPath + "\\install.err";
                if (File.Exists(destinationFile))
                {
                    FileInfo fi = new FileInfo(destinationFile);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(destinationFile);
                }
                destinationFile = Application.StartupPath + "\\git.dat";
                if (File.Exists(destinationFile))
                {
                    FileInfo fi = new FileInfo(destinationFile);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(destinationFile);
                }
                destinationFile = Application.StartupPath + "\\unpack.exe";
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

        void DelTempDir()
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

        void KillGetClientConsole()
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //!是否使用操作系统shell启动
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = "/c TASKKILL /fi 'WINDOWTITLE eq Get Client Console' /im cmd.exe /f&&EXIT";
            p.Start();//启动程序
        }

        void CmdNotFound()
        {
            time_checkcmd.Enabled = false;
            // MessageBox.Show("Has the cleaning script closed?\nYou must restart this installation, or it will left some unuseful files.", "Cleaning script not detected.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            // Sortie();
        }

        void Sortie()
        {
            KillGetClientConsole();
            Thread.Sleep(50);
            DelTrash();
            DelTempDir();
            Thread.Sleep(100);
            Environment.Exit(0);
        }
        #endregion
    }
}
