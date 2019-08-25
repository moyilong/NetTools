using System;
using System.ComponentModel;

namespace 诊断工具
{
    internal class AutoLoadTemplate : Attribute
    {
        public enum CateLogType
        {
            [Description("网络")]
            Network,

            [Description("磁盘")]
            Disk,

            [Description("图形图像")]
            Image,

            [Description("系统")]
            System
        }

        public string TabName { get; set; }
        public CateLogType Catalog { get; set; }
    }

    public interface HelpedAutoLoad
    {
        string HelpDoc { get; }
    }

    public class WIPTemplate : Attribute
    {
    }
}