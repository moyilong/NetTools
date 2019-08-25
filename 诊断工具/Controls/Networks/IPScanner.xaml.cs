using Phenom;
using Phenom.UI;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace 诊断工具.Controls.Networks
{
    /// <summary>
    /// IPScanner.xaml 的交互逻辑
    /// </summary>
    [AutoLoadTemplate(Catalog = AutoLoadTemplate.CateLogType.Network, TabName = "IP扫描")]
    public partial class IPScanner : UserControl, HelpedAutoLoad
    {
        public IPScanner()
        {
            InitializeComponent();
        }

        public string TabName => "IP扫描";

        public string Catalog => "网络";

        public string HelpDoc => @"选择需要搜索的网段，点击扫描
能够将IP地址段内所有可访问的地址列出来";

        private void start_ip_scan_Click(object sender, RoutedEventArgs e)
        {
            if (!(gateway_area.SelectedItem is IPAddress ip))
            {
                this.Error("请选择IP地址区域");
            }
            else
            {
                ipscan.ItemsSource = Methods.IPScanner.ScanIP(ip.ToString(), 3000, Application.Current.MainWindow as MainWindow);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            gateway_area.ItemsSource = PhenomCore.IPAddresses.Where(self => self.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
        }
    }
}