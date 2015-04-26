namespace XiMusic
{
    partial class ctlSearchBox
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtKeyWord = new System.Windows.Forms.TextBox();
            this.btnWebSearch = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtKeyWord
            // 
            this.txtKeyWord.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtKeyWord.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtKeyWord.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtKeyWord.Location = new System.Drawing.Point(1, 0);
            this.txtKeyWord.Margin = new System.Windows.Forms.Padding(0);
            this.txtKeyWord.Multiline = true;
            this.txtKeyWord.Name = "txtKeyWord";
            this.txtKeyWord.Size = new System.Drawing.Size(191, 29);
            this.txtKeyWord.TabIndex = 0;
            // 
            // btnWebSearch
            // 
            this.btnWebSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWebSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnWebSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnWebSearch.Image = global::XiMusic.Properties.Resources.serach;
            this.btnWebSearch.Location = new System.Drawing.Point(195, 1);
            this.btnWebSearch.Name = "btnWebSearch";
            this.btnWebSearch.Size = new System.Drawing.Size(34, 29);
            this.btnWebSearch.TabIndex = 1;
            // 
            // ctlSearchBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkGray;
            this.Controls.Add(this.btnWebSearch);
            this.Controls.Add(this.txtKeyWord);
            this.Name = "ctlSearchBox";
            this.Size = new System.Drawing.Size(229, 31);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    

        #endregion
        private System.Windows.Forms.TextBox txtKeyWord;
        public System.Windows.Forms.Label btnWebSearch;
    }
}
