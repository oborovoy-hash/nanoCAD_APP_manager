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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBoxIcon = new System.Windows.Forms.PictureBox();
            this.labelAppName = new System.Windows.Forms.Label();
            this.listBoxVersions = new System.Windows.Forms.ListBox();
            this.dataGridViewModules = new System.Windows.Forms.DataGridView();
            this.ColumnModuleName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnConfigs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewModules)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
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
            this.splitContainer1.Size = new System.Drawing.Size(1292, 710);
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
            this.listBoxVersions.Size = new System.Drawing.Size(300, 680);
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
            this.dataGridViewModules.Size = new System.Drawing.Size(988, 710);
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1292, 710);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "Менеджер модулей nanoCAD";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewModules)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
    }
}

