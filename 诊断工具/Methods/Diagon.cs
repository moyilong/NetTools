/*using SpeedTest;
using SpeedTest.Models;*/

using Phenom;
using Phenom.Extension;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Net.NetworkInformation;

namespace 诊断工具.Methods
{
    internal static class Diagon
    {
        public class DiagonResult : INotifyPropertyChanged
        {
            public string ProjectName { get; set; }
            private string _Result = "";

            public event PropertyChangedEventHandler PropertyChanged;

            public string Result
            {
                get => _Result;
                set
                {
                    _Result = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Result"));
                }
            }
        }

        private static readonly Tuple<string, Func<DiagonResult, bool>, bool>[] TestProject = new Tuple<string, Func<DiagonResult, bool>, bool>[]
        {
            new Tuple<string, Func<DiagonResult, bool>, bool>("本地连接测试",result=>PingTest("127.0.0.1",result),true),
            new Tuple<string, Func<DiagonResult, bool>, bool>("IP检测",result=>
            {
                if (PhenomCore.IPAddresses.Length ==0)
                {
                    result.Result = "失败!";
                    return false;
                }
                else
                {
                    result.Result=PhenomCore.IPAddresses[0].ToString();
                    return true;
                }
            },true),
            new Tuple<string, Func<DiagonResult, bool>, bool>("百度基本连接测试",result=>PingTest("www.baidu.com",result),true),
            new Tuple<string, Func<DiagonResult, bool>, bool>("百度内容测试",result => WebContentTest("http://www.baidu.com",result),true),
            new Tuple<string, Func<DiagonResult, bool>, bool>("谷歌基本连接测试",result=>PingTest("www.google.com",result),false),
            new Tuple<string, Func<DiagonResult, bool>, bool>("SpeedTest服务器测试",result=>PingTest("www.speedtest.net",result),true),
        };

        private static bool WebContentTest(string domain, DiagonResult result)
        {
            result.Result = "请求中...";
            try
            {
                using (WebClient client = new WebClient())
                {
                    string data = client.DownloadString(domain);
                    if (data == null)
                    {
                        result.Result = "错误!空数据!";
                        return false;
                    }
                    result.Result = "成功! 长度:" + data.Length.ToString();
                    return true;
                }
            }
            catch (Exception e)
            {
                result.Result = e.ToString();
                return false;
            }
        }

        private static bool PingTest(string domain, DiagonResult result)
        {
            try
            {
                using (Ping ping = new Ping())
                {
                    PingReply reply = ping.Send(domain);
                    if (reply.Status == IPStatus.Success)
                    {
                        result.Result = "成功! " + reply.RoundtripTime.ToString();
                        return true;
                    }
                    else
                    {
                        result.Result = "失败 " + reply.Status.ToString();
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                result.Result = e.ToString();
                return false;
            }
        }

        public static ObservableCollection<DiagonResult> RunResult(MainWindow window, Action OnInit, Action OnExit)
        {
            ObservableCollection<DiagonResult> ret = new ObservableCollection<DiagonResult>();
            new Action(() =>
            {
                foreach (Tuple<string, Func<DiagonResult, bool>, bool> i in TestProject)
                {
                    DiagonResult diagon = new DiagonResult
                    {
                        ProjectName = i.Item1
                    };
                    window.Dispatcher.Invoke(() => ret.Add(diagon));
                    if (!i.Item2(diagon) && i.Item3)
                    {
                        break;
                    }
                }
            }).ThreadStart(OnInit, OnExit);
            return ret;
        }
    }
}