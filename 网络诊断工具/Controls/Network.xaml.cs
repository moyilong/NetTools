using System.Windows;
using System.Windows.Controls;
using Phenom.WPF.Extension;
using Phenom.ProgramMethod;
using 诊断工具.Methods;
using Tmds.MDns;
using System.Threading;
using Phenom.Logger;
using System.Collections.ObjectModel;
using Phenom.Extension;

namespace 诊断工具.Controls
{
    /// <summary>
    /// Network.xaml 的交互逻辑
    /// </summary>
    public partial class Network : UserControl
    {
        public Network()
        {
            InitializeComponent();
            ping_test_table.ItemsSource = tester;
            dns_table.ItemsSource = DNSList;
        }
        private ObservableCollection<PingTester> tester = new ObservableCollection<PingTester>();

        private void add_generic_list_Click(object sender, RoutedEventArgs e)
        {
            foreach (var i in Properties.Resources.DomainList.Split('\n'))
                if (!i.IsEmpty())
                    tester.Add(new PingTester(i));
        }



        private void start_monit_Click(object sender, RoutedEventArgs e)
        {
            PingTester.Enable = true;
        }

        private void stop_listen_Click(object sender, RoutedEventArgs e)
        {
            PingTester.Enable = false;
        }
        private ObservableCollection<DNSReslove> DNSList = new ObservableCollection<DNSReslove>();

        private void add_generic_server_Click(object sender, RoutedEventArgs e)
        {
            foreach (var i in Properties.Resources.DNSServer.Split('\n'))
                DNSList.Add(new DNSReslove(i.Trim()));
        }

        private void add_dns_server_Click(object sender, RoutedEventArgs e)
        {
            if (new_dns_server.Text.IsEmpty())
            {
                this.Error("请输入DNS服务器地址");
                return;
            }
            DNSList.Add(new DNSReslove(new_dns_server.Text.Trim()));
        }

        private void add_new_domain_Click(object sender, RoutedEventArgs e)
        {
            new_domain.Text = new_domain.Text.Trim();
            if (new_domain.Text.IsEmpty())
            {
                this.Error("请输入域名");
                return;
            }
            tester.Add(new PingTester(new_domain.Text));
        }
        private void test_domain_Click(object sender, RoutedEventArgs e)
        {
            if (dns_test_domain.Text.IsEmpty())
            {
                this.Error("请输入域名");
                return;
            }
            foreach (var i in DNSList)
                i.Reslover(dns_test_domain.Text.Trim());
        }
        private void diagonstick_Click(object sender, RoutedEventArgs e)
        {
            diagon_result.ItemsSource = Diagon.RunResult(Application.Current.MainWindow as MainWindow,
                () => Dispatcher.Invoke(() => diagonstick.IsEnabled = false),
                () => Dispatcher.Invoke(() => diagonstick.IsEnabled = true)
                );
        }

        private void start_ip_scan_Click(object sender, RoutedEventArgs e)
        {
            ipscan.ItemsSource = IPScanner.ScanIP(gateway_area.Text, 3000, Application.Current.MainWindow as MainWindow);
        }
        static DebugNode node = new DebugNode("Network");
        private void refresh_mdns_Click(object sender, RoutedEventArgs e)
        {
            Async.NoneWaitStart(() =>
            {
                node.Push("开始扫描MDNS");
                ServiceBrowser browser = new ServiceBrowser();
                SynchronizationContext context = new SynchronizationContext();
                browser.StartBrowse("*", context);
                Thread.Sleep(3000);
                browser.StopBrowse();
                node.Push("更新结果");
                Dispatcher.Invoke(() => mdns_result.ItemsSource = browser.Services);
            }, () => Dispatcher.Invoke(() => refresh_mdns.IsEnabled = false), () => Dispatcher.Invoke(() => refresh_mdns.IsEnabled = true));
        }

    }
}