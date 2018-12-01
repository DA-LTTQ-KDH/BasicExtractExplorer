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
    
    public partial class ExtractTo : Form
    {
        SevenZipExtractor zipExtractor;
        public ExtractTo(string filePath)
        {
            InitializeComponent();
            SevenZipExtractor.SetLibraryPath("7z.dll");
            zipExtractor = new SevenZipExtractor(filePath);
            textBoxDestination.Text = Path.GetDirectoryName(filePath)+ "\\" + Path.GetFileNameWithoutExtension(filePath);
            textBoxDestination.Text = textBoxDestination.Text.Replace("\\\\", "\\");
        }
        private void btnDuyet_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.ShowNewFolderButton = true;
            folderBrowserDialog.Description = "Browse For Folder";
            if(folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxDestination.Text = folderBrowserDialog.SelectedPath;
            }
            
            //textBoxDestination.Text = folderBrowserDialog.SelectedPath;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //chuyển quá trình giải nén qua form Processing
            if(!zipExtractor.ArchiveFileData[0].Encrypted)// nếu file bị mã hóa
            {
                Processing processing = new Processing(zipExtractor, textBoxDestination.Text);
                processing.Show();
                this.Hide();
                processing.FormClosed += delegate { Application.Exit(); };
            }
            else
            {
                MessageBox.Show("Unable to decompress this file. Please enter a password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
