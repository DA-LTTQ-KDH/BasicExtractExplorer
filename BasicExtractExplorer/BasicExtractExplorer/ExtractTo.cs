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
        //SevenZipExtractor zipExtractor;
        List<int> fileIndex;
        string filePath;
        public ExtractTo(string filePath)
        {
            InitializeComponent();
            this.filePath = filePath;
            textBoxDestination.Text = Path.GetDirectoryName(filePath)+ "\\" + Path.GetFileNameWithoutExtension(filePath);
            textBoxDestination.Text = textBoxDestination.Text.Replace("\\\\", "\\");
        }
        public ExtractTo(string filePath, List<int> fileIndex)
        {
            InitializeComponent();
            this.fileIndex = fileIndex;
            this.filePath = filePath;
            textBoxDestination.Text = Path.GetDirectoryName(filePath) + "\\" + Path.GetFileNameWithoutExtension(filePath);
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
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if(fileIndex == null) //nếu không yêu cầu giải nén từng tệp chỉ định
            {
                Processing processing = new Processing(filePath, textBoxDestination.Text);
                processing.StartPosition = FormStartPosition.CenterScreen;
                processing.Show();
                this.Hide();
                processing.FormClosed += delegate { Application.Exit(); };
            }
            else
            {
                //chuyển quá trình giải nén qua form Processing
                Processing processing = new Processing(filePath, textBoxDestination.Text, fileIndex);
                processing.StartPosition = FormStartPosition.CenterScreen;
                processing.Show();
                this.Hide();
                processing.FormClosed += delegate { Application.Exit(); };
            }
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
