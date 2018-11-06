using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace 网络诊断工具
{
    public class PingTester : INotifyPropertyChanged,IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string Domain { get;private set; }
        public double MaxDelay { get; set; } = double.MinValue;
        public double MinDelay { get; set; } = double.MaxValue;
        public int RequestCount { get; set; } = 0;
        public int FaildCount { get; set; } = 0;
        public double AvgDelay { get; set; } = 0;
        public double FaildPercent => (double)FaildCount / RequestCount;
        public double LastDelay { get; set; } = 0;
        public string LastResult { get; set; } = "";
        static System.Timers.Timer timer = new System.Timers.Timer()
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

        void TimeCallBack(object sender,EventArgs e)
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
            catch(Exception ex)
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
