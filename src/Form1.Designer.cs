namespace src
{
    partial class Form1
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
            this.mainPanel = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listBoxVersions = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridViewModules = new System.Windows.Forms.DataGridView();
            this.ColumnModuleName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnConfigs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.topPanel = new System.Windows.Forms.Panel();
            this.appNameLabel = new System.Windows.Forms.Label();
            this.appIconPictureBox = new System.Windows.Forms.PictureBox();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewModules)).BeginInit();
            this.topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.appIconPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.splitContainer1);
            this.mainPanel.Controls.Add(this.topPanel);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(1292, 710);
            this.mainPanel.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 50);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listBoxVersions);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridViewModules);
            this.splitContainer1.Size = new System.Drawing.Size(1292, 660);
            this.splitContainer1.SplitterDistance = 300;
            this.splitContainer1.TabIndex = 0;
            // 
            // listBoxVersions
            // 
            this.listBoxVersions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxVersions.FormattingEnabled = true;
            this.listBoxVersions.ItemHeight = 25;
            this.listBoxVersions.Location = new System.Drawing.Point(0, 30);
            this.listBoxVersions.Name = "listBoxVersions";
            this.listBoxVersions.Size = new System.Drawing.Size(300, 630);
            this.listBoxVersions.TabIndex = 1;
            this.listBoxVersions.SelectedIndexChanged += new System.EventHandler(this.listBoxVersions_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(300, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "Версии nanoCAD";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dataGridViewModules
            // 
            this.dataGridViewModules.AllowUserToAddRows = false;
            this.dataGridViewModules.AllowUserToDeleteRows = false;
            this.dataGridViewModules.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewModules.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnModuleName,
            this.ColumnConfigs,
            this.ColumnDescription});
            this.dataGridViewModules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewModules.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewModules.Name = "dataGridViewModules";
            this.dataGridViewModules.RowHeadersVisible = false;
            this.dataGridViewModules.RowHeadersWidth = 62;
            this.dataGridViewModules.RowTemplate.Height = 30;
            this.dataGridViewModules.Size = new System.Drawing.Size(988, 660);
            this.dataGridViewModules.TabIndex = 0;
            // 
            // ColumnModuleName
            // 
            this.ColumnModuleName.HeaderText = "Название";
            this.ColumnModuleName.MinimumWidth = 8;
            this.ColumnModuleName.Name = "ColumnModuleName";
            this.ColumnModuleName.Width = 200;
            // 
            // ColumnConfigs
            // 
            this.ColumnConfigs.HeaderText = "Конфигурации";
            this.ColumnConfigs.MinimumWidth = 8;
            this.ColumnConfigs.Name = "ColumnConfigs";
            this.ColumnConfigs.Width = 200;
            // 
            // ColumnDescription
            // 
            this.ColumnDescription.HeaderText = "Описание";
            this.ColumnDescription.MinimumWidth = 8;
            this.ColumnDescription.Name = "ColumnDescription";
            this.ColumnDescription.Width = 300;
            // 
            // topPanel
            // 
            this.topPanel.BackColor = System.Drawing.SystemColors.Control;
            this.topPanel.Controls.Add(this.appNameLabel);
            this.topPanel.Controls.Add(this.appIconPictureBox);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(1292, 50);
            this.topPanel.TabIndex = 1;
            // 
            // appNameLabel
            // 
            this.appNameLabel.AutoSize = true;
            this.appNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.appNameLabel.Location = new System.Drawing.Point(55, 15);
            this.appNameLabel.Name = "appNameLabel";
            this.appNameLabel.Size = new System.Drawing.Size(150, 25);
            this.appNameLabel.TabIndex = 1;
            this.appNameLabel.Text = "nanoCAD x64";
            // 
            // appIconPictureBox
            // 
            this.appIconPictureBox.Location = new System.Drawing.Point(10, 9);
            this.appIconPictureBox.Name = "appIconPictureBox";
            this.appIconPictureBox.Size = new System.Drawing.Size(32, 32);
            this.appIconPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.appIconPictureBox.TabIndex = 0;
            this.appIconPictureBox.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1292, 710);
            this.Controls.Add(this.mainPanel);
            this.Name = "Form1";
            this.Text = "Менеджер модулей nanoCAD";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.mainPanel.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewModules)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.appIconPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Label appNameLabel;
        private System.Windows.Forms.PictureBox appIconPictureBox;
    }
}

