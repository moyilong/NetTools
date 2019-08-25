using Phenom.Extension;
using Phenom.Logger;
using Phenom.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using static 诊断工具.AutoLoadTemplate;

namespace 诊断工具
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Dictionary<CateLogType, TabControl> Binding = new Dictionary<CateLogType, TabControl>();

        public MainWindow()
        {
            InitializeComponent();
            foreach (Type i in AppDomain.CurrentDomain.ScanType(
                    value => value.BaseType == typeof(UserControl) && value.GetCustomAttribute<AutoLoadTemplate>() != null,
                    value => true
                    ))
            {
                AutoLoadTemplate load = i.GetCustomAttribute<AutoLoadTemplate>();
                nameof(MainWindow).Note($"载入:{i.Name}");
                if (!Binding.ContainsKey(load.Catalog))
                {
                    TabControl vctl = new TabControl();
                    main_area.Items.Add(new TabItem()
                    {
                        Header = load.Catalog.EnumGetFieldAttribute<DescriptionAttribute>()?.Description ?? load.Catalog.ToString(),
                        Content = vctl
                    });
                    Binding[load.Catalog] = vctl;
                    nameof(MainWindow).Note("已经创建父级:", load.Catalog.ToString());
                }
                Binding[load.Catalog].Items.Add(new TabItem
                {
                    Header = load.TabName,
                    Content = Activator.CreateInstance(i) as UserControl
                });
            }
        }

        private void HelpUpdate(object obj, RoutedEventArgs e)
        {
            if (obj is HelpedAutoLoad help)
            {
                help_doc.Text = help.HelpDoc;
            }
            else
            {
                help_doc.Text = string.Empty;
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