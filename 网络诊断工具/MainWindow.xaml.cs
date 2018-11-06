using Newtonsoft.Json.Linq;
using Phenom.Extension;
using Phenom.Network;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Windows;
using static Phenom.WPF.ParentForm;
using Zeroconf;

namespace 网络诊断工具
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ping_test_table.ItemsSource = tester;
            dns_table.ItemsSource = DNSList;
        }
        ObservableCollection<PingTester> tester = new ObservableCollection<PingTester>();

        private void add_generic_list_Click(object sender, RoutedEventArgs e)
        {
            foreach (var i in Properties.Resources.DomainList.Split('\n'))
                if (!i.IsEmpty())
                    tester.Add(new PingTester(i));
        }

        private void add_new_domain_Click(object sender, RoutedEventArgs e)
        {
            new_domain.Text = new_domain.Text.Trim();
            if (new_domain.Text.IsEmpty())
            {
                OldDialog.Error("请输入域名");
                return;
            }
            tester.Add(new PingTester(new_domain.Text));
        }

        private void start_monit_Click(object sender, RoutedEventArgs e)
        {
            PingTester.Enable = true;
        }

        private void stop_listen_Click(object sender, RoutedEventArgs e)
        {
            PingTester.Enable = false;
        }

        private void refresh_machine_info_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            foreach (var i in WebClient.IPList)
                data["本机IP地址"] = i.ToString();
            foreach (var i in NetworkInterface.GetAllNetworkInterfaces())
            {
                data["NIC:" + i.Name] = JObject.FromObject(i).ToString();
            }
            machin_info.ItemsSource = data;
        }
        ObservableCollection<DNSReslove> DNSList = new ObservableCollection<DNSReslove>();
        private void add_generic_server_Click(object sender, RoutedEventArgs e)
        {
            foreach (var i in Properties.Resources.DNSServer.Split('\n'))
                DNSList.Add(new DNSReslove(i.Trim()));
        }

        private void add_dns_server_Click(object sender, RoutedEventArgs e)
        {
            if (new_dns_server.Text.IsEmpty())
            {
                OldDialog.Error("请输入DNS服务器地址");
                return;
            }
            DNSList.Add(new DNSReslove(new_dns_server.Text.Trim()));

        }

        private void test_domain_Click(object sender, RoutedEventArgs e)
        {
            if (dns_test_domain.Text.IsEmpty())
            {
                OldDialog.Error("请输入域名");
                return;
            }
            foreach (var i in DNSList)
                i.Reslover(dns_test_domain.Text.Trim());
        }

        private void start_ip_scan_Click(object sender, RoutedEventArgs e)
        {
            ipscan.ItemsSource = IPScanner.ScanIP(gateway_area.Text, 3000,this);
        }

        private void refresh_mdns_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}