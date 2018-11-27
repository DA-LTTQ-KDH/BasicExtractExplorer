using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BasicExtractExplorer
{
    public partial class AddToArchive : Form
    {
        List<string> paths;
        public AddToArchive(List<string> paths)
        {
            InitializeComponent();
            this.Paths = paths;
            textBoxArchiveName.Text = Paths[0] + ".zip";

        }
        public List<string> Paths
        {
            get { return paths; }
            set { paths = value; }
        }
        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            saveFileDialog.FileName = textBoxArchiveName.Text;
            saveFileDialog.FileOk += (sd, ev) =>
            {
                textBoxArchiveName.Text = saveFileDialog.FileName;
            };
            saveFileDialog.ShowDialog();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            string archiveName = textBoxArchiveName.Text;
            SevenZip.SevenZipCompressor.SetLibraryPath("7z.dll");
            SevenZip.SevenZipCompressor zipCompressor = new SevenZip.SevenZipCompressor();
            zipCompressor.Compressing += ZipCompressor_Compressing;
            zipCompressor.BeginCompressDirectory(Paths[0], archiveName, "", "*", true);
            zipCompressor.CompressionFinished += ZipCompressor_CompressionFinished;
        }

        private void ZipCompressor_CompressionFinished(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ZipCompressor_Compressing(object sender, SevenZip.ProgressEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}
