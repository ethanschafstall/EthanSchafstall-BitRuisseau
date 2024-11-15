using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Diagnostics;

namespace BitRuisseau.services
{
    internal static class NetworkInterfaceService
    {
        public static List<NetworkInterface> GetInterfaces() 
        {
            return NetworkInterface.GetAllNetworkInterfaces().ToList();
        }
    }
}
