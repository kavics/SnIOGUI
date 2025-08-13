using System;
using System.Windows.Forms;

namespace SnIoGui
{
    public partial class CleanForm : Form
    {
        private Button btnCancel;
        
        // Cancel button event handler
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public string ProfileName
        {
            get => txtProfileName.Text;
            set => txtProfileName.Text = value;
        }
        public string ExportPath
        {
            get => txtExportPath.Text;
            set => txtExportPath.Text = value;
        }
        public string CleanedPath
        {
            get => txtCleanedPath.Text;
            set => txtCleanedPath.Text = value;
        }
        public bool Minimal
        {
            get => chkMinimal.Checked;
            set => chkMinimal.Checked = value;
        }
        public string Script
        {
            get => txtScript.Text;
            set => txtScript.Text = value;
        }

        private readonly Target _target;
        private readonly string _cleanerExe;
        private string _generatedScript;

        public CleanForm(Target target, string cleanerExe)
        {
            InitializeComponent();
            btnViewScript.Click += BtnViewScript_Click!;
            btnExecuteScript.Click += BtnExecuteScript_Click!;
            _target = target;
            _cleanerExe = cleanerExe;
            ProfileName = target?.Name ?? string.Empty;
            ExportPath = target?.ExportPath ?? string.Empty;
            
            // Calculate cleaned path from export path
            CleanedPath = CalculateCleanedPath(target?.ExportPath ?? string.Empty);
            
            // Default minimal to true
            Minimal = true;
            
            // Script generation in constructor
            _generatedScript = GenerateScript();
            btnCancel.Click += BtnCancel_Click;

            // Set CancelButton property so ESC triggers Cancel
            this.CancelButton = btnCancel;
        }

        private string CalculateCleanedPath(string exportPath)
        {
            if (string.IsNullOrEmpty(exportPath))
                return string.Empty;
                
            // If export path ends with "_clean", use as is
            if (exportPath.EndsWith("_clean", StringComparison.OrdinalIgnoreCase))
                return exportPath;
                
            // Otherwise append "_clean" to the export path
            return exportPath + "_clean";
        }

        private void BtnViewScript_Click(object sender, EventArgs e)
        {
            // Regenerate script with current form values
            _generatedScript = GenerateScript();
            
            // Set multiline text in txtScript, preserving line breaks
            txtScript.Lines = _generatedScript.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);
        }

        private void BtnExecuteScript_Click(object sender, EventArgs e)
        {
            try
            {
                // Regenerate script with current form values
                _generatedScript = GenerateScript();
                
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

        private string GenerateScript()
        {
            string cleanedPath = txtCleanedPath.Text;
            string exportPath = txtExportPath.Text;
            bool minimal = chkMinimal.Checked;
            
            // Script template with placeholders for clean operation
            string template = "$Exe = \"{CleanerExe}\"\n" +
                             "$SourcePath = \"{ExportPath}\\Root\"\n" +
                             "$TargetPath = \"{CleanedPath}\\Root\"\n" +
                             "$MinimalFlag = \"{MinimalFlag}\"\n\n" +
                             "& $Exe -SOURCE $SourcePath -TARGET $TargetPath $MinimalFlag\n" +
                             "Write-Host 'Press any key to close...'\n" +
                             "$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown')";

            string script = template
                .Replace("{CleanerExe}", _cleanerExe)
                .Replace("{ExportPath}", exportPath)
                .Replace("{CleanedPath}", cleanedPath)
                .Replace("{MinimalFlag}", minimal ? "-MINIMAL" : "");
            
            return script;
        }
    }
}