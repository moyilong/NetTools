using Tahiti.Logger;
using Tahiti.ProgramMethod;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Tmds.MDns;
using System;
using Tahiti.Extension;
namespace 诊断工具.Controls.Networks
{
    /// <summary>
    /// MDNSScanner.xaml 的交互逻辑
    /// </summary>
    public partial class MDNSScanner : UserControl, AutoLoadTemplate,WIPTemplate,HelpedAutoLoad
    {
        public MDNSScanner()
        {
            InitializeComponent();
        }

        private static DebugNode node = new DebugNode("Network");

        public string TabName => "mDNS扫描";

        public string Catalog => "网络";

        public string HelpDoc => @"进行mDNS扫描";

        private void refresh_mdns_Click(object sender, RoutedEventArgs e)
        {
            new Action(() =>
            {
                node.Log("开始扫描MDNS");
                ServiceBrowser browser = new ServiceBrowser();
                SynchronizationContext context = new SynchronizationContext();
                browser.StartBrowse("*", context);
                Thread.Sleep(3000);
                browser.StopBrowse();
                node.Log("更新结果");
                Dispatcher.Invoke(() => mdns_result.ItemsSource = browser.Services);
            }).ThreadStart( () => Dispatcher.Invoke(() => refresh_mdns.IsEnabled = false), () => Dispatcher.Invoke(() => refresh_mdns.IsEnabled = true));
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            refresh_mdns_Click(null, null);
        }
    }
}