namespace XiMusic
{
    partial class frmDesktopLyrics
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
            this.lblPreMusic = new System.Windows.Forms.Label();
            this.lblPlayOrPauseMusic = new System.Windows.Forms.Label();
            this.lblNextMusic = new System.Windows.Forms.Label();
            this.lblSetting = new System.Windows.Forms.Label();
            this.lblStyle = new System.Windows.Forms.Label();
            this.lblLock = new System.Windows.Forms.Label();
            this.lblClose = new System.Windows.Forms.Label();
            this.cmsLrcColorStyle = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.lblDownload = new System.Windows.Forms.Label();
            this.SuspendLayout();
        
            this.lblStyle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblStyle.Location = new System.Drawing.Point(468, 6);
            this.lblStyle.Name = "lblStyle";
            this.lblStyle.Size = new System.Drawing.Size(16, 16);
            this.lblStyle.TabIndex = 0;
            this.lblStyle.Tag = "3";
            this.lblStyle.Click += new System.EventHandler(this.lblStyle_Click);
            // 
            // lblLock
            // 
            this.lblLock.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblLock.Location = new System.Drawing.Point(520, 6);
            this.lblLock.Name = "lblLock";
            this.lblLock.Size = new System.Drawing.Size(16, 16);
            this.lblLock.TabIndex = 0;
            this.lblLock.Tag = "3";
            this.lblLock.Click += new System.EventHandler(this.lblLock_Click);
            // 
            // lblClose
            // 
            this.lblClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblClose.Location = new System.Drawing.Point(546, 6);
            this.lblClose.Name = "lblClose";
            this.lblClose.Size = new System.Drawing.Size(16, 16);
            this.lblClose.TabIndex = 0;
            this.lblClose.Tag = "3";
            this.lblClose.Click += new System.EventHandler(this.lblClose_Click);
            // 
            // cmsLrcColorStyle
            // 
            this.cmsLrcColorStyle.Name = "contextMenuStrip1";
            this.cmsLrcColorStyle.Size = new System.Drawing.Size(61, 4);
            // 
            // lblDownload
            // 
            this.lblDownload.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblDownload.Location = new System.Drawing.Point(490, 6);
            this.lblDownload.Name = "lblDownload";
            this.lblDownload.Size = new System.Drawing.Size(16, 16);
            this.lblDownload.TabIndex = 0;
            this.lblDownload.Tag = "3";
            this.lblDownload.Click += new System.EventHandler(this.lblDownload_Click);
            // 
            // frmDesktopLyrics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(850, 107);
            this.Controls.Add(this.lblStyle);
            this.Controls.Add(this.lblClose);
            this.Controls.Add(this.lblDownload);
            this.Controls.Add(this.lblLock);
            this.Controls.Add(this.lblSetting);
            this.Controls.Add(this.lblNextMusic);
            this.Controls.Add(this.lblPlayOrPauseMusic);
            this.Controls.Add(this.lblPreMusic);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmDesktopLyrics";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "frmDesktopLyrics";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.DiskLrcFrm_Load);    
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DiskLrcFrm_MouseDown);
            this.MouseEnter += new System.EventHandler(this.DiskLrcFrm_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.DiskLrcFrm_MouseLeave);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblPreMusic;
        private System.Windows.Forms.Label lblPlayOrPauseMusic;
        private System.Windows.Forms.Label lblNextMusic;
        private System.Windows.Forms.Label lblSetting;
        private System.Windows.Forms.Label lblStyle;
        private System.Windows.Forms.Label lblLock;
        private System.Windows.Forms.Label lblClose;
        private System.Windows.Forms.ContextMenuStrip cmsLrcColorStyle;
        private System.Windows.Forms.Label lblDownload;

    }
}