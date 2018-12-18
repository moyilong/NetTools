using System;
using System.Collections.Generic;
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

namespace 诊断工具.Controls.Disks
{
    /// <summary>
    /// DiskInfo.xaml 的交互逻辑
    /// </summary>
    public partial class DiskInfo : UserControl,AutoLoadTemplate
    {
        public DiskInfo()
        {
            InitializeComponent();
        }

        public string TabName => "磁盘信息";

        public string Catalog => "磁盘";

        private void refresh_disk_info_Click(object sender, RoutedEventArgs e)
        {
            disk_info.ItemsSource = DriveInfo.GetDrives();
        }

    }
}
