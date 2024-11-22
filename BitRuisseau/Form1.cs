using BitRuisseau.configs;
using BitRuisseau.services;
using Microsoft.VisualBasic;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Windows.Forms;

namespace BitRuisseau
{
    public partial class Form1 : Form
    {
        List<NetworkInterface> networkInterfaces = NetworkInterfaceService.GetInterfaces();

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
                Size = new Size(networkInterfaces.Select(netInt => netInt.Description.Count()).ToList().Max() * TEXT_WIDTH, networkInterfaces.Count * TEXT_HIGHT)
            };

            // Create checklist button for each available interface
            foreach (var item in networkInterfaces)
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

    }
}
