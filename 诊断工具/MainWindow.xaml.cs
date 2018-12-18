using Phenom.Extension;
using Phenom.Logger;
using Phenom.WPF.Extension;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using 诊断工具.Controls;
using 诊断工具.Controls.Disks;
using 诊断工具.Controls.Generic;
using 诊断工具.Controls.Networks;

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
            new IPScanner(),
            new MDNSScanner(),
            new PingTester(),

            new DiskInfo(),
            new ImageWritter(),
            new DiskTest(),
            new BatchRename(),

            new SerialPort(),
            new Toolbox()
        };

        Dictionary<string, TabControl> Binding = new Dictionary<string, TabControl>();

        public MainWindow()
        {
            InitializeComponent();
            Controllers.Foreach((self, id) =>
            {
                if (self is AutoLoad item)
                {
                    TabItem vitem = new TabItem()
                    {
                        Header = item.TabName,
                        Content=self
                    };

                    self.GotFocus += HelpUpdate;

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

     
        void HelpUpdate(object obj,RoutedEventArgs e)
        {
            if (obj is HelpedAutoLoad help)
            {
                help_doc.Text = help.HelpDoc;
            }
            else
            {
                help_doc.Text = "本页面没有帮助文档";
            }
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