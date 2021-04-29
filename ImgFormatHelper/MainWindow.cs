using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IWImgViewer
{
    public partial class MainWindow : Form
    {
        public string ImageName { get; private set; }

        Reader reader;

        Queue<Viewport> viewports = new Queue<Viewport>();

        EasyFilters convertFilters = new EasyFilters();

        public MainWindow()
        {
            InitializeComponent();

            this.DragEnter += new DragEventHandler(Form1_DragEnter);
            this.DragDrop += new DragEventHandler(Form1_DragDrop);

            Reader.OnBitmapRead += Viewer_OnBitmapRead;

            MetaDataLabel.Text = "Ready!\nDrag and drop an image\nover here to start!";
        }

        private void Viewer_OnBitmapRead(Bitmap arg1)
        {
            var viewport = new Viewport(this);
            viewport.Show();
            viewport.ShowImage(arg1);

            viewports.Enqueue(viewport);
        }

        void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            string file = files[0];
            ShowImage(file);
        }
        void ShowImage(string path)
        {
            try
            {
                var v = new Reader(path);
                reader = v;
                ImageName = System.IO.Path.GetFileNameWithoutExtension(path);

                MetaDataLabel.Text = $"{ImageName}\n{reader.ImageFormat}";
            }
            catch (FormatException e)
            {
                MessageBox.Show($"Format not supported:{e.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void closeAllButton_Click(object sender, EventArgs e)
        {
            while(viewports.Count > 0)
            {
                viewports.Dequeue().Close();
            }
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            int index = saveFileDialog1.FilterIndex - 1;
            if (index < 0 || index > reader.CurrentHeader.CompatibleDestinations.Count)
            {
                return;
            }

            var format = reader.CurrentHeader.CompatibleDestinations[index];
            format.From(reader.CurrentHeader);

            using(FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.Create))
            {
                using(BinaryWriter bw = new BinaryWriter(fs))
                {
                    format.Serialize(bw);
                    bw.Write(reader.Data);
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.C)
            {
                convertFilters = new EasyFilters();
                IEnumerable<IImageFileHeader> compatibleDestinationFormats = reader.CurrentHeader.CompatibleDestinations;
                foreach (var format in compatibleDestinationFormats)
                {
                    convertFilters.Add((format.FormatDescription, format.FormatExtension));
                }

                saveFileDialog1.DefaultExt = convertFilters.GetFilterExtensionByOneBasedIndex(1);
                saveFileDialog1.FileName = System.IO.Path.GetFileNameWithoutExtension(ImageName);
                saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                saveFileDialog1.Filter = convertFilters.ToString();
                saveFileDialog1.ShowDialog(this);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}