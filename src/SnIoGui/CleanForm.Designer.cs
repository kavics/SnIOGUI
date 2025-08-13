using System.Windows.Forms;

namespace SnIoGui
{
    partial class CleanForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblProfileName;
        private Label lblExportPath;
        private Label lblCleanedPath;
        private TextBox txtProfileName;
        private TextBox txtExportPath;
        private TextBox txtCleanedPath;
        private CheckBox chkMinimal;
        private Button btnViewScript;
        private TextBox txtScript;
        private Button btnExecuteScript;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CleanForm));
            lblProfileName = new Label();
            lblExportPath = new Label();
            lblCleanedPath = new Label();
            txtProfileName = new TextBox();
            txtExportPath = new TextBox();
            txtCleanedPath = new TextBox();
            chkMinimal = new CheckBox();
            btnViewScript = new Button();
            txtScript = new TextBox();
            btnExecuteScript = new Button();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // lblProfileName
            // 
            lblProfileName.AutoSize = true;
            lblProfileName.Location = new Point(8, 8);
            lblProfileName.Name = "lblProfileName";
            lblProfileName.Size = new Size(77, 15);
            lblProfileName.TabIndex = 0;
            lblProfileName.Text = "Profile name:";
            // 
            // lblExportPath
            // 
            lblExportPath.AutoSize = true;
            lblExportPath.Location = new Point(8, 38);
            lblExportPath.Name = "lblExportPath";
            lblExportPath.Size = new Size(71, 15);
            lblExportPath.TabIndex = 2;
            lblExportPath.Text = "Export path:";
            // 
            // lblCleanedPath
            // 
            lblCleanedPath.AutoSize = true;
            lblCleanedPath.Location = new Point(8, 68);
            lblCleanedPath.Name = "lblCleanedPath";
            lblCleanedPath.Size = new Size(78, 15);
            lblCleanedPath.TabIndex = 4;
            lblCleanedPath.Text = "Cleaned path:";
            // 
            // txtProfileName
            // 
            txtProfileName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtProfileName.Location = new Point(100, 5);
            txtProfileName.Name = "txtProfileName";
            txtProfileName.ReadOnly = true;
            txtProfileName.Size = new Size(550, 23);
            txtProfileName.TabIndex = 1;
            // 
            // txtExportPath
            // 
            txtExportPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtExportPath.Location = new Point(100, 35);
            txtExportPath.Name = "txtExportPath";
            txtExportPath.ReadOnly = true;
            txtExportPath.Size = new Size(550, 23);
            txtExportPath.TabIndex = 3;
            // 
            // txtCleanedPath
            // 
            txtCleanedPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtCleanedPath.Location = new Point(100, 65);
            txtCleanedPath.Name = "txtCleanedPath";
            txtCleanedPath.Size = new Size(550, 23);
            txtCleanedPath.TabIndex = 5;
            // 
            // chkMinimal
            // 
            chkMinimal.AutoSize = true;
            chkMinimal.Location = new Point(100, 95);
            chkMinimal.Name = "chkMinimal";
            chkMinimal.Size = new Size(68, 19);
            chkMinimal.TabIndex = 6;
            chkMinimal.Text = "Minimal";
            chkMinimal.UseVisualStyleBackColor = true;
            // 
            // btnViewScript
            // 
            btnViewScript.Location = new Point(8, 125);
            btnViewScript.Name = "btnViewScript";
            btnViewScript.Size = new Size(140, 40);
            btnViewScript.TabIndex = 7;
            btnViewScript.Text = "View Script";
            btnViewScript.UseVisualStyleBackColor = true;
            // 
            // txtScript
            // 
            txtScript.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtScript.Font = new Font("Consolas", 10F);
            txtScript.Location = new Point(8, 175);
            txtScript.Multiline = true;
            txtScript.Name = "txtScript";
            txtScript.ReadOnly = true;
            txtScript.ScrollBars = ScrollBars.Both;
            txtScript.Size = new Size(770, 189);
            txtScript.TabIndex = 8;
            txtScript.WordWrap = false;
            // 
            // btnExecuteScript
            // 
            btnExecuteScript.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnExecuteScript.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnExecuteScript.Location = new Point(638, 370);
            btnExecuteScript.Name = "btnExecuteScript";
            btnExecuteScript.Size = new Size(140, 40);
            btnExecuteScript.TabIndex = 9;
            btnExecuteScript.Text = "Execute Script";
            btnExecuteScript.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.Location = new Point(492, 370);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(140, 40);
            btnCancel.TabIndex = 10;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // CleanForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 430);
            Controls.Add(lblProfileName);
            Controls.Add(txtProfileName);
            Controls.Add(lblExportPath);
            Controls.Add(txtExportPath);
            Controls.Add(lblCleanedPath);
            Controls.Add(txtCleanedPath);
            Controls.Add(chkMinimal);
            Controls.Add(btnViewScript);
            Controls.Add(txtScript);
            Controls.Add(btnExecuteScript);
            Controls.Add(btnCancel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CleanForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Clean Script Preview";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}