using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace NanoCADModuleManager
{
    public partial class ModuleManagerForm : Form
    {
        private ListBox modulesListBox;
        private Button enableButton;
        private Button disableButton;
        private Button closeButton;

        public ModuleManagerForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.modulesListBox = new ListBox();
            this.enableButton = new Button();
            this.disableButton = new Button();
            this.closeButton = new Button();

            // Form settings
            this.Text = "nanoCAD Module Manager";
            this.Size = new System.Drawing.Size(500, 400);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Modules list box
            this.modulesListBox.Location = new System.Drawing.Point(12, 12);
            this.modulesListBox.Size = new System.Drawing.Size(460, 250);
            this.modulesListBox.SelectionMode = SelectionMode.MultiExtended;
            this.Controls.Add(this.modulesListBox);

            // Enable button
            this.enableButton.Text = "Enable Module";
            this.enableButton.Location = new System.Drawing.Point(12, 270);
            this.enableButton.Size = new System.Drawing.Size(100, 30);
            this.enableButton.Click += EnableButton_Click;
            this.Controls.Add(this.enableButton);

            // Disable button
            this.disableButton.Text = "Disable Module";
            this.disableButton.Location = new System.Drawing.Point(120, 270);
            this.disableButton.Size = new System.Drawing.Size(100, 30);
            this.disableButton.Click += DisableButton_Click;
            this.Controls.Add(this.disableButton);

            // Close button
            this.closeButton.Text = "Close";
            this.closeButton.Location = new System.Drawing.Point(392, 320);
            this.closeButton.Size = new System.Drawing.Size(80, 30);
            this.closeButton.Click += CloseButton_Click;
            this.Controls.Add(this.closeButton);

            LoadModules();
        }

        private void LoadModules()
        {
            try
            {
                var moduleRepository = new ModuleRepository();
                var modules = moduleRepository.GetModules();
                
                modulesListBox.Items.Clear();
                foreach (var module in modules)
                {
                    modulesListBox.Items.Add(module);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading modules: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EnableButton_Click(object sender, EventArgs e)
        {
            ToggleModule(true);
        }

        private void DisableButton_Click(object sender, EventArgs e)
        {
            ToggleModule(false);
        }

        private void ToggleModule(bool enable)
        {
            try
            {
                var selectedItems = modulesListBox.SelectedItems.Cast<Module>().ToList();
                var moduleManager = new ModuleManager();

                foreach (var module in selectedItems)
                {
                    if (enable)
                    {
                        moduleManager.EnableModule(module);
                    }
                    else
                    {
                        moduleManager.DisableModule(module);
                    }
                }

                LoadModules(); // Refresh the list
                MessageBox.Show("Modules updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating modules: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}