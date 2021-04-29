
namespace IWImgViewer
{
    partial class Viewport
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
            this.PreviewBox = new System.Windows.Forms.PictureBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.MetaDataLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PreviewBox)).BeginInit();
            this.SuspendLayout();
            // 
            // PreviewBox
            // 
            this.PreviewBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PreviewBox.BackColor = System.Drawing.Color.Transparent;
            this.PreviewBox.Location = new System.Drawing.Point(0, 0);
            this.PreviewBox.Name = "PreviewBox";
            this.PreviewBox.Size = new System.Drawing.Size(282, 259);
            this.PreviewBox.TabIndex = 1;
            this.PreviewBox.TabStop = false;
            // 
            // MetaDataLabel
            // 
            this.MetaDataLabel.AutoSize = true;
            this.MetaDataLabel.Location = new System.Drawing.Point(12, 9);
            this.MetaDataLabel.Name = "MetaDataLabel";
            this.MetaDataLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.MetaDataLabel.Size = new System.Drawing.Size(0, 13);
            this.MetaDataLabel.TabIndex = 2;
            // 
            // Viewport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(280, 257);
            this.Controls.Add(this.MetaDataLabel);
            this.Controls.Add(this.PreviewBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Viewport";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
            this.Text = "Viewport";
            ((System.ComponentModel.ISupportInitialize)(this.PreviewBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox PreviewBox;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label MetaDataLabel;
    }
}