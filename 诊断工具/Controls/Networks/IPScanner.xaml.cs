using Phenom.WPF.Extension;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace 诊断工具.Controls.Networks
{
    /// <summary>
    /// IPScanner.xaml 的交互逻辑
    /// </summary>
    public partial class IPScanner : UserControl, AutoLoadTemplate
    {
        public IPScanner()
        {
            InitializeComponent();
        }

        public string TabName => "IP扫描";

        public string Catalog => "网络";

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
            gateway_area.ItemsSource = Phenom.Network.WebClient.IPList;
        }
    }
}