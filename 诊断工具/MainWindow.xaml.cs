using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using Phenom.Extension;
using Phenom.Logger;
using Phenom.Network;
using Phenom.ProgramMethod;
using Phenom.WPF;
using Phenom.WPF.Converter;
using Phenom.WPF.Extension;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Tmds.MDns;
using 诊断工具.Controls;
using 诊断工具.Controls.Disks;
using 诊断工具.Controls.Generic;
using 诊断工具.Controls.Networks;
using 诊断工具.Methods;

namespace 诊断工具
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private DebugNode node = new DebugNode("网络诊断");

        public UserControl[] Controllers = new UserControl[]
        {
            new GenericInfo(),
            new cpuid(),

            new DiagonStick(),
            new DNSQuery(),
            new Controls.Networks.IPScanner(),
            new MDNSScanner(),
            new Controls.Networks.PingTester(),

            new ImageWritter(),
            new DiskTest(),
            new DiskInfo(),
            new BatchRename(),

            new Controls.SerialPort(),
            new Toolbox()
        };

        Dictionary<string, TabControl> Binding = new Dictionary<string, TabControl>();

        public MainWindow()
        {
            InitializeComponent();
            Controllers.Foreach((self, id) =>
            {
                if (self is PaggedItem item)
                {
                    TabItem vitem = new TabItem()
                    {
                        Header = item.TabName
                    };
                    vitem.Content = self;
                    if (item.Catalog == null)
                        main_area.Items.Add(vitem);
                    else
                    {
                        if (!Binding.ContainsKey(item.Catalog))
                        {
                            Binding[item.Catalog] = new TabControl();
                            main_area.Items.Add(new TabItem()
                            {
                                Content = Binding[item.Catalog],
                                Header=item.Catalog
                            });
                        }
                        Binding[item.Catalog].Items.Add(vitem);
                    }
                        
                }
            });
        }

     


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!this.Confirm("是否退出?"))
            {
                e.Cancel = true;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                Environment.Exit(0);
            }
            catch
            {
                Process.GetCurrentProcess().Kill();
            }
        }
        private void TabItem_Loaded(object sender, RoutedEventArgs e)
        {
        }

    }
}