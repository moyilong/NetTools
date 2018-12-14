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
        private void TabItem_Loaded(object sender, RoutedEventArgs e)
        {
        }
        struct CPUInfo
        {
            public bool Supported { get; set; }
            public string Name { get; private set; }
            public string Description { get; private set; }
            public string Value { get; private set; }
            public CPUInfo(string line)
            {
                string[] xline = line.Trim().Split(';');
                bool.TryParse(xline[0].Trim(',', '.'), out bool _Supported);
                Supported = _Supported;
                Name = xline[1].Trim(',', '.');
                Description = xline[2].Trim(',','.');
                Value = xline[3].Trim(',', '.');
            }
        }
        readonly string TempPath = Path.Combine(Environment.GetEnvironmentVariable("temp"), $"cpuid_{Guid.NewGuid().ToString()}.txt");
        private void refresh_cpuinfo_Click(object sender, RoutedEventArgs e)
        {
            node.Push("Reading CPU Info");
            Process proces = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cpuid.exe",
                    Arguments = TempPath,
                }
            };
            proces.Start();
            proces.WaitForExit();
            List<CPUInfo> info = new List<CPUInfo>();
            foreach (var i in File.ReadAllText(TempPath, Encoding.GetEncoding("GB2312")).Split('\n'))
                if (!i.Trim().IsEmpty())
                    try
                    {
                        info.Add(new CPUInfo(i));
                    }
                    catch
                    {

                    }
            cpuid_flush_data.ItemsSource = info;
        }
    }
}