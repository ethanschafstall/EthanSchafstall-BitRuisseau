using BitRuisseau.configs;
using BitRuisseau.Models;
using BitRuisseau.services;
using BitRuisseau.tools;
using Microsoft.VisualBasic;
using System.Data;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Windows.Forms;

namespace BitRuisseau
{
    public partial class Form1 : Form
    {
        private List<NetworkInterface> _networkInterfaces = NetworkInterfaceService.GetInterfaces();
        private Catalog _catalog = new Catalog();

        const int TEXT_WIDTH = 6;
        const int TEXT_HIGHT = 20;
        public Form1()
        {
            InitializeComponent();
            //InitInterfacesChecked();
            InitSearchBar();
            Mqtt mqtt = new Mqtt(Mqtt_Config.Broker, Mqtt_Config.Port, Mqtt_Config.ClientId, Mqtt_Config.Username, Mqtt_Config.Password);
            mqtt.ConnectToBroker();
        }
        private void InitInterfacesChecked()
        {

            // CheckedListBox for each network interface
            CheckedListBox networkInterfaceList = new CheckedListBox
            {
                Location = new Point(160, 30),
                Size = new Size(_networkInterfaces.Select(netInt => netInt.Description.Count()).ToList().Max() * TEXT_WIDTH, _networkInterfaces.Count * TEXT_HIGHT)
            };

            // Create checklist button for each available interface
            foreach (var item in _networkInterfaces)
            {
                networkInterfaceList.Items.Add(item.Description, false);
            }
            this.Controls.Add(networkInterfaceList);
        }
        private void InitSearchBar()
        {
            // search text box
            TextBox searchTextBox = new TextBox()
            {
                Location = new Point(12, 12),
                Name = "searchTextBox",
                Size = new Size(260, 20)
            };
            // search results list box
            ListBox resultListBox = new ListBox()
            {
                Location = new Point(12, 38),
                Name = "resultListBox",
                Size = new Size(360, 400)
            };
            // searchResultsWindow
            Form searchResultsWindows = new Form() { Size = new Size(400, 500) };
            searchResultsWindows.FormClosing += (sender, e) =>
            {
                searchResultsWindows.Hide();
                e.Cancel = true;
            };
            searchResultsWindows.Controls.Add(resultListBox);
            // search button
            Button searchButton = new Button()
            {
                Location = new Point(278, 10),
                Name = "searchButton",
                Size = new Size(75, 23),
                Text = "Search"
            };
            searchButton.Click += (sender, e) =>
            {
                searchResultsWindows.Show();
            };

            this.Controls.Add(searchButton);
            this.Controls.Add(searchTextBox);

        }
        private void InitFileBrowser()
        {
            Button browseButton = new Button
            {
                Location = new Point(12, 40),
                Size = new Size(100, 30),
                Text = "Browse Folder"
            };

            TextBox folderPathTextBox = new TextBox
            {
                Location = new Point(120, 40),
                Size = new Size(200, 20),
                ReadOnly = true
            };

            FolderBrowserDialog folderDialog = new FolderBrowserDialog();

            browseButton.Click += (sender, e) =>
            {
                DialogResult result = folderDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    folderPathTextBox.Text = folderDialog.SelectedPath;
                    List<FileModel> files = FileManager.GetFileInfo(folderDialog.SelectedPath);
                    FileSelection(files);
                }
            };

            this.Controls.Add(browseButton);
            this.Controls.Add(folderPathTextBox);
        }

        private void FileSelection(List<FileModel> files)
        {
            DataGridView dataGridView = new DataGridView();
            dataGridView.Height = (files.Count + 2) * TEXT_HIGHT;
            dataGridView.Width = files.Max(file => (file.Name + file.Path + file.SizeInKb + file.Extension).Length) * TEXT_WIDTH;
            dataGridView.AllowUserToResizeColumns = false;
            dataGridView.AllowUserToResizeRows = false;
            dataGridView.Location = new Point(100, 100);

            dataGridView.Columns.Add(new DataGridViewCheckBoxColumn()
            {
                HeaderText = "Select",
                Name = "Select",
                Width = ("Select".Length+1) * TEXT_WIDTH,
            });
            dataGridView.Columns.Add("Name", "File Name");
            dataGridView.Columns["Name"].Width = files.Max(file => file.Name.Length) * TEXT_WIDTH;

            dataGridView.Columns.Add("SizeInKb", "Size in Kb");
            dataGridView.Columns["SizeInKb"].Width = files.Max(file => file.SizeInKb.Length) * TEXT_WIDTH;

            dataGridView.Columns.Add("Path", "Path");
            dataGridView.Columns["Path"].Width = files.Max(file => file.Path.Length) * TEXT_WIDTH/3;

            dataGridView.Columns.Add("Extension", "Ext");
            dataGridView.Columns["Extension"].Width = files.Max(file => file.Extension.Length) * TEXT_WIDTH;

            dataGridView.Columns["Extension"].Width = 50;
            foreach (var file in files)
            { 
                dataGridView.Rows.Add(_catalog.fileModels.Contains(file) ,file.Name, file.SizeInKb, file.Path, file.Extension);
            }
            this.Controls.Add(dataGridView);
            Button selectFilesButton = new Button()
            { 
                Text = "Add Files",
                Location = new Point(dataGridView.Location.X + dataGridView.Width/2, dataGridView.Location.Y + dataGridView.Height)
        };
            selectFilesButton.Click += (sender, e) =>
            {
                List<FileModel> selectedFiles = new List<FileModel>();

                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (row.Cells["Select"].Value != null && (bool)row.Cells["Select"].Value)
                    {
                        var file = row.DataBoundItem as FileModel;
                        if (file != null)
                        {
                            selectedFiles.Add(file);
                        }
                    };
                }
                _catalog.fileModels = selectedFiles;
                dataGridView.DataSource = null;
                this.Controls.Remove(dataGridView);
                this.Controls.Remove(selectFilesButton);
            };
            this.Controls.Add(selectFilesButton);
        }
    }
}
