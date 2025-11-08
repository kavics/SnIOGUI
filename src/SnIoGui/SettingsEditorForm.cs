using SnIoGui.Services;
using System;
using System.Windows.Forms;

namespace SnIoGui
{
    /// <summary>
    /// Form for editing application settings.
    /// </summary>
    public partial class SettingsEditorForm : Form
    {
        private readonly IRuntimeSettingsManager _settingsManager;
        private readonly SnIoGuiSettings _workingSettings;

        public SettingsEditorForm(IRuntimeSettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
            
            // Create a working copy of settings
            _workingSettings = new SnIoGuiSettings
            {
                SnIO = _settingsManager.Settings.SnIO,
                Cleaner = _settingsManager.Settings.Cleaner,
                Targets = new List<Target>(_settingsManager.Settings.Targets ?? new List<Target>())
            };

            InitializeComponent();
            LoadSettings();
        }

        private void LoadSettings()
        {
            txtSnIO.Text = _workingSettings.SnIO ?? string.Empty;
            txtCleaner.Text = _workingSettings.Cleaner ?? string.Empty;
            
            RefreshTargetsList();
        }

        private void RefreshTargetsList()
        {
            listBoxTargets.Items.Clear();
            if (_workingSettings.Targets != null)
            {
                foreach (var target in _workingSettings.Targets)
                {
                    listBoxTargets.Items.Add(target);
                }
            }
            
            UpdateTargetButtons();
        }

        private void UpdateTargetButtons()
        {
            bool hasSelection = listBoxTargets.SelectedIndex >= 0;
            btnEditTarget.Enabled = hasSelection;
            btnDeleteTarget.Enabled = hasSelection;
        }

        private void btnBrowseSnIO_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Executable files (*.exe)|*.exe|All files (*.*)|*.*";
                openFileDialog.Title = "Select SnIO.exe";
                
                if (!string.IsNullOrEmpty(txtSnIO.Text))
                {
                    openFileDialog.FileName = txtSnIO.Text;
                }

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtSnIO.Text = openFileDialog.FileName;
                }
            }
        }

        private void btnBrowseCleaner_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Executable files (*.exe)|*.exe|All files (*.*)|*.*";
                openFileDialog.Title = "Select Cleaner.exe";
                
                if (!string.IsNullOrEmpty(txtCleaner.Text))
                {
                    openFileDialog.FileName = txtCleaner.Text;
                }

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtCleaner.Text = openFileDialog.FileName;
                }
            }
        }

        private void btnAddTarget_Click(object sender, EventArgs e)
        {
            var newTarget = new Target();
            using (var targetEditor = new TargetEditorForm(newTarget))
            {
                if (targetEditor.ShowDialog(this) == DialogResult.OK)
                {
                    _workingSettings.Targets ??= new List<Target>();
                    _workingSettings.Targets.Add(newTarget);
                    RefreshTargetsList();
                }
            }
        }

        private void btnEditTarget_Click(object sender, EventArgs e)
        {
            if (listBoxTargets.SelectedItem is Target selectedTarget)
            {
                using (var targetEditor = new TargetEditorForm(selectedTarget))
                {
                    if (targetEditor.ShowDialog(this) == DialogResult.OK)
                    {
                        RefreshTargetsList();
                    }
                }
            }
        }

        private void btnDeleteTarget_Click(object sender, EventArgs e)
        {
            if (listBoxTargets.SelectedItem is Target selectedTarget)
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete the target '{selectedTarget.Name}'?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    _workingSettings.Targets?.Remove(selectedTarget);
                    RefreshTargetsList();
                }
            }
        }

        private void listBoxTargets_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTargetButtons();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Update the actual settings
            _settingsManager.Settings.SnIO = txtSnIO.Text;
            _settingsManager.Settings.Cleaner = txtCleaner.Text;
            _settingsManager.Settings.Targets = _workingSettings.Targets;

            try
            {
                _settingsManager.SaveSettings();
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to save settings: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
