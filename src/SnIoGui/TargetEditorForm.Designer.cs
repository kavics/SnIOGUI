namespace SnIoGui
{
    partial class TargetEditorForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            lblName = new Label();
            txtName = new TextBox();
            lblUrl = new Label();
            txtUrl = new TextBox();
            lblApiKey = new Label();
            txtApiKey = new TextBox();
            lblHealthApiKey = new Label();
            txtHealthApiKey = new TextBox();
            lblImportPath = new Label();
            txtImportPath = new TextBox();
            btnBrowseImportPath = new Button();
            lblExportPath = new Label();
            txtExportPath = new TextBox();
            btnBrowseExportPath = new Button();
            lblDbServer = new Label();
            txtDbServer = new TextBox();
            lblDbName = new Label();
            txtDbName = new TextBox();
            lblAdminUrl = new Label();
            txtAdminUrl = new TextBox();
            btnSave = new Button();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new Point(12, 15);
            lblName.Name = "lblName";
            lblName.Size = new Size(42, 15);
            lblName.TabIndex = 0;
            lblName.Text = "Name:";
            // 
            // txtName
            // 
            txtName.Location = new Point(120, 12);
            txtName.Name = "txtName";
            txtName.Size = new Size(400, 23);
            txtName.TabIndex = 1;
            // 
            // lblUrl
            // 
            lblUrl.AutoSize = true;
            lblUrl.Location = new Point(12, 44);
            lblUrl.Name = "lblUrl";
            lblUrl.Size = new Size(31, 15);
            lblUrl.TabIndex = 2;
            lblUrl.Text = "URL:";
            // 
            // txtUrl
            // 
            txtUrl.Location = new Point(120, 41);
            txtUrl.Name = "txtUrl";
            txtUrl.Size = new Size(400, 23);
            txtUrl.TabIndex = 3;
            // 
            // lblApiKey
            // 
            lblApiKey.AutoSize = true;
            lblApiKey.Location = new Point(12, 73);
            lblApiKey.Name = "lblApiKey";
            lblApiKey.Size = new Size(52, 15);
            lblApiKey.TabIndex = 4;
            lblApiKey.Text = "API Key:";
            // 
            // txtApiKey
            // 
            txtApiKey.Location = new Point(120, 70);
            txtApiKey.Name = "txtApiKey";
            txtApiKey.Size = new Size(400, 23);
            txtApiKey.TabIndex = 5;
            // 
            // lblHealthApiKey
            // 
            lblHealthApiKey.AutoSize = true;
            lblHealthApiKey.Location = new Point(12, 102);
            lblHealthApiKey.Name = "lblHealthApiKey";
            lblHealthApiKey.Size = new Size(92, 15);
            lblHealthApiKey.TabIndex = 6;
            lblHealthApiKey.Text = "Health API Key:";
            // 
            // txtHealthApiKey
            // 
            txtHealthApiKey.Location = new Point(120, 99);
            txtHealthApiKey.Name = "txtHealthApiKey";
            txtHealthApiKey.Size = new Size(400, 23);
            txtHealthApiKey.TabIndex = 7;
            // 
            // lblImportPath
            // 
            lblImportPath.AutoSize = true;
            lblImportPath.Location = new Point(12, 131);
            lblImportPath.Name = "lblImportPath";
            lblImportPath.Size = new Size(75, 15);
            lblImportPath.TabIndex = 8;
            lblImportPath.Text = "Import Path:";
            // 
            // txtImportPath
            // 
            txtImportPath.Location = new Point(120, 128);
            txtImportPath.Name = "txtImportPath";
            txtImportPath.Size = new Size(320, 23);
            txtImportPath.TabIndex = 9;
            // 
            // btnBrowseImportPath
            // 
            btnBrowseImportPath.Location = new Point(446, 127);
            btnBrowseImportPath.Name = "btnBrowseImportPath";
            btnBrowseImportPath.Size = new Size(75, 23);
            btnBrowseImportPath.TabIndex = 10;
            btnBrowseImportPath.Text = "Browse...";
            btnBrowseImportPath.UseVisualStyleBackColor = true;
            btnBrowseImportPath.Click += btnBrowseImportPath_Click;
            // 
            // lblExportPath
            // 
            lblExportPath.AutoSize = true;
            lblExportPath.Location = new Point(12, 160);
            lblExportPath.Name = "lblExportPath";
            lblExportPath.Size = new Size(72, 15);
            lblExportPath.TabIndex = 11;
            lblExportPath.Text = "Export Path:";
            // 
            // txtExportPath
            // 
            txtExportPath.Location = new Point(120, 157);
            txtExportPath.Name = "txtExportPath";
            txtExportPath.Size = new Size(320, 23);
            txtExportPath.TabIndex = 12;
            // 
            // btnBrowseExportPath
            // 
            btnBrowseExportPath.Location = new Point(446, 156);
            btnBrowseExportPath.Name = "btnBrowseExportPath";
            btnBrowseExportPath.Size = new Size(75, 23);
            btnBrowseExportPath.TabIndex = 13;
            btnBrowseExportPath.Text = "Browse...";
            btnBrowseExportPath.UseVisualStyleBackColor = true;
            btnBrowseExportPath.Click += btnBrowseExportPath_Click;
            // 
            // lblDbServer
            // 
            lblDbServer.AutoSize = true;
            lblDbServer.Location = new Point(12, 189);
            lblDbServer.Name = "lblDbServer";
            lblDbServer.Size = new Size(63, 15);
            lblDbServer.TabIndex = 14;
            lblDbServer.Text = "DB Server:";
            // 
            // txtDbServer
            // 
            txtDbServer.Location = new Point(120, 186);
            txtDbServer.Name = "txtDbServer";
            txtDbServer.Size = new Size(400, 23);
            txtDbServer.TabIndex = 15;
            // 
            // lblDbName
            // 
            lblDbName.AutoSize = true;
            lblDbName.Location = new Point(12, 218);
            lblDbName.Name = "lblDbName";
            lblDbName.Size = new Size(62, 15);
            lblDbName.TabIndex = 16;
            lblDbName.Text = "DB Name:";
            // 
            // txtDbName
            // 
            txtDbName.Location = new Point(120, 215);
            txtDbName.Name = "txtDbName";
            txtDbName.Size = new Size(400, 23);
            txtDbName.TabIndex = 17;
            // 
            // lblAdminUrl
            // 
            lblAdminUrl.AutoSize = true;
            lblAdminUrl.Location = new Point(12, 247);
            lblAdminUrl.Name = "lblAdminUrl";
            lblAdminUrl.Size = new Size(71, 15);
            lblAdminUrl.TabIndex = 18;
            lblAdminUrl.Text = "Admin URL:";
            // 
            // txtAdminUrl
            // 
            txtAdminUrl.Location = new Point(120, 244);
            txtAdminUrl.Name = "txtAdminUrl";
            txtAdminUrl.Size = new Size(400, 23);
            txtAdminUrl.TabIndex = 19;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(364, 283);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 23);
            btnSave.TabIndex = 20;
            btnSave.Text = "OK";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(445, 283);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 21;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // TargetEditorForm
            // 
            AcceptButton = btnSave;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(534, 321);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(txtAdminUrl);
            Controls.Add(lblAdminUrl);
            Controls.Add(txtDbName);
            Controls.Add(lblDbName);
            Controls.Add(txtDbServer);
            Controls.Add(lblDbServer);
            Controls.Add(btnBrowseExportPath);
            Controls.Add(txtExportPath);
            Controls.Add(lblExportPath);
            Controls.Add(btnBrowseImportPath);
            Controls.Add(txtImportPath);
            Controls.Add(lblImportPath);
            Controls.Add(txtHealthApiKey);
            Controls.Add(lblHealthApiKey);
            Controls.Add(txtApiKey);
            Controls.Add(lblApiKey);
            Controls.Add(txtUrl);
            Controls.Add(lblUrl);
            Controls.Add(txtName);
            Controls.Add(lblName);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "TargetEditorForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Target Editor";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblName;
        private TextBox txtName;
        private Label lblUrl;
        private TextBox txtUrl;
        private Label lblApiKey;
        private TextBox txtApiKey;
        private Label lblHealthApiKey;
        private TextBox txtHealthApiKey;
        private Label lblImportPath;
        private TextBox txtImportPath;
        private Button btnBrowseImportPath;
        private Label lblExportPath;
        private TextBox txtExportPath;
        private Button btnBrowseExportPath;
        private Label lblDbServer;
        private TextBox txtDbServer;
        private Label lblDbName;
        private TextBox txtDbName;
        private Label lblAdminUrl;
        private TextBox txtAdminUrl;
        private Button btnSave;
        private Button btnCancel;
    }
}
