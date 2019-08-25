using Newtonsoft.Json.Linq;
using Phenom;
using Phenom.Extension;
using Phenom.Network;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;

namespace 诊断工具.Controls.Generic
{
    /// <summary>
    /// GenericInfo.xaml 的交互逻辑
    /// </summary>
    public partial class GenericInfo : UserControl, HelpedAutoLoad
    {
        public GenericInfo()
        {
            InitializeComponent();
        }

        public string TabName => "本机信息";

        public string Catalog => "系统";

        public string HelpDoc => "查看当前系统信息";

        private void refresh_machine_info_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            foreach (System.Net.IPAddress i in PhenomCore.IPAddresses)
            {
                data["本机IP地址"] = i.ToString();
            }

            data["系统内存"] = Process.GetCurrentProcess().PagedMemorySize64.FormatStroageUnit();
            foreach (NetworkInterface i in NetworkInterface.GetAllNetworkInterfaces())
            {
                data["NIC:" + i.Name] = JObject.FromObject(i).ToString();
            }

            foreach (DriveInfo i in DriveInfo.GetDrives())
            {
                data["Disk:" + i.Name] = JObject.FromObject(i).ToString();
            }

            machin_info.ItemsSource = data;
        }
    }
}