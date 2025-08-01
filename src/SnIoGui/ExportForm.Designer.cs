using System.Windows.Forms;

namespace SnIoGui
{
    partial class ExportForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTargetName;
        private Label lblTargetUrl;
        private Label lblSelectedPath;
        private Label lblExportPath;
        private TextBox txtTargetName;
        private TextBox txtTargetUrl;
        private TextBox txtSelectedPath;
        private TextBox txtExportPath;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportForm));
            lblTargetName = new Label();
            lblTargetUrl = new Label();
            lblSelectedPath = new Label();
            lblExportPath = new Label();
            txtTargetName = new TextBox();
            txtTargetUrl = new TextBox();
            txtSelectedPath = new TextBox();
            txtExportPath = new TextBox();
            btnViewScript = new Button();
            txtScript = new TextBox();
            btnExecuteScript = new Button();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // lblTargetName
            // 
            lblTargetName.AutoSize = true;
            lblTargetName.Location = new Point(8, 8);
            lblTargetName.Name = "lblTargetName";
            lblTargetName.Size = new Size(77, 15);
            lblTargetName.TabIndex = 0;
            lblTargetName.Text = "Profile name:";
            // 
            // lblTargetUrl
            // 
            lblTargetUrl.AutoSize = true;
            lblTargetUrl.Location = new Point(8, 38);
            lblTargetUrl.Name = "lblTargetUrl";
            lblTargetUrl.Size = new Size(63, 15);
            lblTargetUrl.TabIndex = 2;
            lblTargetUrl.Text = "Source url:";
            // 
            // lblSelectedPath
            // 
            lblSelectedPath.AutoSize = true;
            lblSelectedPath.Location = new Point(8, 68);
            lblSelectedPath.Name = "lblSelectedPath";
            lblSelectedPath.Size = new Size(81, 15);
            lblSelectedPath.TabIndex = 4;
            lblSelectedPath.Text = "Selected path:";
            // 
            // lblExportPath
            // 
            lblExportPath.AutoSize = true;
            lblExportPath.Location = new Point(8, 98);
            lblExportPath.Name = "lblExportPath";
            lblExportPath.Size = new Size(71, 15);
            lblExportPath.TabIndex = 6;
            lblExportPath.Text = "Export path:";
            // 
            // txtTargetName
            // 
            txtTargetName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtTargetName.Location = new Point(100, 5);
            txtTargetName.Name = "txtTargetName";
            txtTargetName.ReadOnly = true;
            txtTargetName.Size = new Size(550, 23);
            txtTargetName.TabIndex = 1;
            // 
            // txtTargetUrl
            // 
            txtTargetUrl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtTargetUrl.Location = new Point(100, 35);
            txtTargetUrl.Name = "txtTargetUrl";
            txtTargetUrl.ReadOnly = true;
            txtTargetUrl.Size = new Size(550, 23);
            txtTargetUrl.TabIndex = 3;
            // 
            // txtSelectedPath
            // 
            txtSelectedPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtSelectedPath.Location = new Point(100, 65);
            txtSelectedPath.Name = "txtSelectedPath";
            txtSelectedPath.ReadOnly = true;
            txtSelectedPath.Size = new Size(550, 23);
            txtSelectedPath.TabIndex = 5;
            // 
            // txtExportPath
            // 
            txtExportPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtExportPath.Location = new Point(100, 95);
            txtExportPath.Name = "txtExportPath";
            txtExportPath.ReadOnly = true;
            txtExportPath.Size = new Size(550, 23);
            txtExportPath.TabIndex = 7;
            // 
            // btnViewScript
            // 
            btnViewScript.Location = new Point(8, 125);
            btnViewScript.Name = "btnViewScript";
            btnViewScript.Size = new Size(140, 40);
            btnViewScript.TabIndex = 8;
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
            txtScript.TabIndex = 9;
            txtScript.WordWrap = false;
            // 
            // btnExecuteScript
            // 
            btnExecuteScript.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnExecuteScript.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnExecuteScript.Location = new Point(638, 370);
            btnExecuteScript.Name = "btnExecuteScript";
            btnExecuteScript.Size = new Size(140, 40);
            btnExecuteScript.TabIndex = 10;
            btnExecuteScript.Text = "Execute Script";
            btnExecuteScript.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.Location = new Point(492, 370);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(140, 40);
            btnCancel.TabIndex = 13;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // ExportForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 430);
            Controls.Add(lblTargetName);
            Controls.Add(txtTargetName);
            Controls.Add(lblTargetUrl);
            Controls.Add(txtTargetUrl);
            Controls.Add(lblSelectedPath);
            Controls.Add(txtSelectedPath);
            Controls.Add(lblExportPath);
            Controls.Add(txtExportPath);
            Controls.Add(btnViewScript);
            Controls.Add(txtScript);
            Controls.Add(btnExecuteScript);
            Controls.Add(btnCancel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ExportForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Export Script Preview";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}