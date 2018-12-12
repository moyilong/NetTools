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
using 诊断工具.Methods;

namespace 诊断工具
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private DebugNode node = new DebugNode("网络诊断");
        private SizeToHumanRead SizeConverter = new SizeToHumanRead();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void refresh_machine_info_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            foreach (var i in WebClient.IPList)
                data["本机IP地址"] = i.ToString();
            data["系统内存"] = Process.GetCurrentProcess().PagedMemorySize64.FormatStroageUnit();
            foreach (var i in NetworkInterface.GetAllNetworkInterfaces())
                data["NIC:" + i.Name] = JObject.FromObject(i).ToString();
            foreach (var i in DriveInfo.GetDrives())
                data["Disk:" + i.Name] = JObject.FromObject(i).ToString();
            machin_info.ItemsSource = data;
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

        [DllImport("cpuid")]
        static extern int load_cpuid(ref byte[] buffer);
        private void TabItem_Loaded(object sender, RoutedEventArgs e)
        {
        }
        struct data_type
        {
            public bool Item1 { get; set; }
            public string Item2 { get; set; }
            public string Item3 { get; set; }
            public string Item4 { get; set; }
        }
        private void refresh_cpuinfo_Click(object sender, RoutedEventArgs e)
        {
          
            node.Push("Reading CPU Info");
            node.Push("Executing CPUID....");

            byte[] buffer = new byte[0x65536];
            unchecked
            {
                node.Push("收到:" + load_cpuid(ref buffer) + "数据");
            }
            node.Push("Processing Data...");
            List<data_type> list = new List<data_type>();
            string[] lines = Encoding.GetEncoding("GB2312").GetString(buffer).Split('\n');
            node.Push("Enumerating....");
            foreach (var i in lines)
                if (!i.IsEmpty())
                    try
                    {
                        node.Push("->" + i);
                        string[] px = i.Split(';');
                        list.Add(new data_type
                        {
                            Item1 = px[0] != "false",
                            Item2 = px[1],
                            Item3 = px[2],
                            Item4 = px[4]
                        });
                    }
                    catch
                    {

                    }
         //   cpuid.ItemsSource = list;
            try
            {
                File.Delete("cpuid.txt");
            }
            catch
            {

            }
            //CPUID_TEXT.Text = File.ReadAllText("cpuid.txt");
        }


    }
}