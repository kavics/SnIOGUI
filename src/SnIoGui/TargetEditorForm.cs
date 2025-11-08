using System;
using System.Windows.Forms;

namespace SnIoGui
{
    /// <summary>
    /// Form for editing a single Target.
    /// </summary>
    public partial class TargetEditorForm : Form
    {
        private readonly Target _target;

        public TargetEditorForm(Target target)
        {
            _target = target;
            InitializeComponent();
            LoadTarget();
        }

        private void LoadTarget()
        {
            txtName.Text = _target.Name ?? string.Empty;
            txtUrl.Text = _target.Url ?? string.Empty;
            txtApiKey.Text = _target.ApiKey ?? string.Empty;
            txtHealthApiKey.Text = _target.HealthApiKey ?? string.Empty;
            txtImportPath.Text = _target.ImportPath ?? string.Empty;
            txtExportPath.Text = _target.ExportPath ?? string.Empty;
            txtDbServer.Text = _target.DbServer ?? string.Empty;
            txtDbName.Text = _target.DbName ?? string.Empty;
            txtAdminUrl.Text = _target.AdminUrl ?? string.Empty;
        }

        private void SaveTarget()
        {
            _target.Name = txtName.Text;
            _target.Url = txtUrl.Text;
            _target.ApiKey = txtApiKey.Text;
            _target.HealthApiKey = txtHealthApiKey.Text;
            _target.ImportPath = txtImportPath.Text;
            _target.ExportPath = txtExportPath.Text;
            _target.DbServer = txtDbServer.Text;
            _target.DbName = txtDbName.Text;
            _target.AdminUrl = txtAdminUrl.Text;
        }

        private void btnBrowseImportPath_Click(object sender, EventArgs e)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Select Import Path";
                if (!string.IsNullOrEmpty(txtImportPath.Text))
                {
                    folderDialog.SelectedPath = txtImportPath.Text;
                }

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    txtImportPath.Text = folderDialog.SelectedPath;
                }
            }
        }

        private void btnBrowseExportPath_Click(object sender, EventArgs e)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Select Export Path";
                if (!string.IsNullOrEmpty(txtExportPath.Text))
                {
                    folderDialog.SelectedPath = txtExportPath.Text;
                }

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    txtExportPath.Text = folderDialog.SelectedPath;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            SaveTarget();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
