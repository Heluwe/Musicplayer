namespace XiMusic
{
    partial class frmSearchMusic
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
            this.pnlBack1 = new System.Windows.Forms.Panel();
            this.pMusicList = new System.Windows.Forms.Panel();
            this.picCloseWindow3 = new System.Windows.Forms.PictureBox();
            this.userctlWebList = new XiMusic.ctlMusicList();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlBack1.SuspendLayout();
            this.pMusicList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCloseWindow3)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlBack1
            // 
            this.pnlBack1.BackColor = System.Drawing.Color.CadetBlue;
            this.pnlBack1.Controls.Add(this.pMusicList);
            this.pnlBack1.Controls.Add(this.picCloseWindow3);
            this.pnlBack1.Controls.Add(this.userctlWebList);
            this.pnlBack1.Controls.Add(this.webBrowser1);
            this.pnlBack1.Location = new System.Drawing.Point(0, 0);
            this.pnlBack1.Name = "pnlBack1";
            this.pnlBack1.Size = new System.Drawing.Size(354, 528);
            this.pnlBack1.TabIndex = 0;
            // 
            // pMusicList
            // 
            this.pMusicList.BackColor = System.Drawing.Color.White;
            this.pMusicList.Controls.Add(this.panel1);
            this.pMusicList.Location = new System.Drawing.Point(0, 38);
            this.pMusicList.Name = "pMusicList";
            this.pMusicList.Size = new System.Drawing.Size(354, 455);
            this.pMusicList.TabIndex = 5;
            // 
            // picCloseWindow3
            // 
            this.picCloseWindow3.Image = global::XiMusic.Properties.Resources.close_normal;
            this.picCloseWindow3.Location = new System.Drawing.Point(304, 9);
            this.picCloseWindow3.Name = "picCloseWindow3";
            this.picCloseWindow3.Size = new System.Drawing.Size(37, 23);
            this.picCloseWindow3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picCloseWindow3.TabIndex = 4;
            this.picCloseWindow3.TabStop = false;
            this.picCloseWindow3.Click += new System.EventHandler(this.picCloseWindow3_Click);
            this.picCloseWindow3.MouseEnter += new System.EventHandler(this.picCloseWindow3_MouseEnter);
            this.picCloseWindow3.MouseLeave += new System.EventHandler(this.picCloseWindow3_MouseLeave);
            // 
            // userctlWebList
            // 
            this.userctlWebList.BackColor = System.Drawing.Color.White;
            this.userctlWebList.ItemHeigth = 20;
            this.userctlWebList.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(0)))), ((int)(((byte)(140)))), ((int)(((byte)(214)))));
            this.userctlWebList.ItemPadding = 4;
            this.userctlWebList.ItemTextColor = System.Drawing.Color.White;
            this.userctlWebList.ItemTextHoverColor = System.Drawing.Color.Yellow;
            this.userctlWebList.Location = new System.Drawing.Point(49, 12);
            this.userctlWebList.MaxSize = 10;
            this.userctlWebList.Name = "userctlWebList";
            this.userctlWebList.ShowDelButton = false;
            this.userctlWebList.Size = new System.Drawing.Size(10, 10);
            this.userctlWebList.TabIndex = 1;
            this.userctlWebList.Visible = false;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(12, 12);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(20, 20);
            this.webBrowser1.TabIndex = 2;
            this.webBrowser1.Visible = false;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.CadetBlue;
            this.panel1.Location = new System.Drawing.Point(3, 460);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(348, 30);
            this.panel1.TabIndex = 0;
            // 
            // frmSearchMusic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(353, 528);
            this.Controls.Add(this.pnlBack1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmSearchMusic";
            this.Text = "frmSearchMusic";
            this.pnlBack1.ResumeLayout(false);
            this.pMusicList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picCloseWindow3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBack1;
        private ctlMusicList userctlWebList;
        private System.Windows.Forms.WebBrowser webBrowser1;
     //   private System.Windows.Forms.WebBrowser webBrowser1;
        private ctlSearchBox btnWebSearch;
        private System.Windows.Forms.PictureBox picCloseWindow3;
        private System.Windows.Forms.Panel pMusicList;
        public System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel1;
        // private System.Windows.Forms.Panel pMusicList;
    }
}