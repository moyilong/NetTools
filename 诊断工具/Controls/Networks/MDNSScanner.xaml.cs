﻿using Phenom.Logger;
using Phenom.ProgramMethod;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Tmds.MDns;

namespace 诊断工具.Controls.Networks
{
    /// <summary>
    /// MDNSScanner.xaml 的交互逻辑
    /// </summary>
    public partial class MDNSScanner : UserControl, AutoLoadTemplate
    {
        public MDNSScanner()
        {
            InitializeComponent();
        }

        private static DebugNode node = new DebugNode("Network");

        public string TabName => "mDNS扫描";

        public string Catalog => "网络";

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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            refresh_mdns_Click(null, null);
        }
    }
}