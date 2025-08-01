using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using SenseNet.Client;
using SenseNet.Extensions.DependencyInjection;
using SnIoGui.Services;
using System.Windows.Forms;

namespace SnIoGui
{
    public partial class Form2 : Form
    {
        private readonly SnIoGuiSettings _settings;
        private readonly IRepositoryCollection _repositoryCollection;
        private string? _lastLoadedFilePath;
        private string? _lastLoadedFileContent;
        
        // Debouncing timer for tree selection
        private System.Windows.Forms.Timer _selectionTimer;
        private TreeNode? _pendingSelectedNode;

        public Form2(Microsoft.Extensions.Options.IOptions<SnIoGuiSettings> options, IRepositoryCollection repositoryCollection)
        {
            _settings = options.Value;
            _repositoryCollection = repositoryCollection;
            
            InitializeComponent();
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            this.Text = $"sensenet Exporter V{version?.Major}.{version?.Minor}.{version?.Build}";
            
            // Initialize debouncing timer
            _selectionTimer = new System.Windows.Forms.Timer();
            _selectionTimer.Interval = 150; // 150ms debounce
            _selectionTimer.Tick += SelectionTimer_Tick;
            
            if (_settings.Targets != null)
            {
                var targetsWithEmpty = CommonTools.CreateTargetDropdownDataSource(_settings.Targets);
                cmbTargets.DataSource = targetsWithEmpty;
                cmbTargets.DisplayMember = "Name";
                cmbTargets.SelectedIndex = 0;
            }
            
            txtPath.Text = string.Empty;
            cmbTargets.SelectedIndexChanged += cmbTargets_SelectedIndexChanged;
            
            // Add BeforeExpand event handler for lazy loading
            tree.BeforeExpand += tree_BeforeExpand;
            
            UpdateSearchControls();
            
            // When Form2 is closed, show Form1 again
            this.FormClosed += (s, e) => {
                _selectionTimer?.Dispose(); // Clean up timer
                var form1 = Program.ServiceProvider.GetRequiredService<Form1>();
                if (!form1.Visible)
                    form1.Show();
            };
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
                if (selectedTarget != null && !string.IsNullOrEmpty(selectedTarget.Url))
                {
                    txtPath.Text = selectedTarget.Url;

                    // Get the repository for this target
                    var repository = GetRepositoryByTargetName(selectedTarget.Name);
                    if(repository == null)
                    {
                        MessageBox.Show($"Repository for target '{selectedTarget.Name}' not found.", "Repository Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // For export form, we'll show repository structure
                    AddRepositoryTreeNode(repository);
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

        private async void tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            txtContent.Clear();
            txtContent.ReadOnly = true;
            btnExport.Enabled = false;
            btnSaveContent.Enabled = false;
            _lastLoadedFilePath = null;
            _lastLoadedFileContent = null;

            if (e.Node?.Tag is Content content)
            {
                // Show content information in the text area
                var contentInfo = $"Name: {content.Name}\n" +
                                $"Path: {content.Path}\n" +
                                $"Type: {content.Type}\n" +
                                $"Id: {content.Id}\n" +
                                $"Creation Date: {content.CreationDate}\n" +
                                $"Modification Date: {content.ModificationDate}\n";
                
                if (!string.IsNullOrEmpty(content.DisplayName))
                    contentInfo += $"Display Name: {content.DisplayName}\n";
                
                txtContent.Text = contentInfo;
                btnExport.Enabled = true;  // Enable export for selected content
                
                // Set up debounced loading for this node if it has placeholder children
                if (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Text == "...loading")
                {
                    _pendingSelectedNode = e.Node;
                    _selectionTimer.Stop();
                    _selectionTimer.Start();
                }
            }
            else if (e.Node?.Text == "...loading")
            {
                txtContent.Text = "Loading children...";
            }
            else if (e.Node?.Text.StartsWith("Error:") == true)
            {
                txtContent.Text = e.Node.Text;
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

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (tree.SelectedNode?.Tag is Content content)
            {
                // Get Target values
                var selectedTarget = cmbTargets.SelectedItem as Target;
                string selectedPath = content.Path;
                // TODO: Implement export functionality
                MessageBox.Show($"Export functionality will be implemented for:\nTarget: {selectedTarget?.Name}\nContent: {content.Name}\nPath: {selectedPath}\nType: {content.Type}", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (tree.SelectedNode?.Tag is string path)
            {
                // Fallback for file system paths (existing functionality)
                var selectedTarget = cmbTargets.SelectedItem as Target;
                string selectedPath = path;
                MessageBox.Show($"Export functionality will be implemented for:\nTarget: {selectedTarget?.Name}\nPath: {selectedPath}", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No item selected.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSwitchToImport_Click(object sender, EventArgs e)
        {
            // Get Form1 singleton instance and show it, then hide Form2
            var form1 = Program.ServiceProvider.GetRequiredService<Form1>();
            this.Hide();
            form1.Show();
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

        private IRepository? GetRepositoryByTargetName(string targetName)
        {
            try
            {
                var repository = _repositoryCollection.GetRepositoryAsync(targetName, default).GetAwaiter().GetResult();
                return repository;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error accessing repository '{targetName}': {ex.Message}", "Repository Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }

        private async void AddRepositoryTreeNode(IRepository repository)
        {
            try
            {
                // First load the Root content itself
                var rootContent = await repository.LoadContentAsync("/Root", CancellationToken.None);
                
                if (rootContent != null)
                {
                    // Create the root node with placeholder
                    var rootNode = CreateContentTreeNode(rootContent);
                    tree.Nodes.Add(rootNode);
                    
                    // Expand the root node to show its children immediately
                    rootNode.Expand();
                    UpdateSearchControls();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading repository content: {ex.Message}", "Repository Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void tree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            // Check if this node has a placeholder loading node
            if (e.Node?.Nodes.Count == 1 && e.Node.Nodes[0].Text == "...loading")
            {
                // Get the content from the node's Tag
                if (e.Node.Tag is Content content)
                {
                    // Capture UI data on the main thread before going to background
                    var selectedTarget = cmbTargets.SelectedItem as Target;
                    
                    // Don't cancel the expansion - let the node expand showing the loading placeholder
                    // Load children asynchronously after the expansion
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            // Use the captured target data (no UI access here)
                            if (selectedTarget != null)
                            {
                                var repository = GetRepositoryByTargetName(selectedTarget.Name);
                                if (repository != null)
                                {
                                    // Load children of this content
                                    var request = new LoadCollectionRequest 
                                    { 
                                        Path = content.Path 
                                    };
                                    
                                    var contentCollection = await repository.LoadCollectionAsync(request, CancellationToken.None);
                                    
                                    // Update UI on the main thread
                                    this.Invoke(() =>
                                    {
                                        // Remove the placeholder node
                                        e.Node.Nodes.Clear();
                                        
                                        if (contentCollection != null)
                                        {
                                            foreach (var childContent in contentCollection)
                                            {
                                                // Create tree node with content name and store the Content object in Tag
                                                var childNode = CreateContentTreeNode(childContent);
                                                e.Node.Nodes.Add(childNode);
                                            }
                                        }
                                        
                                        // If no children were loaded, this becomes a leaf node (no action needed)
                                    });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            // Update UI on the main thread with error
                            this.Invoke(() =>
                            {
                                // Remove the placeholder and show error
                                e.Node.Nodes.Clear();
                                var errorNode = new TreeNode($"Error: {ex.Message}")
                                {
                                    ForeColor = System.Drawing.Color.Red
                                };
                                e.Node.Nodes.Add(errorNode);
                            });
                        }
                    });
                }
            }
        }

        private TreeNode CreateContentTreeNode(Content content)
        {
            var contentNode = new TreeNode(content.Name) 
            { 
                Tag = content
            };
            
            // Only add placeholder for content types that might have children
            // This is a heuristic - we could improve this by checking content type
            // For now, we add placeholder to all nodes since we don't know without checking
            var loadingNode = new TreeNode("...loading")
            {
                ForeColor = System.Drawing.Color.Gray
            };
            contentNode.Nodes.Add(loadingNode);
            
            return contentNode;
        }

        private async void SelectionTimer_Tick(object sender, EventArgs e)
        {
            _selectionTimer.Stop();
            
            if (_pendingSelectedNode?.Tag is Content content && 
                _pendingSelectedNode.Nodes.Count == 1 && 
                _pendingSelectedNode.Nodes[0].Text == "...loading")
            {
                // Trigger lazy loading for the selected node
                await LoadChildrenForNode(_pendingSelectedNode, content);
            }
            
            _pendingSelectedNode = null;
        }

        private async Task LoadChildrenForNode(TreeNode node, Content content)
        {
            try
            {
                // Capture UI data on the main thread
                var selectedTarget = cmbTargets.SelectedItem as Target;
                
                if (selectedTarget != null)
                {
                    var repository = GetRepositoryByTargetName(selectedTarget.Name);
                    if (repository != null)
                    {
                        // Load children of this content
                        var request = new LoadCollectionRequest 
                        { 
                            Path = content.Path 
                        };
                        
                        var contentCollection = await repository.LoadCollectionAsync(request, CancellationToken.None);
                        
                        // Update UI - Remove the placeholder node
                        node.Nodes.Clear();
                        
                        if (contentCollection != null)
                        {
                            foreach (var childContent in contentCollection)
                            {
                                // Create tree node with content name and store the Content object in Tag
                                var childNode = CreateContentTreeNode(childContent);
                                node.Nodes.Add(childNode);
                            }
                        }
                        
                        // If no children were loaded, this becomes a leaf node (no action needed)
                    }
                }
            }
            catch (Exception ex)
            {
                // Update UI on the main thread with error
                this.Invoke(() =>
                {
                    // Remove the placeholder and show error
                    node.Nodes.Clear();
                    var errorNode = new TreeNode($"Error: {ex.Message}")
                    {
                        ForeColor = System.Drawing.Color.Red
                    };
                    node.Nodes.Add(errorNode);
                });
            }
        }
    }
}