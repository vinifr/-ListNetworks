using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Management;
using System.Runtime.InteropServices;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Diagnostics;
using SnmpSharpNet;

namespace ListNetworks
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Text = "Search Net Printers";
        }

        private string GetLocalIPv4(NetworkInterfaceType _type)
        {
            string output = null;
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            output = ip.Address.ToString();
                        }
                    }
                }
            }
            return output;
        }

        private bool getSnmpnext(string host, string OID)
        {
            bool result = false;
            /* Get an SNMP Object
            */
            SimpleSnmp snmpVerb = new SimpleSnmp(host, 161, "public", 500, 0);
            if (!snmpVerb.Valid)
            {
                //MessageBox.Show("Seems that IP or comunauty is not cool");
                return result;
            }
            Oid varbind = new Oid(OID);
            Dictionary<Oid, AsnType> snmpDataS = snmpVerb.GetNext(SnmpVersion.Ver1, new string[] { varbind.ToString() });
            if (snmpDataS != null)
            {
                result = true;
            }
            return result;
        }

        private bool getSnmp(string host, string OID)
        {
            bool result = false;
            /* Get an SNMP Object
            */
            SimpleSnmp snmpVerb = new SimpleSnmp(host, 161, "public", 500, 0);
            if (!snmpVerb.Valid)
            {
                //MessageBox.Show("Seems that IP or comunauty is not cool");
                return result;
            }

            Oid varbind = new Oid(OID);

            Dictionary<Oid, AsnType> snmpDataS = snmpVerb.Get(SnmpVersion.Ver1, new string[] { varbind.ToString() });
            if (snmpDataS != null)
            {
                string temp = snmpDataS[varbind].ToString();
                // Get MANUFACTURER
                int startIndex = temp.IndexOf("MFG:");
                int endIndex = temp.IndexOf(";", startIndex);
                string mfg = temp.Substring(startIndex + 4, endIndex - (startIndex + 4));
                // Get MODEL
                startIndex = temp.IndexOf("MDL:");
                endIndex = temp.IndexOf(";", startIndex);
                string printerMDL = temp.Substring(startIndex + 4, endIndex - (startIndex + 4));


                this.Invoke((MethodInvoker)(() => lstBoxPrinters.Items.Add(
                    string.Format("{0}:{1} ({2})", host, printerMDL, mfg))));
                result = true;
            }
            else
                Console.WriteLine("Not Response from " + host);

            return result;
        }       

        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        public static extern int SendARP(int DestIP, int SrcIP, byte[] pMacAddr, ref uint PhyAddrLen);

        void SendArpRequest(IPAddress dst)
        {
            byte[] macAddr = new byte[6];
            uint macAddrLen = (uint)macAddr.Length;
            int uintAddress = BitConverter.ToInt32(dst.GetAddressBytes(), 0);

            if (SendARP(uintAddress, 0, macAddr, ref macAddrLen) == 0)
            {
                getSnmpnext(dst.ToString(), "1.3.6.1.2.1.43");
                if (getSnmpnext(dst.ToString(), "1.3.6.1.2.1.43") == true)
                {
                    /* PRINTER-PORT-MONITOR-MIB - 1.3.6.1.4.1.2699
                     * ppmPrinterIEEE1284DeviceId: 1.3.6.1.4.1.2699.1.2.1.2.1.1.3
                     * ppmPrinterName: 1.3.6.1.4.1.2699.1.2.1.2.1.1.2
                     */
                    getSnmp(dst.ToString(), "1.3.6.1.4.1.2699.1.2.1.2.1.1.3.1");
                }
            }
        }

        private void ScanPrinters()
        {
           string prefix = null;

            string temp = GetLocalIPv4(NetworkInterfaceType.Ethernet);
            if (temp != null)
                prefix = temp.Substring(0, 3);
            if (temp == null || (prefix == "169"))
                temp = GetLocalIPv4(NetworkInterfaceType.Wireless80211);
            string ipBase = temp;
            try
            {
                for (int i = 0; i < 3; i++)
                {
                    ipBase = temp.Remove(ipBase.Length - 1);
                    if (ipBase.EndsWith("."))
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errormessage = " + ex.Message);
            }

            //Generating IP Range
            List<IPAddress> ipAddressList = new List<IPAddress>();
            for (int i = 1; i < 254; i++)
            {
                //Obviously you'll want to safely parse user input to catch exceptions.
                ipAddressList.Add(IPAddress.Parse(ipBase + i));
            }

            foreach (IPAddress ip in ipAddressList)
            {
                Thread thread = new Thread(() => SendArpRequest(ip));
                thread.Start();
            }
            MessageBox.Show("Search End: " + ipBase);
            btSearchNetPrinter.BeginInvoke((Action)delegate()
            {
                btSearchNetPrinter.Enabled = true;
            });
        }
        
        private async void button1_Click(object sender, EventArgs e)
        {
            lstBoxPrinters.Items.Clear();
            Task task = Task.Run(() => ScanPrinters());
            await task;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            lstBoxPrinters.Items.Clear();
        }
    }
}
