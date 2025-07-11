using Microsoft.Data.SqlClient;
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
                var targetsWithEmpty = new List<Target> { new Target { Name = string.Empty } };
                targetsWithEmpty.AddRange(_settings.Targets);
                cmbTargets.DataSource = targetsWithEmpty;
                cmbTargets.DisplayMember = "Name";
                cmbTargets.SelectedIndex = 0;
            }
            txtPath.Text = string.Empty;
            cmbTargets.SelectedIndexChanged += cmbTargets_SelectedIndexChanged;
        }

        private void btnOpenAdminUI_Click(object sender, EventArgs e)
        {
            var selectedTarget = cmbTargets.SelectedItem as Target;
            if (selectedTarget == null || string.IsNullOrWhiteSpace(selectedTarget.AdminUrl))
            {
                MessageBox.Show("The selected Target does not have an AdminUrl configured.", "Open AdminUI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = selectedTarget.AdminUrl,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open AdminUI:\n{ex.Message}", "Open AdminUI", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
        private void btnOpenLog_Click(object sender, EventArgs e)
        {
            // Find latest log file in the running application's logs directory, with robust null-safety
            string? exePath = null;
            string? exeDir = null;
            try
            {
                exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                exeDir = System.IO.Path.GetDirectoryName(exePath);
            }
            catch { }
            if (string.IsNullOrEmpty(exeDir))
            {
                MessageBox.Show("Could not determine application directory.", "Log", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string logsDir = System.IO.Path.Combine(exeDir, "logs");
            if (!System.IO.Directory.Exists(logsDir))
            {
                MessageBox.Show("Logs directory not found.", "Log", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var logFiles = System.IO.Directory.GetFiles(logsDir)
                .OrderByDescending(f => System.IO.File.GetLastWriteTimeUtc(f))
                .ToList();
            if (logFiles.Count == 0)
            {
                MessageBox.Show("No log files found.", "Log", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string latestLog = logFiles.First();
            // Open with default application using ProcessHelper
            try
            {
                if (!ProcessHelper.OpenFileWithDefaultApp(latestLog))
                {
                    MessageBox.Show("Failed to open log file with the default application.", "Log", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open log file:\n{ex.Message}", "Log", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Read ApiKey button event handler
        private void btnReadApiKey_Click(object sender, EventArgs e)
        {
            // Connection string template
            string connStrTemplate = "Data Source={0};Initial Catalog={1};Integrated Security=SSPI;Persist Security Info=False;TrustServerCertificate=True";
            // Get from selected target
            var selectedTarget = cmbTargets.SelectedItem as Target;
            if (selectedTarget == null || string.IsNullOrWhiteSpace(selectedTarget.DbServer) || string.IsNullOrWhiteSpace(selectedTarget.DbName))
            {
                MessageBox.Show("The selected Target is not properly configured (DbServer/DbName).", "Read ApiKey", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string connStr = string.Format(connStrTemplate, selectedTarget.DbServer, selectedTarget.DbName);

            // SQL script to read ApiKey
            string sql = @"SELECT TOP 1 [Value] FROM AccessTokens
WHERE Feature = 'ApiKey' AND UserId = 1 AND
    GETUTCDATE() > CreationDate AND GETUTCDATE() < ExpirationDate
ORDER BY ExpirationDate DESC";
            string? apiKey = null;
            try
            {
                using (var conn = new SqlConnection(connStr))
                using (var cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        apiKey = result.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"SQL error: {ex.Message}", "Read ApiKey", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!string.IsNullOrEmpty(apiKey))
            {
                try
                {
                    Clipboard.SetText(apiKey);
                    MessageBox.Show("ApiKey copied to clipboard!", "Read ApiKey", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Clipboard error: {ex.Message}", "Read ApiKey", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("No ApiKey found.", "Read ApiKey", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
