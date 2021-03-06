﻿using Phenom.Extension;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Controls;

namespace 诊断工具.Methods
{
    internal class IPScanner : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string IP { get; private set; }
        private int Send = 0;
        private int Response = 0;
        public string Hint => $"{Send.ToString()}中{Response.ToString()}";
        public string Status { get; set; } = "等待";
        public double Timeout { get; set; } = 0;

        public static ObservableCollection<IPScanner> ScanIP(string GateWay, double TimeOut, Control ctl)
        {
            string[] sub = GateWay.Split('.');
            if (sub.Length != 4)
            {
                return null;
            }

            ObservableCollection<IPScanner> ret = new ObservableCollection<IPScanner>();
            for (int n = 0; n < byte.MaxValue; n++)
            {
                ret.Add(new IPScanner($"{sub[0]}.{sub[1]}.{sub[2]}.{n}", ret, TimeOut, ctl));
            }

            return ret;
        }

        private readonly double xot = 3000;
        private readonly ObservableCollection<IPScanner> Parent = null;
        private readonly Control father = null;

        public IPScanner(string ip, ObservableCollection<IPScanner> op, double tout, Control fa)
        {
            IP = ip;
            xot = tout;
            Parent = op;
            father = fa;
            new Action(ScanThread).TaskStart();
        }

        private void ScanThread()
        {
            using (Ping ping = new Ping())
            {
                Status = "检测中";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Status"));
                long TotalTimeout = 0;
                for (int n = 0; n < 10; n++)
                {
                    try
                    {
                        Send++;

                        PingReply replay = ping.Send(IPAddress.Parse(IP), (int)xot);
                        if (replay.Status == IPStatus.Success)
                        {
                            TotalTimeout += replay.RoundtripTime;
                            Response++;
                        }
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Hint"));
                    }
                    catch
                    {
                    }
                }
                Status = Response == 0 ? "失败!" : "成功";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Status"));
                Thread.Sleep(3000);
                if (Response == 0)
                {
                    father.Dispatcher.Invoke(() => Parent.Remove(this));
                }
                else
                {
                    Timeout = TotalTimeout / Response;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Timeout"));
                }
            }
        }
    }
}