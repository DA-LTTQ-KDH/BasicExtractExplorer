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
            //Lấy tên archive
            string archiveName = textBoxArchiveName.Text;
            //Load thư viện (64bit)
            SevenZip.SevenZipCompressor.SetLibraryPath("7z.dll");
            SevenZip.SevenZipCompressor zipCompressor = new SevenZip.SevenZipCompressor();
            //chuyển quá trình nén qua form Processing
            Processing processing = new Processing(zipCompressor, Paths, archiveName);
            processing.Show();
            this.Hide();
            processing.FormClosed += delegate { this.Close(); };

        }

        

    }
}
