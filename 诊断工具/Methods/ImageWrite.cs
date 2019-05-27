using Microsoft.Win32.SafeHandles;
using Phenom.Logger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading;
using Phenom.Extension;

namespace 诊断工具.Methods
{
    public enum WorkingMode
    {
        Writting,
        Verifying,
        Syncing
    }

    public class DriverInfo
    {
        public string DispName;
        public int ID;
        public string PathName;

        public override string ToString()
        {
            return DispName;
        }

        public ulong DiskSize
        {
            get
            {
                string[] array = DispName.Split(' ');
                ulong val = ulong.Parse(array[21]);
                string unit = array[22];
                ulong scale = 1;
                switch (unit.ToUpper())
                {
                    case "GB":
                        scale = 1024 * 1024 * 1024;
                        break;

                    case "MB":
                        scale = 1024 * 1024;
                        break;

                    case "KB":
                        scale = 1024;
                        break;

                    case "TB":
                        scale = (ulong)1024 * 1024 * 1024 * 1024;
                        break;
                }
                val *= scale;
                node.Note("Set:" + DispName.Trim() + " Size=" + val.ToString());
                return val;
            }
        }

        private const string node = "DriverInfo";
    }

    public static class WriteLib
    {
        public delegate void ProgressProc(long Max, long Cur, long Min, string Tips, WorkingMode mode);

        #region 引入

        [DllImport("kernel32", SetLastError = true)]
        private static extern IntPtr CreateFile(string FileName, FileAccess DesiredAccess, FileShare ShareMode, IntPtr SecurityAttributes, FileMode CreationDisposition, int FlagsAndAttributes, IntPtr hTemplate);

        [DllImport("kernel32", SetLastError = true)]
        private static extern bool WriteFile(IntPtr hFile, byte[] stream, int Len, ref int WritedLen, IntPtr Overlapped);

        #endregion 引入

        private const string node = "Writter";

        public static int BlockSize { get; set; } = 65536;

        private static string ComputeHash(byte[] data)
        {
            return BitConverter.ToString(MD5.Create().ComputeHash(data));
        }

        public static void Write(string Path, bool NeedVerify, Stream stream, ProgressProc Progress = null)
        {
            IntPtr handle = CreateFile(Path, FileAccess.ReadWrite, FileShare.ReadWrite, IntPtr.Zero, FileMode.Open, 0x20000000, IntPtr.Zero);
            node.Log("打开:" + Path);
            if (handle.ToInt64() == -1)
            {
                throw new Exception("Open handler faild!");
            }

            Dictionary<long, string> HashTable = new Dictionary<long, string>();
            using (FileStream xstream = new FileStream(new SafeFileHandle(handle, true), FileAccess.ReadWrite))
            {
                if (xstream == null)
                {
                    throw new Exception("Open strem faild!");
                }

                while (!xstream.CanWrite)
                {
                    Thread.Sleep(100);
                }

                stream.Seek(0, SeekOrigin.Begin);
                byte[] data = new byte[BlockSize];
                int size = 0;
                Progress?.Invoke(stream.Length, 0, 0, "准备", WorkingMode.Writting);
                int nx = 0;
                while (true)
                {
                    try
                    {
                        while (!xstream.CanWrite)
                        {
                            Thread.Sleep(100);
                        }

                        for (int n = 0; n < data.Length; n++)
                        {
                            data[n] = 0x00;
                        }

                        size = stream.Read(data, 0, BlockSize);
                        if (size == 0 || size == -1)
                        {
                            break;
                        }

                        Progress?.Invoke(stream.Length, xstream.Position, 0, null, WorkingMode.Writting);
                        xstream.Write(data, 0, BlockSize);
                        HashTable[nx++] = data.Hash<MD5>();
                        Progress?.Invoke(stream.Length, xstream.Position, 0, null, WorkingMode.Syncing);
                        xstream.Flush();
                        if (size != BlockSize)
                        {
                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception("写入错误 在" + xstream.Position.ToString("X") + " 区块大小: 0x" + size.ToString("X"), e);
                    }
                }
                node.Log("冲刷缓存");
                Progress?.Invoke(0, 0, 0, "写入缓存", WorkingMode.Writting);
                xstream.Flush();
                if (NeedVerify)
                {
                    node.Log("开始校验");
                    Progress?.Invoke(0, 0, 0, "校验中", WorkingMode.Writting);
                    xstream.Seek(0, SeekOrigin.Begin);
                    nx = 0;
                    for (long n = 0; n < stream.Length; n += BlockSize)
                    {
                        size = xstream.Read(data, 0, BlockSize);
                        Progress?.Invoke(stream.Length, n, 0, "验证 0x" + n.ToString("X"), WorkingMode.Verifying);
                        string crc =data.Hash<MD5>();
                        string val = HashTable[nx++];
                        node.Log("计算值:" + crc + " 参考值:" + val);
                        if (crc != val)
                        {
                            throw new Exception("校验CRC错误 发生在 0x" + xstream.Position.ToString("X") + Environment.NewLine + "SOURCE=" + val + Environment.NewLine + "READ=" + crc);
                        }
                    }
                }
                node.Log("关闭文件流");
                Progress?.Invoke(stream.Length, xstream.Position, 0, "完成", WorkingMode.Verifying);
            }
        }

        public static void RunDiskPart(string script)
        {
            node.Log("运行:" + script);
            File.WriteAllText("script.txt", script);
            Process cmd = Process.Start(new ProcessStartInfo
            {
                RedirectStandardError = true,
                RedirectStandardInput = true,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                FileName = @"Diskpart.exe",
                Arguments = "-s script.txt"
            });
            while (!cmd.HasExited)
            {
                node.Log(cmd.StandardOutput.ReadLine());
            }
        }

        public static DriverInfo[] SecureDiskInfo
        {
            get
            {
                List<DriverInfo> ret = new List<DriverInfo>();
                foreach (DriverInfo i in DiskList)
                {
                    if (i.DiskSize < (ulong)128 * 1024 * 1024 * 1024)
                    {
                        ret.Add(i);
                    }
                }

                return ret.ToArray();
            }
        }

        public static DriverInfo[] DiskList
        {
            get
            {
                File.WriteAllText("script.txt", @"
list disk
exit
");
                Process cmd = new Process()
                {
                    StartInfo = new ProcessStartInfo
                    {
                        RedirectStandardError = true,
                        RedirectStandardInput = true,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        FileName = @"Diskpart.exe",
                        Arguments = "-s script.txt"
                    }
                };
                cmd.Start();
                List<DriverInfo> ret = new List<DriverInfo>();
                cmd.WaitForExit();
                string data = cmd.StandardOutput.ReadToEnd() + Environment.NewLine + cmd.StandardError.ReadToEnd();
                foreach (string line in data.Split('\n'))
                {
                    node.Log(line);
                    if (line.Trim() == string.Empty ||
                        line.IndexOf("Copyright (C) Microsoft Corporation.") != -1 ||
                        line.IndexOf("在计算机上") != -1 ||
                        line.IndexOf("Microsoft DiskPart") != -1 ||
                        line.IndexOf("----") != -1 ||
                        line.IndexOf("###") != -1 ||
                        line.IndexOf("退出") != -1)
                    {
                        continue;
                    }

                    string[] arr = line.Trim().Split(' ');
                    ret.Add(new DriverInfo()
                    {
                        DispName = line,
                        ID = int.Parse(arr[1]),
                        PathName = @"\\.\PHYSICALDRIVE" + arr[1]
                    });
                }
                return ret.ToArray();
            }
        }
    }
}