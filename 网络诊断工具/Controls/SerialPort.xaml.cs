using Phenom.Extension;
using Phenom.ProgramMethod;
using Phenom.WPF;
using Phenom.WPF.Extension;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SerialPort = System.IO.Ports.SerialPort;

namespace 诊断工具.Controls
{
    /// <summary>
    /// SerialPort.xaml 的交互逻辑
    /// </summary>
    public partial class SerialPort : UserControl
    {
        public SerialPort()
        {
            InitializeComponent();
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
        }

        private void serial_port_assistant_refresh_port_Click(object sender, RoutedEventArgs e)
        {
            serial_assistant_port.ItemsSource = System.IO.Ports.SerialPort.GetPortNames();
        }

        private MuteController mute = null;
        private System.IO.Ports.SerialPort MonitedPort = null;

        private void serial_port_assistant_switch_stat_Click(object sender, RoutedEventArgs e)
        {
            if (MonitedPort == null || !MonitedPort.IsOpen)
            {
                if (serial_assistant_port.SelectedItem is null)
                {
                    this.Error("请选择端口");
                    return;
                }
                MonitedPort = new System.IO.Ports.SerialPort(serial_assistant_port.SelectedItem as string)
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
                //Async.ForEach(serial_port_assistant_input_run.Text.Split(' '), (self, id) =>
                serial_port_assistant_input_run.Text.Split(' ').Foreach((self,id)=>
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
