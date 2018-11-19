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
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Tmds.MDns;

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
            ping_test_table.ItemsSource = tester;
            dns_table.ItemsSource = DNSList;
            mute = new MuteController
            {
                serial_assistant_port,
                serial_assistant_speed,
                serial_assistant_databit,
                serial_assistant_stopbit,
                serial_port_assistant_refresh_port
            };
            mute.Add(view_box, MuteController.MuteInvert);
            mute.Add(send_box, MuteController.MuteInvert);
            mute.MuteStatus = false;
            bool[,] Map = new bool[toolbox.ColumnDefinitions.Count, toolbox.RowDefinitions.Count];
            foreach (var i in CommandGroup.CMDS)
            {
                Button btn = new Button()
                {
                    Content = i.Key,
                };
                btn.Click += (e, s) => i.Value();
                for (int n = 0; n < toolbox.ColumnDefinitions.Count; n++)
                {
                    bool success = false;
                    for (int x = 0; x < toolbox.RowDefinitions.Count; x++)
                        if (Map[n, x] == default(bool))
                        {
                            Map[n, x] = !Map[n, x];
                            btn.HorizontalAlignment = HorizontalAlignment.Stretch;
                            btn.VerticalAlignment = VerticalAlignment.Stretch;
                            Grid.SetRow(btn, x);
                            Grid.SetColumn(btn, n);
                            toolbox.Children.Add(btn);
                            success = true;
                            break;
                        }
                    if (success)
                        break;
                }
            }
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
            data["系统内存"] = Process.GetCurrentProcess().PagedMemorySize64.FormatStroageUnit();
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
            if (!(disk_write_text_disk_select.SelectedItem is DriveInfo info))
            {
                this.Error("请选择磁盘");
                return;
            }
            switch (disk_test_type.SelectedIndex)
            {
                case 0:
                    DiskBlackFLashTest(info);
                    break;

                case 1:
                    PerformanceTest(info);
                    break;

                case 2:
                    BlockTest(info);
                    break;
            }
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

        private void TabItem_Loaded_1(object sender, RoutedEventArgs e)
        {
            disk_write_test_disk_select_refresh_Click(null, null);
        }

        private void serial_port_assistant_refresh_port_Click(object sender, RoutedEventArgs e)
        {
            serial_assistant_port.ItemsSource = SerialPort.GetPortNames();
        }

        private MuteController mute = null;
        private SerialPort MonitedPort = null;

        private void serial_port_assistant_switch_stat_Click(object sender, RoutedEventArgs e)
        {
            if (MonitedPort == null || !MonitedPort.IsOpen)
            {
                if (serial_assistant_port.SelectedItem is null)
                {
                    this.Error("请选择端口");
                    return;
                }
                MonitedPort = new SerialPort(serial_assistant_port.SelectedItem as string)
                {
                    BaudRate = int.Parse((serial_assistant_speed.SelectedItem as ComboBoxItem).Content.ToString()),
                    DataBits = int.Parse((serial_assistant_databit.SelectedItem as ComboBoxItem).Content.ToString()),
                    Parity = Parity.None,
                    Handshake = Handshake.None,
                };
                switch (serial_assistant_stopbit.SelectedItem as string)
                {
                    case "0":
                        MonitedPort.StopBits = StopBits.None;
                        break;

                    case "1":
                        MonitedPort.StopBits = StopBits.One;
                        break;

                    case "1.5":
                        MonitedPort.StopBits = StopBits.OnePointFive;
                        break;

                    case "2":
                        MonitedPort.StopBits = StopBits.Two;
                        break;
                }
                MonitedPort.DataReceived += MonitedPort_DataReceived;
                try
                {
                    MonitedPort.Open();
                }
                catch (Exception ex)
                {
                    this.Error("打开串口失败:\n" + ex.ToString());
                    MonitedPort = null;
                    return;
                }
                mute.MuteStatus = true;
            }
            else
            {
                mute.MuteStatus = false;
                MonitedPort.Close();
                MonitedPort = null;
            }
        }

        private void UpdateSerialPortAssistantCount()
        {
            Dispatcher.Invoke(() =>
            {
                serial_assistant_recv_bytes.Content = $"接收:{MonitedPort.BytesToRead.ToString()}";
                serial_assistant_send_bytes.Content = $"发送:{MonitedPort.BytesToWrite.ToString()}";
            });
        }

        private void MonitedPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string data = MonitedPort.ReadExisting();
                StringBuilder builder = new StringBuilder();
                for (int n = 0; n < data.Length; n++)
                    builder.Append($"{((byte)data[n]).ToString("X2")} ");
                Dispatcher.Invoke(() =>
                {
                    ascii_show_run.Text += data;
                    ascii_show_box.ScrollToEnd();
                    hex_show_run.Text += builder.ToString();
                    hex_show_box.ScrollToEnd();
                    UpdateSerialPortAssistantCount();
                });
            }
            catch (Exception ex)
            {
                this.Error(ex);
            }
        }

        private void SendData(bool IsHexMode)
        {
            byte[] ready_to_send = null;
            if (IsHexMode)
            {
                List<byte> ret = new List<byte>();
                Async.ForEach(serial_port_assistant_input_run.Text.Split(' '), (self, id) =>
                {
                    if (ret != null)
                        try
                        {
                            ret.Add(byte.Parse(self));
                        }
                        catch
                        {
                            ret = null;
                        }
                }, false);
                if (ret == null)
                {
                    this.Error("解析HEX数据失败!");
                    return;
                }
                ready_to_send = ret.ToArray();
            }
            else
                ready_to_send = serial_port_assistant_input_run.Text.ToByte();
            MonitedPort.Write(ready_to_send, 0, ready_to_send.Length);
            UpdateSerialPortAssistantCount();
        }

        private void serial_assistant_send_by_ascii_Click(object sender, RoutedEventArgs e)
        {
            SendData(false);
        }

        private void serial_assistant_send_by_hex_Click(object sender, RoutedEventArgs e)
        {
            SendData(true);
        }
    }
}