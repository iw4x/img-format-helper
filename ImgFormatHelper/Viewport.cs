using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IWImgViewer
{
    public partial class Viewport : Form
    {
        MainWindow parent;

        int zoom = 1;

        EasyFilters exportFilters = new EasyFilters(
                ("Portable Graphics Network", "PNG"),
                ("Windows Bitmap", "BMP"),
                ("Tag Image File Format", "TIFF"),
                ("Joint Photographics Experts", "JPEG"),
                ("Windows Icon", "ICO")
            );

        Dictionary<string, ImageFormat> exportFormatMap = new Dictionary<string, ImageFormat>()
        {
            { "PNG", ImageFormat.Png },
            { "BMP", ImageFormat.Bmp },
            { "TIFF", ImageFormat.Tiff },
            { "JPEG", ImageFormat.Jpeg },
            { "ICO", ImageFormat.Icon }
        };

        public Viewport(MainWindow mainWindow)
        {
            parent = mainWindow;
            InitializeComponent();

            saveFileDialog1.FileOk += saveFileDialog1_FileOk;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.X)
            {
                saveFileDialog1.DefaultExt = exportFilters.GetFilterExtensionByOneBasedIndex(1);
                saveFileDialog1.FileName = System.IO.Path.GetFileNameWithoutExtension(parent.ImageName);
                saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                saveFileDialog1.Filter = exportFilters.ToString();
                saveFileDialog1.ShowDialog(this);
                return true;
            }

            if (keyData == Keys.Z) 
            {
                switch (zoom) {
                    case 1:
                        SetSize(2);
                        break;
                    case 2:
                        SetSize(4);
                        break;
                    default:
                        SetSize(1);
                        break;
                }
               
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        public void ShowImage(Bitmap bmp, string name)
        {
            PreviewBox.Image = bmp;
            SetSize(1);
            this.MetaDataLabel.Text = $"{name}{bmp.Width}x{bmp.Height}";

            Refresh();
        }

        private void SetSize(int zoom)
        {
            this.zoom = zoom;
            this.Width = zoom * PreviewBox.Image.Width + 16;
            this.Height = zoom * PreviewBox.Image.Height + 39;
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            var format = exportFilters.GetFilterExtensionByOneBasedIndex(saveFileDialog1.FilterIndex);
            PreviewBox.Image.Save(saveFileDialog1.FileName, exportFormatMap[format.ToUpper()]);
        }
    }
}
