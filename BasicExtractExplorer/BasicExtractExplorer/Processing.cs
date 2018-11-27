using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SevenZip;

namespace BasicExtractExplorer
{
    public partial class Processing : Form
    {
        public Processing(SevenZipCompressor sevenZipCompressor,List<string> paths, string archiveName)
        {
            InitializeComponent();
            //thực hiện nén
            sevenZipCompressor.Compressing += ZipCompressor_Compressing;
            sevenZipCompressor.CompressionFinished += ZipCompressor_CompressionFinished;
            sevenZipCompressor.BeginCompressDirectory(paths[0], archiveName, "", "*", true);
        }
        public Processing(SevenZipExtractor sevenZipExtractor, string folder)
        {
            InitializeComponent();
            //Thực hiện giải nén
        }
        private void ZipCompressor_CompressionFinished(object sender, EventArgs e)
        {
            //Nén xong đóng form
            this.Close();
        }

        private void ZipCompressor_Compressing(object sender, SevenZip.ProgressEventArgs e)
        {
            //hiện quá trình nén
        }
    }
}
