using Phenom.Extension;

using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using 诊断工具.Methods;

namespace 诊断工具.Controls.Networks
{
    /// <summary>
    /// DNSQuery.xaml 的交互逻辑
    /// </summary>
    public partial class DNSQuery : UserControl, AutoLoadTemplate,WIPTemplate,HelpedAutoLoad
    {
        private ObservableCollection<DNSReslove> DNSList = new ObservableCollection<DNSReslove>();

        public DNSQuery()
        {
            InitializeComponent();
            dns_table.ItemsSource = DNSList;
        }

        public string TabName => "DNS查询";

        public string Catalog => "网络";

        public string HelpDoc => @"
1、添加DNS服务器或者添加常用DNS
2、输入要测试的域名，点击测试
";

        private void add_dns_server_Click(object sender, RoutedEventArgs e)
        {
            if (new_dns_server.Text.IsEmpty())
            {
                this.Error("请输入DNS服务器地址");
                return;
            }
            DNSList.Add(new DNSReslove(new_dns_server.Text.Trim()));
        }

        private void add_generic_server_Click(object sender, RoutedEventArgs e)
        {
            foreach (string i in Properties.Resources.DNSServer.Split('\n'))
            {
                DNSList.Add(new DNSReslove(i.Trim()));
            }
        }

        private void test_domain_Click(object sender, RoutedEventArgs e)
        {
            if (dns_test_domain.Text.IsEmpty())
            {
                this.Error("请输入域名");
                return;
            }
            foreach (DNSReslove i in DNSList)
            {
                i.Reslover(dns_test_domain.Text.Trim());
            }
        }
    }
}