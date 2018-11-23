using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace BasicExtractExplorer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            //Đảm bảo treeView trống
            if (treeView != null)
                treeView.Nodes.Clear();
            //Tạo node gốc: My Computer;
            TreeNode ThisPC = new TreeNode("This PC");
            treeView.Nodes.Add(ThisPC);
            //Thêm các node là các ổ drives vào node gốc
            string[] folders = Directory.GetLogicalDrives();
            foreach (string folder in folders)
            {
                ThisPC.Nodes.Add(folder);
            }
            ThisPC.Expand();
        }

        private string GetPath(string treeNodePath) //Lấy đường dẫn từ treeNodePath
        {
            //Loại bỏ phần "My Computer\"
            string[] nodes = treeNodePath.Split('\\');
            string result = nodes[1];
            for (int i = 2; i < nodes.Length; i++)
            {
                result += nodes[i] + '\\';
            }
            return result;
        }




        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (e.Node.Text.CompareTo("This PC") != 0)
                {
                    //Xóa các node con cũ của node được chọn
                    if (e.Node != null)
                        e.Node.Nodes.Clear();
                    //Lấy danh sách thư mục của thư mục hiện tại
                    string selected_node_path = GetPath(e.Node.FullPath);

                    //Kiểm tra đường dẫn hợp lệ
                    if (Directory.Exists(selected_node_path))
                    {
                        string[] folders = Directory.GetDirectories(selected_node_path);
                        //Lấy tên các thư mục là node con của node được chọn
                        foreach (string folder in folders)
                        {
                            e.Node.Nodes.Add(Path.GetFileName(folder));
                        }
                        //Hiện các files và folder lên listView
                        ShowFilesAndFolders();
                        //Hiện đường dẫn lên address
                        toolStripComboBox1.Text = GetPath(treeView.SelectedNode.FullPath);
                    }
                    else
                         if (treeView.SelectedNode.Parent.Text == "This PC") listView.Items.Clear();
                    e.Node.Expand();
                    
                }

            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("Not found this folder");
            }
            catch (System.UnauthorizedAccessException)
            {
                MessageBox.Show("Access is denied");
            }
        }

        private void ShowFilesAndFolders()
        {
            TreeNode node = treeView.SelectedNode;
            // Xóa các item cũ của listView
            if (listView != null)
                listView.Items.Clear();
            try
            {
                // Lấy danh sách thư mục
                string path = GetPath(node.FullPath);
                string[] folders = Directory.GetDirectories(path);
                // Thêm các thư mục vào listView
                foreach (string folder in folders)
                {
                    DirectoryInfo info = new DirectoryInfo(folder);
                    string[] Field = new string[5];
                    Field[0] = info.Name;
                    Field[1] = "Folder";
                    Field[2] = "...";
                    Field[3] = info.CreationTime.ToString();
                    Field[4] = info.LastWriteTime.ToString();
                    ListViewItem item = new ListViewItem(Field);
                    listView.Items.Add(item);
                }

                //Lấy danh sách các tệp tin
                string[] files = Directory.GetFiles(path);
                //Thêm các tệp vào listView
                foreach (string file in files)
                {
                    FileInfo info = new FileInfo(file);
                    string[] Field = new string[5];
                    Field[0] = info.Name;
                    string add = "";
                    if(info.Extension!="") add= info.Extension.ToString().Substring(1);
                    Field[1] = "File "+add;
                    Field[2] = (info.Length / 1024).ToString() + "KB";
                    Field[3] = info.CreationTime.ToString();
                    Field[4] = info.LastWriteTime.ToString();
                    ListViewItem item = new ListViewItem(Field);
                    listView.Items.Add(item);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong!!");
            }
        }
    }
}
