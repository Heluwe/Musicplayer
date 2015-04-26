namespace XiMusic
{
    partial class frmSearchLyrics
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
            this.pnlTop1 = new System.Windows.Forms.Panel();
            this.lvwLyricsList = new XiMusic.ctlUserListView();
            this.btnSearchLyrics = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.txtLrcName = new System.Windows.Forms.TextBox();
            this.btnDownloadLyrics = new System.Windows.Forms.Button();
            this.lblSongName = new System.Windows.Forms.Label();
            this.txtSongName = new System.Windows.Forms.TextBox();
            this.txtSingerName = new System.Windows.Forms.TextBox();
            this.lblSingerName = new System.Windows.Forms.Label();
            this.lblSeachLyrics = new System.Windows.Forms.Label();
            this.picCloseWindow2 = new System.Windows.Forms.PictureBox();
            this.pnlTop1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCloseWindow2)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTop1
            // 
            this.pnlTop1.BackColor = System.Drawing.Color.White;
            this.pnlTop1.Controls.Add(this.lvwLyricsList);
            this.pnlTop1.Controls.Add(this.btnSearchLyrics);
            this.pnlTop1.Controls.Add(this.lblMessage);
            this.pnlTop1.Controls.Add(this.txtLrcName);
            this.pnlTop1.Controls.Add(this.btnDownloadLyrics);
            this.pnlTop1.Controls.Add(this.lblSongName);
            this.pnlTop1.Controls.Add(this.txtSongName);
            this.pnlTop1.Controls.Add(this.txtSingerName);
            this.pnlTop1.Controls.Add(this.lblSingerName);
            this.pnlTop1.Location = new System.Drawing.Point(1, 52);
            this.pnlTop1.Name = "pnlTop1";
            this.pnlTop1.Size = new System.Drawing.Size(320, 282);
            this.pnlTop1.TabIndex = 0;
            // 
            // lvwLyricsList
            // 
            this.lvwLyricsList.Location = new System.Drawing.Point(32, 37);
            this.lvwLyricsList.Name = "lvwLyricsList";
            this.lvwLyricsList.Size = new System.Drawing.Size(241, 112);
            this.lvwLyricsList.TabIndex = 10;
            this.lvwLyricsList.UseCompatibleStateImageBehavior = false;
            this.lvwLyricsList.Click += new System.EventHandler(this.lvwLyricsList_Click);
            // 
            // btnSearchLyrics
            // 
            this.btnSearchLyrics.Location = new System.Drawing.Point(148, 180);
            this.btnSearchLyrics.Name = "btnSearchLyrics";
            this.btnSearchLyrics.Size = new System.Drawing.Size(75, 23);
            this.btnSearchLyrics.TabIndex = 1;
            this.btnSearchLyrics.Text = "搜索";
            this.btnSearchLyrics.UseVisualStyleBackColor = true;
            this.btnSearchLyrics.Click += new System.EventHandler(this.btnSearchLyrics_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.Location = new System.Drawing.Point(11, 216);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(293, 57);
            this.lblMessage.TabIndex = 8;
            this.lblMessage.Text = "注意:搜索歌词时只留下歌曲名，下载的时候如果歌词名和播放的歌曲名有一点差别也可能不能匹配，所以麻烦你手动修改搜索上面的对话框.可以多试几个歌词.注意空格也会影响匹" +
    "配，如果有不清楚的地方与我联系";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtLrcName
            // 
            this.txtLrcName.Location = new System.Drawing.Point(32, 155);
            this.txtLrcName.Name = "txtLrcName";
            this.txtLrcName.Size = new System.Drawing.Size(241, 21);
            this.txtLrcName.TabIndex = 9;
            // 
            // btnDownloadLyrics
            // 
            this.btnDownloadLyrics.Location = new System.Drawing.Point(229, 180);
            this.btnDownloadLyrics.Name = "btnDownloadLyrics";
            this.btnDownloadLyrics.Size = new System.Drawing.Size(75, 23);
            this.btnDownloadLyrics.TabIndex = 2;
            this.btnDownloadLyrics.Text = "下载";
            this.btnDownloadLyrics.UseVisualStyleBackColor = true;
            this.btnDownloadLyrics.Click += new System.EventHandler(this.btnDownloadLyrics_Click);
            // 
            // lblSongName
            // 
            this.lblSongName.AutoSize = true;
            this.lblSongName.Location = new System.Drawing.Point(68, 13);
            this.lblSongName.Name = "lblSongName";
            this.lblSongName.Size = new System.Drawing.Size(29, 12);
            this.lblSongName.TabIndex = 3;
            this.lblSongName.Text = "歌名";
            // 
            // txtSongName
            // 
            this.txtSongName.Location = new System.Drawing.Point(103, 10);
            this.txtSongName.Name = "txtSongName";
            this.txtSongName.Size = new System.Drawing.Size(130, 21);
            this.txtSongName.TabIndex = 5;
            // 
            // txtSingerName
            // 
            this.txtSingerName.Location = new System.Drawing.Point(55, 182);
            this.txtSingerName.Name = "txtSingerName";
            this.txtSingerName.Size = new System.Drawing.Size(87, 21);
            this.txtSingerName.TabIndex = 6;
            this.txtSingerName.Visible = false;
            // 
            // lblSingerName
            // 
            this.lblSingerName.AutoSize = true;
            this.lblSingerName.Location = new System.Drawing.Point(20, 185);
            this.lblSingerName.Name = "lblSingerName";
            this.lblSingerName.Size = new System.Drawing.Size(29, 12);
            this.lblSingerName.TabIndex = 4;
            this.lblSingerName.Text = "歌手";
            this.lblSingerName.Visible = false;
            // 
            // lblSeachLyrics
            // 
            this.lblSeachLyrics.AutoSize = true;
            this.lblSeachLyrics.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSeachLyrics.ForeColor = System.Drawing.Color.White;
            this.lblSeachLyrics.Location = new System.Drawing.Point(127, 18);
            this.lblSeachLyrics.Name = "lblSeachLyrics";
            this.lblSeachLyrics.Size = new System.Drawing.Size(63, 14);
            this.lblSeachLyrics.TabIndex = 0;
            this.lblSeachLyrics.Text = "歌词搜索";
            this.lblSeachLyrics.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picCloseWindow2
            // 
            this.picCloseWindow2.Image = global::XiMusic.Properties.Resources.close_normal;
            this.picCloseWindow2.Location = new System.Drawing.Point(264, 12);
            this.picCloseWindow2.Name = "picCloseWindow2";
            this.picCloseWindow2.Size = new System.Drawing.Size(44, 20);
            this.picCloseWindow2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picCloseWindow2.TabIndex = 1;
            this.picCloseWindow2.TabStop = false;
            this.picCloseWindow2.Click += new System.EventHandler(this.picCloseWindow2_Click);
            this.picCloseWindow2.MouseEnter += new System.EventHandler(this.picCloseWindow2_MouseEnter);
            this.picCloseWindow2.MouseLeave += new System.EventHandler(this.picCloseWindow2_MouseLeave);
            // 
            // frmSearchLyrics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.CadetBlue;
            this.ClientSize = new System.Drawing.Size(320, 334);
            this.Controls.Add(this.picCloseWindow2);
            this.Controls.Add(this.lblSeachLyrics);
            this.Controls.Add(this.pnlTop1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmSearchLyrics";
            this.Text = "frmSearchLyrics";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmSearchLyrics_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmSearchLyrics_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmSearchLyrics_MouseUp);
            this.pnlTop1.ResumeLayout(false);
            this.pnlTop1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCloseWindow2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox picCloseWindow2;
        private System.Windows.Forms.Label lblSeachLyrics;
        private System.Windows.Forms.Button btnSearchLyrics;
        private System.Windows.Forms.Button btnDownloadLyrics;
        private System.Windows.Forms.Label lblSongName;
        private System.Windows.Forms.Label lblSingerName;
        private System.Windows.Forms.TextBox txtSongName;
        private System.Windows.Forms.TextBox txtSingerName;
        protected System.Windows.Forms.Panel pnlTop1;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.TextBox txtLrcName;
        private ctlUserListView lvwLyricsList;
    }
}