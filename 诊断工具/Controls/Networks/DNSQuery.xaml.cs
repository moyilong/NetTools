using Phenom.Extension;
using Phenom.WPF.Extension;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using 诊断工具.Methods;

namespace 诊断工具.Controls.Networks
{
    /// <summary>
    /// DNSQuery.xaml 的交互逻辑
    /// </summary>
    public partial class DNSQuery : UserControl,PaggedItem
    {
        private ObservableCollection<DNSReslove> DNSList = new ObservableCollection<DNSReslove>();

        public DNSQuery()
        {
            InitializeComponent();
            dns_table.ItemsSource = DNSList;
        }

        public string TabName => "DNS查询";

        public string Catalog => "网络";

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
            foreach (var i in Properties.Resources.DNSServer.Split('\n'))
                DNSList.Add(new DNSReslove(i.Trim()));
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
    }
}