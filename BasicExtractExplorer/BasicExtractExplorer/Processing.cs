using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SevenZip;

namespace BasicExtractExplorer
{
    public partial class Processing : Form
    {
        Thread t;
        SevenZipCompressor compressor;
        SevenZipExtractor extractor;
        List<string> paths;
        string archiveName;
        string folder;
        bool isCancel = false;
        public Processing(SevenZipCompressor sevenZipCompressor, List<string> paths, string archiveName)
        {
            InitializeComponent();
            this.Shown += Processing_Compress;
            //DoubleBuffered = true;
            this.Text = "Compressing";
            //thực hiện nén
            compressor = sevenZipCompressor;
            this.paths = paths;
            this.archiveName = archiveName;
        }
        public Processing(SevenZipExtractor sevenZipExtractor, string folder)
        {
            InitializeComponent();
            this.Shown += Processing_Extract;
            this.Text = "Extracting";
            extractor = sevenZipExtractor;
            this.archiveName = extractor.FileName;
            this.folder = folder;
            //Thực hiện giải nén
        }
        public void DoCompress()
        {
            Invoke((MethodInvoker)delegate
            {
                Refresh();
                labelArchiveName.Text = archiveName;
            });
            compressor.Compressing += ZipCompressor_Compressing;
            compressor.CompressionFinished += ZipCompressor_CompressionFinished;
            compressor.FileCompressionStarted += SevenZipCompressor_FileCompressionStarted;
            compressor.FileCompressionFinished += SevenZipCompressor_FileCompressionFinished;
            //if (File.Exists(archiveName)) File.Delete(archiveName);
            foreach (string path in paths)
            {
                compressor.CompressionMode = File.Exists(archiveName) ? SevenZip.CompressionMode.Append : SevenZip.CompressionMode.Create;
                //FileStream archive = new FileStream(archiveName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                compressor.DirectoryStructure = true;
                compressor.EncryptHeaders = true;
                compressor.DefaultItemName = archiveName;
                compressor.IncludeEmptyDirectories = true;
                try
                {
                    if (File.GetAttributes(path).HasFlag(FileAttributes.Directory)) // là thư mục
                    {
                        compressor.CompressDirectory(path, archiveName);
                    }
                    else
                    {
                        compressor.CompressFiles(archiveName, path);
                    }
                }
                catch(ThreadAbortException)
                {
                    MessageBox.Show("Aborted");
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
            }
            Invoke((MethodInvoker)delegate
            {
                Close();
            });
        }
        public void DoExtract()
        {
            Invoke((MethodInvoker)delegate
            {
                Refresh();
                labelArchiveName.Text = archiveName;
            //});
            extractor.Extracting += Extractor_Extracting;
            extractor.FileExtractionStarted += Extractor_FileExtractionStarted;
            extractor.ExtractionFinished += Extractor_ExtractionFinished;
            extractor.ExtractArchive(folder);
            //Invoke((MethodInvoker)delegate
            //{
                Close();
            });
        }

        private void Extractor_ExtractionFinished(object sender, EventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                //Nén xong đóng form
                progressBarTotal.Value = 100;
                //Refresh();
                //this.Close();
            });
        }

        private void Extractor_FileExtractionStarted(object sender, FileInfoEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                labelFile.Text = Path.GetFileName(e.FileInfo.FileName);
                progressBarTotal.Value = e.PercentDone;
                labelPercent.Text = e.PercentDone.ToString() + "%";
                e.Cancel = isCancel;
                Refresh();

            });
        }

        private void Extractor_Extracting(object sender, ProgressEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                //hiện quá trình nén
                progressBarTotal.Value = e.PercentDone;
                groupBox1.Refresh();
            });
        }

        private void SevenZipCompressor_FileCompressionFinished(object sender, EventArgs e)
        {
            //Refresh();
        }

        private void SevenZipCompressor_FileCompressionStarted(object sender, FileNameEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                labelFile.Text = e.FileName;
                progressBarTotal.Value = e.PercentDone;
                labelPercent.Text = e.PercentDone.ToString() + "%";
                e.Cancel = isCancel;
                Refresh();

            });
           
        }

        
        private void ZipCompressor_CompressionFinished(object sender, EventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                //Nén xong đóng form
                progressBarTotal.Value = 100;
                //Refresh();
                //this.Close();
            });
        }

        private void ZipCompressor_Compressing(object sender, SevenZip.ProgressEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                //hiện quá trình nén
                progressBarTotal.Value = e.PercentDone;
                groupBox1.Refresh();
            });
        }
        private void Processing_Compress(object sender, EventArgs e)
        {
            t = new Thread(new ThreadStart(DoCompress));
            //DoCompress();
            t.Start();
        }
        private void Processing_Extract(object sender, EventArgs e)
        {
            t = new Thread(new ThreadStart(DoExtract));  
            t.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (t.ThreadState == ThreadState.Suspended)
                t.Resume();
            t.Abort();
            compressor = null;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (t.ThreadState != ThreadState.Suspended)
            {
                
                button2.Text = "Resume";
                t.Suspend();
            }
            else
            {
                button2.Text = "Pause";
                t.Resume();
            }
                
        }
    }
}
