using Phenom.Extension;
using Phenom.Logger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace 诊断工具.Controls.ADB
{
    public static class ADBBasic
    {
        static string BasicPath = Environment.GetEnvironmentVariable("windir") + Path.DirectorySeparatorChar;
        public static void InstallADB()
        {

            File.WriteAllBytes(BasicPath + "adb.exe", ADB.adb);
            File.WriteAllBytes(BasicPath + "AdbWinUsbApi.dll", ADB.AdbWinUsbApi);
            File.WriteAllBytes(BasicPath + "AdbWinApi.dll", ADB.AdbWinApi);
        }

        public static string[] RunADB(string argments)
        {
            Process proces = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = BasicPath + "adb.exe",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError=true,
                    Arguments=argments
                }
            };
            proces.Start();
            proces.WaitForExit();
            List<string> ret = new List<string>();
            proces.StandardOutput.ReadToEnd().Split('\n').Foreach((value, id) =>
            {
                if (!(value.IsEmpty() || value[0] == '*'))
                    ret.Add(value);
            });
            node.Push("ADB:" + ret.ToArray().Connect(Environment.NewLine));
            return ret.ToArray();
        }
        static DebugNode node = new DebugNode("ADB");
        static public Dictionary<string, string> DeviceList
        {
            get
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                RunADB("devices").Foreach((self, id) =>
                {
                    if (id != 0)
                    {
                        string[] parse = self.Split(' ','\t');
                        val[parse[0].Trim()] = parse[parse.Length - 1].Trim();
                    }
                });
                return val;
            }
        }
        static public string InstallAPK(string apkfile)=> RunADB($"install '{apkfile}'").Connect(Environment.NewLine);
        static public void RestartServices()
        {
            RunADB("restart-server");
        }
        public enum RebootMode
        {
            Normally,
            Recovery,
            Fastboot,
        }
        static public void Reboot (RebootMode mode)
        {
            if (mode == RebootMode.Normally)
                RunADB("reboot");
            else
                RunADB($"reboot {mode.ToString().ToLower()}");
        }
        static public string Connect(string connect) => RunADB($"connect {connect}").Connect(Environment.NewLine);
    }
}
