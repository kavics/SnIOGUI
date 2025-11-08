namespace SnIoGui
{
    partial class SettingsEditorForm
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
            lblSnIO = new Label();
            txtSnIO = new TextBox();
            btnBrowseSnIO = new Button();
            lblCleaner = new Label();
            txtCleaner = new TextBox();
            btnBrowseCleaner = new Button();
            groupBoxTargets = new GroupBox();
            btnDeleteTarget = new Button();
            btnEditTarget = new Button();
            btnAddTarget = new Button();
            listBoxTargets = new ListBox();
            btnSave = new Button();
            btnCancel = new Button();
            groupBoxTargets.SuspendLayout();
            SuspendLayout();
            // 
            // lblSnIO
            // 
            lblSnIO.AutoSize = true;
            lblSnIO.Location = new Point(12, 15);
            lblSnIO.Name = "lblSnIO";
            lblSnIO.Size = new Size(45, 15);
            lblSnIO.TabIndex = 0;
            lblSnIO.Text = "SnIO.exe:";
            // 
            // txtSnIO
            // 
            txtSnIO.Location = new Point(100, 12);
            txtSnIO.Name = "txtSnIO";
            txtSnIO.Size = new Size(450, 23);
            txtSnIO.TabIndex = 1;
            // 
            // btnBrowseSnIO
            // 
            btnBrowseSnIO.Location = new Point(556, 11);
            btnBrowseSnIO.Name = "btnBrowseSnIO";
            btnBrowseSnIO.Size = new Size(75, 23);
            btnBrowseSnIO.TabIndex = 2;
            btnBrowseSnIO.Text = "Browse...";
            btnBrowseSnIO.UseVisualStyleBackColor = true;
            btnBrowseSnIO.Click += btnBrowseSnIO_Click;
            // 
            // lblCleaner
            // 
            lblCleaner.AutoSize = true;
            lblCleaner.Location = new Point(12, 44);
            lblCleaner.Name = "lblCleaner";
            lblCleaner.Size = new Size(75, 15);
            lblCleaner.TabIndex = 3;
            lblCleaner.Text = "Cleaner.exe:";
            // 
            // txtCleaner
            // 
            txtCleaner.Location = new Point(100, 41);
            txtCleaner.Name = "txtCleaner";
            txtCleaner.Size = new Size(450, 23);
            txtCleaner.TabIndex = 4;
            // 
            // btnBrowseCleaner
            // 
            btnBrowseCleaner.Location = new Point(556, 40);
            btnBrowseCleaner.Name = "btnBrowseCleaner";
            btnBrowseCleaner.Size = new Size(75, 23);
            btnBrowseCleaner.TabIndex = 5;
            btnBrowseCleaner.Text = "Browse...";
            btnBrowseCleaner.UseVisualStyleBackColor = true;
            btnBrowseCleaner.Click += btnBrowseCleaner_Click;
            // 
            // groupBoxTargets
            // 
            groupBoxTargets.Controls.Add(btnDeleteTarget);
            groupBoxTargets.Controls.Add(btnEditTarget);
            groupBoxTargets.Controls.Add(btnAddTarget);
            groupBoxTargets.Controls.Add(listBoxTargets);
            groupBoxTargets.Location = new Point(12, 70);
            groupBoxTargets.Name = "groupBoxTargets";
            groupBoxTargets.Size = new Size(619, 250);
            groupBoxTargets.TabIndex = 6;
            groupBoxTargets.TabStop = false;
            groupBoxTargets.Text = "Targets";
            // 
            // btnDeleteTarget
            // 
            btnDeleteTarget.Location = new Point(538, 93);
            btnDeleteTarget.Name = "btnDeleteTarget";
            btnDeleteTarget.Size = new Size(75, 23);
            btnDeleteTarget.TabIndex = 3;
            btnDeleteTarget.Text = "Delete";
            btnDeleteTarget.UseVisualStyleBackColor = true;
            btnDeleteTarget.Click += btnDeleteTarget_Click;
            // 
            // btnEditTarget
            // 
            btnEditTarget.Location = new Point(538, 58);
            btnEditTarget.Name = "btnEditTarget";
            btnEditTarget.Size = new Size(75, 23);
            btnEditTarget.TabIndex = 2;
            btnEditTarget.Text = "Edit...";
            btnEditTarget.UseVisualStyleBackColor = true;
            btnEditTarget.Click += btnEditTarget_Click;
            // 
            // btnAddTarget
            // 
            btnAddTarget.Location = new Point(538, 22);
            btnAddTarget.Name = "btnAddTarget";
            btnAddTarget.Size = new Size(75, 23);
            btnAddTarget.TabIndex = 1;
            btnAddTarget.Text = "Add...";
            btnAddTarget.UseVisualStyleBackColor = true;
            btnAddTarget.Click += btnAddTarget_Click;
            // 
            // listBoxTargets
            // 
            listBoxTargets.FormattingEnabled = true;
            listBoxTargets.ItemHeight = 15;
            listBoxTargets.Location = new Point(6, 22);
            listBoxTargets.Name = "listBoxTargets";
            listBoxTargets.Size = new Size(526, 214);
            listBoxTargets.TabIndex = 0;
            listBoxTargets.SelectedIndexChanged += listBoxTargets_SelectedIndexChanged;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(475, 326);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 23);
            btnSave.TabIndex = 7;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(556, 326);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 8;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // SettingsEditorForm
            // 
            AcceptButton = btnSave;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(643, 361);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(groupBoxTargets);
            Controls.Add(btnBrowseCleaner);
            Controls.Add(txtCleaner);
            Controls.Add(lblCleaner);
            Controls.Add(btnBrowseSnIO);
            Controls.Add(txtSnIO);
            Controls.Add(lblSnIO);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SettingsEditorForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Settings";
            groupBoxTargets.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblSnIO;
        private TextBox txtSnIO;
        private Button btnBrowseSnIO;
        private Label lblCleaner;
        private TextBox txtCleaner;
        private Button btnBrowseCleaner;
        private GroupBox groupBoxTargets;
        private ListBox listBoxTargets;
        private Button btnAddTarget;
        private Button btnEditTarget;
        private Button btnDeleteTarget;
        private Button btnSave;
        private Button btnCancel;
    }
}
