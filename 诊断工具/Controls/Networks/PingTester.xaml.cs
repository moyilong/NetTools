using Phenom.Extension;

using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace 诊断工具.Controls.Networks
{
    /// <summary>
    /// PingTester.xaml 的交互逻辑
    /// </summary>
    public partial class PingTester : UserControl, AutoLoadTemplate
    {
        public PingTester()
        {
            InitializeComponent();
            ping_test_table.ItemsSource = tester;
        }

        public string TabName => "网络连通性测试(Ping)";
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
            foreach (string i in Properties.Resources.DomainList.Split('\n'))
            {
                if (!i.IsEmpty())
                {
                    tester.Add(new Methods.PingTester(i));
                }
            }
        }
    }
}