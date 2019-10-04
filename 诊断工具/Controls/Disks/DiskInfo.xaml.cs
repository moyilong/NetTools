using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace 诊断工具.Controls.Disks
{
    /// <summary>
    /// DiskInfo.xaml 的交互逻辑
    /// </summary>
    [AutoLoadTemplate(Catalog = AutoLoadTemplate.CateLogType.Disk, TabName = "磁盘")]
    public partial class DiskInfo : UserControl
    {
        public DiskInfo()
        {
            InitializeComponent();
        }

        private void refresh_disk_info_Click(object sender, RoutedEventArgs e)
        {
            disk_info.ItemsSource = DriveInfo.GetDrives();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            refresh_disk_info_Click(sender, e);
        }
    }
}