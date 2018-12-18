using Phenom.Extension;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace 诊断工具.Controls.Generic
{
    /// <summary>
    /// cpuid.xaml 的交互逻辑
    /// </summary>
    public partial class cpuid : UserControl,PaggedItem
    {
        public cpuid()
        {
            InitializeComponent();
        }

        public string TabName => "CPUID";

        public string Catalog =>"系统";

        struct CPUInfo
        {
            public bool Supported { get; set; }
            public string Name { get; private set; }
            public string Description { get; private set; }
            public string Value { get; private set; }
            public CPUInfo(string line)
            {
                string[] xline = line.Trim().Split(';');
                bool.TryParse(xline[0].Trim(',', '.'), out bool _Supported);
                Supported = _Supported;
                Name = xline[1].Trim(',', '.');
                Description = xline[2].Trim(',', '.');
                Value = xline[3].Trim(',', '.');
            }
        }
        readonly string TempPath = Path.Combine(Environment.GetEnvironmentVariable("temp"), $"cpuid_{Guid.NewGuid().ToString()}.txt");
        private void refresh_cpuinfo_Click(object sender, RoutedEventArgs e)
        {
            Process proces = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cpuid.exe",
                    Arguments = TempPath,
                }
            };
            proces.Start();
            proces.WaitForExit();
            List<CPUInfo> info = new List<CPUInfo>();
            foreach (var i in File.ReadAllText(TempPath, Encoding.GetEncoding("GB2312")).Split('\n'))
                if (!i.Trim().IsEmpty())
                    try
                    {
                        info.Add(new CPUInfo(i));
                    }
                    catch
                    {

                    }
            cpuid_flush_data.ItemsSource = info;
        }
    }
}
