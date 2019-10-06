using Phenom.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using static 诊断工具.AutoLoadTemplate;

namespace 诊断工具
{
    /// <summary>
    /// MDIWPF.xaml 的交互逻辑
    /// </summary>
    public partial class MDIWPF : Window
    {
        private readonly Dictionary<CateLogType, MenuItem> MenuList = new Dictionary<CateLogType, MenuItem>();
        private readonly Dictionary<MenuItem, Type> FormType = new Dictionary<MenuItem, Type>();

        [DllImport("user32.dll", EntryPoint = "SetParent")]
        public extern static IntPtr SetParent(IntPtr childPtr, IntPtr parentPtr);

        public MDIWPF()
        {
            InitializeComponent();
            foreach (Type i in AppDomain.CurrentDomain.ScanType(value => value.GetCustomAttribute<AutoLoadTemplate>() != null, value => true))
            {
                AutoLoadTemplate template = i.GetCustomAttribute<AutoLoadTemplate>();
                if (!MenuList.ContainsKey(template.Catalog))
                    MenuList[template.Catalog] = new MenuItem()
                    {
                        Header = template.Catalog.EnumGetFieldAttribute<DescriptionAttribute>()?.Description ?? template.Catalog.ToString(),
                    };
                MenuItem item = new MenuItem
                {
                    Header = template.TabName
                };
                item.Click += Item_Click; ;
                MenuList[template.Catalog].Items.Add(item);
                FormType[item] = i;
            }

            foreach (MenuItem i in MenuList.Values)
                main_menu.Items.Add(i);
        }

        WindowInteropHelper selfInterop => new WindowInteropHelper(this);

        private void Item_Click(object sender, RoutedEventArgs e)
        {
            Type type = FormType[sender as MenuItem];
            AutoLoadTemplate template = type.GetCustomAttribute<AutoLoadTemplate>();
            Window window = new Window
            {
                Content = Activator.CreateInstance(type) as Control,
                Title = template.TabName,
                Width = Width,
                Height = Height,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            window.Show();
            SetParent(new WindowInteropHelper(window).Handle, selfInterop.Handle);
        }
    }
}