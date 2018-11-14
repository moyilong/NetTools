using Phenom.ProgramMethod;
using Phenom.WPF.Extension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace 网络诊断工具
{
   partial class  MainWindow
    {
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

        void PerformanceTest(DriveInfo info)
        {

        }

        void DisktestThread(Action self) => Async.StartOnce(self, () =>
        {
            Dispatcher.Invoke(() => disk_write_test.IsEnabled = false);
        }, () =>
        {
            Dispatcher.Invoke(() => disk_write_test.IsEnabled = true);
        });
        void BlockTest(DriveInfo info)
        {
            ulong[] block_split = new ulong[]
            {
                128,
                512,
                4096,
                128 *1024,
                512*1024,
                4096*1024,
                16*1024*1024
            };
            string filename = info.RootDirectory.FullName + Path.DirectorySeparatorChar + "test_file.idx";
            DisktestThread(() =>
            {
                foreach (var i in block_split)
                {
                    List<double> WriteResult = new List<double>();
                    List<double> ReadResult = new List<double>();
                    for (int n = 0; n < 128; n++)
                    {
                        byte[] test_data = new byte[i];
                        new Random().NextBytes(test_data);
                        TimeStamp stamp = new TimeStamp();
                        stamp.Start();
                        File.WriteAllBytes(filename, test_data);
                        stamp.Stop();
                        WriteResult.Add(1000 * i / stamp.Diff.TotalMilliseconds);
                        stamp.Start();
                        File.ReadAllBytes(filename);
                        stamp.Stop();
                        ReadResult.Add(1000 * i / stamp.Diff.TotalMilliseconds);
                    }
                    PushMessage($"块大小:{i.ToString()} 读取:{ReadResult.Average().ToString()} 写入:{WriteResult.Average().ToString()}");
                }
            });
        }
        void DiskBlackFLashTest(DriveInfo info)
        {
            ulong BlockLength = ulong.Parse(disk_write_text_disk_size.Text.Trim('M')) * 1024 * 1024;
            ulong BlockSize = (((ulong)info.AvailableFreeSpace) / BlockLength) - 1;
            node.Push($"容量:{BlockSize.ToString()} {BlockLength.ToString()}");
            if (info.DriveType == DriveType.Fixed)
                BlockSize = BlockSize / 100;
            if (!this.Confirm($"测试大小:{BlockLength.ToString()} x {BlockSize.ToString()}"))
                return;
            disk_write_test_progress.Maximum = BlockSize * 2;
            disk_write_test_progress.Value = 0;
            string ComputeFilePath(ulong id)
            {
                return info.RootDirectory.ToString() + Path.DirectorySeparatorChar + "TestData" + Path.DirectorySeparatorChar + "IDW_TEST_0x" + id.ToString("X8");
            }
            Directory.CreateDirectory(Path.GetDirectoryName(ComputeFilePath(0)));
            Dictionary<ulong, string> HashTable = new Dictionary<ulong, string>((int)BlockSize);
            DisktestThread(() =>
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
                        this.Tips($"结束，成功率:{(100 * finish).ToString()}%{Environment.NewLine}估计容量:{info.TotalSize * finish}{Environment.NewLine}标称容量:{info.TotalSize.ToString()}");
                    });
                }
                catch (Exception xe)
                {
                    this.Error("发生错误:" + xe.ToString());
                    return;
                }
            });
        }
    }
}
