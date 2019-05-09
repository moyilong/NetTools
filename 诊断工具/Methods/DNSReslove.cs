using ARSoft.Tools.Net.Dns;
using Tahiti.Extension;
using Tahiti.Logger;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;

namespace 诊断工具.Methods
{
    internal class DNSReslove : INotifyPropertyChanged
    {
        private const string node = "DNS";

        public DNSReslove(string DNS)
        {
            Server = DNS;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Reslover(string domain)
        {
            if (Server == "本机直查")
            {
                try
                {
                    Stopwatch stamp = new Stopwatch();
                    stamp.Start();
                    IPAddress[] address = Dns.GetHostAddresses(domain);
                    stamp.Stop();
                    foreach (IPAddress i in address)
                    {
                        Result += i.ToString() + Environment.NewLine;
                    }
                    TimeOut = stamp.ElapsedMilliseconds;
                }
                catch (Exception e)
                {
                    Result = e.ToString();
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Result"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TimeOut"));
                return;
            }
            Domain = domain;
            node.Note("连接DNS服务器..........");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Domain"));
            DnsClient client = new DnsClient(Server.ToIPAddress(), 1000);
            TimeSpan span_begin = new TimeSpan();
            node.Note("请求响应......");
            DnsMessage message = client.Resolve(new ARSoft.Tools.Net.DomainName(new string[] { domain }), RecordType.Any, RecordClass.INet);
            TimeSpan span_end = new TimeSpan();
            node.Note("处理结果");
            if (message == null)
            {
                Result = "空结果";
            }
            else
            {
                Result = $"共查询到{message.AnswerRecords.Count.ToString()}个记录{message.ReturnCode.ToString()}{Environment.NewLine}";
                foreach (DnsRecordBase record in message.AnswerRecords)
                {
                    Result += record.GetType().Name + "记录:";
                    if (record is ARecord)
                    {
                        Result += (record as ARecord).Address.ToString() + Environment.NewLine;
                    }
                    else if (record is CNameRecord)
                    {
                        Result += (record as CNameRecord).CanonicalName.ToString();
                    }
                    else if (record is NaptrRecord)
                    {
                        Result += (record as NaptrRecord).Replacement.ToString();
                    }
                    else if (record is PtrRecord)
                    {
                        Result += (record as PtrRecord).PointerDomainName.ToString();
                    }
                    else
                    {
                        Result += record.ToString() + Environment.NewLine;
                    }
                }
            }
            TimeOut = (span_begin - span_end).TotalMilliseconds;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Result"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TimeOut"));
        }

        public string Domain { get; set; }
        public string Result { get; set; }
        public double TimeOut { get; set; } = double.MaxValue;
        public string Server { get; set; }
    }
}