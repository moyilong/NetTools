using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.NetworkInformation;

namespace 诊断工具.Methods
{
    internal class PingTester : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Domain { get; private set; }
        public double MaxDelay { get; set; } = double.MinValue;
        public double MinDelay { get; set; } = double.MaxValue;
        public int RequestCount { get; set; } = 0;
        public int FaildCount { get; set; } = 0;
        public double AvgDelay { get; set; } = 0;
        public double FaildPercent => (double)FaildCount / RequestCount;
        public double LastDelay { get; set; } = 0;
        public string LastResult { get; set; } = "";

        public PingTester()
        {
            PropertyChanged += PingTester_PropertyChanged;
        }

        private void PingTester_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "StatusView")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("StatusView"));
            }
        }

        public string StatusView
        {
            get
            {
                List<string> faild_list = new List<string>();
                if (RequestCount != 0)
                {
                    if (FaildCount / RequestCount > 0.01)
                    {
                        faild_list.Add("丢包率高");
                    }
                }
                else
                {
                    faild_list.Add("未响应");
                }

                if (AvgDelay > 300 || LastDelay > 800)
                {
                    faild_list.Add("延迟高");
                }

                if (faild_list.Count == 0)
                {
                    return "好";
                }
                else
                {
                    string ret = "";
                    foreach (string i in faild_list)
                    {
                        ret += i + ";";
                    }

                    return ret;
                }
            }
        }

        private static System.Timers.Timer timer = new System.Timers.Timer()
        {
            Interval = 1000,
        };

        public static bool Enable
        {
            get => timer.Enabled;
            set => timer.Enabled = value;
        }

        public PingTester(string domain)
        {
            Domain = domain.Trim();
            timer.Elapsed += TimeCallBack;
        }

        private void TimeCallBack(object sender, EventArgs e)
        {
            RequestCount++;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RequestCount"));
            Ping ping = new Ping();
            try
            {
                PingReply reply = ping.Send(Domain, 3000);
                if (reply.Status != IPStatus.Success)
                {
                    FaildCount++;
                    LastResult = reply.Status.ToString();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LastResult"));
                }
                else
                {
                    double time = (double)reply.RoundtripTime;
                    if (time > MaxDelay)
                    {
                        MaxDelay = time;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MaxDelay"));
                    }
                    if (time < MinDelay)
                    {
                        MinDelay = time;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MinDelay"));
                    }
                    AvgDelay = AvgDelay * 0.3 + time * 0.7;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AvgDelay"));
                    LastDelay = time;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LastDelay"));
                    LastResult = time.ToString();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LastResult"));
                }
            }
            catch (Exception ex)
            {
                LastResult = ex.ToString();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LastResult"));
                FaildCount++;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FaildCount"));
            }
        }

        public void Dispose()
        {
            timer.Elapsed -= TimeCallBack;
        }
    }
}