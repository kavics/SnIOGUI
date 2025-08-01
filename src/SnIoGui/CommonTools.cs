using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using SnIoGui.Services;
using System.Windows.Forms;
using SenseNet.Client;

namespace SnIoGui
{
    /// <summary>
    /// Wrapper class to hold both Content and cached JSON data
    /// </summary>
    public class ContentNodeData
    {
        public Content Content { get; set; }
        public string? CachedJsonData { get; set; }
        public bool JsonLoaded { get; set; }

        public ContentNodeData(Content content)
        {
            Content = content;
            CachedJsonData = null;
            JsonLoaded = false;
        }
        
        /// <summary>
        /// Clears the cached JSON data, forcing a reload on next access
        /// </summary>
        public void ClearCache()
        {
            CachedJsonData = null;
            JsonLoaded = false;
        }
    }

    /// <summary>
    /// Common tools and utilities shared between Form1 and Form2
    /// </summary>
    public static class CommonTools
    {
        /// <summary>
        /// Opens the Admin UI for the specified target
        /// </summary>
        /// <param name="selectedTarget">The target to open admin UI for</param>
        public static void OpenAdminUI(Target? selectedTarget)
        {
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

        /// <summary>
        /// Opens the latest log file with the default application
        /// </summary>
        public static void OpenLatestLogFile()
        {
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

        /// <summary>
        /// Reads the API key from the database and copies it to clipboard
        /// </summary>
        /// <param name="selectedTarget">The target to read API key from</param>
        public static void ReadApiKeyToClipboard(Target? selectedTarget)
        {
            const string connStrTemplate = "Data Source={0};Initial Catalog={1};Integrated Security=SSPI;Persist Security Info=False;TrustServerCertificate=True";
            
            if (selectedTarget == null || string.IsNullOrWhiteSpace(selectedTarget.DbServer) || string.IsNullOrWhiteSpace(selectedTarget.DbName))
            {
                MessageBox.Show("The selected Target is not properly configured (DbServer/DbName).", "Read ApiKey", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            string connStr = string.Format(connStrTemplate, selectedTarget.DbServer, selectedTarget.DbName);

            const string sql = @"SELECT TOP 1 [Value] FROM AccessTokens
WHERE Feature = 'ApiKey' AND UserId = 1 AND
    GETUTCDATE() > CreationDate AND GETUTCDATE() < ExpirationDate
ORDER BY ExpirationDate DESC";
            
            string? apiKey = null;
            try
            {
                using var conn = new SqlConnection(connStr);
                using var cmd = new SqlCommand(sql, conn);
                conn.Open();
                var result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                    apiKey = result.ToString();
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

        /// <summary>
        /// Shows the health check dialog for the specified target
        /// </summary>
        /// <param name="selectedTarget">The target to check health for</param>
        /// <param name="parentForm">The parent form for the dialog</param>
        /// <returns>Task representing the async operation</returns>
        public static async Task ShowHealthCheckAsync(Target? selectedTarget, Form parentForm)
        {
            if (selectedTarget == null || string.IsNullOrWhiteSpace(selectedTarget.Name) || selectedTarget.Name == "Select a target...")
            {
                MessageBox.Show("Please select a target first.", "Health Check", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using var healthForm = new HealthResultForm(selectedTarget);
                healthForm.ShowDialog(parentForm);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Health check failed:\n{ex.Message}", "Health Check Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Performs tree search functionality
        /// </summary>
        /// <param name="tree">The TreeView to search in</param>
        /// <param name="searchText">The search pattern</param>
        public static void SearchInTreeView(TreeView tree, string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return;

            try
            {
                var regex = new System.Text.RegularExpressions.Regex(searchText, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                var foundNodes = new List<TreeNode>();

                SearchNodes(tree.Nodes, regex, foundNodes);

                if (foundNodes.Count > 0)
                {
                    foreach (var node in foundNodes)
                    {
                        ExpandToNode(node);
                    }
                    tree.SelectedNode = foundNodes[0];
                    foundNodes[0].EnsureVisible();
                    tree.Focus();
                }
                else
                {
                    MessageBox.Show($"No files found matching pattern: {searchText}", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($"Invalid regex pattern: {ex.Message}", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Recursively searches through tree nodes
        /// </summary>
        private static void SearchNodes(TreeNodeCollection nodes, System.Text.RegularExpressions.Regex regex, List<TreeNode> foundNodes)
        {
            foreach (TreeNode node in nodes)
            {
                if (regex.IsMatch(node.Text))
                {
                    foundNodes.Add(node);
                }
                SearchNodes(node.Nodes, regex, foundNodes);
            }
        }

        /// <summary>
        /// Expands all parent nodes to make the specified node visible
        /// </summary>
        private static void ExpandToNode(TreeNode node)
        {
            var parent = node.Parent;
            while (parent != null)
            {
                parent.Expand();
                parent = parent.Parent;
            }
        }

        /// <summary>
        /// Saves file content from TextBox to the file system
        /// </summary>
        /// <param name="selectedNode">The selected tree node</param>
        /// <param name="textContent">The TextBox containing the content</param>
        public static void SaveFileContent(TreeNode? selectedNode, TextBox textContent)
        {
            if (selectedNode?.Tag is string path && System.IO.File.Exists(path))
            {
                try
                {
                    System.IO.File.WriteAllText(path, textContent.Text);
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

        /// <summary>
        /// Creates the target dropdown data source with empty option
        /// </summary>
        /// <param name="targets">List of available targets</param>
        /// <returns>List with empty option and all targets</returns>
        public static List<Target> CreateTargetDropdownDataSource(List<Target>? targets)
        {
            var targetsWithEmpty = new List<Target> { new Target { Name = "Select a target..." } };
            if (targets != null)
            {
                targetsWithEmpty.AddRange(targets);
            }
            return targetsWithEmpty;
        }

        /// <summary>
        /// Cleans up Unicode escape sequences in strings and replaces them with regular characters
        /// </summary>
        /// <param name="input">The string to clean</param>
        /// <returns>Cleaned string with regular characters instead of Unicode escapes</returns>
        public static string CleanUnicodeEscapes(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;
                
            // Replace common Unicode escape sequences with their actual characters
            return input
                .Replace("\\u0027", "'")  // apostrophe
                .Replace("\\u0022", "\"") // quotation mark
                .Replace("\\u0026", "&")  // ampersand
                .Replace("\\u003C", "<")  // less than
                .Replace("\\u003E", ">")  // greater than
                .Replace("\\u005C", "\\") // backslash
                .Replace("\\u002F", "/");  // forward slash
        }

        /// <summary>
        /// Displays directory and file listing for the specified path
        /// </summary>
        /// <param name="path">Directory path to list</param>
        /// <param name="textContent">TextBox to display the listing in</param>
        public static void DisplayDirectoryListing(string path, TextBox textContent)
        {
            try
            {
                var dirs = System.IO.Directory.GetDirectories(path)
                    .Select(System.IO.Path.GetFileName)
                    .Where(n => n != null)
                    .Cast<string>()
                    .ToList();
                    
                var files = System.IO.Directory.GetFiles(path)
                    .Select(System.IO.Path.GetFileName)
                    .Where(n => n != null)
                    .Cast<string>()
                    .ToList();
                    
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
                textContent.Lines = lines.ToArray();
            }
            catch
            {
                textContent.Text = "[Could not read directory]";
            }
        }

        /// <summary>
        /// Extracts Content from TreeNode.Tag, supporting both ContentNodeData and legacy Content formats
        /// </summary>
        /// <param name="nodeTag">The TreeNode.Tag object</param>
        /// <returns>Content object if found, null otherwise</returns>
        public static Content? GetContentFromNodeTag(object? nodeTag)
        {
            return nodeTag switch
            {
                ContentNodeData contentNodeData => contentNodeData.Content,
                Content directContent => directContent,
                _ => null
            };
        }

        /// <summary>
        /// Extracts ContentNodeData from TreeNode.Tag, creating a wrapper if needed for legacy Content
        /// </summary>
        /// <param name="nodeTag">The TreeNode.Tag object</param>
        /// <returns>ContentNodeData object if Content is found, null otherwise</returns>
        public static ContentNodeData? GetContentNodeDataFromTag(object? nodeTag)
        {
            return nodeTag switch
            {
                ContentNodeData contentNodeData => contentNodeData,
                Content directContent => new ContentNodeData(directContent),
                _ => null
            };
        }

        /// <summary>
        /// Creates a LoadCollectionRequest with standard settings including sorting by Name
        /// </summary>
        /// <param name="path">The content path to load children from</param>
        /// <param name="selectFields">Fields to select (optional, defaults to Name, Path, Type, Id)</param>
        /// <param name="orderByFields">Fields to order by (optional, defaults to Name)</param>
        /// <returns>Configured LoadCollectionRequest</returns>
        public static LoadCollectionRequest CreateSortedCollectionRequest(string path, 
            string[]? selectFields = null, 
            string[]? orderByFields = null)
        {
            return new LoadCollectionRequest
            {
                Path = path,
                Select = selectFields ?? new[] { "Name", "Path", "Type", "Id" },
                OrderBy = orderByFields ?? new[] { "Name" }
            };
        }
    }
}