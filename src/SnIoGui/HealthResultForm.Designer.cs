namespace SnIoGui
{
    partial class HealthResultForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panelHeader = new Panel();
            lblCheckTime = new Label();
            lblCheckTimeLabel = new Label();
            lblTargetName = new Label();
            lblTargetLabel = new Label();
            lblStatus = new Label();
            panelContent = new Panel();
            dgvDetails = new DataGridView();
            lblDetailsLabel = new Label();
            panelButtons = new Panel();
            btnClose = new Button();
            panelHeader.SuspendLayout();
            panelContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDetails).BeginInit();
            panelButtons.SuspendLayout();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = SystemColors.Control;
            panelHeader.BorderStyle = BorderStyle.FixedSingle;
            panelHeader.Controls.Add(lblCheckTime);
            panelHeader.Controls.Add(lblCheckTimeLabel);
            panelHeader.Controls.Add(lblTargetName);
            panelHeader.Controls.Add(lblTargetLabel);
            panelHeader.Controls.Add(lblStatus);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Padding = new Padding(10);
            panelHeader.Size = new Size(584, 85);
            panelHeader.TabIndex = 0;
            // 
            // lblCheckTime
            // 
            lblCheckTime.AutoSize = true;
            lblCheckTime.Location = new Point(383, 45);
            lblCheckTime.Name = "lblCheckTime";
            lblCheckTime.Size = new Size(69, 15);
            lblCheckTime.TabIndex = 4;
            lblCheckTime.Text = "Check Time";
            // 
            // lblCheckTimeLabel
            // 
            lblCheckTimeLabel.AutoSize = true;
            lblCheckTimeLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblCheckTimeLabel.Location = new Point(300, 45);
            lblCheckTimeLabel.Name = "lblCheckTimeLabel";
            lblCheckTimeLabel.Size = new Size(72, 15);
            lblCheckTimeLabel.TabIndex = 3;
            lblCheckTimeLabel.Text = "Checked at:";
            // 
            // lblTargetName
            // 
            lblTargetName.AutoSize = true;
            lblTargetName.Location = new Point(66, 45);
            lblTargetName.Name = "lblTargetName";
            lblTargetName.Size = new Size(74, 15);
            lblTargetName.TabIndex = 2;
            lblTargetName.Text = "Target Name";
            // 
            // lblTargetLabel
            // 
            lblTargetLabel.AutoSize = true;
            lblTargetLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTargetLabel.Location = new Point(13, 45);
            lblTargetLabel.Name = "lblTargetLabel";
            lblTargetLabel.Size = new Size(46, 15);
            lblTargetLabel.TabIndex = 1;
            lblTargetLabel.Text = "Target:";
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 238);
            lblStatus.Location = new Point(13, 10);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(67, 25);
            lblStatus.TabIndex = 0;
            lblStatus.Text = "Status";
            // 
            // panelContent
            // 
            panelContent.Controls.Add(dgvDetails);
            panelContent.Controls.Add(lblDetailsLabel);
            panelContent.Dock = DockStyle.Fill;
            panelContent.Location = new Point(0, 85);
            panelContent.Name = "panelContent";
            panelContent.Padding = new Padding(10);
            panelContent.Size = new Size(584, 316);
            panelContent.TabIndex = 1;
            // 
            // dgvDetails
            // 
            dgvDetails.BackgroundColor = SystemColors.Window;
            dgvDetails.BorderStyle = BorderStyle.Fixed3D;
            dgvDetails.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDetails.Dock = DockStyle.Fill;
            dgvDetails.Location = new Point(10, 35);
            dgvDetails.Name = "dgvDetails";
            dgvDetails.Size = new Size(564, 271);
            dgvDetails.TabIndex = 1;
            // 
            // lblDetailsLabel
            // 
            lblDetailsLabel.AutoSize = true;
            lblDetailsLabel.Dock = DockStyle.Top;
            lblDetailsLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDetailsLabel.Location = new Point(10, 10);
            lblDetailsLabel.Name = "lblDetailsLabel";
            lblDetailsLabel.Padding = new Padding(0, 0, 0, 10);
            lblDetailsLabel.Size = new Size(48, 25);
            lblDetailsLabel.TabIndex = 0;
            lblDetailsLabel.Text = "Details:";
            // 
            // panelButtons
            // 
            panelButtons.Controls.Add(btnClose);
            panelButtons.Dock = DockStyle.Bottom;
            panelButtons.Location = new Point(0, 401);
            panelButtons.Name = "panelButtons";
            panelButtons.Padding = new Padding(10);
            panelButtons.Size = new Size(584, 50);
            panelButtons.TabIndex = 2;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnClose.Location = new Point(525, 13);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(49, 27);
            btnClose.TabIndex = 0;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // HealthResultForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(584, 451);
            Controls.Add(panelContent);
            Controls.Add(panelButtons);
            Controls.Add(panelHeader);
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(500, 400);
            Name = "HealthResultForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Health Check Results";
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            panelContent.ResumeLayout(false);
            panelContent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDetails).EndInit();
            panelButtons.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panelHeader;
        private Label lblStatus;
        private Label lblTargetLabel;
        private Label lblTargetName;
        private Label lblCheckTimeLabel;
        private Label lblCheckTime;
        private Panel panelContent;
        private DataGridView dgvDetails;
        private Label lblDetailsLabel;
        private Panel panelButtons;
        private Button btnClose;
    }
}