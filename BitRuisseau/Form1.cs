using BitRuisseau.services;
using Microsoft.VisualBasic;
using System.Net.NetworkInformation;
using System.Reflection;

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
            InitInterfacesChecked();
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
    }
}
