using Newtonsoft.Json.Linq;
using Phenom.Extension;
using Phenom.Logger;
using Phenom.Network;
using Phenom.ProgramMethod;
using Phenom.WPF.Extension;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Threading;
using System.Windows;
using Tmds.MDns;

namespace 网络诊断工具
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private DebugNode node = new DebugNode("网络诊断");

        public MainWindow()
        {
            InitializeComponent();
            ping_test_table.ItemsSource = tester;
            dns_table.ItemsSource = DNSList;
            DebugNode.Registry(debug);
        }

        private ObservableCollection<PingTester> tester = new ObservableCollection<PingTester>();

        private void add_generic_list_Click(object sender, RoutedEventArgs e)
        {
            foreach (var i in Properties.Resources.DomainList.Split('\n'))
                if (!i.IsEmpty())
                    tester.Add(new PingTester(i));
        }

        private void add_new_domain_Click(object sender, RoutedEventArgs e)
        {
            new_domain.Text = new_domain.Text.Trim();
            if (new_domain.Text.IsEmpty())
            {
                this.Error("请输入域名");
                return;
            }
            tester.Add(new PingTester(new_domain.Text));
        }

        private void start_monit_Click(object sender, RoutedEventArgs e)
        {
            PingTester.Enable = true;
        }

        private void stop_listen_Click(object sender, RoutedEventArgs e)
        {
            PingTester.Enable = false;
        }

        private void refresh_machine_info_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            foreach (var i in WebClient.IPList)
                data["本机IP地址"] = i.ToString();
            foreach (var i in NetworkInterface.GetAllNetworkInterfaces())
                data["NIC:" + i.Name] = JObject.FromObject(i).ToString();
            foreach (var i in DriveInfo.GetDrives())
                data["Disk:" + i.Name] = JObject.FromObject(i).ToString();
            machin_info.ItemsSource = data;
        }

        private ObservableCollection<DNSReslove> DNSList = new ObservableCollection<DNSReslove>();

        private void add_generic_server_Click(object sender, RoutedEventArgs e)
        {
            foreach (var i in Properties.Resources.DNSServer.Split('\n'))
                DNSList.Add(new DNSReslove(i.Trim()));
        }

        private void add_dns_server_Click(object sender, RoutedEventArgs e)
        {
            if (new_dns_server.Text.IsEmpty())
            {
                this.Error("请输入DNS服务器地址");
                return;
            }
            DNSList.Add(new DNSReslove(new_dns_server.Text.Trim()));
        }

        private void test_domain_Click(object sender, RoutedEventArgs e)
        {
            if (dns_test_domain.Text.IsEmpty())
            {
                this.Error("请输入域名");
                return;
            }
            foreach (var i in DNSList)
                i.Reslover(dns_test_domain.Text.Trim());
        }

        private void start_ip_scan_Click(object sender, RoutedEventArgs e)
        {
            ipscan.ItemsSource = IPScanner.ScanIP(gateway_area.Text, 3000, this);
        }

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

        private void enable_mdns_invent_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void enable_mdns_invent_Unchecked(object sender, RoutedEventArgs e)
        {
        }

        private void diagonstick_Click(object sender, RoutedEventArgs e)
        {
            diagon_result.ItemsSource = Diagon.RunResult(this,
                () => Dispatcher.Invoke(() => diagonstick.IsEnabled = false),
                () => Dispatcher.Invoke(() => diagonstick.IsEnabled = true)
                );
        }

        private void refresh_dns_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("ipconfig", "/flushdns");
        }

        private void refresh_disk_info_Click(object sender, RoutedEventArgs e)
        {
            disk_info.ItemsSource = DriveInfo.GetDrives();
        }

        private void disk_write_test_disk_select_refresh_Click(object sender, RoutedEventArgs e)
        {
            disk_write_text_disk_select.ItemsSource = DriveInfo.GetDrives();
        }

        private void disk_write_test_disk_begin_Click(object sender, RoutedEventArgs e)
        {
            DriveInfo info = disk_write_text_disk_select.SelectedItem as DriveInfo;
            if (info == null)
            {
                this.Error("请选择磁盘");
                return;
            }
            ulong BlockLength = ulong.Parse(disk_write_text_disk_size.Text.Trim('M')) * 1024 * 1024;
            ulong BlockSize = (((ulong)info.AvailableFreeSpace) / BlockLength) - 1;
            node.Push($"容量:{BlockSize.ToString()} {BlockLength.ToString()}");
            if (info.DriveType == DriveType.Fixed)
                BlockSize = BlockSize / 100;
            if (!this.Confirm($"测试大小:{BlockLength.ToString()} x {BlockSize.ToString()}"))
                return;
            disk_write_test_progress.Maximum = BlockLength * 2;
            disk_write_test_progress.Value = 0;
            void PushMessage(string message)
            {
                Dispatcher.Invoke(() =>
                {
                    if (disk_write_text_info.Text.Length > 1000)
                        disk_write_text_info.Text = "";
                    disk_write_text_info.Text += message + Environment.NewLine;
                    disk_wrtie_test_output.ScrollToEnd();
                });
            }
            void UpdateProgress()
            {
                Dispatcher.Invoke(() => disk_write_test_progress.Value += 1);
            }
            string ComputeFilePath(ulong id)
            {
                return info.RootDirectory.ToString() + Path.DirectorySeparatorChar + "TestData" + Path.DirectorySeparatorChar + "IDW_TEST_0x" + id.ToString("X8");
            }
            Directory.CreateDirectory(Path.GetDirectoryName(ComputeFilePath(0)));
            Dictionary<ulong, string> HashTable = new Dictionary<ulong, string>((int)BlockSize);
            Async.StartOnce(() =>
            {
                try
                {
                    byte[] data = new byte[BlockSize];
                    new Random().NextBytes(data);
                    PushMessage("开始写入文件");
                    for (ulong n = 0; n < BlockSize; n++)
                    {
                        PushMessage("准备数据...");
                        for (int nx = 0; nx < 32; nx++)
                        {
                            data[new Random().Next(0, data.Length)] = (byte)new Random().Next();
                            data[new Random().Next(0, data.Length)] = (byte)new Random().Next();
                        }
                        PushMessage("计算哈希值...");
                        HashTable[n] = data.GenericHash(new MD5CryptoServiceProvider());
                        string file = ComputeFilePath(n);
                        PushMessage("写入文件:" + file);
                        File.WriteAllBytes(file, data);
                        UpdateProgress();
                    }
                    ulong faild_count = 0;
                    PushMessage("写入成功!");
                    for (ulong n = 0; n < BlockSize; n++)
                    {
                        string file = ComputeFilePath(n);
                        byte[] xdata = File.ReadAllBytes(file);
                        File.Delete(file);
                        string md5 = xdata.GenericHash(new MD5CryptoServiceProvider());
                        if (md5 != HashTable[n])
                        {
                            faild_count++;
                            PushMessage($"校验失败:0x{n.ToString("X8")} {HashTable[n]} != {md5}");
                        }
                        UpdateProgress();
                    }
                    PushMessage("结束，统计中");
                    double finish = (BlockSize - faild_count) / BlockSize;
                    Dispatcher.Invoke(() =>
                    {
                        this.Tips($"结束，成功率:{finish.ToString()} 估计容量:{info.TotalSize * finish}");
                    });
                }
                catch (Exception xe)
                {
                    this.Error("发生错误:" + xe.ToString());
                    return;
                }
            }, () =>
             {
                 Dispatcher.Invoke(() => disk_write_test.IsEnabled = false);
             }, () =>
             {
                 Dispatcher.Invoke(() => disk_write_test.IsEnabled = true);
             });
        }

        private void TabItem_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void refresh_cpuinfo_Click(object sender, RoutedEventArgs e)
        {
            node.Push("Reading CPU Info");
            Process proces = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cpuid.exe",
                    Arguments = "cpuid.txt ",
                }
            };
            proces.Start();
            proces.WaitForExit();
            CPUID_TEXT.Text = File.ReadAllText("cpuid.txt");
        }
    }
}