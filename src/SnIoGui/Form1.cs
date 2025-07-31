using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using SnIoGui.Services;
using System.Windows.Forms;

namespace SnIoGui
{
    public partial class Form1 : Form
    {
        private readonly SnIoGuiSettings _settings;
        private string? _lastLoadedFilePath;
        private string? _lastLoadedFileContent;

        public Form1(Microsoft.Extensions.Options.IOptions<SnIoGuiSettings> options)
        {
            _settings = options.Value;
            InitializeComponent();
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            this.Text = $"sensenet Importer V{version?.Major}.{version?.Minor}.{version?.Build}";
            
            if (_settings.Targets != null)
            {
                var targetsWithEmpty = CommonTools.CreateTargetDropdownDataSource(_settings.Targets);
                cmbTargets.DataSource = targetsWithEmpty;
                cmbTargets.DisplayMember = "Name";
                cmbTargets.SelectedIndex = 0;
            }
            
            txtPath.Text = string.Empty;
            cmbTargets.SelectedIndexChanged += cmbTargets_SelectedIndexChanged;
            UpdateSearchControls();
            
            // Handle proper application termination when Form1 is closed
            this.FormClosed += (s, e) => Application.Exit();
        }

        private void btnOpenAdminUI_Click(object sender, EventArgs e)
        {
            var selectedTarget = cmbTargets.SelectedItem as Target;
            CommonTools.OpenAdminUI(selectedTarget);
        }

        private void cmbTargets_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear tree and content
            tree.Nodes.Clear();
            txtContent.Clear();

            if (cmbTargets.SelectedIndex > 0 && _settings != null)
            {
                var selectedTarget = cmbTargets.SelectedItem as Target;
                if (selectedTarget != null && !string.IsNullOrEmpty(selectedTarget.ImportPath))
                {
                    txtPath.Text = selectedTarget.ImportPath;
                    // Automatically trigger btnGo_Click when a valid path is set
                    btnGo_Click(sender, e);
                }
                else
                {
                    txtPath.Text = string.Empty;
                }
            }
            else
            {
                txtPath.Text = string.Empty;
            }
            UpdateSearchControls();
        }

        // Show file or directory content in textarea when a node is selected
        // (Removed duplicate _lastLoadedFilePath and _lastLoadedFileContent fields)

        private void tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            txtContent.Clear();
            txtContent.ReadOnly = true;
            btnImport.Enabled = false;
            btnSaveContent.Enabled = false;
            _lastLoadedFilePath = null;
            _lastLoadedFileContent = null;
            
            if (e.Node?.Tag is string path)
            {
                if (System.IO.Directory.Exists(path))
                {
                    // Enable import only if path contains a segment named "Root"
                    btnImport.Enabled = path.Split(System.IO.Path.DirectorySeparatorChar)
                        .Any(segment => string.Equals(segment, "Root", StringComparison.OrdinalIgnoreCase));
                    
                    CommonTools.DisplayDirectoryListing(path, txtContent);
                }
                else if (System.IO.File.Exists(path))
                {
                    try
                    {
                        var fileContent = System.IO.File.ReadAllText(path);
                        txtContent.Text = fileContent;
                        txtContent.ReadOnly = false;
                        _lastLoadedFilePath = path;
                        _lastLoadedFileContent = fileContent;
                        btnSaveContent.Enabled = false;
                    }
                    catch { txtContent.Text = "[Could not read file]"; }
                }
            }
        }
        private void txtContent_TextChanged(object sender, EventArgs e)
        {
            if (_lastLoadedFilePath != null && !_lastLoadedFileContent.Equals(txtContent.Text))
            {
                btnSaveContent.Enabled = true;
            }
            else
            {
                btnSaveContent.Enabled = false;
            }
        }

        private void btnCollapseAll_Click(object sender, EventArgs e)
        {
            tree.CollapseAll();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(sender, e);
                e.Handled = true;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            CommonTools.SearchInTreeView(tree, txtSearch.Text);
        }

        private void UpdateSearchControls()
        {
            bool hasNodes = tree.Nodes.Count > 0;
            txtSearch.Enabled = hasNodes;
            btnSearch.Enabled = hasNodes;
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
            UpdateSearchControls();
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
            CommonTools.SaveFileContent(tree.SelectedNode, txtContent);
        }
        private void btnImport_Click(object sender, EventArgs e)
        {
            if (tree.SelectedNode?.Tag is string path)
            {
                // Get Target values
                var selectedTarget = cmbTargets.SelectedItem as Target;
                string selectedPath = path;
                // Open ImportForm modally, passing only Target and selectedPath
                string snioExe = _settings?.SnIO ?? "SnIO.exe";
                using (var importForm = new ImportForm(selectedTarget, selectedPath, snioExe))
                {
                    importForm.ShowDialog(this);
                }
            }
            else
            {
                MessageBox.Show("No item selected.", "Import", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSwitchToExport_Click(object sender, EventArgs e)
        {
            // Get Form2 singleton instance and show it, then hide Form1
            var form2 = Program.ServiceProvider.GetRequiredService<Form2>();
            this.Hide();
            form2.Show();
        }

        private void btnOpenLog_Click(object sender, EventArgs e)
        {
            CommonTools.OpenLatestLogFile();
        }
        // Read ApiKey button event handler
        private void btnReadApiKey_Click(object sender, EventArgs e)
        {
            var selectedTarget = cmbTargets.SelectedItem as Target;
            CommonTools.ReadApiKeyToClipboard(selectedTarget);
        }

        private async void btnHealth_Click(object sender, EventArgs e)
        {
            var selectedTarget = cmbTargets.SelectedItem as Target;
            
            // Disable the button during health check
            btnHealth.Enabled = false;
            btnHealth.Text = "⏳";

            try
            {
                await CommonTools.ShowHealthCheckAsync(selectedTarget, this);
            }
            finally
            {
                // Re-enable the button and return to default thermometer icon
                btnHealth.Enabled = true;
                btnHealth.Text = "🌡️";
            }
        }
    }
}
