using Phenom.Extension;
using Phenom.Logger;
using Phenom.ProgramMethod;
using Phenom.WPF.Extension;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
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
using Path = System.IO.Path;

namespace 诊断工具.Controls.Disks
{
    /// <summary>
    /// DiskTest.xaml 的交互逻辑
    /// </summary>
    public partial class DiskTest : UserControl, AutoLoad
    {
        public DiskTest()
        {
            InitializeComponent();
        }

        private void DisktestThread(Action self) => Async.StartOnce(self, () =>
        {
            Dispatcher.Invoke(() => disk_write_test.IsEnabled = false);
        }, () =>
        {
            Dispatcher.Invoke(() => disk_write_test.IsEnabled = true);
        });

        private void PerformanceTest(DriveInfo info)
        {
        }

        static void HalfRandom(byte[] data)
        {
            for (int nx = 0; nx < 32; nx++)
                data[TestRandom.Next(0, data.Length)] = (byte)TestRandom.Next();
        }
        static Random TestRandom = new Random();
        static DebugNode node = new DebugNode("Disk");

        public string TabName => "磁盘测试";

        public string Catalog => "磁盘";

        private void PushMessage(string message)
        {
            Dispatcher.Invoke(() =>
            {
                if (disk_write_text_info.Text.Length > 1000)
                    disk_write_text_info.Text = "";
                disk_write_text_info.Text += message + Environment.NewLine;
                disk_wrtie_test_output.ScrollToEnd();
            });
        }
        private void UpdateProgress()
        {
            Dispatcher.Invoke(() => disk_write_test_progress.Value += 1);
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
        private void disk_write_test_disk_select_refresh_Click(object sender, RoutedEventArgs e)
        {
            disk_write_text_disk_select.ItemsSource = DriveInfo.GetDrives();
        }

        private void DiskBlackFLashTest(DriveInfo info)
        {
            ulong BlockLength = ulong.Parse(disk_write_text_disk_size.Text.Trim('M')) * 1024 * 1024;
            ulong BlockSize = (((ulong)info.AvailableFreeSpace) / BlockLength) - 1;
            node.Push($"容量:{BlockSize.FormatStroageUnit()} {BlockLength.FormatStroageUnit()}");
            if (info.DriveType == DriveType.CDRom)
            {
                this.Error("不能测试光驱!");
                return;
            }
            if (!this.Confirm($"测试大小:{BlockLength.FormatStroageUnit()} x {BlockSize.FormatStroageUnit()}"))
                return;
            disk_write_test_progress.Maximum = BlockSize * 2;
            disk_write_test_progress.Value = 0;
            string ComputeFilePath(ulong id)
            {
                return info.RootDirectory.ToString() + Path.DirectorySeparatorChar + "TestData" + Path.DirectorySeparatorChar + "IDW_TEST_0x" + id.ToString("X8");
            }
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(ComputeFilePath(0)));
            }
            catch (Exception ex)
            {
                this.Error(ex, "创建测试目录失败");
                return;
            }
           
            Dictionary<ulong, string> HashTable = new Dictionary<ulong, string>((int)BlockSize);
            DisktestThread(() =>
            {
                try
                {
                    byte[] data = new byte[BlockLength];
                    TestRandom.NextBytes(data);
                    PushMessage("开始写入文件");
                    for (ulong n = 0; n < BlockSize; n++)
                    {
                        PushMessage("准备数据...");
                        HalfRandom(data);
                        PushMessage("计算哈希值...");
                        Thread thread = Async.NoneWaitStart(() =>
                        {
                            HashTable[n] = data.GenericHash(new MD5CryptoServiceProvider());
                        });
                        string file = ComputeFilePath(n);
                        PushMessage("写入文件:" + file);
                        File.WriteAllBytes(file, data);
                        UpdateProgress();
                        while (thread.ThreadState == System.Threading.ThreadState.Running)
                            Thread.Sleep(100);
                    }
                    ulong faild_count = 0;
                    PushMessage("写入成功!");
                    for (ulong n = 0; n < BlockSize; n++)
                    {
                        string file = ComputeFilePath(n);
                        PushMessage("读取:" + file);
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
                        this.Tips($"结束，成功率:{(100 * finish).ToString()}%{Environment.NewLine}估计容量:{(info.TotalSize * finish).FormatStroageUnit()}{Environment.NewLine}标称容量:{info.TotalSize.FormatStroageUnit()}");
                    });
                }
                catch (Exception xe)
                {
                    this.Error("发生错误:" + xe.ToString());
                    return;
                }
            });
        }


        private void BlockTest(DriveInfo info)
        {
            long[] block_split = new long[]
            {
                128,
                256,
                512,
                1024,
                2048,
                4096,
                8192,
                16*1024,
                32*1024,
                64*1024,
                128 *1024,
                256 *1024,
                512*1024,
                1024*1024,
                2048*1024,
                4096*1024,
                8192*1024,
                16*1024*1024,
                32*1024*1024,
            };
            string filename = info.RootDirectory.FullName + Path.DirectorySeparatorChar + "test_file.idx";
            DisktestThread(() =>
            {
                byte[] test_data = new byte[block_split.Max()];
                byte[] read_data = new byte[block_split.Max()];
                TestRandom.NextBytes(test_data);
                FileStream fs = File.Open(filename, FileMode.OpenOrCreate);
                foreach (var i in block_split)
                {
                    List<double> WriteResult = new List<double>();
                    List<double> ReadResult = new List<double>();
                    for (int n = 0; n < 128; n++)
                    {
                        try
                        {
                            fs.Seek(0, SeekOrigin.Begin);
                            int begin = TestRandom.Next(0, test_data.Length - (int)i);
                            HalfRandom(test_data);
                            Stopwatch stamp = new Stopwatch();
                            stamp.Start();
                            fs.Write(test_data, begin, (int)i);
                            fs.Flush();
                            stamp.Stop();
                            WriteResult.Add(1000 * i / stamp.ElapsedMilliseconds);
                            stamp.Start();
                            fs.Seek(0, SeekOrigin.Begin);
                            fs.Read(read_data, 0, (int)i);
                            stamp.Stop();
                            ReadResult.Add(1000 * i / stamp.ElapsedMilliseconds);
                        }
                        catch (DivideByZeroException)
                        {

                        }
                        catch (Exception e)
                        {
                            PushMessage($"块大小{i.FormatStroageUnit()}错误:{Environment.NewLine}{e.ToString()}");
                            break;
                        }
                    }
                    try
                    {
                        PushMessage($"块大小:{i.FormatStroageUnit()} 读取:{ReadResult.Average().FormatStroageUnit()} 写入:{WriteResult.Average().FormatStroageUnit()}");
                    }
                    catch (InvalidOperationException)
                    {
                        PushMessage($"块大小:{i.FormatStroageUnit()} MaxedOut!");
                    }
                    catch (Exception e)
                    {
                        PushMessage($"块大小{i.FormatStroageUnit()}错误:{Environment.NewLine}{e.ToString()}");
                    }
                }
                fs.Close();
            });
        }
    }
}
