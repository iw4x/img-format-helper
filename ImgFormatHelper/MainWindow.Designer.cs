
namespace IWImgViewer
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.MetaDataLabel = new System.Windows.Forms.Label();
            this.closeAllButton = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.SplashPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.SplashPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // MetaDataLabel
            // 
            this.MetaDataLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MetaDataLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MetaDataLabel.Location = new System.Drawing.Point(0, 0);
            this.MetaDataLabel.Name = "MetaDataLabel";
            this.MetaDataLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.MetaDataLabel.Size = new System.Drawing.Size(284, 261);
            this.MetaDataLabel.TabIndex = 1;
            this.MetaDataLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // closeAllButton
            // 
            this.closeAllButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.closeAllButton.Location = new System.Drawing.Point(0, 238);
            this.closeAllButton.Name = "closeAllButton";
            this.closeAllButton.Size = new System.Drawing.Size(284, 23);
            this.closeAllButton.TabIndex = 4;
            this.closeAllButton.Text = "Close all viewports";
            this.closeAllButton.UseVisualStyleBackColor = true;
            this.closeAllButton.Click += new System.EventHandler(this.closeAllButton_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
            // 
            // SplashPictureBox
            // 
            this.SplashPictureBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.SplashPictureBox.Image = global::IWImgViewer.Properties.Resources.cardicon_juggernaut_1;
            this.SplashPictureBox.InitialImage = global::IWImgViewer.Properties.Resources.cardicon_juggernaut_1;
            this.SplashPictureBox.Location = new System.Drawing.Point(108, 12);
            this.SplashPictureBox.Name = "SplashPictureBox";
            this.SplashPictureBox.Size = new System.Drawing.Size(64, 64);
            this.SplashPictureBox.TabIndex = 5;
            this.SplashPictureBox.TabStop = false;
            this.SplashPictureBox.WaitOnLoad = true;
            // 
            // MainWindow
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.SplashPictureBox);
            this.Controls.Add(this.closeAllButton);
            this.Controls.Add(this.MetaDataLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainWindow";
            this.Text = "IW4 Image Format Helper";
            ((System.ComponentModel.ISupportInitialize)(this.SplashPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label MetaDataLabel;
        private System.Windows.Forms.Button closeAllButton;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.PictureBox SplashPictureBox;
    }
}

