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
    public partial class Processing : Form
    {
        SevenZipCompressor compressor;
        List<string> paths;
        string archiveName;
        public Processing(SevenZipCompressor sevenZipCompressor, List<string> paths, string archiveName)
        {
            InitializeComponent();
            DoubleBuffered = true;
            //thực hiện nén
            compressor = sevenZipCompressor;
            this.paths = paths;
            this.archiveName = archiveName;
        }

        public void DoCompress()
        {
            Refresh();
            labelArchiveName.Text = archiveName;
            compressor.Compressing += ZipCompressor_Compressing;
            compressor.CompressionFinished += ZipCompressor_CompressionFinished;
            compressor.FileCompressionStarted += SevenZipCompressor_FileCompressionStarted;
            compressor.FileCompressionFinished += SevenZipCompressor_FileCompressionFinished;
            if (File.Exists(archiveName)) File.Delete(archiveName);
            foreach (string path in paths)
            {
                compressor.CompressionMode = File.Exists(archiveName) ? SevenZip.CompressionMode.Append : SevenZip.CompressionMode.Create;
                //FileStream archive = new FileStream(archiveName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                compressor.DirectoryStructure = true;
                compressor.EncryptHeaders = true;
                compressor.DefaultItemName = archiveName;
                compressor.IncludeEmptyDirectories = true;
                if (!File.Exists(path)) // Không phải file -> thư mục
                {
                    compressor.CompressDirectory(path, archiveName);
                }
                else
                {
                    compressor.CompressFiles(archiveName, path);
                }
                
            }
            
        }
        private void SevenZipCompressor_FileCompressionFinished(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            progressBarFile.Value = 100;
            Refresh();
        }

        private void SevenZipCompressor_FileCompressionStarted(object sender, FileNameEventArgs e)
        {
            labelFile.Text = e.FileName;
            progressBarTotal.Value = e.PercentDone;
            Refresh();
        }

        public Processing(SevenZipExtractor sevenZipExtractor, string folder)
        {
            InitializeComponent();
            //Thực hiện giải nén
        }
        private void ZipCompressor_CompressionFinished(object sender, EventArgs e)
        {
            //Nén xong đóng form
            progressBarTotal.Value = 100;
            Refresh();
            //this.Close();
        }

        private void ZipCompressor_Compressing(object sender, SevenZip.ProgressEventArgs e)
        {
            //hiện quá trình nén
            progressBarTotal.Value += e.PercentDone;
            Refresh();
        }

        private void Processing_Load(object sender, EventArgs e)
        {
            
        }

        private void Processing_Shown(object sender, EventArgs e)
        {
            DoCompress();
        }

        private void Processing_Activated(object sender, EventArgs e)
        {
            
        }
    }
}
