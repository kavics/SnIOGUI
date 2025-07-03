namespace SnIoGui
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Show file or directory content in textarea when a node is selected
        private void tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            txtContent.Clear();
            txtContent.ReadOnly = true;
            if (e.Node?.Tag is string path)
            {
                if (System.IO.Directory.Exists(path))
                {
                    try
                    {
                        var dirs = System.IO.Directory.GetDirectories(path).Select(System.IO.Path.GetFileName).Where(n => n != null).Cast<string>().ToList();
                        var files = System.IO.Directory.GetFiles(path).Select(System.IO.Path.GetFileName).Where(n => n != null).Cast<string>().ToList();
                        var lines = new List<string>();
                        if (dirs.Count > 0)
                        {
                            lines.Add("DIRECTORIES");
                            lines.AddRange(dirs.Select(d => "    " + d));
                        }
                        if (files.Count > 0)
                        {
                            lines.Add("FILES");
                            lines.AddRange(files.Select(f => "    " + f));
                        }
                        txtContent.Lines = lines.ToArray();
                    }
                    catch { }
                }
                else if (System.IO.File.Exists(path))
                {
                    try
                    {
                        txtContent.Text = System.IO.File.ReadAllText(path);
                        txtContent.ReadOnly = false;
                    }
                    catch { txtContent.Text = "[Could not read file]"; }
                }
            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            tree.Nodes.Clear();
            txtContent.Clear();
            string rootPath = txtPath.Text;
            if (System.IO.Directory.Exists(rootPath))
            {
                // Do not show the root directory, only its children
                foreach (var dir in System.IO.Directory.GetDirectories(rootPath))
                {
                    var dirNode = new TreeNode(System.IO.Path.GetFileName(dir)) { Tag = dir };
                    tree.Nodes.Add(dirNode);
                    AddDirectoryNodes(dirNode, dir);
                    dirNode.Expand(); // open first level by default
                }
                foreach (var file in System.IO.Directory.GetFiles(rootPath))
                {
                    var fileNode = new TreeNode(System.IO.Path.GetFileName(file)) { Tag = file };
                    tree.Nodes.Add(fileNode);
                }
            }
        }
        private void AddDirectoryNodes(TreeNode parentNode, string path)
        {
            try
            {
                foreach (var dir in System.IO.Directory.GetDirectories(path))
                {
                    var dirNode = new TreeNode(System.IO.Path.GetFileName(dir)) { Tag = dir };
                    parentNode.Nodes.Add(dirNode);
                    AddDirectoryNodes(dirNode, dir);
                }
                foreach (var file in System.IO.Directory.GetFiles(path))
                {
                    var fileNode = new TreeNode(System.IO.Path.GetFileName(file)) { Tag = file };
                    parentNode.Nodes.Add(fileNode);
                }
            }
            catch { /* ignore errors (e.g. access denied) */ }
        }
        // Show a MessageBox with the selected node's path when Import is clicked
        // Save the content of txtContent to a file
        private void btnSaveContent_Click(object sender, EventArgs e)
        {
            if (tree.SelectedNode?.Tag is string path && System.IO.File.Exists(path))
            {
                try
                {
                    System.IO.File.WriteAllText(path, txtContent.Text);
                    MessageBox.Show("Content saved.", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving file:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Select a file node to save.", "Save", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void btnImport_Click(object sender, EventArgs e)
        {
            if (tree.SelectedNode?.Tag is string path)
            {
                MessageBox.Show(path, "Selected Path", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No item selected.", "Import", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
