using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NanoCADModuleManager
{
    public partial class ModuleManagerForm : Form
    {
        private ListBox versionsListBox;
        private DataGridView modulesDataGridView;
        private Label modulesLabel;
        private Label versionsLabel;

        public Action<string> OnVersionSelected { get; set; }
        public Action<string, bool> OnModuleToggled { get; set; }

        public ModuleManagerForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "nanoCAD Module Manager";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Create main panel
            var mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(5)
            };

            // Create split container to separate versions and modules
            var splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                SplitterDistance = 250,
                Orientation = Orientation.Vertical
            };

            // Left panel for versions
            var leftPanel = new Panel { Dock = DockStyle.Fill };
            
            versionsLabel = new Label
            {
                Text = "nanoCAD Versions:",
                Font = new Font(this.Font, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 25
            };

            versionsListBox = new ListBox
            {
                Dock = DockStyle.Fill,
                IntegralHeight = false
            };
            versionsListBox.SelectedIndexChanged += VersionsListBox_SelectedIndexChanged;

            leftPanel.Controls.Add(versionsListBox);
            leftPanel.Controls.Add(versionsLabel);

            // Right panel for modules
            var rightPanel = new Panel { Dock = DockStyle.Fill };
            
            modulesLabel = new Label
            {
                Text = "Modules for Selected Version:",
                Font = new Font(this.Font, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 25
            };

            modulesDataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                ReadOnly = false
            };

            // Add columns to the DataGridView
            var nameColumn = new DataGridViewTextBoxColumn
            {
                Name = "Name",
                HeaderText = "Name",
                DataPropertyName = "Name",
                Width = 150
            };

            var configsColumn = new DataGridViewTextBoxColumn
            {
                Name = "Configurations",
                HeaderText = "Configurations",
                DataPropertyName = "CompatibleConfigsDisplay",
                Width = 200
            };

            var descriptionColumn = new DataGridViewTextBoxColumn
            {
                Name = "Description",
                HeaderText = "Description",
                DataPropertyName = "Description",
                Width = 200
            };

            var loadColumn = new DataGridViewCheckBoxColumn
            {
                Name = "Load",
                HeaderText = "Load",
                DataPropertyName = "IsLoaded",
                Width = 60
            };

            modulesDataGridView.Columns.Add(nameColumn);
            modulesDataGridView.Columns.Add(configsColumn);
            modulesDataGridView.Columns.Add(descriptionColumn);
            modulesDataGridView.Columns.Add(loadColumn);

            modulesDataGridView.CellContentClick += ModulesDataGridView_CellContentClick;

            rightPanel.Controls.Add(modulesDataGridView);
            rightPanel.Controls.Add(modulesLabel);

            // Add panels to split container
            splitContainer.Panel1.Controls.Add(leftPanel);
            splitContainer.Panel2.Controls.Add(rightPanel);

            mainPanel.Controls.Add(splitContainer);

            this.Controls.Add(mainPanel);
        }

        private void VersionsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (versionsListBox.SelectedItem != null)
            {
                string selectedVersion = versionsListBox.SelectedItem.ToString();
                OnVersionSelected?.Invoke(selectedVersion);
            }
        }

        private void ModulesDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == modulesDataGridView.Columns["Load"].Index && e.RowIndex >= 0)
            {
                var row = modulesDataGridView.Rows[e.RowIndex];
                var module = (ModuleModel)row.DataBoundItem;
                
                if (module != null)
                {
                    bool newValue = !(bool)row.Cells["Load"].Value;
                    OnModuleToggled?.Invoke(module.Name, newValue);
                }
            }
        }

        public void SetVersions(List<string> versions)
        {
            versionsListBox.Items.Clear();
            foreach (var version in versions)
            {
                versionsListBox.Items.Add(version);
            }
        }

        public void SetModules(List<ModuleModel> modules)
        {
            modulesDataGridView.DataSource = null;
            modulesDataGridView.DataSource = modules;
        }

        public class ModuleModel
        {
            public string Name { get; set; }
            public List<string> CompatibleConfigs { get; set; }
            public string Description { get; set; }
            public bool IsLoaded { get; set; }

            public string CompatibleConfigsDisplay
            {
                get
                {
                    return CompatibleConfigs != null ? string.Join(", ", CompatibleConfigs) : "";
                }
            }
        }
    }
}