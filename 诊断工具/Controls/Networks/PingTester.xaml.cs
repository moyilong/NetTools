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

namespace 诊断工具.Controls.Networks
{
    /// <summary>
    /// PingTester.xaml 的交互逻辑
    /// </summary>
    public partial class PingTester : UserControl,PaggedItem
    {
        public PingTester()
        {
            InitializeComponent();
            ping_test_table.ItemsSource = tester;
        }

        public string TabName => "网络连通性测试";
        public string Catalog => "网络";

        private void start_monit_Click(object sender, RoutedEventArgs e)
        {
            Methods.PingTester.Enable = true;
        }

        private void stop_listen_Click(object sender, RoutedEventArgs e)
        {
            Methods.PingTester.Enable = false;
        }

        private ObservableCollection<Methods.PingTester> tester = new ObservableCollection<Methods.PingTester>();
        private void add_new_domain_Click(object sender, RoutedEventArgs e)
        {
            new_domain.Text = new_domain.Text.Trim();
            if (new_domain.Text.IsEmpty())
            {
                this.Error("请输入域名");
                return;
            }
            tester.Add(new Methods.PingTester(new_domain.Text));
        }
        private void add_generic_list_Click(object sender, RoutedEventArgs e)
        {
            foreach (var i in Properties.Resources.DomainList.Split('\n'))
                if (!i.IsEmpty())
                    tester.Add(new Methods.PingTester(i));
        }

    }
}