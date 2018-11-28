using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SevenZip;

namespace BasicExtractExplorer
{
    public partial class AddToArchive : Form
    {
        List<string> paths;
        SevenZipCompressor zipCompressor;
        OutArchiveFormat format;
        CompressionLevel level;
        public AddToArchive(List<string> paths)
        {
            InitializeComponent();
            SevenZipCompressor.SetLibraryPath("7z.dll");
            comboBoxLevel.SelectedIndex = 0;
            zipCompressor = new SevenZipCompressor();
            this.Paths = paths;
            textBoxArchiveName.Text = Paths[0] + ".zip";
            format = OutArchiveFormat.Zip;
        }
        public List<string> Paths
        {
            get { return paths; }
            set { paths = value; }
        }
        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            saveFileDialog.FileName = Path.GetFileName(textBoxArchiveName.Text);
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.InitialDirectory =saveFileDialog.FileName;
            saveFileDialog.Filter = "zip|*.zip|tar|*.tar|7z|*.7z|bzip2|*.bz2|gzip|*.gz|xz|*.xz";
            saveFileDialog.FileOk += (sd, ev) =>
            {
                textBoxArchiveName.Text = saveFileDialog.FileName;
                switch (saveFileDialog.FilterIndex)
                {
                    case 0:
                        format = OutArchiveFormat.Zip;
                        //textBoxArchiveName.Text = Path.ChangeExtension(textBoxArchiveName.Text, ".zip");
                        break;
                    case 1:
                        format = OutArchiveFormat.Zip;
                        //textBoxArchiveName.Text = Path.ChangeExtension(textBoxArchiveName.Text, ".zip");
                        break;
                    case 2:
                        format = OutArchiveFormat.Tar;
                        //textBoxArchiveName.Text = Path.ChangeExtension(textBoxArchiveName.Text, ".tar");
                        break;
                    case 3:
                        format = OutArchiveFormat.SevenZip;
                        //textBoxArchiveName.Text = Path.ChangeExtension(textBoxArchiveName.Text, ".7z");
                        break;
                    case 4:
                        format = OutArchiveFormat.BZip2;
                        //textBoxArchiveName.Text = Path.ChangeExtension(textBoxArchiveName.Text, ".bz2");
                        break;
                    case 5:
                        format = OutArchiveFormat.GZip;
                        //textBoxArchiveName.Text = Path.ChangeExtension(textBoxArchiveName.Text, ".gz");
                        break;
                    case 6:
                        //textBoxArchiveName.Text = Path.ChangeExtension(textBoxArchiveName.Text, ".xz");
                        format = OutArchiveFormat.XZ;
                        break;
                    default:
                        format = OutArchiveFormat.Zip;
                        break;
                }
            };
            saveFileDialog.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Lấy tên archive
            string archiveName = textBoxArchiveName.Text;
            zipCompressor.ArchiveFormat = format;
            zipCompressor.PreserveDirectoryRoot = true;
            zipCompressor.CompressionLevel = level;
            //chuyển quá trình nén qua form Processing
            Processing processing = new Processing(zipCompressor, Paths, archiveName);
            processing.Show();
            this.Hide();
            processing.FormClosed += delegate { Application.Exit(); };

        }

        private void comboBoxLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxLevel.SelectedIndex)
            {
                case 1:
                    level = CompressionLevel.High;
                    break;
                case 2:
                    level = CompressionLevel.Low;
                    break;
                case 3:
                    level = CompressionLevel.Ultra;
                    break;
                case 4:
                    level = CompressionLevel.None;
                    break;
                case 5:
                    level = CompressionLevel.Normal;
                    break;
                default:
                    level = CompressionLevel.Normal;
                    break;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
