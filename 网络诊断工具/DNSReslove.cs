using ARSoft.Tools.Net.Dns;
using Phenom.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 网络诊断工具
{
    class DNSReslove : INotifyPropertyChanged
    {
        public DNSReslove(string DNS)
        {
            Server = DNS;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Reslover(string domain)
        {
            Domain = domain;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Domain"));
            DnsClient client = new DnsClient(Server.ToIPAddress(), 1000);
            TimeSpan span_begin = new TimeSpan();
            DnsMessage message = client.Resolve(new ARSoft.Tools.Net.DomainName(new string[]{
                domain
            }), RecordType.Any, RecordClass.INet);
            TimeSpan span_end = new TimeSpan();
            if (message == null)
            {
                if (message == null)
                    Result = "空结果";
                else
                    Result = message.ReturnCode.ToString();
            }
            else
            {
                //Result = (message.AnswerRecords[0] as ARecord).Address.ToString();
                Result = $"共查询到{message.AnswerRecords.Count.ToString()}个记录" + Environment.NewLine;
                foreach (var record in message.AnswerRecords)
                {
                    Result += record.GetType().Name + "记录:";
                    if (record is ARecord)
                        Result += (record as ARecord).Address.ToString() + Environment.NewLine;
                    else if (record is CNameRecord)
                        Result += (record as CNameRecord).CanonicalName.ToString();
                    else if (record is NaptrRecord)
                        Result += (record as NaptrRecord).Replacement.ToString();
                    else if (record is PtrRecord)
                        Result += (record as PtrRecord).PointerDomainName.ToString();
                    else
                        Result += record.ToString() + Environment.NewLine;
                }
                TimeOut = (span_begin - span_end).TotalMilliseconds;
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Result"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TimeOut"));
        }
        public string Domain { get; set; }
        public string Result { get; set; }
        public double TimeOut { get; set; } = double.MaxValue;
        public string Server { get; set; }
    }
}
