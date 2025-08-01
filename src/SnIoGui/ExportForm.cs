using System;
using System.Windows.Forms;
using System.Linq;

namespace SnIoGui
{
    public partial class ExportForm : Form
    {
        private Button btnCancel;
        
        // Cancel button event handler
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public string TargetName
        {
            get => txtTargetName.Text;
            set => txtTargetName.Text = value;
        }
        public string TargetUrl
        {
            get => txtTargetUrl.Text;
            set => txtTargetUrl.Text = value;
        }
        public string SelectedPath
        {
            get => txtSelectedPath.Text;
            set => txtSelectedPath.Text = value;
        }
        public string ExportPath
        {
            get => txtExportPath.Text;
            set => txtExportPath.Text = value;
        }
        public string Script
        {
            get => txtScript.Text;
            set => txtScript.Text = value;
        }

        private readonly Target _target;
        private readonly string _snioExe;
        private string _generatedScript;

        public ExportForm(Target target, string selectedPath, string snioExe)
        {
            InitializeComponent();
            btnViewScript.Click += BtnViewScript_Click!;
            btnExecuteScript.Click += BtnExecuteScript_Click!;
            _target = target;
            _snioExe = snioExe;
            TargetName = target?.Name ?? string.Empty;
            TargetUrl = target?.Url ?? string.Empty;
            SelectedPath = selectedPath;
            ExportPath = target?.ExportPath ?? string.Empty;
            
            // Calculate target path (the content path to export from)
            var targetPath = CalculateTargetPath(selectedPath);
            
            // Script generation in constructor, using the calculated targetPath and apiKey
            string apiKey = target?.ApiKey ?? string.Empty;
            string exportPath = target?.ExportPath ?? string.Empty;
            _generatedScript = GenerateScript(TargetName, TargetUrl, SelectedPath, targetPath, apiKey, exportPath, _snioExe);
            btnCancel.Click += BtnCancel_Click;

            // Set CancelButton property so ESC triggers Cancel
            this.CancelButton = btnCancel;
        }

        private string CalculateTargetPath(string selectedPath)
        {
            // For export, we use the selected path as the target path
            // Convert from Windows path format to sensenet path format if needed
            if (string.IsNullOrEmpty(selectedPath)) return "/Root";
            
            // If it's already a sensenet path (starts with /), return as is
            if (selectedPath.StartsWith("/")) return selectedPath;
            
            // If it's a file system path, we'll need to convert it
            // For now, assume it's already the correct sensenet path format
            return selectedPath.Replace("\\", "/");
        }

        private void BtnViewScript_Click(object sender, EventArgs e)
        {
            // Set multiline text in txtScript, preserving line breaks
            txtScript.Lines = _generatedScript.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);
        }

        private void BtnExecuteScript_Click(object sender, EventArgs e)
        {
            try
            {
                // Save script to a temporary file
                string tempScriptPath = System.IO.Path.GetTempFileName() + ".ps1";
                System.IO.File.WriteAllText(tempScriptPath, _generatedScript);

                // Start PowerShell so it closes after execution (no -NoExit)
                bool started = ProcessHelper.RunPowerShellScriptFileInWindow(tempScriptPath, keepOpen: false);
                if (started)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to start PowerShell process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while starting PowerShell:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GenerateScript(string targetName, string targetUrl, string selectedPath, string targetPath, string apiKey, string exportPath, string snioExe)
        {
            // Calculate the actual export path by combining the base export path with the parent path of the selected path
            string actualExportPath = CalculateActualExportPath(selectedPath, exportPath);
            
            // Script template with placeholders for export operation
            const string template =
                "$Exe = \"{SnIO}\"\n" +
                "$Url = \"{TargetUrl}\"\n" +
                "$ApiKey = \"{ApiKey}\"\n" +
                "$SourcePath = \"{TargetPath}\"\n" +
                "$ExportPath = \"{ExportPath}\"\n\n" +
                "& $Exe EXPORT --DISPLAY:LEVEL Verbose -SOURCE -URL $Url -PATH $SourcePath -APIKEY $ApiKey -TARGET $ExportPath\n" +
                "Write-Host 'Press any key to close...'\n" +
                "$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown')";

            string script = template
                .Replace("{SnIO}", snioExe)
                .Replace("{TargetUrl}", targetUrl)
                .Replace("{ApiKey}", apiKey)
                .Replace("{TargetPath}", targetPath)
                .Replace("{ExportPath}", actualExportPath);
            return script;
        }

        private string CalculateActualExportPath(string selectedPath, string baseExportPath)
        {
            // If selectedPath is empty or just "/Root", use the base export path
            if (string.IsNullOrEmpty(selectedPath) || selectedPath == "/Root")
            {
                return baseExportPath;
            }

            // Remove the last segment from the selected path to get the parent path
            // Example: "/Root/IMS/Public/Manfred/Renters" -> "/Root/IMS/Public/Manfred"
            string parentPath = GetParentPath(selectedPath);
            
            // Combine base export path with the parent path
            // Remove trailing slashes from baseExportPath to avoid double slashes
            string cleanBaseExportPath = baseExportPath.TrimEnd('\\', '/');
            
            // Convert sensenet path to Windows path format for the filesystem
            string windowsParentPath = parentPath.Replace("/", "\\");
            
            return $"{cleanBaseExportPath}{windowsParentPath}";
        }

        private string GetParentPath(string path)
        {
            // Handle empty or root cases
            if (string.IsNullOrEmpty(path) || path == "/" || path == "/Root")
            {
                return "/Root";
            }

            // Split the path and remove the last segment
            var parts = path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            
            if (parts.Length <= 1)
            {
                return "/Root";
            }

            // Rebuild the path without the last segment
            return "/" + string.Join("/", parts.Take(parts.Length - 1));
        }
    }
}