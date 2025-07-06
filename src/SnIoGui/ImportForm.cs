using System;
using System.Windows.Forms;
using System.Linq;

namespace SnIoGui
{
    public partial class ImportForm : Form
    {
        private Label lblTargetPath;
        private TextBox txtTargetPath;
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
        public string Script
        {
            get => txtScript.Text;
            set => txtScript.Text = value;
        }

        private readonly Target _target;
        private readonly string _snioExe;
        public ImportForm(Target target, string selectedPath, string snioExe)
        {
            InitializeComponent();
            btnViewScript.Click += BtnViewScript_Click;
            btnExecuteScript.Click += BtnExecuteScript_Click;
            _target = target;
            _snioExe = snioExe;
            TargetName = target?.Name ?? string.Empty;
            TargetUrl = target?.Url ?? string.Empty;
            SelectedPath = selectedPath;
            // Calculate target path (e.g., only the part after "Root")
            var targetPath = CalculateTargetPath(selectedPath);
            txtTargetPath.Text = targetPath;
            // Script generation in constructor, using the calculated targetPath and apiKey
            string apiKey = target?.ApiKey ?? string.Empty;
            _generatedScript = GenerateScript(TargetName, TargetUrl, SelectedPath, targetPath, apiKey, _snioExe);
            btnCancel.Click += BtnCancel_Click;

            // Set CancelButton property so ESC triggers Cancel
            this.CancelButton = btnCancel;
        }

        private string CalculateTargetPath(string selectedPath)
        {
            // Always start with '/'.
            if (string.IsNullOrEmpty(selectedPath)) return "/";
            // Split by both separators and remove empty segments
            var parts = selectedPath.Split(new[] { System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar, '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) return "/";

            // If the last segment is "Root", return "/"
            if (string.Equals(parts.Last(), "Root", StringComparison.OrdinalIgnoreCase))
                return "/";

            // Find the last occurrence of "Root"
            int lastRootIdx = Array.FindLastIndex(parts, p => string.Equals(p, "Root", StringComparison.OrdinalIgnoreCase));
            if (lastRootIdx == -1) return "/";

            // Exclude the last segment (selected node itself)
            if (parts.Length - 1 <= lastRootIdx)
                return "/";

            var targetParts = parts.Skip(lastRootIdx).Take(parts.Length - lastRootIdx - 1);
            var targetPath = "/" + string.Join("/", targetParts);
            return string.IsNullOrEmpty(targetPath) ? "/" : targetPath;
        }

        private string _generatedScript;

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
                    MessageBox.Show("Script execution started in a new PowerShell window.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private string GenerateScript(string targetName, string targetUrl, string selectedPath, string targetPath, string apiKey, string snioExe)
        {
            // Script template with placeholders
            const string template =
                "$Exe = \"{SnIO}\"\n" +
                "$Url = \"{TargetUrl}\"\n" +
                "$ApiKey = \"{ApiKey}\"\n" +
                "$Source = \"{SelectedPath}\"\n" +
                "$Path = \"{TargetPath}\"\n\n" +
                "& $Exe IMPORT --DISPLAY:LEVEL Verbose -SOURCE $Source -TARGET -URL $Url -PATH $Path -APIKEY $ApiKey";

            string script = template
                .Replace("{SnIO}", snioExe)
                .Replace("{TargetUrl}", targetUrl)
                .Replace("{ApiKey}", apiKey)
                .Replace("{SelectedPath}", selectedPath)
                .Replace("{TargetPath}", targetPath);
            return script;
        }
    }
}
