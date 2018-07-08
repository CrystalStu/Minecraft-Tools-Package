namespace ModernInstallerMinecraft
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.lb_close = new System.Windows.Forms.Label();
            this.lb_mini = new System.Windows.Forms.Label();
            this.lb_title = new System.Windows.Forms.Label();
            this.bt_install = new System.Windows.Forms.Button();
            this.lb_main = new System.Windows.Forms.Label();
            this.lb_submain = new System.Windows.Forms.Label();
            this.time_status = new System.Windows.Forms.Timer(this.components);
            this.lb_status = new System.Windows.Forms.Label();
            this.lb_img_copyright = new System.Windows.Forms.Label();
            this.time_checkcmd = new System.Windows.Forms.Timer(this.components);
            this.lb_dest = new System.Windows.Forms.Label();
            this.tb_dest = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lb_close
            // 
            resources.ApplyResources(this.lb_close, "lb_close");
            this.lb_close.BackColor = System.Drawing.Color.Transparent;
            this.lb_close.ForeColor = System.Drawing.Color.White;
            this.lb_close.Name = "lb_close";
            this.lb_close.Click += new System.EventHandler(this.lb_close_Click);
            this.lb_close.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lb_close_MouseDown);
            this.lb_close.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lb_close_MouseUp);
            // 
            // lb_mini
            // 
            resources.ApplyResources(this.lb_mini, "lb_mini");
            this.lb_mini.BackColor = System.Drawing.Color.Transparent;
            this.lb_mini.ForeColor = System.Drawing.Color.White;
            this.lb_mini.Name = "lb_mini";
            this.lb_mini.Click += new System.EventHandler(this.lb_mini_Click);
            this.lb_mini.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lb_mini_MouseDown);
            this.lb_mini.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lb_mini_MouseUp);
            // 
            // lb_title
            // 
            resources.ApplyResources(this.lb_title, "lb_title");
            this.lb_title.BackColor = System.Drawing.Color.Transparent;
            this.lb_title.ForeColor = System.Drawing.Color.White;
            this.lb_title.Name = "lb_title";
            this.lb_title.Click += new System.EventHandler(this.lb_title_Click);
            this.lb_title.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lb_title_MouseDown);
            this.lb_title.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lb_title_MouseUp);
            // 
            // bt_install
            // 
            resources.ApplyResources(this.bt_install, "bt_install");
            this.bt_install.BackColor = System.Drawing.Color.Transparent;
            this.bt_install.FlatAppearance.BorderColor = System.Drawing.Color.GreenYellow;
            this.bt_install.ForeColor = System.Drawing.Color.GreenYellow;
            this.bt_install.Name = "bt_install";
            this.bt_install.UseVisualStyleBackColor = false;
            this.bt_install.Click += new System.EventHandler(this.bt_install_Click);
            // 
            // lb_main
            // 
            resources.ApplyResources(this.lb_main, "lb_main");
            this.lb_main.BackColor = System.Drawing.Color.Transparent;
            this.lb_main.ForeColor = System.Drawing.Color.White;
            this.lb_main.Name = "lb_main";
            // 
            // lb_submain
            // 
            resources.ApplyResources(this.lb_submain, "lb_submain");
            this.lb_submain.BackColor = System.Drawing.Color.Transparent;
            this.lb_submain.Cursor = System.Windows.Forms.Cursors.Default;
            this.lb_submain.ForeColor = System.Drawing.Color.White;
            this.lb_submain.Name = "lb_submain";
            // 
            // time_status
            // 
            this.time_status.Interval = 30;
            this.time_status.Tick += new System.EventHandler(this.time_status_Tick);
            // 
            // lb_status
            // 
            resources.ApplyResources(this.lb_status, "lb_status");
            this.lb_status.BackColor = System.Drawing.Color.Transparent;
            this.lb_status.Cursor = System.Windows.Forms.Cursors.Default;
            this.lb_status.ForeColor = System.Drawing.Color.White;
            this.lb_status.Name = "lb_status";
            // 
            // lb_img_copyright
            // 
            resources.ApplyResources(this.lb_img_copyright, "lb_img_copyright");
            this.lb_img_copyright.BackColor = System.Drawing.Color.Transparent;
            this.lb_img_copyright.ForeColor = System.Drawing.Color.Gainsboro;
            this.lb_img_copyright.Name = "lb_img_copyright";
            // 
            // time_checkcmd
            // 
            this.time_checkcmd.Enabled = true;
            this.time_checkcmd.Tick += new System.EventHandler(this.time_checkcmd_Tick);
            // 
            // lb_dest
            // 
            resources.ApplyResources(this.lb_dest, "lb_dest");
            this.lb_dest.BackColor = System.Drawing.Color.Transparent;
            this.lb_dest.ForeColor = System.Drawing.Color.White;
            this.lb_dest.Name = "lb_dest";
            // 
            // tb_dest
            // 
            resources.ApplyResources(this.tb_dest, "tb_dest");
            this.tb_dest.Name = "tb_dest";
            // 
            // Main
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Indigo;
            this.BackgroundImage = global::ModernInstallerMinecraft.Properties.Resources.background;
            this.Controls.Add(this.tb_dest);
            this.Controls.Add(this.lb_dest);
            this.Controls.Add(this.lb_img_copyright);
            this.Controls.Add(this.lb_status);
            this.Controls.Add(this.lb_submain);
            this.Controls.Add(this.lb_main);
            this.Controls.Add(this.bt_install);
            this.Controls.Add(this.lb_title);
            this.Controls.Add(this.lb_mini);
            this.Controls.Add(this.lb_close);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Main";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
            this.Load += new System.EventHandler(this.Main_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Main_MouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lb_close;
        private System.Windows.Forms.Label lb_mini;
        private System.Windows.Forms.Label lb_title;
        private System.Windows.Forms.Button bt_install;
        private System.Windows.Forms.Label lb_main;
        private System.Windows.Forms.Label lb_submain;
        private System.Windows.Forms.Timer time_status;
        private System.Windows.Forms.Label lb_status;
        private System.Windows.Forms.Label lb_img_copyright;
        private System.Windows.Forms.Timer time_checkcmd;
        private System.Windows.Forms.Label lb_dest;
        private System.Windows.Forms.TextBox tb_dest;
    }
}

