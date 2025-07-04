using System.Windows.Forms;

namespace SnIoGui
{
    partial class ImportForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTargetName;
        private Label lblTargetUrl;
        private Label lblSelectedPath;
        private TextBox txtTargetName;
        private TextBox txtTargetUrl;
        private TextBox txtSelectedPath;
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
            this.lblTargetName = new Label();
            this.lblTargetUrl = new Label();
            this.lblSelectedPath = new Label();
            this.txtTargetName = new TextBox();
            this.txtTargetUrl = new TextBox();
            this.txtSelectedPath = new TextBox();
            this.btnViewScript = new Button();
            this.txtScript = new TextBox();
            this.btnExecuteScript = new Button();
            this.SuspendLayout();
            // 
            // lblTargetName
            // 
            this.lblTargetName.AutoSize = true;
            this.lblTargetName.Location = new System.Drawing.Point(8, 8);
            this.lblTargetName.Name = "lblTargetName";
            this.lblTargetName.Size = new System.Drawing.Size(77, 15);
            this.lblTargetName.Text = "Profile name:";
            // 
            // txtTargetName
            // 
            this.txtTargetName.Location = new System.Drawing.Point(100, 5);
            this.txtTargetName.Name = "txtTargetName";
            this.txtTargetName.ReadOnly = true;
            this.txtTargetName.Size = new System.Drawing.Size(550, 23);
            this.txtTargetName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            // 
            // lblTargetUrl
            // 
            this.lblTargetUrl.AutoSize = true;
            this.lblTargetUrl.Location = new System.Drawing.Point(8, 38);
            this.lblTargetUrl.Name = "lblTargetUrl";
            this.lblTargetUrl.Size = new System.Drawing.Size(61, 15);
            this.lblTargetUrl.Text = "Target url:";
            // 
            // txtTargetUrl
            // 
            this.txtTargetUrl.Location = new System.Drawing.Point(100, 35);
            this.txtTargetUrl.Name = "txtTargetUrl";
            this.txtTargetUrl.ReadOnly = true;
            this.txtTargetUrl.Size = new System.Drawing.Size(550, 23);
            this.txtTargetUrl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            // 
            // lblSelectedPath
            // 
            this.lblSelectedPath.AutoSize = true;
            this.lblSelectedPath.Location = new System.Drawing.Point(8, 68);
            this.lblSelectedPath.Name = "lblSelectedPath";
            this.lblSelectedPath.Size = new System.Drawing.Size(80, 15);
            this.lblSelectedPath.Text = "Selected path:";
            // 
            // txtSelectedPath
            // 
            this.txtSelectedPath.Location = new System.Drawing.Point(100, 65);
            this.txtSelectedPath.Name = "txtSelectedPath";
            this.txtSelectedPath.ReadOnly = true;
            this.txtSelectedPath.Size = new System.Drawing.Size(550, 23);
            this.txtSelectedPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            // lblTargetPath
            // 
            this.lblTargetPath = new Label();
            this.lblTargetPath.AutoSize = true;
            this.lblTargetPath.Location = new System.Drawing.Point(8, 98);
            this.lblTargetPath.Name = "lblTargetPath";
            this.lblTargetPath.Size = new System.Drawing.Size(70, 15);
            this.lblTargetPath.Text = "Target path:";
            // 
            // txtTargetPath
            // 
            this.txtTargetPath = new TextBox();
            this.txtTargetPath.Location = new System.Drawing.Point(100, 95);
            this.txtTargetPath.Name = "txtTargetPath";
            this.txtTargetPath.ReadOnly = true;
            this.txtTargetPath.Size = new System.Drawing.Size(550, 23);
            this.txtTargetPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            // 
            // btnViewScript
            // 
            this.btnViewScript.Location = new System.Drawing.Point(8, 125);
            this.btnViewScript.Name = "btnViewScript";
            this.btnViewScript.Size = new System.Drawing.Size(140, 40);
            this.btnViewScript.Text = "View Script";
            this.btnViewScript.UseVisualStyleBackColor = true;
            // 
            // txtScript
            // 
            this.txtScript.Location = new System.Drawing.Point(8, 175);
            this.txtScript.Multiline = true;
            this.txtScript.Name = "txtScript";
            this.txtScript.ReadOnly = true;
            this.txtScript.ScrollBars = ScrollBars.Both;
            this.txtScript.Size = new System.Drawing.Size(770, 184);
            this.txtScript.WordWrap = false;
            this.txtScript.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.txtScript.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtScript.Text = string.Empty;
            // 
            // btnExecuteScript
            // 
            this.btnExecuteScript.Location = new System.Drawing.Point(638, 370);
            this.btnExecuteScript.Name = "btnExecuteScript";
            this.btnExecuteScript.Size = new System.Drawing.Size(140, 40);
            this.btnExecuteScript.Text = "Execute Script";
            this.btnExecuteScript.UseVisualStyleBackColor = true;
            this.btnExecuteScript.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.btnExecuteScript.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);

            // btnCancel
            // 
            this.btnCancel = new Button();
            this.btnCancel.Location = new System.Drawing.Point(492, 370);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(140, 40);
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            // 
            // ImportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 430);
            this.Controls.Add(this.lblTargetName);
            this.Controls.Add(this.txtTargetName);
            this.Controls.Add(this.lblTargetUrl);
            this.Controls.Add(this.txtTargetUrl);
            this.Controls.Add(this.lblSelectedPath);
            this.Controls.Add(this.txtSelectedPath);
            this.Controls.Add(this.btnViewScript);
            this.Controls.Add(this.txtScript);
            this.Controls.Add(this.btnExecuteScript);
            this.Controls.Add(this.lblTargetPath);
            this.Controls.Add(this.txtTargetPath);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImportForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Import Script Preview";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
