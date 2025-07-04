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

        public ImportForm(string targetName, string targetUrl, string selectedPath)
        {
            InitializeComponent();
            btnViewScript.Click += BtnViewScript_Click;
            btnExecuteScript.Click += BtnExecuteScript_Click;
            TargetName = targetName;
            TargetUrl = targetUrl;
            SelectedPath = selectedPath;
            // Calculate target path (e.g., only the part after "Root")
            var targetPath = CalculateTargetPath(selectedPath);
            txtTargetPath.Text = targetPath;
            // Script generation in constructor, using the calculated targetPath
            _generatedScript = GenerateScript(TargetName, TargetUrl, SelectedPath, targetPath);
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

        private readonly string _generatedScript;

        private void BtnViewScript_Click(object sender, EventArgs e)
        {
            // A generált scriptet bemásolja a txtScript-be
            txtScript.Text = _generatedScript;
        }
        private void BtnExecuteScript_Click(object sender, EventArgs e)
        {
            // Egyelőre csak bezárja a formot
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private string GenerateScript(string targetName, string targetUrl, string selectedPath, string targetPath)
        {
            // Customize script generation here
            return $"# Import script for {targetName}\n$targetUrl = '{targetUrl}'\n$importPath = '{selectedPath}'\n$targetPath = '{targetPath}'\nWrite-Host \"Importing from $importPath to $targetUrl (target path: $targetPath)...\"\n# Add your import logic here";
        }
    }
}
