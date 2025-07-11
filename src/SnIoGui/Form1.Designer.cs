﻿namespace SnIoGui
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            actionPanel = new Panel();
            btnReadApiKey = new Button();
            btnOpenAdminUI = new Button();
            btnOpenLog = new Button();
            btnImport = new Button();
            btnSaveContent = new Button();
            panelNav = new Panel();
            navLayout = new TableLayoutPanel();
            cmbTargets = new ComboBox();
            txtPath = new TextBox();
            btnGo = new Button();
            mainSplit = new SplitContainer();
            tree = new TreeView();
            txtContent = new TextBox();
            actionPanel.SuspendLayout();
            panelNav.SuspendLayout();
            navLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)mainSplit).BeginInit();
            mainSplit.Panel1.SuspendLayout();
            mainSplit.Panel2.SuspendLayout();
            mainSplit.SuspendLayout();
            SuspendLayout();
            // 
            // actionPanel
            // 
            actionPanel.Controls.Add(btnReadApiKey);
            actionPanel.Controls.Add(btnOpenAdminUI);
            actionPanel.Controls.Add(btnOpenLog);
            actionPanel.Controls.Add(btnImport);
            actionPanel.Controls.Add(btnSaveContent);
            actionPanel.Dock = DockStyle.Top;
            actionPanel.Location = new Point(0, 36);
            actionPanel.Name = "actionPanel";
            actionPanel.Padding = new Padding(2);
            actionPanel.Size = new Size(900, 32);
            actionPanel.TabIndex = 1;
            // 
            // btnReadApiKey
            // 
            btnReadApiKey.Dock = DockStyle.Left;
            btnReadApiKey.Location = new Point(352, 2);
            btnReadApiKey.Name = "btnReadApiKey";
            btnReadApiKey.Size = new Size(120, 28);
            btnReadApiKey.TabIndex = 2;
            btnReadApiKey.Text = "Read ApiKey";
            btnReadApiKey.UseVisualStyleBackColor = true;
            btnReadApiKey.Click += btnReadApiKey_Click;
            // 
            // btnOpenAdminUI
            // 
            btnOpenAdminUI.Dock = DockStyle.Left;
            btnOpenAdminUI.Location = new Point(222, 2);
            btnOpenAdminUI.Name = "btnOpenAdminUI";
            btnOpenAdminUI.Size = new Size(130, 28);
            btnOpenAdminUI.TabIndex = 3;
            btnOpenAdminUI.Text = "Open AdminUI";
            btnOpenAdminUI.UseVisualStyleBackColor = true;
            btnOpenAdminUI.Click += btnOpenAdminUI_Click;
            // 
            // btnOpenLog
            // 
            btnOpenLog.Dock = DockStyle.Left;
            btnOpenLog.Location = new Point(132, 2);
            btnOpenLog.Name = "btnOpenLog";
            btnOpenLog.Size = new Size(90, 28);
            btnOpenLog.TabIndex = 1;
            btnOpenLog.Text = "Open Log";
            btnOpenLog.Click += btnOpenLog_Click;
            // 
            // btnImport
            // 
            btnImport.Dock = DockStyle.Left;
            btnImport.Enabled = false;
            btnImport.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnImport.Location = new Point(2, 2);
            btnImport.Name = "btnImport";
            btnImport.Size = new Size(130, 28);
            btnImport.TabIndex = 0;
            btnImport.Text = "Import Selected";
            btnImport.Click += btnImport_Click;
            // 
            // btnSaveContent
            // 
            btnSaveContent.Dock = DockStyle.Right;
            btnSaveContent.Enabled = false;
            btnSaveContent.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSaveContent.Location = new Point(778, 2);
            btnSaveContent.Name = "btnSaveContent";
            btnSaveContent.Size = new Size(120, 28);
            btnSaveContent.TabIndex = 1;
            btnSaveContent.Text = "Save Content";
            btnSaveContent.Click += btnSaveContent_Click;
            // 
            // panelNav
            // 
            panelNav.Controls.Add(navLayout);
            panelNav.Dock = DockStyle.Top;
            panelNav.Location = new Point(0, 0);
            panelNav.Name = "panelNav";
            panelNav.Padding = new Padding(2);
            panelNav.Size = new Size(900, 36);
            panelNav.TabIndex = 2;
            // 
            // navLayout
            // 
            navLayout.ColumnCount = 3;
            navLayout.ColumnStyles.Add(new ColumnStyle());
            navLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            navLayout.ColumnStyles.Add(new ColumnStyle());
            navLayout.Controls.Add(cmbTargets, 0, 0);
            navLayout.Controls.Add(txtPath, 1, 0);
            navLayout.Controls.Add(btnGo, 2, 0);
            navLayout.Dock = DockStyle.Fill;
            navLayout.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            navLayout.Location = new Point(2, 2);
            navLayout.Name = "navLayout";
            navLayout.RowCount = 1;
            navLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            navLayout.Size = new Size(896, 32);
            navLayout.TabIndex = 0;
            // 
            // cmbTargets
            // 
            cmbTargets.Dock = DockStyle.Left;
            cmbTargets.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTargets.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            cmbTargets.Location = new Point(3, 3);
            cmbTargets.Name = "cmbTargets";
            cmbTargets.Size = new Size(200, 25);
            cmbTargets.TabIndex = 0;
            // 
            // txtPath
            // 
            txtPath.Dock = DockStyle.Fill;
            txtPath.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            txtPath.Location = new Point(210, 4);
            txtPath.Margin = new Padding(4, 4, 8, 4);
            txtPath.Name = "txtPath";
            txtPath.Size = new Size(632, 25);
            txtPath.TabIndex = 0;
            // 
            // btnGo
            // 
            btnGo.Anchor = AnchorStyles.None;
            btnGo.Location = new Point(853, 3);
            btnGo.Name = "btnGo";
            btnGo.Size = new Size(40, 26);
            btnGo.TabIndex = 1;
            btnGo.Text = "GO";
            btnGo.Click += btnGo_Click;
            // 
            // mainSplit
            // 
            mainSplit.Dock = DockStyle.Fill;
            mainSplit.Location = new Point(0, 68);
            mainSplit.Name = "mainSplit";
            // 
            // mainSplit.Panel1
            // 
            mainSplit.Panel1.Controls.Add(tree);
            mainSplit.Panel1MinSize = 100;
            // 
            // mainSplit.Panel2
            // 
            mainSplit.Panel2.Controls.Add(txtContent);
            mainSplit.Panel2MinSize = 100;
            mainSplit.Size = new Size(900, 532);
            mainSplit.SplitterDistance = 276;
            mainSplit.TabIndex = 0;
            // 
            // tree
            // 
            tree.Dock = DockStyle.Fill;
            tree.HideSelection = false;
            tree.Location = new Point(0, 0);
            tree.Name = "tree";
            tree.Size = new Size(276, 532);
            tree.TabIndex = 0;
            tree.AfterSelect += tree_AfterSelect;
            // 
            // txtContent
            // 
            txtContent.Dock = DockStyle.Fill;
            txtContent.Location = new Point(0, 0);
            txtContent.Multiline = true;
            txtContent.Name = "txtContent";
            txtContent.ScrollBars = ScrollBars.Both;
            txtContent.Size = new Size(620, 532);
            txtContent.TabIndex = 0;
            txtContent.TextChanged += txtContent_TextChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 600);
            Controls.Add(mainSplit);
            Controls.Add(actionPanel);
            Controls.Add(panelNav);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(700, 400);
            Name = "Form1";
            Text = "Form1";
            actionPanel.ResumeLayout(false);
            panelNav.ResumeLayout(false);
            navLayout.ResumeLayout(false);
            navLayout.PerformLayout();
            mainSplit.Panel1.ResumeLayout(false);
            mainSplit.Panel2.ResumeLayout(false);
            mainSplit.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)mainSplit).EndInit();
            mainSplit.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Panel actionPanel;
        private Button btnImport;
        private Button btnOpenLog;
        private Button btnSaveContent;
        private Button btnReadApiKey;
        private Button btnOpenAdminUI;
        private Panel panelNav;
        private TableLayoutPanel navLayout;
        private ComboBox cmbTargets;
        private TextBox txtPath;
        private Button btnGo;
        private SplitContainer mainSplit;
        private TreeView tree;
        private TextBox txtContent;
    }
}
