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
        int isCopying; //0: nothing, 1: đang copy, 2: đang cut
        List<string> fileSelectedName; //Danh sách tên các file/folder đang được chọn để copy hoặc cut
        List<string> typeSelectedFile; //Danh sách Loại các item đang được chọn để copy hoặc cut
        string old_selected_node_path; //Đường dẫn cũ tại folder chọn copy hoặc cut, trước khi Paste

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

            isCopying = 0; //không đang copy hay cut
            fileSelectedName = new List<string>();
            typeSelectedFile = new List<string>();
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
                MessageBox.Show("Something went wrong!");
            }
        }


        #region các hàm Delete
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode.Text == "This PC") return;
            //Tạo đường dẫn đến node đang được chọn ở treeView
            string selected_node_path = GetPath(treeView.SelectedNode.FullPath);

            
            while (listView.SelectedItems.Count > 0)   //Kiểm tra có item nào trong listView được chọn không
            {
                //Ghép đường dẫn đến item đang xét
                string path =selected_node_path + listView.SelectedItems[0].SubItems[0].Text;
                //kiem tra duong dan
                try
                {
                    if (listView.SelectedItems[0].SubItems[1].Text.CompareTo("Folder") == 0)
                    {
                        //Kiểm tra và xóa Folder
                        if (Directory.Exists(path))
                            Directory.Delete(path, true);
                    }
                    else
                    {
                        //Kiểm tra và xóa File
                        if (File.Exists(path))
                            File.Delete(path);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Access to "+path+" is denied", "warning");
                }

                listView.Items.Remove(listView.SelectedItems[0]); // Xóa SelectedItems ở đầu
            }
        }

        private void deleteDelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton4_Click(sender, e);
        }
        #endregion

        #region refresh
        private void toolStripButton12_Click(object sender, EventArgs e)
        { 
            treeView_AfterSelect(sender, new TreeViewEventArgs(treeView.SelectedNode));
        }
        #endregion

        #region View
        private void largeIconToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            listView.View = View.LargeIcon;
        }

        private void largeIconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            largeIconToolStripMenuItem1_Click(sender, e);
        }

        private void smallIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView.View = View.SmallIcon;
        }

        private void smallIconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            smallIconsToolStripMenuItem_Click(sender, e);
        }

        private void listToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            listView.View = View.List;
        }

        private void listToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listToolStripMenuItem1_Click(sender, e);
        }

        private void detailsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            listView.View = View.Details;
        }

        private void detailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            detailsToolStripMenuItem1_Click(sender, e);
        }
        #endregion

        #region Các hàm Copy
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count == 0) return;
            if (isCopying != 2) isCopying = 1; // Xác định xem đang copy hay cut
            old_selected_node_path = GetPath(treeView.SelectedNode.FullPath);
            if (!(Directory.Exists(old_selected_node_path))) //Kiểm tra đường dẫn tồn tại
            {
                isCopying = 0;
                return;
            }

            fileSelectedName.Clear();
            //typeSelectedFile.Clear();

            for (int i = 0; i < listView.SelectedItems.Count; i++)
            {
                fileSelectedName.Add(listView.SelectedItems[i].SubItems[0].Text);
                typeSelectedFile.Add(listView.SelectedItems[i].SubItems[1].Text);

            }
            //pathCut = selected_node_path;
        }

        private void copyCrltCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton1_Click(sender, e);
        }
        # endregion

        #region Các hàm Cut
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count == 0) return;
            isCopying = 2;
            toolStripButton1_Click(sender, e);
        }

        private void cutCrltXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton2_Click(sender, e);
        }
        #endregion

        #region Các hàm Paste
        private void toolStripButton3_Click(object sender, EventArgs e)
        {

            if (isCopying == 0 || fileSelectedName.Count == 0) return; //Không đang copy hay cut
            string selected_node_path = GetPath(treeView.SelectedNode.FullPath);

            string desPath = selected_node_path; //Địa chỉ đích
            if (desPath == old_selected_node_path && isCopying == 2) //Nếu Cut và Paste tại cùng thư mục thì thoát
            {
                isCopying = 0;
                return;
            }

            for (int i = 0; i < fileSelectedName.Count; i++)
            {
                if (typeSelectedFile[i].CompareTo("Folder") == 0) //Copy Folder
                {
                    int count = 0; //Đếm folder có tên trùng
                    //Kiểm tra xem có Folder nào trùng tên với item đang được Paste không
                    foreach (ListViewItem item in listView.Items)
                        if ((fileSelectedName[i] == item.Text) && (item.SubItems[1].Text.CompareTo("Folder") == 0))
                        {
                            count++;
                            break;
                        }

                    string NewName= fileSelectedName[i];
                    //Nếu có Folder trùng tên, đánh số thứ tự cho tên cũ
                    while (count > 0) 
                    {
                        int stop = 1; //Dừng lặp
                        NewName = fileSelectedName[i] + "(" + count.ToString() + ")";//Tên mới 
                        foreach (ListViewItem item in listView.Items)
                        {
                            if ((NewName == item.Text) && (item.SubItems[1].Text.CompareTo("Folder") == 0))
                            {
                                count++;
                                stop = 0;
                                break;
                            }
                        }
                        if (stop == 1) break;
                    }

                    Directory.CreateDirectory(desPath+NewName); //Tạo Folder copy

                    string old_fileSelectedPath = old_selected_node_path + fileSelectedName[i];
                    string desItem = desPath + NewName;
                    //Tạo các Folder con 
                    foreach (string dirPath in Directory.GetDirectories(old_fileSelectedPath, "*", SearchOption.AllDirectories))
                        Directory.CreateDirectory(dirPath.Replace(old_fileSelectedPath, desItem));

                    //Tạo các File con
                    foreach (string newPath in Directory.GetFiles(old_fileSelectedPath, "*.*", SearchOption.AllDirectories))
                        File.Copy(newPath, newPath.Replace(old_fileSelectedPath, desItem), true);

                }
                else //Copy File
                {
                    
                    int count = 0; //Đếm file có tên trùng
                    //Kiểm tra xem có File nào trùng tên với item đang được Paste không
                    foreach (ListViewItem item in listView.Items)
                        if ((fileSelectedName[i] == item.Text) && (item.SubItems[1].Text.CompareTo("Folder") != 0))
                        {
                            count++;
                            break;
                        }

                    string NewName = fileSelectedName[i]; //Tên mới
                    //Nếu có File trùng tên, đánh số thứ tự cho tên cũ
                    while (count > 0)
                    {
                        int stop = 1;
                        NewName = fileSelectedName[i].Substring(0, fileSelectedName[i].LastIndexOf(".")) + "(" + count.ToString() + ")";
                        foreach (ListViewItem item in listView.Items)
                            if((item.SubItems[1].Text.CompareTo("Folder") != 0) && (NewName==item.Text.Substring(0, item.Text.LastIndexOf("."))))
                            {
                                count++;
                                stop = 0;
                                break;
                            }

                        if (stop == 1)
                        {
                            NewName += fileSelectedName[i].Substring(fileSelectedName[i].LastIndexOf("."));
                            break;
                        }
                    }
                    
                    //Copy File tới địa chỉ mới
                    try
                    {
                        File.Copy(old_selected_node_path + fileSelectedName[i], desPath + NewName);
                    }
                    catch
                    {
                        MessageBox.Show("Access is denied","Warning");
                    }
                    
                }
                
            }

            toolStripButton12_Click(sender, e); //Refresh lại listView

            //Nếu đang cut: Xóa items bị cut
            if (isCopying == 2) 
            {
                for (int i = 0; i < fileSelectedName.Count; i++)
                    if (typeSelectedFile[i] == "Folder")
                    {
                        Directory.Delete(old_selected_node_path+fileSelectedName[i], true);
                    }
                    else
                    {
                        File.Delete(old_selected_node_path + fileSelectedName[i]);
                    }
                isCopying = 0;
            }

        }

        private void pasteCrltVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton3_Click(sender, e);
        }
        #endregion

        

        //Up
        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode.Text == "This PC") return;
            treeView.SelectedNode = treeView.SelectedNode.Parent;
            if (treeView.SelectedNode.Text == "This PC")
                listView.Items.Clear();

        }

        //Close App
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Select All items
        private void selectAllCrltAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView.SelectedItems.Clear();
            foreach (ListViewItem item in listView.Items)
            {
                item.Selected = true;
            }
        }

        #region Rename
        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {

            listView.SelectedItems[0].BeginEdit();

        }

        private void listView_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            //Lấy đường dẫn của thư mục hiện tại
            string currentPath = GetPath(treeView.SelectedNode.FullPath);
            //Lấy tên trước chỉnh sửa
            string currentName = listView.SelectedItems[0].Text;
            //Lấy tên sau khi chỉnh sửa
            string newName = e.Label;

            string extension = ""; //Phần mở rộng trong tên
            //Lấy phần mở rộng nếu là File
            if ((listView.SelectedItems[0].SubItems[1].Text != "Folder") && (listView.SelectedItems[0].Text.LastIndexOf(".") >= 0))
                extension = listView.SelectedItems[0].Text.Substring(listView.SelectedItems[0].Text.LastIndexOf("."));
            //Kiểm tra có tồn tại tên giống newName không
            if (!Directory.Exists(currentPath + newName) && !File.Exists(currentPath + newName))
            {
                bool change = true;
                if (extension != "") //Nếu là File
                {
                    if ((newName.Length < currentName.Length) || (newName.Substring(newName.Length - currentName.Length) != extension))
                    {
                        DialogResult result = MessageBox.Show("New Name can change the type of this file!", "Warning", MessageBoxButtons.YesNo);
                        if (result == System.Windows.Forms.DialogResult.No)
                            change = false;
                    }
                }

                if (change == true)
                    Directory.Move(currentPath + listView.SelectedItems[0].Text, currentPath + newName);
            }
            else
                MessageBox.Show("Tên \"" + newName + "\" đã tồn tại!", "Can't rename", MessageBoxButtons.OK);

            e.CancelEdit = true; //Kết thúc Edit
            //Làm mới lại listView
            toolStripButton12.PerformClick();
        }
        #endregion
    }
}
